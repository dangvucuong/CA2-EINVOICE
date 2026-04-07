import { Box } from "@primer/react";
import AppList from "./AppList";
import Humberger from "./Humberger";
import styles from "./NavBar.module.css";
import UserPanel from "./UserPanel";

const NavBar = () => {
  return (
    <Box
      sx={{
        minHeight: window.innerHeight,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        width: "100%",
      }}
      className={styles.navbar}
    >
      <Box className={styles.humberger}>
        <Humberger />
      </Box>
      <Box
        className={styles.appList}
        sx={{
          height: window.innerHeight - 180,
          overflowY: "auto",
        }}
      >
        <AppList />
      </Box>
      <Box className={styles.userPanel}>
        <UserPanel />
      </Box>
    </Box>
  );
};

export default NavBar;
