import { Dialog } from "@primer/react";
import { Box } from "@primer/react";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  html: string;
}

export default function XemThongBaoSaiSotModal({
  isOpen,
  onClose,
  html,
}: Props) {

  return (
    <Dialog
      isOpen={isOpen}
      onDismiss={onClose}
      sx={{
        width: "1100px",
    maxWidth: "98vw"
      }}
    >
      <Dialog.Header>
        Xem thông báo sai sót
      </Dialog.Header>

      <Box
        sx={{
          maxHeight: "75vh",
          overflow: "auto",
          backgroundColor: "white",
          p: 3
        }}
      >
        <div 
          dangerouslySetInnerHTML={{
            __html: html
          }}
        />
      </Box>

    </Dialog>
  );
}