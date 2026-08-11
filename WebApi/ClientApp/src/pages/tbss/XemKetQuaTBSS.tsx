import React, { useEffect, useRef, useState } from "react";
import Modal from "../../component-ui/modal";
import { axiosClient } from "../../api/axiosClient";
import { NotifyHelper } from "../../helpers/toast";
import { useReactToPrint } from "react-to-print";
import { appInfo } from "../../AppInfo";
import { Box } from "@primer/react";
import Button from "../../component-ui/button";
import { PrintIcon } from "../../component-ui/icon";
import { DownloadIcon } from "@primer/octicons-react";
import { thongBaoSaiSotApi } from "../../api/tbss/thongBaoSaiSotApi";

function XemKetQuaTBSS({
  isOpen,
  onClose,
  tbssId,
}: {
  isOpen: boolean;
  onClose: () => void;
  tbssId: number;
}) {
  const [html, setHtml] = useState<string>("");
  const contentRef = useRef<HTMLDivElement>(null);
  const [isExporting, setIsExporting] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (tbssId) {
      loadKetQuaAsync(tbssId);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tbssId]);

  const loadKetQuaAsync = async (id: number) => {
    setIsLoading(true);
    const res = await thongBaoSaiSotApi.getHtmlKetQua(id);
    if (res.is_success) {
      setHtml(cleanHtml(res.data));
    } else {
      NotifyHelper.Error(res.message || "Không tải được kết quả");
    }
    setIsLoading(false);
  };

  function cleanHtml(input: string): string {
    const htmlContent = String(input);
    const match = htmlContent.match(/<html[\s\S]*<\/html>/i);
    return match ? match[0] : htmlContent;
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
        html: html,
        file_name: "Kết quả",
      },
      {
        headers: {
          Authorization: `Bearer ${localStorage.access_token}`,
          language: localStorage.getItem("language"),
        },
        responseType: "blob",
      }
    );

    const url = window.URL.createObjectURL(response);
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", "KetQuaTBSS.pdf");
    document.body.appendChild(link);
    link.click();
    link.remove();

    setIsExporting(false);
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Xem kết quả" width="1200px">
      {html && (
        <Box
          sx={{
            display: "flex",
            flex: 1,
            marginBottom: 3,
            justifyContent: "center",
          }}
        >
          <Button
            text="In kết quả"
            onClick={handlePrint}
            variant="invisible"
            size="medium"
            leadingVisual={PrintIcon}
          />

          <Button
            text="Tải xuống"
            isLoading={isExporting}
            onClick={() => {
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
      <Box
        sx={{
          flex: 1,
          p: 3,
          justifyContent: "center",
          display: "flex",
          width: "1200px",
        }}
      >
        {isLoading ? (
          <div>Đang tải...</div>
        ) : html ? (
          <div
            ref={contentRef}
            style={{ display: "flex", justifyContent: "center" }}
            dangerouslySetInnerHTML={{ __html: html }}
          ></div>
        ) : (
          <div>Không có dữ liệu kết quả</div>
        )}
      </Box>
    </Modal>
  );
}

export default XemKetQuaTBSS;
