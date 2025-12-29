import { Box } from "@primer/react";
import { BetterSystemStyleObject } from "@primer/react/lib/sx";
import { useState } from "react";
import { useDropzone } from "react-dropzone";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
interface IUploadProps {
  sx?: BetterSystemStyleObject;
  onUploadSuccess: (data: string, fileName?: string) => void;
  accept?: any;
  icon?: React.ReactNode;
  // isUploadCert?: boolean
}
const UploadChungTu = (props: IUploadProps) => {
  const { onUploadSuccess } = props;
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

  const fileToBase64 = (file: File): Promise<string> =>
    new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const result = reader.result as string;
        // Cắt bỏ phần "data:...;base64,"
        const base64 = result.split(",")[1];
        resolve(base64);
      };
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });

  const onDrop = async (acceptedFiles: any) => {
    if (acceptedFiles.length > 0) {
      setIsUploading(true);
      try {
        const file = acceptedFiles[0];
        const base64 = await fileToBase64(file);

        // TODO: gọi API upload hoặc callback bên ngoài
        onUploadSuccess(base64, file?.name);
      } catch (err) {
        NotifyHelper.Error("Không đọc được file");
        console.error(err);
      } finally {
        setIsUploading(false);
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
        <input {...getInputProps()} accept=".xls,.xlsx" />
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
            backgroundColor: isDragActive ? "#DCF4FF" : "transparent",
          }}
        >
          {props.icon ? <Box>{props.icon}</Box> : null}
          {isUploading && <Text text="Uploading..." />}
          {!isUploading && (
            <Text
              text="Kéo thả hoặc nhấn vào đây để upload"
              sx={{
                fontWeight: 600,
                color: "blue",
              }}
            />
          )}
        </Box>
      </div>
    </Box>
  );
};

export default UploadChungTu;
