import { DownloadIcon, TrashIcon } from "@primer/octicons-react";
import { Box, FormControl, IconButton, Link, SubNav } from "@primer/react";
import { useMemo, useState } from "react";
import { appInfo } from "../../AppInfo";
import Steps from "../../component-data/steps";
import { IStepData } from "../../component-data/steps/Steps";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
import SelectBoxLoaiChungTuPhatHanh from "../../component-data/selectbox-loai-chung-tu-phat-hanh";
import SelectBoxMauSoChungTuPhatHanh from "../../component-data/selectbox-mau-so-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import UploadChungTu from "./UploadChungTu";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { useAuth } from "../../hooks/useAuth";

interface IChungTuImportModalProps {
  onClose: () => void;
  onSuccess: () => void;
}
const _steps: IStepData[] = [
  {
    id: 1,
    name: "Chọn dữ liệu",
    is_active: true,
  },
  {
    id: 2,
    name: "Upload file",
    is_active: false,
  },
];
const ChungTuImportModal = (props: IChungTuImportModalProps) => {
  const [stepId, setStepId] = useState(1);
  const [isSaving, setIsSaving] = useState(false);
  const { user } = useAuth();

  const [uploadedFile, setUploadedFile] = useState<any>(null);
  const [dataFilter, setDataFilter] = useState({
    loai_chung_tu: "",
    mau_so: "",
    ky_hieu: "",
  });
  const [template, setTemplate] = useState<"">("");

  const width = useMemo(() => {
    if (stepId === 1) {
      return "xlarge";
    }
    if (stepId === 2) {
      return "xlarge";
    }
    return "90%%";
  }, [stepId]);

  const handleSubmit = async () => {
    if (uploadedFile) {
      setIsSaving(true);

      await Daylochungtu({
        fileName: uploadedFile.fileName,
        fileBase64: uploadedFile.fileBase64,
        madonvi: user?.donvi?.ma_dv || "",
        mauso: "03/TNCN",
        kyhieu: dataFilter.ky_hieu,
        ngaylap: new Date().toISOString(),
      });

      setIsSaving(false);
    }
  };

  const Daylochungtu = async ({
    fileName,
    fileBase64,
    madonvi,
    mauso,
    kyhieu,
    ngaylap,
  }: {
    fileName: string;
    fileBase64: string;
    madonvi: string;
    mauso: string;
    kyhieu: string;
    ngaylap: string;
  }) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <DayLoChungTu xmlns="http://tempuri.org/">
      <fileName>${fileName}</fileName>
      <fileBase64>${fileBase64}</fileBase64>
      <madonvi>${madonvi}</madonvi>
      <mauso>${mauso}</mauso>
      <kyhieu>${kyhieu}</kyhieu>
      <ngaylap>${ngaylap}</ngaylap>
    </DayLoChungTu>
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

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      console.log(parseRes?.data);
      props.onSuccess();
      NotifyHelper.Success(
        "Đây lô thành công " + parseRes?.count + " chứng từ"
      );
    } else {
      NotifyHelper.Error(parseRes.message || "Lỗi import");
    }
  };

  return (
    <Modal
      title={"Import"}
      onClose={props?.onClose}
      isOpen={true}
      width={width}
      height={"auto"}
    >
      <form>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <Box>
            <Steps
              steps={_steps.map((x) => ({
                ...x,
                is_active: x.id === stepId,
              }))}
              isNotShowHoanThanhStep={true}
            />
          </Box>
          <Box sx={{ mt: 3 }}>
            {stepId === 1 && (
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <FormControl>
                  <FormControl.Label>Loại hóa đơn</FormControl.Label>
                  <SelectBoxLoaiChungTuPhatHanh
                    isShowClearBtn
                    value={dataFilter.loai_chung_tu}
                    onValueChanged={(value: string) => {
                      setDataFilter({
                        ...dataFilter,
                        loai_chung_tu: value,
                      });
                    }}
                  />
                </FormControl>
                <Box>
                  <FormControl>
                    <FormControl.Label>Mẫu số</FormControl.Label>
                    <SelectBoxMauSoChungTuPhatHanh
                      value={dataFilter.mau_so}
                      onValueChanged={(value: string) => {
                        setDataFilter({ ...dataFilter, mau_so: value });
                      }}
                      loai_chung_tu={dataFilter.loai_chung_tu}
                    />
                  </FormControl>
                </Box>
                <Box>
                  <FormControl>
                    <FormControl.Label>Ký hiệu</FormControl.Label>
                    <SelectBoxKyHieuChungTuPhatHanh
                      value={dataFilter.ky_hieu}
                      onValueChanged={(value: string) => {
                        setDataFilter({ ...dataFilter, ky_hieu: value });
                      }}
                      mau_so={"03/TNCN"}
                    />
                  </FormControl>
                </Box>
              </Box>
            )}
            {stepId === 2 && (
              <Box>
                <Box
                  sx={{
                    mb: 2,
                    display: "flex",
                    justifyContent: "center",
                  }}
                >
                  <SubNav aria-label="Main">
                    <SubNav.Links>
                      <SubNav.Link
                        selected={template === ""}
                        sx={{
                          cursor: "pointer",
                        }}
                        onClick={() => {
                          setTemplate("");
                        }}
                      >
                        Mặc định
                      </SubNav.Link>
                    </SubNav.Links>
                  </SubNav>
                </Box>
                <Box>
                  {uploadedFile && (
                    <Box>
                      <Heading text="File đã upload" />
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "space-between",
                          my: 2,
                        }}
                      >
                        <Text
                          text={uploadedFile?.fileName || ""}
                          sx={{ mb: 1, color: "fg.muted" }}
                        />

                        <IconButton
                          title="Xóa file"
                          aria-label="Xóa file"
                          sx={{
                            mt: "-5px",
                          }}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            setUploadedFile(null);
                          }}
                        />
                      </Box>
                    </Box>
                  )}
                  <UploadChungTu
                    onUploadSuccess={(data, fileName) => {
                      setUploadedFile({
                        fileName: fileName,
                        fileBase64: data,
                      });
                    }}
                  />
                </Box>
                <Box
                  sx={{
                    mt: 2,
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "center",
                  }}
                >
                  {template === "" && (
                    <Link
                      href={`${appInfo.baseApiURL.replace(
                        "/api",
                        ""
                      )}/Template/CA2-template_CTTNCNND70.xlsx`}
                      target="_blank"
                    >
                      <Button
                        text="Tải file mẫu"
                        size="medium"
                        variant="invisible"
                        leadingVisual={DownloadIcon}
                      />
                    </Link>
                  )}

                  <Box>
                    <Text
                      text="Vui lòng format dữ liệu theo file mẫu để import dữ liệu chính xác"
                      sx={{
                        color: "fg.muted",
                      }}
                    />
                  </Box>
                </Box>
              </Box>
            )}
          </Box>

          <ModalActions>
            <Button
              onClick={() => {
                props.onClose();
              }}
              text="Đóng"
            />
            {stepId === 1 && (
              <Button
                variant="primary"
                type="button"
                text="Tiếp tục"
                onClick={() => {
                  if (dataFilter.ky_hieu !== "") {
                    setStepId(2);
                  } else {
                    NotifyHelper.Error("Vui lòng chọn đầy đủ dữ liệu");
                  }
                }}
              />
            )}
            {stepId === 2 && (
              <Button
                variant="primary"
                type="button"
                text="Import"
                isLoading={isSaving}
                onClick={handleSubmit}
              />
            )}
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default ChungTuImportModal;
