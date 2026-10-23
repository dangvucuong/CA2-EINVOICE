import { Box } from "@primer/react";
import { BetterSystemStyleObject } from "@primer/react/lib/sx";
import { useState } from "react";
import { useDropzone } from "react-dropzone";
import { uploadApi } from "../../api/user/uploadApi";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import styles from "./Upload.module.css";
//eeee
interface IUploadProps {
  sx?: BetterSystemStyleObject;
  onUploadSuccess: (data: IUploadRespone) => void;
  accept?: any;
  icon?: React.ReactNode;
}
const Upload = (props: IUploadProps) => {
  const [isUploading, setIsUploading] = useState(false);
  const accept: any = props.accept
    ? props.accept
    : {
        "image/*": [],
        "application/pdf": [],
        "application/msword": [".doc", ".docx"],
        "application/vnd.ms-excel": [".xls", ".xlsx"],
        "application/msg": [".msg"],
      };
  const onDrop = async (acceptedFiles: any) => {
    // Do something with the files
    console.log({
      acceptedFiles,
    });
    if (acceptedFiles.length > 0) {
      setIsUploading(true);
      const res = await uploadApi.upload(acceptedFiles[0]);
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
  return (
    <Box sx={props.sx}>
      <div {...getRootProps()}>
        <input {...getInputProps()} accept={accept} />
        {/* {
                isDragActive ?
                    <p>Drop the files here ...</p> :
                    <p>Drag 'n' drop some files here, or click to select files</p>
            } */}
        <Box
          sx={{
            p: 3,
            textAlign: "center",
            borderStyle: "dashed",
            borderWidth: "1px",
            borderColor: "border.default",
            borderRadius: 2,
            cursor: "pointer",
            display: "flex",
            flexDirection: "column",
            justifyContent: "center",
            color: "fg.muted",
          }}
          className={isDragActive ? styles.isDragActive : ""}
        >
          {props.icon ? <Box>{props.icon}</Box> : null}
          {isUploading && <Text text="Uploading..." />}
          {!isUploading && (
            <Text
              text="Kéo thả hoặc nhấn vào đây để upload"
              sx={{
                color: "blue",
                fontSize: 14,
                fontWeight: 600,
              }}
            />
          )}
        </Box>
      </div>
    </Box>
  );
};

export default Upload;
