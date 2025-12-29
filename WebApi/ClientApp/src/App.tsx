import { ThemeProvider, theme } from '@primer/react';
import deepmerge from 'deepmerge';
import { useEffect, useState } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import './App.css';
import BaseStyles from "./BaseStyles";
import LoaderPage from './component-ui/loader-page';
import AuthorizedContent from './contents/authorized';
import PublicContent from './contents/un-authorized/PublicContent';
import { CommonProvider } from './contexts/common';
import { useAppDispatch } from './hooks/useAppDispatch';
import { useAppSelector } from './hooks/useAppSelector';
import { rootAction } from './state/actions/rootAction';
import { eAccountReducerStatus } from './state/reducer-models/account/IAccountReducer';
import { HubProvider } from './contexts/HubProvider';
import { HubPublicProvider } from './contexts/HubPublicProvider';
import { useLogoutOnAllTabsClose } from './hooks/useLogoutOnAllTabsClose';
import { clearAccessToken, clearRefreshToken } from './api/apiClient';
// https://primer.style/guides/react/theme-reference
const customTheme = deepmerge(theme, {
  fonts: {

  },
  colorSchemes: {
    light: {
      colors: {
        accent: {
          fg: "#DE3F0F",
        },
        btn: {
          primary: {
            bg: "#DE3F0F",
            focusBg: "#DE3F0F",
            hoverBg: "#DE3F0F",
            selectedBg: "#DE3F0F",
            disabledBg: "rgba(222, 63, 15,0.6)"
          }
        }
      }
    }
  }
})
const TAB_KEY = "app_open_tabs";
function App() {
  const { status, user } = useAppSelector(x => x.accountReducer);
  const { lan } = useAppSelector(x => x.common.localizedResourceReducer);
  const dispatch = useAppDispatch();

  const [isCheckedTab, setIsCheckedTab] = useState(false);
  const getTabs = () => {
    try {
      return JSON.parse(localStorage.getItem(TAB_KEY) || "[]") as {
        id: string;
        lastActive: number;
      }[];
    } catch {
      return [];
    }
  };
  const handleCheckClosedTab = () => {
    const tabs = getTabs().filter((t) => Date.now() - t.lastActive < 5000);
    // debugger
    if (tabs.length <= 0) {
      clearAccessToken();
      clearRefreshToken();
      dispatch(rootAction.accountAction.loadProfileError(""));
    } else {
      setIsCheckedTab(true)
    }

  }
  useEffect(() => {
    dispatch(rootAction.common.appConfigAction.loadStart());
  }, [])
  useEffect(() => {
    if (isCheckedTab) {
      dispatch(rootAction.accountAction.loadProfileStart());
    }
  }, [isCheckedTab])
  useEffect(() => {
    handleCheckClosedTab();
  }, [])
  // useEffect(() => {
  //   dispatch(rootAction.common.localizedResourceAction.loadStart(lan))
  // }, [lan])

  if (status === eAccountReducerStatus.is_getting_profile) {
    return <LoaderPage />
  }

  return (
    <Router>
      <ThemeProvider theme={customTheme}>
        <CommonProvider>
          <BaseStyles>
            {user && <>
              <HubProvider>
                <AuthorizedContent />
              </HubProvider>
            </>}
            {!user && status &&
              <HubPublicProvider>
                <PublicContent />
              </HubPublicProvider>}
          </BaseStyles>
        </CommonProvider>
      </ThemeProvider>
    </Router>
  );
}

export default App;
