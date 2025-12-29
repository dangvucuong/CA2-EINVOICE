import { Box } from "@primer/react";
import { useEffect } from "react";
import AppButton from "../../component-ui/app-button";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";

const AppList = () => {
  const { status, subSystems } = useAppSelector((x) => x.user.subSystemReducer);
  const { user } = useAuth();
  const menus = user?.menus ?? [];

  const dispatch = useAppDispatch();
  useEffect(() => {
    if (status === eReducerStatusBase.is_not_initialization) {
      dispatch(rootAction.user.subSystemAction.loadStart());
    }
  }, [status]);
  return (
    <Box
      sx={{
        width: "100%",
      }}
    >
      {menus.map((menu) => {
        return <AppButton key={menu.id} app={menu} />;
      })}
    </Box>
  );
};

export default AppList;
