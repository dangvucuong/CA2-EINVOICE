import { Box, Overlay } from "@primer/react";
import React, { useRef } from "react";
import { useAppSelector } from "../hooks/useAppSelector";
import { eNavSubMode } from "../models/commons/eNavSubMode";
import { RootState } from "../state/reducers/rootReducer";
import Header from "./header";
import NavBar from "./nav-bar";
import NavSub from "./nav-sub";
import PageContent from "./page";
import { rootAction } from "../state/actions/rootAction";
import NotifyList from "../component-data/notify";
import { useAppDispatch } from "../hooks/useAppDispatch";
import { useWindowSize } from "../hooks/useWindowSize";

interface IMainLayoutProps {
  children?: React.ReactNode;
}
const MainLayout = (props: IMainLayoutProps) => {
  const state = useAppSelector((x: RootState) => x.common.mainLayoutReducer);
  const { navSubMode, isOpenNotifyOverlay } = state;
  const buttonRef = useRef<HTMLButtonElement>(null);
  const confirmButtonRef = useRef<HTMLButtonElement>(null);
  const anchorRef = useRef<HTMLDivElement>(null);
  const dispatch = useAppDispatch();
  const { width, height } = useWindowSize();

  // const {navSubMode}= useSelector((x: RootState) => x.common.mainLayoutReducer)
  // console.log({
  //     state
  // });

  return (
    <Box
      sx={{
        display: "flex",
        height: window.innerHeight,
        overflow: "hidden",
      }}
    >
      <Box
        sx={{
          width: 88,
          borderRightStyle: "solid",
          borderRightWidth: 1,
          // borderRightColor: "border.default"
          // backgroundColor: "neutral.emphasisPlus"
        }}
        display={["none", "none", "block"]}
      >
        <NavBar />
      </Box>

      <Box
        sx={
          width < 768
            ? {
                position: "fixed",
                top: 0,
                left: navSubMode === eNavSubMode.FULL ? 0 : "-240px",
                width: "240px",
                height: "100%",
                bg: "canvas.default",
                boxShadow: "shadow.large",
                zIndex: 999,
                transition: "left 0.3s ease-in-out",
                display: "block",
              }
            : {
                width: "200px",
                borderRightStyle: "solid",
                borderRightWidth: 1,
                borderRightColor: "border.default",
                display: navSubMode === eNavSubMode.FULL ? "block" : "none",
              }
        }
      >
        <NavSub />
      </Box>

      <Box
        sx={{
          flex: 1,
          // overflowY:"hidden"
        }}
      >
        <Header />
        <Box
          sx={{
            p: ["10px", "10px", 3],
            overflow: "scroll",
            height: window.innerHeight - 80,
            width: [
              "100vw",
              "100vw",
              width - (navSubMode === eNavSubMode.FULL ? 288 : 80),
            ],
          }}
        >
          <PageContent />
        </Box>
      </Box>
      {isOpenNotifyOverlay && (
        <Overlay
          initialFocusRef={confirmButtonRef}
          returnFocusRef={buttonRef}
          ignoreClickRefs={[buttonRef]}
          onEscape={() => {
            dispatch(
              rootAction.common.mainLayoutAction.showNotifyOverlay(false)
            );
          }}
          onClickOutside={() => {
            dispatch(
              rootAction.common.mainLayoutAction.showNotifyOverlay(false)
            );
          }}
          width="auto"
          anchorSide="inside-left"
          right={0}
          position="fixed"
        >
          <Box
            sx={{
              height: "100vh",
              width: "350px",
              display: "flex",
            }}
          >
            <NotifyList />
          </Box>
        </Overlay>
      )}
    </Box>
  );
};

export default MainLayout;
