import { EyeIcon } from "@primer/octicons-react";
import { Box } from "@primer/react";
import { useState } from "react";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ToKhaiView from "../../pages/to-khai/ToKhaiView";
interface IPrintToKhaiButtonProps {
  id: number;
  status?: number;
}

const PrintToKhaiButton = (props: IPrintToKhaiButtonProps) => {
  const [isShowPrintModal, setIsShowPrintModal] = useState(false);

  return (
    <>
      <Button
        text="Xem tờ khai"
        sx={{ mr: 2, minWidth: "100px" }}
        size="large"
        variant="invisible"
        leadingVisual={EyeIcon}
        disabled={props.id <= 0}
        onClick={() => {
          setIsShowPrintModal(true);
        }}
      />
      {isShowPrintModal && (
        <Modal
          onClose={() => {
            setIsShowPrintModal(false);
          }}
          isOpen={true}
          width={"90%"}
          title="Thông tin tờ khai"
        >
          <Box
            sx={{
              flex: 1,
              p: 3,
              justifyContent: "center",
              display: "flex",
            }}
          >
            <ToKhaiView id={props.id} status={props?.status} />
          </Box>
        </Modal>
      )}
    </>
  );
};

export default PrintToKhaiButton;
