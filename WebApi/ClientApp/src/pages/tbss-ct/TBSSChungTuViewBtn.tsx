import { Box, Button, Link } from "@primer/react";
import { DownloadIcon, EyeIcon } from "@primer/octicons-react";
import React, { useEffect, useRef, useState } from "react";
import { thongBaoSaiSotApi } from "../../api/tbss/thongBaoSaiSotApi";
import { NotifyHelper } from "../../helpers/toast";
import Modal from "../../component-ui/modal";
import { useReactToPrint } from "react-to-print";
import { PrintIcon } from "../../component-ui/icon";
import { parseSoapResponse } from "../../helpers/common";
import { axiosClient } from "../../api/axiosClient";
interface ITBSSChungTuViewBtnProps {
  matbss_ct: number;
}
const TBSSChungTuViewBtn = (props: ITBSSChungTuViewBtnProps) => {
  const { matbss_ct } = props;
  const [isShowModal, setIsShowModal] = useState(false);
  const [html, setHtml] = useState("");
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  useEffect(() => {
    if (isShowModal) {
      getHtmlAsync();
    }
  }, [matbss_ct, isShowModal]);

  const getHtmlAsync = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemChiTietTBSS  xmlns="http://tempuri.org/">
      <matbss_ct>${matbss_ct}</matbss_ct>
    </XemChiTietTBSS>
  </soap12:Body>
</soap12:Envelope>`;
    // setIsLoading(true);

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
    // setIsLoading(false);

    if (parseRes.status === "success") {
      console.log(parseRes.data);
      setHtml(parseRes.data);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };
  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {},
  });

  return (
    <>
      <Button
        leadingVisual={EyeIcon}
        variant="invisible"
        onClick={() => {
          setIsShowModal(true);
        }}
      ></Button>
      {isShowModal && (
        <Modal
          isOpen
          onClose={() => {
            setIsShowModal(false);
          }}
          sx={{
            width: "1000px",
          }}
          title={
            <Box sx={{ display: "flex", gap: 2 }}>
              <Button
                leadingVisual={PrintIcon}
                variant="invisible"
                onClick={handlePrint}
              >
                In
              </Button>
            </Box>
          }
        >
          <Box
            id="htmlView"
            dangerouslySetInnerHTML={{ __html: html }}
            ref={contentRef}
          />
        </Modal>
      )}
    </>
  );
};

export default TBSSChungTuViewBtn;
