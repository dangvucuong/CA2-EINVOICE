import { Box } from "@primer/react";
import ChartLichSuPhatHanh from "./ChartLichSuPhatHanh";
import ChartSoLuongHoaDon from "./ChartSoLuongHoaDon";
import ChartThongKeTrangThai from "./ChartThongKeTrangThai";
import styles from "./DashBoardPage.module.css";
import ThongTinCQTChuQuan from "./ThongTinCQTChuQuan";
import ThongTinDonvi from "./ThongTinDonvi";
import ThongTinThanhToan from "./ThongTinThanhToan";
import ThongTinCKS from "./ThongTinCKS";
const DashBoardPage = () => {
  return (
    <Box className="row" sx={{ pl: 2 }}>
      <Box className="col-lg-8 col-sm-12" sx={{ p: 1 }}>
        <Box className="row">
          <Box className="col-lg-6 col-md-12 col-sm-12" sx={{ p: 1 }}>
            <Box id="chart_soluong" className={styles.boxComponent}>
              <ChartSoLuongHoaDon />
            </Box>
          </Box>
          <Box className="col-lg-6 col-md-12 col-sm-12" sx={{ p: 1 }}>
            <Box id="chart_phathanh" className={styles.boxComponent}>
              <ChartThongKeTrangThai />
            </Box>
          </Box>
          <Box className="col-lg-12" sx={{ p: 1 }}>
            <Box id="lich_su_phat_hanh" className={styles.boxComponent}>
              <ChartLichSuPhatHanh />
            </Box>
          </Box>
        </Box>
      </Box>
      <Box className="col-lg-4 col-sm-12" sx={{ p: 1 }}>
        <Box id="chart_donvi" className={styles.boxComponent}>
          <ThongTinDonvi />
        </Box>
        {/* <Box id="chart_donvi" className={styles.boxComponent}>
          <ThongTinThanhToan />
        </Box>
        <Box id="chart_donvi" className={styles.boxComponent}>
          <ThongTinCQTChuQuan />
        </Box> */}
        <Box id="chart_donvi" className={styles.boxComponent}>
          <ThongTinCKS />
        </Box>
      </Box>
    </Box>
  );
};

export default DashBoardPage;
