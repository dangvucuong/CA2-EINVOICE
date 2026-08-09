import React, { memo, useEffect, useRef, useState } from "react";
import Modal from "../../component-ui/modal";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { NotifyHelper } from "../../helpers/toast";
import { useReactToPrint } from "react-to-print";
import { appInfo } from "../../AppInfo";
import { Box } from "@primer/react";
import Button from "../../component-ui/button";
import { PrintIcon } from "../../component-ui/icon";
import { DownloadIcon, WorkflowIcon } from "@primer/octicons-react";
import { inchuyendoiChungTu } from "../../helpers/chungTuDownloadHelper";

function XemChungTu({
  isOpen,
  onClose,
  machungtu,
  user,
  inChuyenDoi = false,
  onInChuyenDoiApplied,
}: {
  isOpen: boolean;
  onClose: () => void;
  machungtu: string;
  user: any;
  inChuyenDoi?: boolean;
  onInChuyenDoiApplied?: () => void;
}) {
  const [thongdiep, setThongDiep] = useState<any>(null);
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  const [isExporting, setIsExporting] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (!isOpen || !machungtu) {
      return;
    }

    const load = async () => {
      setThongDiep(null);
      setIsLoading(true);
      try {
        if (inChuyenDoi) {
          const inchuyendoiRes = await inchuyendoiChungTu(
            machungtu,
            user?.donvi_ma_dv
          );
          if (inchuyendoiRes?.status !== "success") {
            NotifyHelper.Error(
              inchuyendoiRes?.message ?? "In chuyển đổi không thành công"
            );
            onClose();
            return;
          }
          onInChuyenDoiApplied?.();
        }
        await LayHtmlChungTu(machungtu);
      } finally {
        setIsLoading(false);
      }
    };

    load();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen, machungtu, inChuyenDoi]);

  const LayHtmlChungTu = async (machungtu: string | undefined) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemChungTu xmlns="http://tempuri.org/">
      <machungtu>${machungtu}</machungtu>
      <madonvi>${user?.donvi_ma_dv}</madonvi>
    </XemChungTu>
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
      setThongDiep(parseRes.data);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

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
        html: thongdiep,
        file_name: "ChungTu",
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
    link.setAttribute("download", `ChungTu.pdf`); //or any other extension
    document.body.appendChild(link);
    link.click();
    link.remove();

    setIsExporting(false);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={inChuyenDoi ? "In chuyển đổi chứng từ" : "Xem chứng từ"}
      width={"1200px"}
    >
      {thongdiep && (
        <Box
          sx={{
            display: "flex",
            flex: 1,
            marginBottom: 3,
            justifyContent: "center",
          }}
        >
          <Button
            text={inChuyenDoi ? "In chứng từ chuyển đổi" : "In tờ khai"}
            onClick={handlePrint}
            variant="invisible"
            size="medium"
            leadingVisual={inChuyenDoi ? WorkflowIcon : PrintIcon}
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
            dangerouslySetInnerHTML={{ __html: thongdiep }}
          ></div>
        ) : isLoading ? (
          <div>Đang tải...</div>
        ) : (
          <div />
        )}
      </Box>
    </Modal>
  );
}

export default memo(XemChungTu);
