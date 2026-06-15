import { HubConnectionBuilder } from "@microsoft/signalr";
import React, { createContext, useContext, useEffect, useState } from "react";
import { Toaster } from "react-hot-toast";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Connection, hubConnection } from "signalr-no-jquery";
import { v4 as uuidv4 } from "uuid";
import { appInfo } from "../AppInfo";
import { useAppSelector } from "../hooks/useAppSelector";
import { useAuth } from "../hooks/useAuth";

type CommonContextType = {
  children: React.ReactNode;
};

type CommonStoreType = {
  checkAccesiableTo: (
    path: string,
    httpMethod: "GET" | "POST" | "PUT" | "DELETE"
  ) => boolean;
  createUUID: () => string;
  translate: (code: string) => string;
  _signalrHubProxy: any;
  _signalrConnection: any;
  _signalrConnected: boolean;
  _signalrReConnectedCount: number;
  _signalrStopped: boolean;
  _signalrReConnectMaxCount: number;
  _signalrSelectCert: () => void;
  handleReconnect: () => void;
  handleDisconnect: () => void;
  _signalrSignLogin: (code: string, serial: string) => void;
  getMSTFromCertSubject: (subject: string) => string;
  signalRConnectionServer: any;
  isSignalRReady: () => boolean;
  reconnectSignalR: () => void;
};
const SignalRStates = {
  connecting: 1,
  connected: 2,
  disconnecting: 3,
  disconnected: 4,
  reconnecting: 5
};
const CommonContext = createContext({} as CommonStoreType);
const useCommonContext = () => useContext(CommonContext);
const reConnectTime: number = 5000;
const reConnectMaxCount: number = 10;

// const reConnectTime: number = 5000000;
const CommonProvider = ({ children }: CommonContextType) => {
  const { user } = useAuth();
  const { localized_resources } = useAppSelector(
    (x) => x.common.localizedResourceReducer
  );
  const { createUUID } = useCommonContext();
  const [hubProxy, sethubProxy] = useState<any>();
  const [connection, setConnection] = useState<Connection>();
  const [isConnected, setIsConnected] = useState(false);
  const [isStoppedConnect, setIsStoppedConnect] = useState(false);
  const [reConnectCount, setReConnectCount] = useState<number>(0);

  const [connectionServer, setConnectionServer] = useState<any>(null);
  useEffect(() => {
    if (user) {
      const domain = appInfo.baseApiURL.includes("http")
        ? appInfo.baseApiURL.replace("/api", "")
        : window.location.origin;
      console.log({
        hubDomainServer: domain,
      });

      const newConnection = new HubConnectionBuilder()
        .withUrl(
          domain +
          "/hubs/hoa-don?userId=" +
          user?.user_id.toString() +
          "&donVi=" +
          user?.donvi_ma_dv
        )
        .withAutomaticReconnect()
        .build();

      setConnectionServer(newConnection);
    }
  }, [user]);
  useEffect(() => {
    if (connectionServer) {
      connectionServer
        .start()
        .then((result: any) => {
          console.log("ConnectionServer Connected!");

        })
        .catch((e: any) => {
          console.log("connectionServer failed: ", e);
          const intervalId = setInterval(() => {
            console.log("Try reconnect connectionServer");
            if (connectionServer) {
              try {
                connectionServer.start().done(function () {
                  clearInterval(intervalId);
                });
              } catch (error) { }
            }
          }, reConnectTime);
        });
    }
    return () => {
      if (connectionServer) connectionServer.stop();
    };
  }, [connectionServer]);
  useEffect(() => {
    const localConnection = hubConnection(`${appInfo.chuKySoSignalrUrl}`);
    setConnection(localConnection);
  }, []);
  useEffect(() => {
    if (connection) {
      var hubProxy = connection.createHubProxy("chathub");

      hubProxy.on("addMessage", function (eventName, data) {

        if (eventName === "SERVER") {
          const ketquas = data.split("|");
          const [returnCode, code, signedtext] = ketquas;
          console.log({
              returnCode,
              code,
              signedtext
          });

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
            console.log({
              data,
            });
          }
        }
      });

      sethubProxy(hubProxy);
      connection
        .start()
        .done(function () {
          console.log("Now connected, connection ID=" + connection.id);
          setIsConnected(true);
        })
        .fail(function () {
          console.log("Could not connect");
          const intervalId = setInterval(() => {
            console.log("Try reconnect");
            try {
              connection
                .start()
                .done(function () {
                  console.log("Now connected, connection ID=" + connection.id);
                  setIsConnected(true);
                  clearInterval(intervalId);
                  setReConnectCount(0);
                  setIsStoppedConnect(false);
                })
                .fail(function () {
                  setReConnectCount((p) => {
                    // console.log({ p });

                    if (p + 1 > reConnectMaxCount) {
                      clearInterval(intervalId);
                      setIsStoppedConnect(true);
                    }
                    return p + 1;
                  });
                });
            } catch (error) {
              console.log({
                error,
              });
            }
          }, reConnectTime);
        });

      connection.reconnecting(() => {
        setIsConnected(false);

        console.log(
          "reconnecting:  SignalR connection closed. Trying to reconnect..."
        );
        const intervalId = setInterval(() => {
          console.log("Try reconnect");
          connection
            .start()
            .done(function () {
              console.log("Now connected, connection ID=" + connection.id);
              setIsConnected(true);
              clearInterval(intervalId);
            })
            .fail(function () {
              setReConnectCount((p) => {
                // console.log({ p });

                if (p + 1 > reConnectMaxCount) {
                  clearInterval(intervalId);
                  setIsStoppedConnect(true);
                }
                return p + 1;
              });
            });
        }, reConnectTime);
      });
    }
    return () => {
      if (connection) {
        connection.stop();
        setIsConnected(false);
      }
    };
  }, [connection]);
  const SelectCert = () => {
    try {
      var code = uuidv4();
      var content = code + "|0|LoadCert|Cert";
      // _signalrHubProxy.send(content)
      hubProxy
        .invoke("send", content)
        .done(function () {
          // console.log({
          //   sendSuccess: content,
          // });
        })
        .fail(function (error: any) {
          console.log("Invocation failed. Error: " + error);
        });
    } catch (error) { }
  };

  const SignLogin = (code: string, serial: string) => {
    // var code = uuidv4();
    var content = code + "|" + serial + "|Login|Text";
    // _signalrHubProxy.send(content)
    hubProxy
      .invoke("send", content)
      .done(function () {
        // console.log({
        //   sendSuccess: content,
        // });
        // return code;
      })
      .fail(function (error: any) {
        console.log("Invocation failed. Error: " + error);
        // return false;
      });
  };

  const getMSTFromCertSubject = (subject: string) => {
    // Tìm vị trí của MST:
    const indexOfMST = subject.indexOf("MST:");
    const indexOfCCCD = subject.indexOf("CCCD:");
    // Kiểm tra xem có MST không
    if (indexOfMST !== -1) {
      // Tìm vị trí của dấu phẩy kế tiếp sau MST:
      const indexOfComma = subject.indexOf(",", indexOfMST);

      // Kiểm tra xem có dấu phẩy kế tiếp sau MST không
      if (indexOfComma !== -1) {
        // Lấy phần text sau MST: và trước dấu phẩy
        const mstContent = subject.substring(indexOfMST + 4, indexOfComma);
        // console.log("Nội dung MST:", mstContent);
        return mstContent;
      } else {
        const mstContent = subject.substring(indexOfMST + 4);
        // console.log("Nội dung MST:", mstContent);
        return mstContent;
      }
    } else {
      if (indexOfCCCD !== -1) {
        const indexOfComma = subject.indexOf(",", indexOfCCCD);
        if (indexOfComma !== -1) {
          const cccdContent = subject
            .substring(indexOfCCCD + 5, indexOfComma)
            .trim();
          return cccdContent;
        } else {
          const cccdContent = subject.substring(indexOfCCCD + 5).trim();
          return cccdContent;
        }
      }
    }
    console.log("Không tìm thấy MST trong chuỗi");
    return "";
  };

  const handleReconnect = () => {
    if (connection && !isConnected) {
      setIsStoppedConnect(false);
      setReConnectCount(0);
      const intervalId = setInterval(() => {
        console.log("Try reconnect");
        connection
          .start()
          .done(function () {
            console.log("Now connected, connection ID=" + connection.id);
            setIsConnected(true);
            clearInterval(intervalId);
            setReConnectCount(0);
            setIsStoppedConnect(false);
          })
          .fail(function () {
            setReConnectCount((p) => {
              if (p + 1 > reConnectMaxCount) {
                clearInterval(intervalId);
                setIsStoppedConnect(true);
              }
              return p + 1;
            });
          });
      }, reConnectTime);
    }
  };

  const handleDisconnect = () => {
    if (connection) {
      connection.stop();
    }
    setIsConnected(false);
    setIsStoppedConnect(true);
    // setConnection(undefined)
  };
  const isSignalRReady = () => {
    
    // CHECK 1: Proxy tồn tại
    if (!hubProxy) {
      console.log("❌ SignalR proxy not initialized");
      return false;
    }

    // CHECK 2: Connection state
    const state = hubProxy.state;
    console.log("📡 SignalR State:", state);

    // ✅ READY STATES
    return state === 2;// SignalRStates.connected
  }
  const reconnectSignalR = () => {
    console.log("🔄 Reconnecting...");
    if (hubProxy.current?.state === 2) {
      return; // Đã connect
    }

    // Stop cũ
    if (hubProxy.current) {
      hubProxy.current.connection.stop();
    }

    // Re-init
    initializeSignalR()
      .then(() => {
        console.log("✅ Reconnected!");
        // NotifyHelper.Success("Kết nối lại thành công");
        // Retry send sau 500ms
        // setTimeout(Send, 500);
      })
      .catch((error: any) => {
        console.error("❌ Reconnect failed:", error);
        // NotifyHelper.Error("Không thể kết nối");
      });
  }
  function initializeSignalR() {
    return new Promise((resolve, reject) => {
      try {
        // Tạo connection KHÔNG JQUERY
        // const connection = new signalR.hubConnection();
        const localConnection = hubConnection(`${appInfo.chuKySoSignalrUrl}`);
        setConnection(localConnection);

       

      } catch (error) {
        reject(error);
      }
    });
  }
  const store: CommonStoreType = {
    checkAccesiableTo: (
      endpoint: string,
      httpMethod: "GET" | "POST" | "PUT" | "DELETE"
    ) => {
      const api = user?.apis
        .filter((x) => x.method === httpMethod)
        .find(
          (x) => x.endpoint === endpoint || x.endpoint === `api/${endpoint}`
        );
      if (api) return true;
      return false;
    },
    createUUID: () => {
      return uuidv4();
    },
    translate: (code: string) => {
      return localized_resources.get(code) ?? code;
    },
    _signalrHubProxy: hubProxy,
    _signalrConnection: connection,
    _signalrConnected: isConnected,
    _signalrReConnectedCount: reConnectCount,
    _signalrStopped: isStoppedConnect,
    _signalrReConnectMaxCount: reConnectMaxCount,
    handleReconnect: handleReconnect,
    handleDisconnect: handleDisconnect,
    _signalrSelectCert: SelectCert,
    _signalrSignLogin: SignLogin,
    getMSTFromCertSubject,
    isSignalRReady,
    reconnectSignalR,
    signalRConnectionServer: connectionServer,
  };
  return (
    <CommonContext.Provider value={store}>
      {children}
      <ToastContainer />
      <Toaster
        toastOptions={{
          success: {
            style: {
              background: "#1B7F36",
              color: "#fff",
            },
          },
          error: {
            style: {
              background: "#A30F26",
              color: "#fff",
            },
          },
          position: "bottom-right",
        }}
      />
    </CommonContext.Provider>
  );
};
export { CommonProvider, useCommonContext };
