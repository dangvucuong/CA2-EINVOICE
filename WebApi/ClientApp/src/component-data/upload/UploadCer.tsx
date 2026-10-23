import { Box } from "@primer/react";
import { BetterSystemStyleObject } from "@primer/react/lib/sx";
import { useState } from "react";
import { useDropzone } from "react-dropzone";
import { uploadApi } from "../../api/user/uploadApi";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
import { IUploadCerRespone } from "../../models/responses/upload/IUploadCerRespone";
import styles from "./Upload.module.css";
import { parseSoapResponse } from "../../helpers/common";
import { axiosClient } from "../../api/axiosClient";
import { useAuth } from "../../hooks/useAuth";
interface IUploadCerProps {
  sx?: BetterSystemStyleObject;
  onUploadSuccess: (data: IUploadCerRespone) => void;
}
const UploadCer = (props: IUploadCerProps) => {
  const [isUploading, setIsUploading] = useState(false);
  const { user } = useAuth();
  const accept: any = ".cer, .crt";

  const onDrop = async (acceptedFiles: any) => {
    //convert file to base64, just get base64 string, remove "data:application/x-x509-ca-cert;base64,"
    const base64 = await new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(acceptedFiles[0]);
      reader.onload = () => resolve(reader.result?.toString().split(",")[1]);
      reader.onerror = (error) => reject(error);
    });

    const checkCer = await GetMSTFromCertBase64(base64 as string);

    if (!checkCer) {
      NotifyHelper.Error(
        "Mã số thuế trên chứng thư số không khớp với mã số thuế người nộp thuế"
      );
      return;
    }

    // Do something with the files
    if (acceptedFiles.length > 0) {
      setIsUploading(true);
      const res = await uploadApi.uploadCert(acceptedFiles[0]);
      setIsUploading(false);
      if (res.is_success) {
        props.onUploadSuccess(res.data);
        NotifyHelper.Success("Success");
      } else {
        NotifyHelper.Error(res.message ?? "Error");
      }
    }
  };
  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: accept,
  });

  const GetMSTFromCertBase64 = async (base64: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <GetMSTFromCertBase64 xmlns="http://tempuri.org/">
      <certBase64>${base64}</certBase64>
    </GetMSTFromCertBase64>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );

    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(res, "text/xml");

    // Lấy giá trị MST
    const mst = xmlDoc.getElementsByTagName("GetMSTFromCertBase64Result")[0]
      .textContent;

    if (mst !== user?.donvi_ma_dv) {
      return false;
    }

    return true;
  };

  return (
    <Box sx={props.sx}>
      <div {...getRootProps()}>
        <input {...getInputProps()} accept=".cer, .crt" />
        {/* {
                isDragActive ?
                    <p>Drop the files here ...</p> :
                    <p>Drag 'n' drop some files here, or click to select files</p>
            } */}
        <Box
          sx={{
            p: 3,
            textAlign: "center",
            borderStyle: "solid",
            borderWidth: "1px",
            borderColor: "border.default",
            borderRadius: 2,
            cursor: "pointer",
          }}
          className={isDragActive ? styles.isDragActive : ""}
        >
          {isUploading && <Text text="Uploading..." />}
          {!isUploading && <Text text="Kéo thả hoặc nhấn vào đây để upload" />}
        </Box>
      </div>
    </Box>
  );
};

export default UploadCer;
