import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import React, { createContext, useContext, useEffect, useState } from "react";
import { v4 as uuid } from "uuid";
import { appInfo } from "../AppInfo";

type HubPublicContextType = {
  children: React.ReactNode;
};

type PublicHubStoreType = {
  _connectionServer: any;
  sessionId: string;
  isConnected: boolean;
};

const HubPublicContext = createContext({} as PublicHubStoreType);
const useHubPublicContext = () => useContext(HubPublicContext);
const reConnectTime: number = 5000;

const HubPublicProvider = ({ children }: HubPublicContextType) => {
  const [connectionServer, setConnectionServer] = useState<any>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [sessionId, setSessionId] = useState("");
  useEffect(() => {
    setSessionId(uuid());
  }, []);

  const store: PublicHubStoreType = {
    _connectionServer: connectionServer,
    sessionId: sessionId,
    isConnected,
  };
  useEffect(() => {
    if (sessionId !== "") {
      const domain = appInfo.baseApiURL.includes("http")
        ? appInfo.baseApiURL.replace("/api", "")
        : window.location.origin;
      const newConnection = new HubConnectionBuilder()
        .withUrl(domain + "/hubs/hoa-don?userId=" + sessionId)
        .withAutomaticReconnect()
        .build();

      setConnectionServer(newConnection);
    }
  }, [sessionId]);

  useEffect(() => {
    if (!connectionServer) return;

    let cancelled = false;
    let intervalId: ReturnType<typeof setInterval> | undefined;

    const tryStart = async () => {
      if (cancelled || !connectionServer) return false;

      if (connectionServer.state === HubConnectionState.Connected) {
        setIsConnected(true);
        return true;
      }

      // Chỉ start khi Disconnected — tránh lỗi "Cannot start a HubConnection..."
      if (connectionServer.state !== HubConnectionState.Disconnected) {
        return false;
      }

      try {
        await connectionServer.start();
        if (!cancelled) setIsConnected(true);
        return true;
      } catch (e) {
        return false;
      }
    };

    tryStart().then((ok) => {
      if (ok || cancelled) return;
      intervalId = setInterval(async () => {
        const success = await tryStart();
        if (success && intervalId) clearInterval(intervalId);
      }, reConnectTime);
    });

    return () => {
      cancelled = true;
      if (intervalId) clearInterval(intervalId);
      if (connectionServer) {
        connectionServer.stop().catch(() => {});
      }
      setIsConnected(false);
    };
  }, [connectionServer]);

  return (
    <HubPublicContext.Provider value={store}>{children}</HubPublicContext.Provider>
  );
};
export { HubPublicProvider, useHubPublicContext };
