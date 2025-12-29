import { UploadIcon } from "@primer/octicons-react";
import Button from "../../component-ui/button";
import { memo, useState } from "react";
import ChungTuImportModal from "./ChungTuImportModal";
interface IChungTuImportButtonProps {
  onSuccess: () => void;
}
const ChungTuImportButton = (props: IChungTuImportButtonProps) => {
  const [isShowImportModal, setIsShowImportModal] = useState(false);

  return (
    <>
      <Button
        text="Lập lô"
        leadingVisual={UploadIcon}
        size="medium"
        sx={{ ml: 1 }}
        onClick={() => {
          setIsShowImportModal(true);
        }}
      />
      {isShowImportModal && (
        <ChungTuImportModal
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

export default memo(ChungTuImportButton);
