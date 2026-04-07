import { Box, Button, Link } from "@primer/react";
import { DownloadIcon, EyeIcon } from "@primer/octicons-react";
import React, { useEffect, useRef, useState } from "react";
import { thongBaoSaiSotApi } from "../../api/tbss/thongBaoSaiSotApi";
import { NotifyHelper } from "../../helpers/toast";
import Modal from "../../component-ui/modal";
import { useReactToPrint } from "react-to-print";
import { PrintIcon } from "../../component-ui/icon";
import { appInfo } from "../../AppInfo";
interface ITBSSViewBtnProps {
  id: number;
}
const TBSSViewBtn = (props: ITBSSViewBtnProps) => {
  const [isShowModal, setIsShowModal] = useState(false);
  const [html, setHtml] = useState("");
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  useEffect(() => {
    if (isShowModal) {
      getHtmlAsync();
    }
  }, [props.id, isShowModal]);
  const getHtmlAsync = async () => {
    const res = await thongBaoSaiSotApi.getHtmlView(props.id);
    if (res.is_success) {
      setHtml(res.data);
    } else {
      NotifyHelper.Error("Không tải được file");
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
        aria-label={`Xem: ${props.id}`}
        title={`Xem: ${props.id}`}
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
              <Link href={`${appInfo.baseApiURL}/tbss/${props.id}/download`}>
                <Button leadingVisual={DownloadIcon} variant="invisible">
                  Tải xuống
                </Button>
              </Link>
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

export default TBSSViewBtn;
