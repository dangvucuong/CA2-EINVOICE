import { UploadIcon } from "@primer/octicons-react";
import { useState } from "react";
import Button from "../../component-ui/button/Button";
import DaiLyImportModal from "./DaiLyImportModal";
interface IDaiLyImportButtonProps {
  onSuccess: () => void;
}
const DaiLyImportButton = (props: IDaiLyImportButtonProps) => {
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
        <DaiLyImportModal
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

export default DaiLyImportButton;
