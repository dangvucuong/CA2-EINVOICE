import React, { useEffect, useRef, useState } from "react";
import Modal from "../../component-ui/modal";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { NotifyHelper } from "../../helpers/toast";
import { useReactToPrint } from "react-to-print";
import { appInfo } from "../../AppInfo";
import { Box } from "@primer/react";
import Button from "../../component-ui/button";
import { PrintIcon } from "../../component-ui/icon";
import { DownloadIcon } from "@primer/octicons-react";

function XemKetQuaTBSSCT({
  isOpen,
  onClose,
  matbss,
  user,
  type,
}: {
  isOpen: boolean;
  onClose: () => void;
  matbss: string;
  user: any;
  type: number; // 5 xem tờ khai, 6 xem kết quả
}) {
  const [thongdiep, setThongDiep] = useState<any>(null);
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  const [isExporting, setIsExporting] = useState(false);

  useEffect(() => {
    if (matbss) {
      XemKetQuaTBSS(matbss);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [matbss]);

  const XemKetQuaTBSS = async (matbssct: string | undefined) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemKetQuaTBSS xmlns="http://tempuri.org/">
      <matbssct>${matbssct}</matbssct>
      <madonvi>${user?.donvi_ma_dv}</madonvi>
    </XemKetQuaTBSS>
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

    if (parseRes.status === "success") {
      setThongDiep({
        Thongdiep: cleanHtml(parseRes.data),
      });
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
        html: thongdiep.Thongdiep,
        file_name: "Kết quả", // tên file xuất ra
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
    link.setAttribute("download", "KetQua.pdf");
    document.body.appendChild(link);
    link.click();
    link.remove();

    setIsExporting(false);
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Xem kết quả" width="1200px">
      {thongdiep?.Thongdiep && (
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
      <Box
        sx={{
          flex: 1,
          p: 3,
          justifyContent: "center",
          display: "flex",
          width: "1200px",
        }}
      >
        {thongdiep ? (
          <div
            ref={contentRef}
            style={{ display: "flex", justifyContent: "center" }}
            dangerouslySetInnerHTML={{ __html: thongdiep.Thongdiep }}
          ></div>
        ) : (
          <div>Đang tải...</div>
        )}
      </Box>
    </Modal>
  );
}

export default XemKetQuaTBSSCT;
