import { Redirect, Route, Switch } from "react-router-dom";
import unAuthorizedRouter from "../../layouts/page/unAuthorizedRouter";
import styles from "./UnAuthorizedContent.module.css";
import { Box } from "@primer/react";
import Heading from "../../component-ui/heading/Heading";
import { eSize } from "../../models/commons/eSize";
import Footer from "../../pages/login/Footer";
import Text from "../../component-ui/text";
import GioiThieuSanPham from "./GioiThieuSanPham";
import ThongBaoHeThong from "./ThongBaoHeThong";
const UnAuthorizedContent = () => {
  const defaultPage = () => {
    return "/login";
  };
  return (
    <div>
      <Box
        sx={{
          display: "flex",
          height: window.innerHeight,
          flexDirection: "column",
          overflowY: ["auto", "auto", "hidden"],
        }}
      >
        <Box
          sx={{
            height: window.innerHeight - 72,
            background: "url('../../images/login-bg.png')",
            backgroundSize: "cover",
            backgroundRepeat: "no-repeat",
            display: "flex",
            mt: ["60px", "60px", "0px"],
            mb: ["40px", "40px", "0px"],
          }}
        >
          <Box sx={{ flex: 1, display: ["none", "none", "block"] }}>
            <Heading
              size={eSize.large}
              sx={{
                whiteSpace: "pre-line",
                textAlign: "center",
                color: "#00579B",
                marginTop: "74px",
              }}
              text={`Phần mềm hóa đơn điện tử \n Ca2 E-invoice`}
            />
            <GioiThieuSanPham />
            {/* <ThongBaoHeThong /> */}
          </Box>

          <Box
            sx={{
              flex: 1,
            }}
            className={styles.loginFormContent}
          >
            <Box>
              <img
                src="../../images/logo.svg"
                alt="Logo"
                style={{ height: "40px" }}
              />
            </Box>

            <Box
              sx={{
                minWidth: ["300px", "350px"],
                textAlign: "center",
              }}
            >
              <Switch>
                {unAuthorizedRouter.map(({ path, component }) => (
                  <Route strict key={path} path={path} component={component} />
                ))}
                <Redirect to={defaultPage()} />
              </Switch>
            </Box>
          </Box>
        </Box>
        <Footer />
      </Box>
    </div>
  );
};

export default UnAuthorizedContent;
