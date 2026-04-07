import { Box } from "@primer/react";
import React from "react";
import RemoteSigningConfig from "./RemoteSigningConfig";
import UsbSigingConfig from "./UsbSigingConfig";
import ConfigThongTinCKS from "./ConfigThongTinCKS";

const KySoConfigPage = () => {
  return (
    <Box>
      <Box
        sx={{
          display: "flex",
          flexWrap: "wrap",
        }}
      >
        <Box>
          <RemoteSigningConfig />
        </Box>
        <Box sx={{ ml: 3 }}>
          <UsbSigingConfig />
        </Box>
      </Box>

      <ConfigThongTinCKS />
    </Box>
  );
};

export default KySoConfigPage;
