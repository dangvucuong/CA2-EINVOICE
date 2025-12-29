import { PencilIcon } from "@primer/octicons-react";
import { Box } from "@primer/react";
import { useMemo, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading";
import { useAuth } from "../../hooks/useAuth";
import DonViEditThongTinLienHeModal from "./DonViEditThongTinLienHeModal";
import styles from "./ThongTinDonvi.module.css";
const ThongTinDonvi = () => {
  const { user } = useAuth();
  const [isShowEditModal, setIsShowEditModal] = useState<boolean>(false);

  const donVi = useMemo(() => {
    if (user) {
      return user.donvi;
    }
  }, [user]);

  return (
    <Box
      sx={
        {
          // width: "400px"
        }
      }
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Box sx={{ flex: 1, display: "flex", flexDirection: "column" }}>
          <Heading text="Thông tin liên hệ với Cơ quan thuế" />
        </Box>
        <Box
          sx={{
            display: "flex",
          }}
        >
          Cơ quan thuế sẽ gửi các thông báo quan trọng liên quan đến hoá đơn
          điện tử tới email/ số điện thoại/ địa chỉ này. Để thay đổi thông tin,
          đơn vị cần lập tờ khai thay đổi thông tin sử dụng HĐĐT gửi CQT và được
          chấp nhận.
        </Box>
        <Box sx={{ ml: -2, display: "flex" }}>
          <Box sx={{ flex: 1, ml: -1 }}>
            <Link to={`../../to-khai`}>
              <Button text="Lập tờ khai" variant="invisible" size="medium" />
            </Link>
          </Box>

          <Button
            text="Điều chỉnh thông tin liên hệ"
            variant="invisible"
            leadingVisual={PencilIcon}
            size="medium"
            onClick={() => {
              setIsShowEditModal(true);
            }}
          />
        </Box>
        <Box>
          <table>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Đơn vị</td>
              <td>
                <b>{donVi?.ten_dv}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Mã số thuế</td>
              <td>
                <b>{donVi?.mst}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Điện thoại</td>
              <td>
                <b>{donVi?.dien_thoai}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Email</td>
              <td>
                <b>{donVi?.email}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Website</td>
              <td>
                <b>{donVi?.website}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Địa chỉ</td>
              <td>
                <b>{donVi?.dia_chi}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>CQT chủ quản</td>
              <td>
                <b>{donVi?.donvi_chuquan}</b>
              </td>
            </tr>
            <tr className={styles.tr}>
              <td className={styles.labelTd}>Số tài khoản</td>
              <td>
                <b>
                  {donVi?.stk} - {donVi?.ngan_hang}
                </b>
              </td>
            </tr>
          </table>
        </Box>
      </Box>
      {isShowEditModal && donVi && (
        <DonViEditThongTinLienHeModal
          donVi={donVi}
          onCancel={() => {
            setIsShowEditModal(false);
          }}
          onSuccess={() => {
            setIsShowEditModal(false);
            window.location.reload();
          }}
        />
      )}
    </Box>
  );
};

export default ThongTinDonvi;
