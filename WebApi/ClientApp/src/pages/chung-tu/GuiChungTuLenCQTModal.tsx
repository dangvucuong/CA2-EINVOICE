import React from "react";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Button from "../../component-ui/button";
import { Box } from "@primer/react";

function GuiChungTuLenCQTModal({
  onClose = () => {},
  GuichungtulenCQT = () => {},
  isSending = false,
}: {
  onClose: () => void;
  GuichungtulenCQT: () => void;
  isSending: boolean;
}) {
  return (
    <Modal
      title={"Lưu ý"}
      onClose={() => {
        onClose();
      }}
      isOpen={true}
      width="1000px"
      height={"auto"}
      // key={khachHangEditing?.id ?? 0}
    >
      <Box
        display={"grid"}
        sx={{
          gap: 2,
        }}
      >
        <Box>Bạn có chắc chắn muốn gửi chứng từ lên cơ quan thuế không?</Box>

        <ModalActions>
          <Button
            onClick={() => {
              onClose();
            }}
            text="Đóng"
          />

          <Button
            text="Xác nhận"
            variant="primary"
            onClick={() => {
              GuichungtulenCQT();
            }}
            isLoading={isSending}
          />
        </ModalActions>
      </Box>
    </Modal>
  );
}

export default GuiChungTuLenCQTModal;
