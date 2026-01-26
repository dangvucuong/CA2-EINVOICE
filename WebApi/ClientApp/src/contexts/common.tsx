// common.tsx (updated to use @microsoft/signalr)
import React, { createContext, useContext, useEffect, useState } from "react";
import { v4 as uuidv4 } from "uuid";
import * as signalR from "@microsoft/signalr";

import { Toaster } from "react-hot-toast";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { useAppSelector } from "../hooks/useAppSelector";
import { useAuth } from "../hooks/useAuth";
import { appInfo } from "../AppInfo";

type CommonContextType = {
  children: React.ReactNode;
};

type CommonStoreType = {
  checkAccesiableTo: (
    path: string,
    httpMethod: "GET" | "POST" | "PUT" | "DELETE",
  ) => boolean;
  createUUID: () => string;
  translate: (code: string) => string;

  // signalr core
  _signalrConnection: signalR.HubConnection | null; // thay _signalrHubProxy
  _signalrConnected: boolean;
  _signalrReConnectedCount: number;
  _signalrStopped: boolean;
  _signalrReConnectMaxCount: number;

  _signalrSelectCert: () => Promise<void>;
  handleReconnect: () => void;
  handleDisconnect: () => void;
  _signalrSignLogin: (code: string, serial: string) => Promise<void>;
  getMSTFromCertSubject: (subject: string) => string;

  signalRConnectionServer: any; // giữ nguyên hub server riêng của hệ thống bạn
  isSignalRReady: () => boolean;
  reconnectSignalR: () => Promise<void>;

  _signalrHubProxy: any; // giữ nguyên để tránh lỗi biên dịch tạm thời
};

const CommonContext = createContext({} as CommonStoreType);
export const useCommonContext = () => useContext(CommonContext);

const reConnectTime = 5000;
const reConnectMaxCount = 10;

export const CommonProvider = ({ children }: CommonContextType) => {
  const { user } = useAuth();
  const { localized_resources } = useAppSelector(
    (x) => x.common.localizedResourceReducer,
  );

  // SIGNALR (server hub already in your web)
  const [connectionServer, setConnectionServer] =
    useState<signalR.HubConnection | null>(null);

  // LOCAL SignalR (WinForms host)
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null,
  );
  const [isConnected, setIsConnected] = useState<boolean>(false);
  const [isStoppedConnect, setIsStoppedConnect] = useState<boolean>(false);
  const [reConnectCount, setReConnectCount] = useState<number>(0);

  // ---------------------------
  // server hub connection (kept similar)
  // ---------------------------
  useEffect(() => {
    if (user) {
      const domain = appInfo.baseApiURL.includes("http")
        ? appInfo.baseApiURL.replace("/api", "")
        : window.location.origin;

      const newConnection = new signalR.HubConnectionBuilder()
        .withUrl(
          domain +
            "/hubs/hoa-don?userId=" +
            user?.user_id.toString() +
            "&donVi=" +
            user?.donvi_ma_dv,
        )
        .withAutomaticReconnect()
        .build();

      setConnectionServer(newConnection);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user]);

  useEffect(() => {
    if (!connectionServer) return;

    let isMounted = true;
    connectionServer
      .start()
      .then(() => {
        if (!isMounted) return;
        console.log("ConnectionServer Connected!");
      })
      .catch((e) => {
        console.error("connectionServer failed: ", e);
        // fallback retry (simple)
        const intervalId = setInterval(() => {
          if (!connectionServer) {
            clearInterval(intervalId);
            return;
          }
          connectionServer
            .start()
            .then(() => {
              clearInterval(intervalId);
              console.log("ConnectionServer reconnected");
            })
            .catch(() => {
              /* keep retrying */
            });
        }, reConnectTime);
      });

    return () => {
      isMounted = false;
      if (connectionServer) connectionServer.stop().catch(() => {});
    };
  }, [connectionServer]);

  // ---------------------------
  // initialize local SignalR connection (to WinForms-hosted hub)
  // ---------------------------
  useEffect(() => {
    // if there's no url configured, skip
    if (!appInfo.chuKySoSignalrUrl) return;

    const conn = new signalR.HubConnectionBuilder()
      .withUrl(appInfo.chuKySoSignalrUrl) // expect full hub url like http://127.0.0.1:5000/chathub
      .withAutomaticReconnect()
      .build();

    setConnection(conn);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // ---------------------------
  // manage lifecycle & events for local connection
  // ---------------------------
  useEffect(() => {
    if (!connection) return;

    let reconnectAttempts = 0;
    let manualRetryInterval: any = null;

    // event handler: incoming messages from server
    const onAddMessage = (sender: string, message: string) => {
      try {
        if (sender === "SERVER") {
          const ketquas = message.split("|");
          const [returnCode, code, signedtext] = ketquas;

          if (signedtext === "CertInf") {
            const [nhaCungCap, serial, tuNgay, denNgay, subject] =
              ketquas.slice(3);
            const data: any = {
              returnCode,
              code,
              signedtext,
              nhaCungCap,
              serial,
              tuNgay,
              denNgay,
              subject,
            };
            console.log({ data });
          }
        }
      } catch (err) {
        console.error("onAddMessage handler error", err);
      }
    };

    connection.on("addMessage", onAddMessage);

    connection.onreconnecting((err) => {
      console.warn("SignalR reconnecting", err);
      setIsConnected(false);
    });

    connection.onreconnected((connectionId) => {
      console.log("SignalR reconnected, id:", connectionId);
      setIsConnected(true);
      reconnectAttempts = 0;
      setReConnectCount(0);
      setIsStoppedConnect(false);
      if (manualRetryInterval) {
        clearInterval(manualRetryInterval);
        manualRetryInterval = null;
      }
    });

    connection.onclose((err) => {
      console.warn("SignalR closed", err);
      setIsConnected(false);
      // try manual retry if automatic reconnect exhausted
      if (!isStoppedConnect) {
        // start a manual retry loop up to reConnectMaxCount
        let tries = 0;
        manualRetryInterval = setInterval(() => {
          tries++;
          connection
            .start()
            .then(() => {
              clearInterval(manualRetryInterval);
              manualRetryInterval = null;
            })
            .catch(() => {
              setReConnectCount((p) => p + 1);
              if (tries >= reConnectMaxCount) {
                clearInterval(manualRetryInterval);
                manualRetryInterval = null;
                setIsStoppedConnect(true);
              }
            });
        }, reConnectTime);
      }
    });

    // start connection
    connection
      .start()
      .then(() => {
        console.log(
          "Local SignalR connected, id:",
          (connection as any).connectionId ?? "n/a",
        );
        setIsConnected(true);
        setReConnectCount(0);
        setIsStoppedConnect(false);
      })
      .catch((err) => {
        console.log(err);

        console.error("Local SignalR start failed:", err);
        // schedule retries
        let tries = 0;
        const intervalId = setInterval(() => {
          tries++;
          connection
            .start()
            .then(() => {
              clearInterval(intervalId);
              setIsConnected(true);
              setReConnectCount(0);
            })
            .catch(() => {
              setReConnectCount((p) => p + 1);
              if (tries >= reConnectMaxCount) {
                clearInterval(intervalId);
                setIsStoppedConnect(true);
              }
            });
        }, reConnectTime);
      });

    return () => {
      // cleanup
      connection.off("addMessage", onAddMessage);
      try {
        connection.stop().catch(() => {});
      } catch {}
      if (manualRetryInterval) clearInterval(manualRetryInterval);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [connection]);

  // ---------------------------
  // Actions: SelectCert, SignLogin (use invoke)
  // ---------------------------
  const SelectCert = async (): Promise<void> => {
    try {
      if (!connection) {
        console.warn("SelectCert: connection not ready");
        return;
      }
      const code = uuidv4();
      const content = `${code}|0|LoadCert|Cert`;
      await connection.invoke("Send", content);
      // note: server will respond via addMessage
    } catch (err) {
      console.error("SelectCert invoke error:", err);
    }
  };

  const SignLogin = async (code: string, serial: string): Promise<void> => {
    try {
      if (!connection) {
        console.warn("SignLogin: connection not ready");
        return;
      }
      const content = `${code}|${serial}|Login|Text`;
      await connection.invoke("Send", content);
    } catch (err) {
      console.error("SignLogin invoke error:", err);
    }
  };

  // ---------------------------
  // Utilities
  // ---------------------------
  const getMSTFromCertSubject = (subject: string) => {
    const indexOfMST = subject.indexOf("MST:");
    const indexOfCCCD = subject.indexOf("CCCD:");
    if (indexOfMST !== -1) {
      const indexOfComma = subject.indexOf(",", indexOfMST);
      if (indexOfComma !== -1) {
        return subject.substring(indexOfMST + 4, indexOfComma);
      } else {
        return subject.substring(indexOfMST + 4);
      }
    } else if (indexOfCCCD !== -1) {
      const indexOfComma = subject.indexOf(",", indexOfCCCD);
      if (indexOfComma !== -1) {
        return subject.substring(indexOfCCCD + 5, indexOfComma).trim();
      } else {
        return subject.substring(indexOfCCCD + 5).trim();
      }
    }
    console.log("Không tìm thấy MST trong chuỗi");
    return "";
  };

  // ---------------------------
  // reconnect / disconnect helpers
  // ---------------------------
  const handleReconnect = () => {
    if (connection && !isConnected) {
      setIsStoppedConnect(false);
      setReConnectCount(0);
      let tries = 0;
      const intervalId = setInterval(() => {
        tries++;
        connection
          .start()
          .then(() => {
            clearInterval(intervalId);
            setIsConnected(true);
            setReConnectCount(0);
            setIsStoppedConnect(false);
          })
          .catch(() => {
            setReConnectCount((p) => p + 1);
            if (tries >= reConnectMaxCount) {
              clearInterval(intervalId);
              setIsStoppedConnect(true);
            }
          });
      }, reConnectTime);
    }
  };

  const handleDisconnect = () => {
    if (connection) {
      connection.stop().catch(() => {});
    }
    setIsConnected(false);
    setIsStoppedConnect(true);
  };

  const isSignalRReady = () => {
    return connection !== null && isConnected;
  };

  const reconnectSignalR = async (): Promise<void> => {
    if (!connection) return;
    if (isConnected) return;
    try {
      await connection.start();
    } catch (err) {
      console.error("reconnectSignalR failed", err);
      throw err;
    }
  };

  // ---------------------------
  // Provider store
  // ---------------------------
  const store: CommonStoreType = {
    checkAccesiableTo: (
      endpoint: string,
      httpMethod: "GET" | "POST" | "PUT" | "DELETE",
    ) => {
      const api = user?.apis
        .filter((x) => x.method === httpMethod)
        .find(
          (x) => x.endpoint === endpoint || x.endpoint === `api/${endpoint}`,
        );
      return !!api;
    },
    createUUID: () => uuidv4(),
    translate: (code: string) => localized_resources.get(code) ?? code,
    _signalrConnection: connection,
    _signalrConnected: isConnected,
    _signalrReConnectedCount: reConnectCount,
    _signalrStopped: isStoppedConnect,
    _signalrReConnectMaxCount: reConnectMaxCount,
    handleReconnect,
    handleDisconnect,
    _signalrSelectCert: SelectCert,
    _signalrSignLogin: SignLogin,
    getMSTFromCertSubject,
    isSignalRReady,
    reconnectSignalR,
    signalRConnectionServer: connectionServer,

    _signalrHubProxy: null,
  };

  return (
    <CommonContext.Provider value={store}>
      {children}
      <ToastContainer />
      <Toaster
        toastOptions={{
          success: { style: { background: "#1B7F36", color: "#fff" } },
          error: { style: { background: "#A30F26", color: "#fff" } },
          position: "bottom-right",
        }}
      />
    </CommonContext.Provider>
  );
};

//export { CommonProvider, useCommonContext };
