import { ActionList, ActionMenu, Box, CounterLabel } from "@primer/react";
import clsx from "clsx";
import { useHistory } from "react-router-dom";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { eNavSubMode } from "../../models/commons/eNavSubMode";
import { IMenuViewModel } from "../../models/responses/user/IMenu";
import { rootAction } from "../../state/actions/rootAction";
import styles from "./AppButton.module.css";
import { memo, useLayoutEffect, useMemo, useRef, useState } from "react";
import { getNotifyCount } from "../../layouts/nav-sub/NavSub";
interface IAppButtonProps {
  app: IMenuViewModel;
  // name: string,
  // icon_name?: string,
  // icon?: React.ReactNode,
  // is_focused?: boolean,
  // is_disabled?: boolean,
  // route: string
}
interface ISubMenuProps {
  menuParent: IMenuViewModel;
  key?: string;
}
const SubMenu = (props: ISubMenuProps) => {
  const { menuParent } = props;

  const { notifySummary } = useAppSelector((x) => x.notify.notifyReducer);
  const history = useHistory();
  return (
    <>
      <p
        style={{
          color: "#de3f0f",
          fontSize: 12,
          padding: "6px 8px",
          margin: 0,
        }}
      >
        {menuParent.name}{" "}
      </p>

      {menuParent.items.length > 0 && (
        <>
          {menuParent.items.map((menu) => {
            const notifyCount = getNotifyCount(menu.path, notifySummary);
            return (
              <ActionList.Item
                onClick={() => {
                  history.push(`../../${menu.path}`);
                }}
                sx={{
                  color: "#fff",
                }}
              >
                {/* <Link to={`#`} className='link'> */}
                {menu.name}
                {/* </Link> */}
                {notifyCount > 0 && (
                  <ActionList.TrailingVisual>
                    <CounterLabel
                      sx={{
                        backgroundColor: "accent.fg",
                      }}
                    >
                      {notifyCount}
                    </CounterLabel>
                  </ActionList.TrailingVisual>
                )}
              </ActionList.Item>
            );
          })}
        </>
      )}
    </>
  );
};
const AppButton = (props: IAppButtonProps) => {
  const dispatch = useAppDispatch();
  const { app } = props;
  const { name, icon } = app;
  const { navSubMode } = useAppSelector((x) => x.common.mainLayoutReducer);
  const { user, appSelected } = useAuth();
  const buttonRef = useRef<HTMLDivElement>(null);
  const [menuPos, setMenuPos] = useState<{ top: number; left: number }>({
    top: 0,
    left: 0,
  });
  const [hovered, setHovered] = useState(false);
  // 🧭 Cập nhật vị trí menu khi hover vào
  useLayoutEffect(() => {
    if (hovered && buttonRef.current) {
      const rect = buttonRef.current.getBoundingClientRect();
      setMenuPos({
        top: rect.top, // 👈 nằm cùng hàng ngang với nút
        left: rect.right, // 👈 cách nút cha 8px
      });
    }
  }, [hovered]);

  // const menus = user?.menus ?? [];
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const menuAll = user?.menus ?? [];
  const menus = useMemo(() => {
    if (appSelected) {
      return menuAll.filter((x) => x.id === appSelected.id);
    }
    return menuAll;
  }, [appSelected, menuAll]);

  const is_focused = app.id === appSelected?.id;
  const is_disabled = false;

  return (
    <>
      {navSubMode === eNavSubMode.FULL && (
        <Box
          className={clsx(
            styles.container,
            is_focused ? styles.active : "",
            is_disabled ? styles.disabled : ""
          )}
          onClick={() => {
            dispatch(rootAction.accountAction.changeAppSelected(app));
          }}
        >
          <Box className={styles.icon}>
            <Box className={styles.iconState}>
              {is_focused && <div className={styles.activeState}>&nbsp;</div>}
            </Box>
            <Box className={styles.iconImage}>
              <div
                className={clsx(
                  styles.iconApp,
                  is_focused ? styles.active : ""
                )}
              >
                {icon && (
                  <img
                    alt=""
                    src={`../../images/${icon}`}
                    style={{
                      width: "24px",
                    }}
                  />
                )}
              </div>
            </Box>
            <Box className={styles.iconState}></Box>
          </Box>
          <Box className={styles.appName}>{name}</Box>
        </Box>
      )}
      {navSubMode === eNavSubMode.POPUP && (
        <Box
          className={clsx(
            styles.container,
            is_focused ? styles.active : "",
            is_disabled ? styles.disabled : ""
          )}
          onMouseEnter={() => {
            dispatch(rootAction.accountAction.changeAppSelected(app));
            setHovered(true);
          }}
          onMouseLeave={() => {
            setHovered(false);
          }}
          sx={{ position: "relative" }}
          ref={buttonRef}
        >
          <Box className={styles.icon}>
            <Box className={styles.iconState}>
              {is_focused && <div className={styles.activeState}>&nbsp;</div>}
            </Box>
            <Box className={styles.iconImage}>
              <div
                className={clsx(
                  styles.iconApp,
                  is_focused ? styles.active : ""
                )}
              >
                {icon && (
                  <img
                    alt=""
                    src={`../../images/${icon}`}
                    style={{
                      width: "24px",
                    }}
                  />
                )}
              </div>
            </Box>
            <Box className={styles.iconState}></Box>
          </Box>
          <Box className={styles.appName}>{name}</Box>

          {/* Submenu */}
          {hovered && (
            <Box
              className="submenu"
              sx={{
                position: "fixed",
                top: `${menuPos.top}px`,
                left: `${menuPos.left}px`,
                background: "#343436",
                border: "1px solid #ccc",
                borderRadius: "8px",
                boxShadow: "0 2px 10px rgba(0,0,0,0.15)",
                padding: "8px",
                zIndex: 999999,
                minWidth: "200px",
              }}
              // để menu không biến mất khi rê chuột vào submenu
              onMouseEnter={() => setHovered(true)}
              onMouseLeave={() => setHovered(false)}
            >
              {menus.map((menu) => (
                <SubMenu menuParent={menu} key={menu.id.toString()} />
              ))}
            </Box>
          )}
        </Box>
      )}
    </>
  );
};

export default memo(AppButton);
