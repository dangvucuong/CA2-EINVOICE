import {
  DownloadIcon,
  ShieldCheckIcon,
  ChecklistIcon,
  ChevronLeftIcon,
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
import axios from "axios";
import { appInfo } from "../../AppInfo";
import { useHistory } from "react-router-dom";
interface IToKhaiViewProps {
  id: number;
  status?: number;
}
const ToKhaiView = (props: IToKhaiViewProps) => {
  const [htmlData, setHtmlData] = useState<string>("");
  const [htmlDataKetQua, setHtmlDataKetQua] = useState<string>("");
  const history = useHistory();

  const [isLoading, setIsLoading] = useState(false);
  const [isShowLoading] = useDebounce(isLoading, 300);
  const [isExporting, setIsExporting] = useState(false);
  const [mode, setMode] = useState<"info" | "ket-qua">("info");

  const { user } = useAuth();
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref

  useEffect(() => {
    if (props.id > 0) {
      if (mode === "info") {
        if (htmlData === "") {
          handlePrintAsync();
        }
      }
      if (mode === "ket-qua") {
        if (htmlDataKetQua === "") {
          handlePrintKetQuaAsync();
        }
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.id, mode, htmlData, htmlDataKetQua]);
  const handlePrintAsync = async () => {
    setIsLoading(true);
    const res = await toKhaiApi.getHtmlPrint(props.id);
    if (res.is_success) {
      let modifiedHtml = res.data;
      if (props?.status === 1) {
        modifiedHtml = modifiedHtml.replace(/paramSign/g, "display:none");
      }

      setHtmlData(modifiedHtml);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };
  const handlePrintKetQuaAsync = async () => {
    setIsLoading(true);
    const res = await toKhaiApi.getHtmlKetQuaPrint(props.id);
    if (res.is_success) {
      setHtmlDataKetQua(res.data);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };

  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {},
  });

  const handleExportWithFunction = async () => {
    setIsExporting(true);
    const endpoint =
      mode === "info"
        ? `${appInfo.baseApiURL}/to-khai/${props.id}/pdf`
        : `${appInfo.baseApiURL}/to-khai/${props.id}/pdf/ket-qua`;

    const response = await axios.get(
      endpoint,
      // {
      //     ...filter,
      //     tu_ngay: filter.tu_ngay === "" ? undefined : filter.tu_ngay,
      //     den_ngay: filter.den_ngay === "" ? undefined : filter.den_ngay,
      // },
      {
        headers: {
          Authorization: `Bearer ${localStorage.access_token}`,
          language: localStorage.getItem("language"),
        },
        responseType: "blob", // Important for handling binary data
      }
    );

    // Create a URL for the file blob
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", "ToKhai.pdf"); // File name
    document.body.appendChild(link);
    link.click();
    link.remove();
    setIsExporting(false);
  };

  return (
    <Box>
      <Button
        leadingVisual={ChevronLeftIcon}
        size="large"
        variant="invisible"
        text="Quay lại"
        sx={{
          backgroundColor: "#fff!important",
        }}
        onClick={() => {
          history.push("/to-khai");
        }}
      />
      <Box
        sx={{
          textAlign: "center",
        }}
      >
        <SegmentedControl
          aria-label="File view"
          onChange={(index) => {
            if (index === 0) {
              setMode("info");
            }
            if (index === 1) {
              setMode("ket-qua");
            }
          }}
          size={"small"}
        >
          <SegmentedControl.Button
            selected={mode === "info"}
            aria-label={"Preview"}
            leadingIcon={ChecklistIcon}
          >
            Thông tin tờ khai
          </SegmentedControl.Button>
          <SegmentedControl.Button
            selected={mode === "ket-qua"}
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
            {mode === "info" && (
              <Box
                id="htmlView"
                dangerouslySetInnerHTML={{ __html: htmlData }}
                ref={contentRef}
              />
            )}
            {mode === "ket-qua" && (
              <Box
                id="htmlView"
                dangerouslySetInnerHTML={{ __html: htmlDataKetQua }}
                ref={contentRef}
              />
            )}
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
                  text="In"
                  onClick={handlePrint}
                  variant="invisible"
                  size="medium"
                  leadingVisual={PrintIcon}
                />

                <Button
                  text="Tải xuống PDF"
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

                {/* <Button text='Export' onClick={() => {
                                    handleExportWithFunction();

                                }}
                                    variant='invisible'
                                    size='medium'
                                    leadingVisual={DownloadIcon}

                                /> */}
              </Box>
            )}
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default ToKhaiView;
