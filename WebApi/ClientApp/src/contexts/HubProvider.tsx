import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import React, { createContext, useContext, useEffect, useState } from "react";
import { appInfo } from "../AppInfo";
import { useAuth } from "../hooks/useAuth";

type HubContextType = {
  children: React.ReactNode;
};

type HubStoreType = {
  _connectionServer: any;
};

const HubContext = createContext({} as HubStoreType);
const useHubContext = () => useContext(HubContext);
const reConnectTime: number = 5000;

const HubProvider = ({ children }: HubContextType) => {
  const { user } = useAuth();
  const [connectionServer, setConnectionServer] = useState<any>(null);
  const [isConnected, setIsConnected] = useState(false);

  const store: HubStoreType = {
    _connectionServer: connectionServer,
  };
  useEffect(() => {
    if (user?.user_id) {
      const domain = appInfo.baseApiURL.includes("http")
        ? appInfo.baseApiURL.replace("/api", "")
        : window.location.origin;

      const newConnection = new HubConnectionBuilder()
        .withUrl(domain + "/hubs/process?userId=" + user.user_id.toString())
        .withAutomaticReconnect()
        .build();

      setConnectionServer(newConnection);
    } else {
      setConnectionServer(null);
    }
  }, [user?.user_id]);

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

  return <HubContext.Provider value={store}>{children}</HubContext.Provider>;
};
export { HubProvider, useHubContext };
