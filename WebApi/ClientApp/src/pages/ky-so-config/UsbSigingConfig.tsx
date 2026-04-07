import { Box } from "@primer/react";
import { DownloadIcon } from "@primer/octicons-react";
import React from "react";
import Heading from "../../component-ui/heading";
import { eSize } from "../../models/commons/eSize";
import Button from "../../component-ui/button";
import SignalrConnectionStatus from "../../component-data/signalr-connection-status";
import { useAuth } from "../../hooks/useAuth";
import Text from "../../component-ui/text";

const UsbSigingConfig = ({ isDisabled = false }: { isDisabled?: boolean }) => {
  const { user } = useAuth();
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        borderRadius: 2,
        border: "1px",
        borderStyle: "solid",
        borderColor: "border.default",
        p: 3,
        // pb: 4,
        // pt: 4,
        width: "500px",
        // height:"200px",
        justifyContent: "center",
      }}
    >
      <Box
        sx={{
          display: "flex",
          mt: 2,
          height: "90px",
        }}
      >
        <Box id="icon">
          <img alt="USB" src="../../images/usb.svg" />
        </Box>
        <Box
          id="content"
          sx={{
            ml: 2,
          }}
        >
          <Heading text="Ký số trực tiếp qua USB" size={eSize.medium} />
          <Box
            sx={{
              color: "fg.muted",
            }}
          >
            <Box>1. Máy tính đang dùng cần cài đặt tool ký số</Box>
            <Box>2. Bạn cần cắm USB chứa chứng thư số để thực hiện ký số</Box>
          </Box>
        </Box>
      </Box>

      {isDisabled && (
        <Box
          sx={{
            color: "red",
            flex: 2,
            textAlign: "center",
            fontSize: 14,
            fontWeight: "bold",
          }}
        >
          <Text
            text="Bạn chưa chạy tool ký số."
            sx={{
              display: "block",
            }}
          ></Text>
          <Text
            text="Vui lòng chạy tool ký số để sử dụng tính năng này."
            sx={{
              display: "block",
            }}
          ></Text>
        </Box>
      )}

      <Box
        sx={{
          mt: 4,
          display: "flex",
        }}
      >
        <Box
          id="left"
          sx={{
            flex: 1,
          }}
        >
          <SignalrConnectionStatus />
        </Box>

        <Box
          id="right"
          sx={{
            display: "flex",
          }}
        >
          <Button
            text="Tải về tool ký số"
            variant="primary"
            leadingVisual={DownloadIcon}
            size="medium"
            sx={{
              ml: 2,
            }}
            onClick={() => {
              window.open(
                "https://hsdt.nacencomm.vn/downloads/setup.msi",
                "_blank"
              );
            }}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default UsbSigingConfig;
