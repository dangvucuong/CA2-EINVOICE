import { Box } from "@primer/react";
import React from "react";
import Text from "../../component-ui/text";
import { Link, useLocation } from "react-router-dom";
import { CheckCircleIcon } from "@primer/octicons-react";

function ThongBaoHeThong() {
  const location = useLocation();

  if (!location.pathname.includes("/login")) {
    return <></>;
  }

  return (
    <Box
      sx={{
        backgroundColor: "#fff",
        width: "90%",
        margin: "24px auto 0 auto",
        borderRadius: "12px",
        boxShadow: "0 4px 16px rgba(0, 0, 0, 0.08)",
        fontSize: "15px",
        lineHeight: "1.7",
        color: "#333",
        overflow: "hidden", // 👈 quan trọng
      }}
    >
      <Box
        sx={{
          overflowY: "auto",
          maxHeight: "calc(100vh - 300px)",
          p: 2,
          padding: "16px",
        }}
      >
        <Box
          sx={{
            textAlign: "center",
            mb: 2,
            color: "#de3f0f",
            fontWeight: "bold",
            fontSize: "18px",
          }}
        >
          <Text text="THÔNG BÁO VỀ VIỆC CHUYỂN ĐỔI MẪU HOÁ ĐƠN NĂM 2026"></Text>
        </Box>
        {/* Giới thiệu */}
        <Box sx={{ mb: 4 }}>
          <Text
            sx={{
              display: "block",
              fontWeight: 500,
              mb: 2,
              whiteSpace: "pre-line",
            }}
            text={`Kính gửi Quý khách hàng,
Căn cứ theo điều 10 nghị định 123/2020/NĐ-CP và điều 4 thông tư 78/2021/TT-BTC, quy định về ký hiệu, số hoá đơn/chứng từ như sau:

Số hoá đơn bắt đầu từ số 1 vào ngày 01/01 hoặc ngày bắt đầu sử dụng hoá đơn và kết thúc vào ngày 31/12 hoặc ngày cuối cùng của năm dương lịch.

Vì vậy, để đảm bảo tuân thủ quy định quản lý Thuế, vào lúc 00h00 ngày 01/01/2026 hệ thống Hoá đơn điện tử sẽ bắt đầu quy trình tự động thực hiện tạo mẫu hóa đơn với ký hiệu mới. 
Cụ thể như sau:

👉 Thực hiện tạo mẫu hóa đơn mới tương ứng cho tất cả mẫu hóa đơn năm 2026, với quy tắc:

✓ Mẫu số hóa đơn: Giữ nguyên

✓ Kí hiệu hóa đơn: Chỉ thay đổi phần thể hiện năm: từ 25 sang 26

Ví dụ: Mẫu 1C25TAA sẽ tạo mẫu mới là 1C26TAA

👉 Các hóa đơn xuất nháp của mẫu 25, chưa cấp số hóa đơn (số hóa đơn đang là 0000000): Sẽ được chuyển sang mẫu hóa đơn 26

👉 Số lượng đăng ký ban đầu của mẫu hóa đơn mới: Là số lượng chưa sử dụng của mẫu năm trước (mẫu năm 2025)

👉 Việc xuất hóa đơn mới có ngày hóa đơn từ ngày 01/01/2026 bắt buộc theo quy định sau:

✓ Mẫu 2026: Được áp dụng

✓ Mẫu 2025: Bị ngừng (không được sử dụng)`}
          ></Text>
        </Box>
      </Box>
    </Box>
  );
}

export default ThongBaoHeThong;
