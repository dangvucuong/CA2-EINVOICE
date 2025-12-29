import {
  Box,
  CounterLabel,
  TreeView,
  TreeViewSubTreeProps,
} from "@primer/react";

import { Link, matchPath, useHistory, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { IMenuViewModel } from "../../models/responses/user/IMenu";
import styles from "./NavSub.module.css";
import { useMemo } from "react";
import { useAppSelector } from "../../hooks/useAppSelector";
import { INotifySummaryRespone } from "../../models/responses/notify/INotifySummary";
import { useWindowSize } from "../../hooks/useWindowSize";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { eNavSubMode } from "../../models/commons/eNavSubMode";
import { rootAction } from "../../state/actions/rootAction";
import { XIcon } from "@primer/octicons-react";
import UserPanel from "../nav-bar/UserPanel";

interface ISubMenuProps extends TreeViewSubTreeProps {
  menuParent: IMenuViewModel;
  key?: string;
}
export const getNotifyCount = (
  path: string,
  notifySummary?: INotifySummaryRespone
): number => {
  if (notifySummary) {
    switch (path) {
      case "contact":
        return notifySummary.register_new_count ?? 0;

      default:
        break;
    }
  }
  return 0;
};
const SubMenu = (props: ISubMenuProps) => {
  const { menuParent } = props;
  let location = useLocation();
  const history = useHistory();
  const { notifySummary } = useAppSelector((x) => x.notify.notifyReducer);

  return (
    <>
      {menuParent.items.length > 0 && (
        <>
          {menuParent.items.map((menu) => {
            const notifyCount = getNotifyCount(menu.path, notifySummary);
            return (
              <TreeView.Item
                key={menu.id}
                id={menu.id.toString()}
                current={matchPath(location.pathname, `/${menu.path}`) !== null}
                onSelect={() => {
                  history.push(`../../${menu.path}`);
                }}
              >
                {menu.icon && (
                  <TreeView.LeadingVisual>
                    <img
                      src={`../../images/${menu.icon}`}
                      alt="icon"
                      style={{
                        width: "16px",
                      }}
                    />
                  </TreeView.LeadingVisual>
                )}
                <Link to={`../../${menu.path}`} className="link">
                  {menu.name}
                </Link>
                <SubMenu menuParent={menu} />
                {notifyCount > 0 && (
                  <TreeView.TrailingVisual>
                    <CounterLabel
                      scheme="primary"
                      sx={{
                        backgroundColor: "accent.fg",
                      }}
                    >
                      {notifyCount}
                    </CounterLabel>
                  </TreeView.TrailingVisual>
                )}
              </TreeView.Item>
            );
          })}
        </>
      )}
    </>
  );
};
const NavSub = () => {
  const { user, appSelected } = useAuth();
  const dispatch = useAppDispatch();
  const { navSubMode } = useAppSelector((x) => x.common.mainLayoutReducer);
  const { width, height } = useWindowSize();

  const menuAll = user?.menus ?? [];

  const handleClick = () => {
    const mode = eNavSubMode.POPUP;
    dispatch(rootAction.common.mainLayoutAction.changeNavSubMode(mode));
  };

  const menus = useMemo(() => {
    if (width < 768) {
      return menuAll;
    }

    if (appSelected) {
      return menuAll.filter((x) => x.id === appSelected.id);
    }
    return menuAll;
  }, [appSelected, menuAll]);

  return (
    <Box
      className={styles.navsub}
      sx={{
        minHeight: window.innerHeight,
        pl: width < 768 ? 0 : 2,
        pb: 2,
        overflowY: "auto",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          mr: 2,
        }}
      >
        <Box
          sx={{
            height: "84px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <img alt="logo" src="../../images/logo-white.svg" height={"40px"} />
        </Box>
        <Box
          sx={{ cursor: "pointer" }}
          onClick={handleClick}
          display={["block", "block", "none"]}
        >
          <XIcon size={32} />
        </Box>
      </Box>
      <Box
        sx={{
          height: window.innerHeight - 144,
          overflowY: "auto",
        }}
      >
        <TreeView aria-label="Menus">
          {menus.map((menu) => {
            return (
              <TreeView.Item
                key={menu.id}
                id={menu.id.toString()}
                defaultExpanded
              >
                {menu.icon && (
                  <TreeView.LeadingVisual>
                    <img
                      src={`../../images/${menu.icon}`}
                      alt="icon"
                      style={{
                        width: "16px",
                      }}
                    />
                  </TreeView.LeadingVisual>
                )}
                {menu.name}
                <TreeView.SubTree>
                  <SubMenu menuParent={menu} />
                </TreeView.SubTree>
              </TreeView.Item>
            );
          })}
        </TreeView>
      </Box>

      <Box
        display={["block", "block", "none"]}
        sx={{
          display: "flex",
          justifyContent: "center",
          height: "60px",
          alignItems: "center",
        }}
      >
        <UserPanel />
      </Box>
    </Box>
  );
};

export default NavSub;
