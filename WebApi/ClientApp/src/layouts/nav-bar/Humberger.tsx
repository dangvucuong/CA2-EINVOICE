import { Box } from "@primer/react";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eNavSubMode } from "../../models/commons/eNavSubMode";
import { rootAction } from "../../state/actions/rootAction";
import { ThreeBarsIcon } from "@primer/octicons-react";
import styles from "./Humberger.module.css";

const Humberger = () => {
  const dispatch = useAppDispatch();
  const { navSubMode } = useAppSelector((x) => x.common.mainLayoutReducer);
  const handleClick = () => {
    const mode =
      navSubMode === eNavSubMode.FULL ? eNavSubMode.POPUP : eNavSubMode.FULL;
    dispatch(rootAction.common.mainLayoutAction.changeNavSubMode(mode));
  };
  return (
    <Box className={styles.container} onClick={handleClick}>
      <Box className={styles.humberger}>
        <ThreeBarsIcon />
      </Box>
    </Box>
  );
};

export default Humberger;
