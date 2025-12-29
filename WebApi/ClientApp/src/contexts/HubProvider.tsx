import { HubConnectionBuilder } from "@microsoft/signalr";
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
    if (user) {
      const domain = appInfo.baseApiURL.includes("http")
        ? appInfo.baseApiURL.replace("/api", "")
        : window.location.origin;
      //   console.log({
      //     hubDomainServer: domain,
      //     user,
      //   });

      const newConnection = new HubConnectionBuilder()
        .withUrl(domain + "/hubs/process?userId=" + user.user_id.toString())
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
          setIsConnected(true);
        })
        .catch((e: any) => {
          //  console.log("connectionServer failed: ", e);
          const intervalId = setInterval(() => {
            //  console.log("Try reconnect connectionServer");
            if (connectionServer) {
              try {
                connectionServer.start().done(function () {
                  setIsConnected(true);
                  clearInterval(intervalId);
                });
              } catch (error) {}
            }
          }, reConnectTime);
        });
    }
    return () => {
      if (connectionServer) {
        connectionServer.stop();
        setIsConnected(false);
      }
    };
  }, [connectionServer]);

  return <HubContext.Provider value={store}>{children}</HubContext.Provider>;
};
export { HubProvider, useHubContext };
