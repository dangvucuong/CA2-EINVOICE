import { SearchIcon, DownloadIcon } from "@primer/octicons-react";
import { Box, Flash, TextInput } from "@primer/react";
import { useRef, useState } from "react";
import { Helmet } from "react-helmet";
import { Link } from "react-router-dom";
import { axiosClient } from "../../api/axiosClient";
import { appInfo } from "../../AppInfo";
import Button from "../../component-ui/button";
import { PrintIcon } from "../../component-ui/icon";
import { parseSoapResponse } from "../../helpers/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import styles from "../tra-cuu/TraCuuPage.module.css";

const escapeXml = (value: string) =>
  value
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&apos;");

const TraCuuChungTuPage = () => {
  const { user } = useAuth();
  const [maTraCuu, setMaTraCuu] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isExporting, setIsExporting] = useState(false);
  const [htmlContent, setHtmlContent] = useState<string>("");
  const [notFound, setNotFound] = useState(false);
  const contentRef = useRef<HTMLDivElement>(null);

  const handleSearch = async () => {
    const key = maTraCuu.trim();
    if (!key) {
      NotifyHelper.Error("Vui lòng nhập mã tra cứu");
      return;
    }

    setIsLoading(true);
    setNotFound(false);
    setHtmlContent("");
    try {
      const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemChungTuTheoMaTraCuu xmlns="http://tempuri.org/">
      <matracuu>${escapeXml(key)}</matracuu>
    </XemChungTuTheoMaTraCuu>
  </soap12:Body>
</soap12:Envelope>`;

      const res: string = await axiosClient.post(
        process.env.REACT_APP_API_CHUNG_TU as string,
        soap,
        {
          headers: {
            "Content-Type": "text/xml; charset=utf-8",
          },
        }
      );

      const parseRes = parseSoapResponse(res);
      if (parseRes?.status === "success" && parseRes?.data) {
        setHtmlContent(parseRes.data);
        setNotFound(false);
      } else {
        setHtmlContent("");
        setNotFound(true);
        NotifyHelper.Error(parseRes?.message ?? "Không tìm thấy chứng từ");
      }
    } catch (e: any) {
      setHtmlContent("");
      setNotFound(true);
      NotifyHelper.Error(e?.message ?? "Không thể tra cứu chứng từ");
    } finally {
      setIsLoading(false);
    }
  };

  const handlePrint = () => {
    if (!contentRef.current) return;
    const printWindow = window.open("", "_blank");
    if (!printWindow) {
      NotifyHelper.Error("Trình duyệt đang chặn cửa sổ in");
      return;
    }
    printWindow.document.write(contentRef.current.innerHTML);
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
  };

  const handleExportPdf = async () => {
    if (!htmlContent) return;
    setIsExporting(true);
    try {
      const endpoint = `${appInfo.baseApiURL}/hoa-don/pdf/from-html`;
      const response: any = await axiosClient.post(
        endpoint,
        {
          html: htmlContent,
          file_name: `ChungTu_${maTraCuu.trim()}`,
        },
        {
          responseType: "blob",
        }
      );
      const url = window.URL.createObjectURL(response);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `ChungTu_${maTraCuu.trim()}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch (e: any) {
      NotifyHelper.Error(e?.message ?? "Tải PDF thất bại");
    } finally {
      setIsExporting(false);
    }
  };

  return (
    <Box
      sx={{
        height: window.innerHeight,
        overflowY: "auto",
      }}
    >
      <Helmet>
        <title>Tra cứu chứng từ</title>
      </Helmet>
      {!user && (
        <Box
          id="header"
          className={styles.navsub}
          sx={{
            height: "72px",
            display: "flex",
            alignItems: "center",
          }}
        >
          <Box sx={{ ml: 3 }}>
            <img
              alt="logo"
              src="../../images/logo-white.svg"
              height={"40px"}
            />
          </Box>
          <Box sx={{ flex: 1 }} />
          <Box
            sx={{
              mr: 3,
              display: "flex",
            }}
          >
            <Link to="/login">
              <Button text="Đăng nhập" size="medium" sx={{ mr: 1 }} />
            </Link>
            <Link to="/register">
              <Button text="Đăng ký" size="medium" />
            </Link>
          </Box>
        </Box>
      )}
      <Box sx={{ p: 3 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            width: "100%",
          }}
        >
          <Box sx={{ display: "flex" }}>
            <TextInput
              leadingVisual={SearchIcon}
              placeholder="Tìm kiếm theo mã tra cứu"
              value={maTraCuu}
              width={320}
              onChange={(e) => {
                setMaTraCuu(e.target.value);
              }}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  handleSearch();
                }
              }}
            />
            <Button
              text="Tìm kiếm"
              variant="primary"
              size="medium"
              sx={{ ml: 1 }}
              isLoading={isLoading}
              onClick={handleSearch}
            />
          </Box>
        </Box>

        {htmlContent && (
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              width: "100%",
              mt: 3,
              mb: 2,
              gap: 2,
            }}
          >
            <Button
              text="In chứng từ"
              onClick={handlePrint}
              variant="invisible"
              size="medium"
              leadingVisual={PrintIcon}
            />
            <Button
              text="Tải PDF"
              isLoading={isExporting}
              onClick={handleExportPdf}
              variant="invisible"
              size="medium"
              leadingVisual={DownloadIcon}
            />
          </Box>
        )}

        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            width: "100%",
            mt: 3,
          }}
        >
          {htmlContent ? (
            <div
              ref={contentRef}
              style={{ display: "flex", justifyContent: "center" }}
              dangerouslySetInnerHTML={{ __html: htmlContent }}
            />
          ) : notFound ? (
            <Flash variant="default">
              Không tìm thấy chứng từ có mã tra cứu trùng khớp
            </Flash>
          ) : null}
        </Box>
      </Box>
    </Box>
  );
};

export default TraCuuChungTuPage;
