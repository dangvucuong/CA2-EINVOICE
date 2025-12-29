import { Box, Link } from "@primer/react";
import { DownloadIcon } from "@primer/octicons-react";
import Modal from "../../component-ui/modal";
import { HOA_DON_API } from "../../api/hoa-don/hoaDonApi";
import { useMemo, useRef } from "react";
import Button from "../../component-ui/button";
import { useReactToPrint } from "react-to-print";
import { PrintIcon } from "../../component-ui/icon";
import { appInfo } from "../../AppInfo";
import HoaDonView from "../../pages/hoa-don-form/HoaDonView";
interface IPrintModalProps {
  html: string;
  id?: number;
  onClose: () => void;
  disabledPdf?: boolean;
  disabledXml?: boolean;
}
const PrintModal = (props: IPrintModalProps) => {
  console.log({
    html: props.html,
  });
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref

  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {
      // setIsShowPaging(true);
      // console.log({
      //     onAfterPrint: "xxx"
      // });
    },
  });
  const printPdfUrl = useMemo(() => {
    if (props.id) {
      return `${appInfo.baseApiURL}/${HOA_DON_API}/${props.id}/pdf`;
    }
    return "#";
  }, [props.id]);
  return (
    <Modal
      onClose={props.onClose}
      isOpen={true}
      width={"90%"}
      title="Thông tin hóa đơn"
    >
      <Box sx={{ display: "flex" }}>
        <Button
          text="In hóa đơn"
          onClick={() => {
            setTimeout(() => {
              handlePrint();
            }, 300);
          }}
          variant="invisible"
          size="medium"
          leadingVisual={PrintIcon}
        />
        {props.disabledPdf !== true && (
          <Link href={printPdfUrl}>
            <Button
              text="Tải xuống PDF"
              onClick={() => {
                // setIsShowPaging(false);
                // setTimeout(() => {
                //     handleExportWithFunction();
                // }, 300)
              }}
              variant="invisible"
              size="medium"
              leadingVisual={DownloadIcon}
            />
          </Link>
        )}
        {props.disabledXml !== true && (
          <Link href={`${appInfo.baseApiURL}/hoa-don/${props.id}/download`}>
            <Button
              text="Tải xuống XML"
              onClick={() => {}}
              variant="invisible"
              size="medium"
              leadingVisual={DownloadIcon}
            />
          </Link>
        )}
      </Box>
      <Box
        sx={{
          flex: 1,
          p: 3,
          justifyContent: "center",
          display: "flex",
        }}
      >
        <Box
          dangerouslySetInnerHTML={{ __html: props.html }}
          ref={contentRef}
        />
      </Box>
    </Modal>
  );
};

export default PrintModal;
