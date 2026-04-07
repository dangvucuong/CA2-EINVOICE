import { useEffect, useState } from "react";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import PrintIcon from "../../component-ui/icon/print";
import { NotifyHelper } from "../../helpers/toast";
import { eHoaDonHinhThuc } from "../../models/commons/eHoaDonHinhThuc";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import PrintModal from "../print-modal";

interface IPrintHoaDonBienBanButtonProps {
  id: number;
  hoaDon?: IHoaDon,
  onClose?: () => void;
  onOpenedModal?: () => void;
}


const PrintHoaDonBienBanButton = (props: IPrintHoaDonBienBanButtonProps) => {
  const [hoaDon, setHoaDon] = useState(props.hoaDon);
  const [isLoading, setIsLoading] = useState(false);
  const [htmlData, setHtmlData] = useState<string>("");
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);

  const handlePrintAsync = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.getBienBanHtml(props.id);
    if (res.is_success) {
      setHtmlData(res.data);
      setIsShowPrintModal(true);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };
  useEffect(() => {
    setHoaDon(props.hoaDon)

  }, [props.hoaDon])
  return (
    <>
      {hoaDon && (
        hoaDon.hoa_don_hinh_thuc_id === eHoaDonHinhThuc.HOA_DON_DIEU_CHINH ||
        hoaDon.hoa_don_hinh_thuc_id === eHoaDonHinhThuc.HOA_DON_THAY_THE

      ) &&
        <>
          <Button
            text="In biên bản"
            sx={{ minWidth: "100px" }}
            size="large"
            variant="invisible"
            leadingVisual={PrintIcon}
            isLoading={isLoading}
            onClick={handlePrintAsync}
          />
          {isShowPrintModal && (
            <PrintModal
              html={htmlData}
              id={props.id}
              disabledPdf
              disabledXml
              onClose={() => {
                setIsShowPrintModal(false);
              }}
            />
          )}
        </>
      }

    </>
  );
};


export default PrintHoaDonBienBanButton

