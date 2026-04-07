import { QuestionIcon } from "@primer/octicons-react";
import { Box, IconButton } from "@primer/react";
import SignalrConnectionStatus from "../../component-data/signalr-connection-status";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAuth } from "../../hooks/useAuth";
import styles from "./Header.module.css";
import Humberger from "../nav-bar/Humberger";
import LapHoaDonButton from "../../component-ui/lap-hoa-don-button";
const lagugaeIcon = () => {
  return <i className="fa-solid fa-globe"></i>;
};
const Header = () => {
  const { user } = useAuth();
  const dispatch = useAppDispatch();

  return (
    <Box
      className={styles.header}
      sx={{
        borderBottom: 1,
        borderBottomStyle: "solid",
        borderBottomColor: "border.default",
        height: "72px",
        pt: 2,
        pb: 2,
        pl: 3,
        pr: 3,
        display: "flex",
        alignItems: "center",
        // backgroundColor:"canvas.subtle"
      }}
    >
      <Box display={["block", "block", "none"]}>
        <Humberger />
      </Box>
      <Box>
        <p className={styles.name}>{user?.full_name ?? ""}</p>
        <p className={styles.code}>Mã số thuế: {user?.donvi_ma_dv ?? ""}</p>
      </Box>
      <Box
        sx={{
          flex: 1,
        }}
      >
        &nbsp;
      </Box>
      <Box
        sx={{
          display: "flex",
        }}
      >
        <LapHoaDonButton />
        {/* <Box className={styles.item}>
                    <TextInput leadingVisual={SearchIcon}
                        placeholder='Tìm kiếm'
                    >
                    </TextInput>

                </Box> */}

        <Box className={styles.item}>
          <SignalrConnectionStatus />
        </Box>
        {/* <Box className={styles.item}>
                    <LanguageSection />
                </Box> */}
        {/* <Box className={styles.item}>
                    <IconButton icon={BellIcon} aria-label="Bell"
                        onClick={() => {
                            dispatch(rootAction.common.mainLayoutAction.showNotifyOverlay(true))
                        }}
                    />
                </Box> */}
        <Box className={styles.item}>
          <IconButton icon={QuestionIcon} aria-label="Bell" />
        </Box>
      </Box>
    </Box>
  );
};

export default Header;
