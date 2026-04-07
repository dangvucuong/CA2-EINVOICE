import {
  DownloadIcon,
  ShieldCheckIcon,
  ChecklistIcon,
} from "@primer/octicons-react";
import { Box, SegmentedControl } from "@primer/react";
import { useEffect, useRef, useState } from "react";
import { useReactToPrint } from "react-to-print";
import { useDebounce } from "use-debounce";
import { toKhaiApi } from "../../api/to-khai/toKhaiApi";
import Button from "../../component-ui/button/Button";
import PrintIcon from "../../component-ui/icon/print";
import PlaceHolder from "../../component-ui/place-holder";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { appInfo } from "../../AppInfo";
interface IToKhaiCTViewProps {
  matokhaiCT: string;
}
const ToKhaiCTView = (props: IToKhaiCTViewProps) => {
  const { matokhaiCT } = props;
  const [htmlData, setHtmlData] = useState<string>("");

  const [isLoading, setIsLoading] = useState(false);
  const [isShowLoading] = useDebounce(isLoading, 300);
  const [isExporting, setIsExporting] = useState(false);
  const [mode, setMode] = useState<number>(5); // 5 xem tờ khai, 6 xem kết quả

  const { user } = useAuth();
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  useEffect(() => {
    if (matokhaiCT) {
      XemThongDiep(matokhaiCT);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [matokhaiCT, mode]);

  const XemThongDiep = async (matokhaiCT: string | undefined) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemThongDiep xmlns="http://tempuri.org/">
      <MatokhaiCT>${matokhaiCT}</MatokhaiCT>
      <madonvi>${user?.donvi_ma_dv}</madonvi>
      <type>${mode}</type>
    </XemThongDiep>
  </soap12:Body>
</soap12:Envelope>`;
    setIsLoading(true);

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );
    setIsLoading(false);

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setHtmlData(cleanHtml(parseRes.data));
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  function cleanHtml(input: string): string {
    // Ép input thành string cho chắc
    const html = String(input);

    // Tìm đoạn nguyên khối từ <html ...> đến </html>
    const match = html.match(/<html[\s\S]*<\/html>/i);

    return match ? match[0] : html;
  }

  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {},
  });

  const handleExportWithFunction = async () => {
    setIsExporting(true);
    const endpoint = `${appInfo.baseApiURL}/hoa-don/pdf/from-html`;

    const response: any = await axiosClient.post(
      endpoint,
      {
        html: htmlData, // nội dung HTML cần in
        file_name: mode === 5 ? "Tờ Khai" : "Kết quả", // tên file xuất ra
      },
      {
        headers: {
          Authorization: `Bearer ${localStorage.access_token}`,
          language: localStorage.getItem("language"),
        },
        responseType: "blob", // Important for binary data
      }
    );

    // Create a URL for the file blob
    const url = window.URL.createObjectURL(response);
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", mode === 5 ? "ToKhai.pdf" : "KetQua.pdf");
    document.body.appendChild(link);
    link.click();
    link.remove();

    setIsExporting(false);
  };
  return (
    <Box>
      <Box
        sx={{
          textAlign: "center",
        }}
      >
        <SegmentedControl
          aria-label="File view"
          onChange={(index) => {
            if (index === 0) {
              setMode(5);
            }
            if (index === 1) {
              setMode(6);
            }
          }}
          size={"small"}
        >
          <SegmentedControl.Button
            selected={mode === 5}
            aria-label={"Preview"}
            leadingIcon={ChecklistIcon}
          >
            Thông tin tờ khai
          </SegmentedControl.Button>
          <SegmentedControl.Button
            selected={mode === 6}
            aria-label={"Raw"}
            leadingIcon={ShieldCheckIcon}
          >
            Kết quả phản hồi
          </SegmentedControl.Button>
        </SegmentedControl>
      </Box>
      {isShowLoading && <PlaceHolder line_number={10} />}
      {!isShowLoading && (
        <Box>
          <Box
            sx={{
              flex: 1,
              p: 3,
              justifyContent: "center",
              display: "flex",
            }}
          >
            <Box
              id="htmlView"
              dangerouslySetInnerHTML={{ __html: htmlData }}
              ref={contentRef}
            />
          </Box>
          <Box
            sx={{
              display: "flex",
              position: "sticky",
              bottom: user ? "-100px" : "-16px",
              alignItems: "center",
              backgroundColor: "#f6f8fa",
              zIndex: 1000,
              p: 3,
              m: -3,
              mt: 3,
              // mb: -4
            }}
          >
            {htmlData && (
              <Box sx={{ display: "flex", flex: 1 }}>
                <Button
                  text="In tờ khai"
                  onClick={handlePrint}
                  variant="invisible"
                  size="medium"
                  leadingVisual={PrintIcon}
                />

                <Button
                  text="Tải xuống"
                  isLoading={isExporting}
                  onClick={() => {
                    // setIsShowPaging(false);
                    setTimeout(() => {
                      handleExportWithFunction();
                    }, 300);
                  }}
                  variant="invisible"
                  size="medium"
                  leadingVisual={DownloadIcon}
                />
              </Box>
            )}
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default ToKhaiCTView;
