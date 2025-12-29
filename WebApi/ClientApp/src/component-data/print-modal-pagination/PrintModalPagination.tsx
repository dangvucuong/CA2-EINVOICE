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
interface IPrintModalPaginationProps {
  id?: number;
  onClose: () => void;
}
const PrintModalPagination = (props: IPrintModalPaginationProps) => {
  return (
    <Modal
      onClose={props.onClose}
      isOpen={true}
      width={"90%"}
      title="Thông tin hóa đơn"
    >
      <HoaDonView id={props.id as number} showBackButton={false} />
    </Modal>
  );
};

export default PrintModalPagination;
