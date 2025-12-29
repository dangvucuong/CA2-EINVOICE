import React from "react";
import styles from "./Footer.module.css";
import { LocationIcon, MailIcon } from "@primer/octicons-react";

import { Box } from "@primer/react";
const Footer = () => {
  return (
    <Box className={styles.footer}>
      <Box
        sx={{
          p: 2,
          flex: 1,
        }}
        className={styles.block}
      >
        <Box className={styles.info}>
          <Box sx={{ mr: 1 }}>
            <LocationIcon />
          </Box>
          Trụ sở chính: Tầng 3, Tòa nhà Bohemia, Số 25 Nguyễn Huy Tưởng, Phường
          Thanh Xuân, Thành phố Hà Nội, Việt Nam
        </Box>
        <Box className={styles.info}>
          <Box sx={{ mr: 1 }}>
            <LocationIcon />
          </Box>
          Chi nhánh: Tầng 3, số 16 Sông Thao, Phường Tân Sơn Hòa, Thành phố Hồ
          Chí Minh, Việt Nam
        </Box>
      </Box>
      <Box
        sx={{
          p: 2,
          width: "300px",
        }}
        className={styles.block}
      >
        <Box className={styles.info}>
          <Box sx={{ mr: 1 }}>
            <MailIcon />
          </Box>
          Email: support@cavn.vn
        </Box>
        <Box className={styles.info}>
          <Box sx={{ mr: 1 }}>
            <i className="fa-solid fa-phone"></i>
          </Box>
          Hotline: 1900 5454 07
        </Box>
      </Box>
      <Box
        sx={{
          p: 2,
          flex: 1,
        }}
        className={styles.block}
      >
        <Box className={styles.info}>
          <Box sx={{ mr: 1 }}>
            <i className="fa-regular fa-copyright"></i>
          </Box>
          Bản quyền thuộc Công ty Cổ phần Công nghệ thẻ Nacencomm
        </Box>
      </Box>
    </Box>
  );
};

export default Footer;
