import { Box } from "@primer/react";
import React from "react";
import Text from "../../component-ui/text";
import { Link, useLocation } from "react-router-dom";
import { CheckCircleIcon } from "@primer/octicons-react";

function GioiThieuSanPham() {
  const location = useLocation();

  if (!location.pathname.includes("/register")) {
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
          <Text text="Giới thiệu"></Text>
        </Box>
        {/* Giới thiệu */}
        <Box sx={{ mb: 4 }}>
          <Text
            sx={{
              display: "block",
              fontWeight: 500,
              mb: 2,
            }}
            text="Công ty cổ phần Công nghệ thẻ Nacencomm là đơn vị tiên phong
                  trong lĩnh vực chữ ký số. Được thành lập năm 1996, Nacencomm
                  với trách nhiệm, uy tín và tận tâm mang đến sứ mệnh trở thành
                  đơn vị cung cấp nền tảng doanh nghiệp số hàng đầu."
          ></Text>

          <Text
            sx={{ display: "block" }}
            text="Phủ sóng và hiện diện trên khắp 63 tỉnh thành, Nacencomm cung
                  cấp hơn 20 sản phẩm, dịch vụ và giải pháp. Chúng tôi luôn sẵn
                  sàng đồng hành cùng doanh nghiệp phát triển bền vững, không
                  ngừng sáng tạo giá trị mới cho khách hàng, đối tác và cộng
                  đồng."
          ></Text>
        </Box>

        {/* Ba cột lợi thế */}
        <Box
          sx={{
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "center",
            textAlign: "center",
            gap: "24px",
            mb: 4,
          }}
        >
          {[
            {
              title: "An toàn nhất",
              desc: "Lấy yếu tố bảo mật là trọng điểm, sản phẩm và giải pháp của chúng tôi luôn đảm bảo tuyệt đối mọi thủ tục, chính sách và pháp lý.",
              icon: <CheckCircleIcon size={24} />,
            },
            {
              title: "Dễ sử dụng",
              desc: "Chúng tôi mang công nghệ tiếp cận khách hàng với phương châm đem lại trải nghiệm tốt nhất, thân thiện và dễ dàng nhất.",
            },
            {
              title: "Hỗ trợ tốt nhất",
              desc: "Chúng tôi không ngừng nỗ lực, liên tục cải tiến để khách hàng cảm thấy hài lòng và có ấn tượng tốt nhất.",
            },
          ].map((item) => (
            <Box
              key={item.title}
              sx={{
                width: ["100%", "calc(33.333% - 16px)"], // 👈 3 box bằng nhau
                backgroundColor: "#fff",
                borderRadius: "12px",
                p: 3,
                boxShadow: "0 2px 8px rgba(0,0,0,0.05)",
                border: "1px solid #f0f0f0",
                display: "flex",
                flexDirection: "column",
                justifyContent: "flex-start",
                alignItems: "center",
                transition: "all 0.2s ease",
              }}
            >
              <Text
                sx={{
                  color: "#de3f0f",
                  fontWeight: "bold",
                  fontSize: "16px",
                  mb: 2,
                  display: "block",
                }}
                text={item.title}
              />
              <Text
                sx={{ color: "#555", lineHeight: "1.6" }}
                text={item.desc}
              />
            </Box>
          ))}
        </Box>

        {/* Sản phẩm */}
        <Box sx={{ textAlign: "center" }}>
          <Text
            sx={{
              color: "#de3f0f",
              fontWeight: "bold",
              fontSize: "18px",
              display: "block",
            }}
            text="Sản phẩm của chúng tôi"
          ></Text>

          <Box sx={{ my: 2 }}>
            <a
              href="https://nacencomm.vn/product/chu-ky-so-usb-token"
              target="_blank"
              rel="noopener noreferrer"
              style={{
                display: "inline-block",
                padding: "10px 20px",
                backgroundColor: "#00579B",
                color: "#fff",
                borderRadius: "6px",
                textDecoration: "none",
                fontWeight: "bold",
                boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
              }}
            >
              Nhận báo giá
            </a>
          </Box>

          <Box
            as="img"
            src="../../images/sanphamcuachungtoi.png"
            alt="Sản phẩm của chúng tôi"
            sx={{
              width: "100%",
              maxWidth: "900px",
              borderRadius: "12px",
              margin: "0 auto",
              boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
            }}
          />
        </Box>
      </Box>
    </Box>
  );
}

export default GioiThieuSanPham;
