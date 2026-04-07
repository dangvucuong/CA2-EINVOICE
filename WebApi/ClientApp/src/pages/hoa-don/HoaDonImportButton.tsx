import { UploadIcon } from "@primer/octicons-react";
import Button from "../../component-ui/button";
import { useState } from "react";
import HoaDonImportModal from "./HoaDonImportModal";
interface IHoaDonImportButtonProps {
  onSuccess: () => void;
}
const HoaDonImportButton = (props: IHoaDonImportButtonProps) => {
  const [isShowImportModal, setIsShowImportModal] = useState(false);

  return (
    <>
      <Button
        text="Lập HĐ theo lô"
        leadingVisual={UploadIcon}
        size="medium"
        sx={{ ml: 1 }}
        onClick={() => {
          setIsShowImportModal(true);
        }}
      />
      {isShowImportModal && (
        <HoaDonImportModal
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

export default HoaDonImportButton;
