import { SignOutIcon, UnverifiedIcon } from "@primer/octicons-react";
import { ActionList, ActionMenu, Text } from "@primer/react";
import { clearAccessToken, clearRefreshToken } from "../../api/apiClient";
import { useAppSelector } from "../../hooks/useAppSelector";
import styles from "./UserPanel.module.css";
import { useHistory } from "react-router-dom";
const UserPanel = () => {
  const { user } = useAppSelector((x) => x.accountReducer);
  const history = useHistory();
  return (
    <div className={styles.userPanel}>
      <div className={styles.avatar}>
        <ActionMenu>
          <ActionMenu.Anchor>
            <div
              style={{
                cursor: "pointer",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                width: "40px",
                height: "40px",
                borderRadius: "50%",
              }}
              className={styles.user_btn}
            >
              <img
                alt="avatar"
                src={`../../images/user.png`}
                width={"40px"}
                height={"40px"}
                style={{
                  borderRadius: "50%",
                }}
              />
            </div>
          </ActionMenu.Anchor>

          <ActionMenu.Overlay id="userpanel_overlay">
            <ActionList>
              <ActionList.Item>
                <Text
                  sx={{
                    fontWeight: "bold",
                  }}
                >
                  {user?.full_name}
                </Text>
                <br />
                <Text>{user?.email}</Text>
              </ActionList.Item>
              <ActionList.Divider />
              <ActionList.Item
                onClick={() => {
                  history.push("../../change-pw");
                }}
              >
                <ActionList.LeadingVisual>
                  <UnverifiedIcon />
                </ActionList.LeadingVisual>
                Đổi mật khẩu
              </ActionList.Item>
              <ActionList.Divider />
              <ActionList.Item
                variant="danger"
                onClick={() => {
                  clearAccessToken();
                  clearRefreshToken();
                  window.location.reload();
                }}
              >
                <ActionList.LeadingVisual>
                  <SignOutIcon />
                </ActionList.LeadingVisual>
                Đăng xuất
              </ActionList.Item>
            </ActionList>
          </ActionMenu.Overlay>
        </ActionMenu>
      </div>
    </div>
  );
};

export default UserPanel;
