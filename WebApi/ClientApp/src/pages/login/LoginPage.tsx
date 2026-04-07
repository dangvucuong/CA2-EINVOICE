import { Box } from "@primer/react";

import { UnderlineNav } from "@primer/react";
import { useEffect, useState } from "react";
import { Helmet } from "react-helmet";
import { Link, useHistory, useLocation } from "react-router-dom";
import Heading from "../../component-ui/heading";
import LoginMST from "./LoginMST";
import LoginPW from "./LoginPW";
import LoginRS from "./LoginRS";
import Text from "../../component-ui/text";
import Button from "../../component-ui/button";

const cardStyle = {
  flex: 1,
  fontSize: 18,
  borderRadius: "12px",
  padding: "20px",
  textAlign: "center",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  cursor: "pointer",
  fontWeight: "bold",
  background: "linear-gradient(135deg, #f9f9f9, #f1f1f1)",
  transition: "all 0.3s ease",
  boxShadow: "0 2px 6px rgba(0,0,0,0.08)",
  "&:hover": {
    background: "linear-gradient(135deg, #ffffff, #f7f7f7)",
    transform: "translateY(-4px)",
    boxShadow: "0 6px 12px rgba(0,0,0,0.15)",
    color: "red",
  },
};

const LoginPage = () => {
  const [loginMode, setLoginMode] = useState<"mst" | "pw" | "rs">("pw");
  const location = useLocation<any>();
  const [showLoginForm, setShowLoginForm] = useState(
    // () => !!location.state?.showLoginForm
    true
  );

  const history = useHistory();

  // Clear state sau khi khởi tạo state trong component
  // useEffect(() => {
  //   if (location.state?.showLoginForm) {
  //     history.replace({ ...location, state: {} });
  //   }
  // }, [location, history]);

  return (
    <>
      <Box sx={{ mt: 4, ml: -3, mr: -3 }}>
        <Helmet>
          <title>Đăng nhập</title>
        </Helmet>
        <Heading text="Đăng nhập" sx={{ mb: 3 }} />
        {showLoginForm ? (
          <Box>
            <UnderlineNav
              aria-label="Repository"
              sx={{
                width: "100%",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <UnderlineNav.Item
                href="#"
                onClick={() => {
                  setLoginMode("mst");
                }}
                aria-current={loginMode === "mst" ? "page" : undefined}
                sx={{
                  flex: 1,
                }}
              >
                Serial
              </UnderlineNav.Item>
              <UnderlineNav.Item
                href="#"
                onClick={() => {
                  setLoginMode("rs");
                }}
                aria-current={loginMode === "rs" ? "page" : undefined}
                sx={{
                  flex: 1,
                }}
              >
                Remote Signing
              </UnderlineNav.Item>
              <UnderlineNav.Item
                href="#"
                onClick={() => {
                  setLoginMode("pw");
                }}
                sx={{
                  flex: 1,
                }}
                aria-current={loginMode === "pw" ? "page" : undefined}
              >
                Mật khẩu
              </UnderlineNav.Item>
            </UnderlineNav>

            <Box
              sx={{
                mt: 3,
              }}
            >
              {loginMode === "mst" && (
                <>
                  <LoginMST />
                </>
              )}
              {loginMode === "rs" && <LoginRS />}
              {loginMode === "pw" && (
                <>
                  <LoginPW
                    setShowLoginForm={(data) => {
                      setShowLoginForm(data);
                    }}
                  />
                </>
              )}
            </Box>
          </Box>
        ) : (
          <Box sx={{ display: "flex", gap: 4, my: 8 }}>
            <Box sx={cardStyle} onClick={() => setShowLoginForm(true)}>
              <img
                src="../../images/doanh_nghiep.jpg"
                alt="doanh_nghiep"
                style={{
                  height: "50px",
                }}
              />
              Doanh nghiệp
            </Box>
            <Box
              sx={cardStyle}
              onClick={() => {
                window.open("https://ca2einv-hkd.nacencomm.vn", "_self");
              }}
            >
              <img
                src="../../images/hkd.jpg"
                alt="doanh_nghiep"
                style={{
                  height: "50px",
                }}
              />
              Hộ kinh doanh
            </Box>
            <Box
              sx={{
                ...cardStyle,
                opacity: 0.6,
                "&:hover": {
                  background: "linear-gradient(135deg, #fafafa, #f2f2f2)",
                  transform: "none",
                  boxShadow: "0 2px 6px rgba(0,0,0,0.05)",
                  color: "inherit",
                },
              }}
            >
              <img
                src="../../images/ca_nhan.jpg"
                alt="doanh_nghiep"
                style={{
                  height: "50px",
                }}
              />
              Cá nhân
            </Box>
          </Box>
        )}

        <Box>
          <UnderlineNav
            aria-label="Others"
            sx={{
              width: "100%",
              alignItems: "center",
              justifyContent: "center",
              borderBottom: "none",
            }}
          >
            <UnderlineNav.Item
              href="https://hsdt.nacencomm.vn/downloads/setup.msi"
              target="_blank"
              sx={{
                flex: 1,
              }}
            >
              Bộ cài ký số
            </UnderlineNav.Item>
            <Link
              to={"../../tra-cuu"}
              style={{
                textDecoration: "none",
              }}
            >
              <UnderlineNav.Item
                href="#"
                sx={{
                  flex: 1,
                }}
              >
                Tra cứu
              </UnderlineNav.Item>
            </Link>
            <Link
              to={"../../register"}
              style={{
                textDecoration: "none",
              }}
            >
              <UnderlineNav.Item
                href="#"
                sx={{
                  flex: 1,
                }}
              >
                Đăng ký
              </UnderlineNav.Item>
            </Link>
          </UnderlineNav>
        </Box>
      </Box>
    </>
  );
};

export default LoginPage;
