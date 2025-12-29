import { ActionList } from "@primer/react";
import { useEffect, useState } from "react";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import PrintIcon from "../../component-ui/icon/print";
import { NotifyHelper } from "../../helpers/toast";
import PrintModal from "../print-modal";
import { IIHoaDonAddOrEditModel } from "../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { EyeIcon } from "@primer/octicons-react";

interface IPrintHoaDonButtonProps {
  id: number;
  onClose?: () => void;
  onOpenedModal?: () => void;
  showText?: boolean;
}
interface IPreviewHoaDonButtonProps {
  data: IIHoaDonAddOrEditModel;
}

const PrintHoaDonButton = (props: IPrintHoaDonButtonProps) => {
  const [isLoading, setIsLoading] = useState(false);
  const [htmlData, setHtmlData] = useState<string>("");
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);

  const handlePrintAsync = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.getPrintHtml(props.id, 10);
    if (res.is_success) {
      setHtmlData(res.data);
      setIsShowPrintModal(true);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };

  return (
    <>
      <Button
        text="Xem Hóa đơn"
        sx={{ mr: 2, minWidth: "100px" }}
        size="large"
        variant="invisible"
        leadingVisual={EyeIcon}
        isLoading={isLoading}
        onClick={handlePrintAsync}
      />
      {isShowPrintModal && (
        <PrintModal
          html={htmlData}
          id={props.id}
          onClose={() => {
            setIsShowPrintModal(false);
          }}
        />
      )}
    </>
  );
};

const PreViewHoaDonButton = (props: IPreviewHoaDonButtonProps) => {
  const [isLoading, setIsLoading] = useState(false);
  const [htmlData, setHtmlData] = useState<string>("");
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);

  const handlePrintAsync = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.getPreviewHtml(props.data);
    if (res.is_success) {
      setHtmlData(res.data);
      setIsShowPrintModal(true);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };

  return (
    <>
      <Button
        text="Xem trước"
        sx={{ mr: 2, minWidth: "100px" }}
        size="large"
        variant="invisible"
        leadingVisual={PrintIcon}
        isLoading={isLoading}
        onClick={handlePrintAsync}
      />
      {isShowPrintModal && (
        <PrintModal
          html={htmlData}
          onClose={() => {
            setIsShowPrintModal(false);
          }}
        />
      )}
    </>
  );
};

const PrintHoaDonButtonActionListItem = (props: IPrintHoaDonButtonProps) => {
  const [isLoading, setIsLoading] = useState(false);
  const [htmlData, setHtmlData] = useState<string>("");
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);
  const { showText = true } = props;

  const handlePrintAsync = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.getPrintHtml(props.id);
    if (res.is_success) {
      setHtmlData(res.data);
      setIsShowPrintModal(true);
      if (props.onOpenedModal) {
        props.onOpenedModal();
      }
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };

  return (
    <>
      <ActionList.Item
        // onSelect={() => {
        //     handlePrintAsync()
        // }}
        onClick={() => {
          handlePrintAsync();
        }}
      >
        <ActionList.LeadingVisual>
          <EyeIcon />
        </ActionList.LeadingVisual>
        {showText && "Xem hóa đơn"}
      </ActionList.Item>
      {isShowPrintModal && (
        <PrintModal
          id={props.id}
          html={htmlData}
          onClose={() => {
            console.log({
              xxx: "closed",
            });

            setIsShowPrintModal(false);
            if (props.onClose) {
              props.onClose();
            }
          }}
        />
      )}
    </>
  );
};

export default PrintHoaDonButton;
export { PrintHoaDonButtonActionListItem };
export { PreViewHoaDonButton };
