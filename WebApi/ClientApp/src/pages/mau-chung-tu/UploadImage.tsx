import { Box } from "@primer/react";
import React, { memo, useState } from "react";

function UploadImage({
  sx,
  id,
  noImageText,
  onChangeValue,
}: {
  sx?: any;
  id?: string;
  noImageText?: string;
  onChangeValue?: (value: string, fileName: string) => void;
}) {
  const [preview, setPreview] = useState<string>();

  // Hàm resize ảnh 50%
  const resizeImage50 = (file: File, callback: (base64: string) => void) => {
    const reader = new FileReader();
    reader.onload = function (event) {
      const img = new Image();
      img.onload = function () {
        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");

        const newWidth = img.width * 0.3;
        const newHeight = img.height * 0.3;

        canvas.width = newWidth;
        canvas.height = newHeight;

        ctx?.drawImage(img, 0, 0, newWidth, newHeight);

        // Xuất base64 ảnh JPEG (dung lượng nhẹ hơn PNG)
        const resizedBase64 = canvas.toDataURL("image/JPEG", 0.4);

        callback(resizedBase64);
      };
      img.src = event.target?.result as string;
    };
    reader.readAsDataURL(file);
  };

  return (
    <Box sx={sx}>
      <Box>
        <Box
          onClick={() => {
            document.getElementById(id || "upload")?.click();
          }}
          sx={{
            width: "100%",
            height: "auto",
            minHeight: 200,
            border: "1px solid #ccc",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            mb: 2,
            overflow: "hidden",
            borderRadius: 10,
            bg: "#f6f8fa",
          }}
        >
          {preview ? (
            <img
              src={preview}
              alt="Preview"
              style={{ width: "100%", height: "100%", objectFit: "cover" }}
            />
          ) : (
            <span>
              {noImageText || "Chưa có ảnh"} <br />
            </span>
          )}
        </Box>
      </Box>

      <Box sx={{ textAlign: "center" }}>
        <label
          htmlFor={id}
          style={{
            cursor: "pointer",
            color: "#0969da",
            textDecoration: "underline",
          }}
        >
          Tải ảnh lên
        </label>
        <input
          type="file"
          accept="image/*"
          hidden
          id={id}
          onChange={(e) => {
            const file = e.target.files?.[0];
            if (file) {
              resizeImage50(file, (resizedBase64) => {
                setPreview(resizedBase64);
                if (onChangeValue) {
                  // Trả về base64 đã resize (bỏ prefix)
                  const pureBase64 = resizedBase64.split(",")[1];
                  console.log("pureBase64", pureBase64);
                  onChangeValue(pureBase64, file.name);
                }
              });
            }
          }}
        />
      </Box>
    </Box>
  );
}

export default memo(UploadImage);
