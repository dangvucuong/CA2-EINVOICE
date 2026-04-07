import { HubConnectionBuilder } from "@microsoft/signalr";
import React, { createContext, useContext, useEffect, useState } from "react";
import { v4 as uuid } from "uuid"
import { appInfo } from "../AppInfo";

type HubPublicContextType = {
  children: React.ReactNode;
};

type PublicHubStoreType = {
  _connectionServer: any;
  sessionId: string;
  isConnected: boolean
};

const HubPublicContext = createContext({} as PublicHubStoreType);
const useHubPublicContext = () => useContext(HubPublicContext);
const reConnectTime: number = 5000;

const HubPublicProvider = ({ children }: HubPublicContextType) => {

  const [connectionServer, setConnectionServer] = useState<any>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [sessionId, setSessionId] = useState("");
  useEffect(() => {
    setSessionId(uuid())
  }, [])

  const store: PublicHubStoreType = {
    _connectionServer: connectionServer,
    sessionId: sessionId,
    isConnected
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
    if (connectionServer) {
      connectionServer
        .start()
        .then((result: any) => {
          setIsConnected(true);
        })
        .catch((e: any) => {
          const intervalId = setInterval(() => {
            if (connectionServer) {
              try {
                connectionServer.start().done(function () {
                  setIsConnected(true);
                  clearInterval(intervalId);
                });
              } catch (error) { }
            }
          }, reConnectTime);
        });
    }
    return () => {
      console.log({
        HubPublicProvider: "cleanup"
      });

      if (connectionServer) {
        connectionServer.stop();
        setIsConnected(false);
      }
    };
  }, [connectionServer]);

  return <HubPublicContext.Provider value={store}>{children}</HubPublicContext.Provider>;
};
export { HubPublicProvider, useHubPublicContext };
