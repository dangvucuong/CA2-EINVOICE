import {
  PlusCircleIcon,
  ShieldLockIcon,
  TrashIcon,
  UploadIcon,
} from "@primer/octicons-react";
import { ActionList, Box, FormControl, IconButton } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import { Control, UseFormSetValue, UseFormWatch } from "react-hook-form";
import { UploadCer } from "../../component-data/upload";
import Button from "../../component-ui/button";
import { useCommonContext } from "../../contexts/common";
import { IUploadCerRespone } from "../../models/responses/upload/IUploadCerRespone";
import { IToKhaiCT } from "./ToKhaiCTForm";
import { useAuth } from "../../hooks/useAuth";
import { NotifyHelper } from "../../helpers/toast";
interface ToKhaiFormCTSProps {
  register: any;
  errors: any;
  control: Control<IToKhaiCT>;
  watch: UseFormWatch<IToKhaiCT>;
  setValue: UseFormSetValue<IToKhaiCT>;
  cerFiles: IUploadCerRespone[];
  setCerFiles: React.Dispatch<React.SetStateAction<IUploadCerRespone[]>>;
}
const ToKhaiCTFormCTS = (props: ToKhaiFormCTSProps) => {
  const { cerFiles, setCerFiles } = props;
  const { user } = useAuth();

  const [isShowUpload, setIsShowUpload] = useState(false);
  const {
    _signalrConnected,
    createUUID,
    _signalrHubProxy,
    _signalrSelectCert,
    _signalrSignLogin,
    getMSTFromCertSubject,
  } = useCommonContext();

  const handler = function (eventName: any, data: any) {
    // console.log({
    //   data,
    // });
    if (eventName === "SERVER") {
      const ketquas = data.split("|");
      const [returnCode, code, signedtext] = ketquas;

      if (signedtext === "CertInf") {
        const [nhaCungCap, serial, tuNgay, denNgay, subject] = ketquas.slice(3);
        let issuer = nhaCungCap;
        const match = nhaCungCap.match(/CN=([^,]+)/);
        if (match) {
          issuer = match[1];
        } else {
        }
        // const data: any = {
        //   returnCode,
        //   code,
        //   signedtext,
        //   nhaCungCap,
        //   serial,
        //   tuNgay,
        //   denNgay,
        //   subject,
        //   issuer,
        // };

        // setCerFiles([
        //   ...cerFiles,
        //   {
        //     file_name: createUUID(),
        //     url: createUUID(),
        //     cer_info: {
        //       not_after: denNgay,
        //       not_before: tuNgay,
        //       issuer: issuer,
        //       serial_number: serial,
        //       signature_algorithm: "",
        //       subject: subject,
        //       version: "",
        //     },
        //   },
        // ]);

        const newCerFile: IUploadCerRespone = {
          file_name: createUUID(),
          url: createUUID(),
          cer_info: {
            not_after: denNgay,
            not_before: tuNgay,
            issuer: issuer,
            serial_number: serial,
            signature_algorithm: "",
            subject: subject,
            version: "",
          },
        };

        setCerFiles(() => {
          const mstCert = getMSTFromCertSubject(subject);

          console.log(cerFiles, " cerFilessssssssssss");

          if (mstCert !== user?.donvi_ma_dv) {
            NotifyHelper.Error(
              "Mã số thuế trên chứng thư số không khớp với mã số thuế người nộp thuế"
            );
            return cerFiles;
          }

          const existingCertIndex = cerFiles.findIndex(
            (cert) => cert.cer_info.serial_number === serial
          );

          if (existingCertIndex >= 0) {
            // Nếu đã tồn tại, thay thế cert cũ
            const updatedCerFiles = [...cerFiles];
            updatedCerFiles[existingCertIndex] = newCerFile;

            return updatedCerFiles;
          } else {
            // Nếu chưa tồn tại, thêm mới
            return [...cerFiles, newCerFile];
          }
        });
      }
    }
  };

  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", handler);

      // ✅ cleanup khi unmount hoặc reconnect
      return () => {
        _signalrHubProxy.off("addMessage", handler);
      };
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [_signalrConnected, _signalrHubProxy, cerFiles]);

  return (
    <Box
      sx={{
        display: "grid",
        gap: 2,
      }}
    >
      <Box>
        {cerFiles.length > 0 && (
          <Box
            sx={{
              border: 1,
              borderStyle: "solid",
              borderRadius: 2,
              borderColor: "border.default",
              mb: 3,
            }}
          >
            <ActionList
              selectionVariant="single"
              showDividers
              role="menu"
              aria-label="cers"
            >
              {cerFiles.map((cerFile, idx) => {
                // debugger
                const certInfo = cerFile.cer_info;
                return (
                  <ActionList.Item
                    role="menuitemradio"
                    selected={true}
                    aria-checked={true}
                    // className='listItemSelected'
                    key={idx}
                  >
                    <ActionList.LeadingVisual>
                      <ShieldLockIcon />
                    </ActionList.LeadingVisual>
                    Nhà cung cấp: {certInfo.issuer}
                    <ActionList.Description variant="block">
                      {/* <Box>
                        File: <b>{cerFile.file_name}</b>
                      </Box> */}
                      <Box>
                        Số serial: <b>{certInfo.serial_number}</b>
                      </Box>
                      <Box>
                        Thời hạn sử dụng: Từ{" "}
                        <b>
                          {moment(certInfo.not_before).format("DD/MM/YYYY")}
                        </b>{" "}
                        đến{" "}
                        <b>{moment(certInfo.not_after).format("DD/MM/YYYY")}</b>
                      </Box>
                    </ActionList.Description>
                    <ActionList.TrailingVisual>
                      <IconButton
                        aria-label={`Xóa`}
                        title={`Xóa`}
                        icon={TrashIcon}
                        variant="invisible"
                        onClick={() => {
                          setCerFiles(
                            cerFiles.filter((x) => x.url !== cerFile.url)
                          );
                        }}
                      />
                    </ActionList.TrailingVisual>
                  </ActionList.Item>
                );
              })}
            </ActionList>
          </Box>
        )}
        <Box sx={{ ml: 0 }}>
          <Box sx={{ display: "flex" }}>
            <Button
              text="Tải lên chứng thư số"
              leadingVisual={UploadIcon}
              variant="invisible"
              onClick={() => {
                setIsShowUpload(true);
              }}
            ></Button>
            <Button
              text="Chọn chứng thư số"
              leadingVisual={PlusCircleIcon}
              variant="invisible"
              disabled={!_signalrConnected}
              onClick={() => {
                console.log(cerFiles, " cerFilessssssssssss");

                _signalrSelectCert();
              }}
            ></Button>
          </Box>
          {isShowUpload && (
            <UploadCer
              onUploadSuccess={(data: IUploadCerRespone) => {
                setIsShowUpload(false);

                setCerFiles([...cerFiles, data]);
              }}
            />
          )}
        </Box>
        {cerFiles.length <= 0 && (
          <FormControl.Validation id={"phuong_thuc"} variant="error">
            Vui lòng chọn Chứng thư số
          </FormControl.Validation>
        )}
      </Box>
    </Box>
  );
};

export default ToKhaiCTFormCTS;
