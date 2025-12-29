import { UploadIcon } from "@primer/octicons-react";
import { useState } from "react";
import Button from "../../component-ui/button/Button";
import HangHoaImportModal from "./HangHoaImportModal";
interface IHangHoaImportButtonProps {
  onSuccess: () => void;
}
const HangHoaImportButton = (props: IHangHoaImportButtonProps) => {
  const [isShowImportModal, setIsShowImportModal] = useState(false);

  return (
    <>
      <Button
        text="Nhập khẩu"
        leadingVisual={UploadIcon}
        size="medium"
        sx={{ ml: 1 }}
        onClick={() => {
          setIsShowImportModal(true);
        }}
      />
      {isShowImportModal && (
        <HangHoaImportModal
          onClose={() => {
            setIsShowImportModal(false);
          }}
          onSuccess={() => {
            setIsShowImportModal(false);
            props.onSuccess();
          }}
        />
      )}
    </>
  );
};

export default HangHoaImportButton;
