import { ActionList } from "@primer/react";
import { useEffect, useState } from "react";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import PrintIcon from "../../component-ui/icon/print";
import { NotifyHelper } from "../../helpers/toast";
import PrintModal from "../print-modal";
import { IIHoaDonAddOrEditModel } from "../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { EyeIcon } from "@primer/octicons-react";
import PrintModalPagination from "../print-modal-pagination";

interface IViewHoaDonModalProps {
  id: number;
  onClose?: () => void;
  onOpenedModal?: () => void;
  showText?: boolean;
  hinhThucHoaDonId?: number;
}

const ViewHoaDonButtonActionListItem = (props: IViewHoaDonModalProps) => {
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);
  const { showText = true, hinhThucHoaDonId = 1 } = props;

  return (
    <>
      <ActionList.Item
        onClick={() => {
          setIsShowPrintModal(true);
        }}
      >
        <ActionList.LeadingVisual>
          <EyeIcon />
        </ActionList.LeadingVisual>
        {showText && "Xem hóa đơn"}
      </ActionList.Item>

      {isShowPrintModal && (
        <PrintModalPagination
          id={props.id}
          onClose={() => {
            setIsShowPrintModal(false);
          }}
          hinhThucHoaDonId={hinhThucHoaDonId}
        />
      )}
    </>
  );
};

export default ViewHoaDonButtonActionListItem;
