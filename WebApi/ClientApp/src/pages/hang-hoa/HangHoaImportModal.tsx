import { DownloadIcon } from "@primer/octicons-react";
import { Box, Checkbox, FormControl, Link } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { hangHoaApi } from "../../api/category/hangHoaApi";
import { appInfo } from "../../AppInfo";
import Files from "../../component-data/files/Files";
import Steps from "../../component-data/steps";
import { IStepData } from "../../component-data/steps/Steps";
import Upload from "../../component-data/upload";
import Button from "../../component-ui/button";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import PlaceHolder from "../../component-ui/place-holder";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
import { eSize } from "../../models/commons/eSize";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";

interface IValidateDataResultProps {
  file: IUploadRespone;
  onValidDone: (isValid: "" | "success" | "error", dataSource: any[]) => void;
}
interface IHangHoaImportModalProps {
  onClose: () => void;
  onSuccess: () => void;
}
const _steps: IStepData[] = [
  {
    id: 1,
    name: "Upload file",
    is_active: false,
  },
  {
    id: 2,
    name: "Kiểm tra",
    is_active: false,
  },
  {
    id: 3,
    name: "Import",
    is_active: false,
  },
];
const HangHoaImportModal = (props: IHangHoaImportModalProps) => {
  const [stepId, setStepId] = useState(1);
  const [isSaving, setIsSaving] = useState(false);

  const [uploadedFile, setUploadedFile] = useState<IUploadRespone>();
  const [isDataVilid, setIsDataVilid] = useState<"" | "success" | "error">("");
  const [dataSource, setDataSource] = useState<any[]>([]);

  const width = useMemo(() => {
    if (stepId === 1) {
      return "xlarge";
    }
    // if (stepId === 2) {
    //     return "xlarge";
    // }
    if (stepId === 3) {
      return "xlarge";
    }

    return "90%%";
  }, [stepId]);

  const handleSubmit = async () => {
    if (isDataVilid !== "success") {
      NotifyHelper.Warning("Vui lòng đảm bảo dữ liệu hợp lệ");
    } else {
      if (uploadedFile) {
        setIsSaving(true);
        const res = await hangHoaApi.importData({
          ...uploadedFile,
        });
        setIsSaving(false);
        if (res.is_success) {
          NotifyHelper.Success("Import success");
          props.onSuccess();
        } else {
          NotifyHelper.Error(res.message ?? "Error");
        }
      }
    }
  };
  return (
    <Modal
      title={"Import"}
      onClose={() => {
        props.onClose();
      }}
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
              <Box>
                <Box>
                  {uploadedFile && (
                    <Box>
                      <Heading text="File đã upload" />
                      <Files
                        files={[uploadedFile]}
                        isPreviewImg={false}
                        onFileRemove={() => {
                          setUploadedFile(undefined);
                        }}
                      />
                    </Box>
                  )}
                  <Upload
                    onUploadSuccess={(data) => {
                      setUploadedFile(data);
                      setStepId(2);
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
                  <Link
                    href={`${appInfo.baseApiURL.replace(
                      "/api",
                      ""
                    )}/Template/hang-hoa.xlsx`}
                    target="_blank"
                  >
                    <Button
                      text="Tải file mẫu"
                      size="medium"
                      variant="invisible"
                      leadingVisual={DownloadIcon}
                    />
                  </Link>
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
            {stepId === 2 && uploadedFile && (
              <Box>
                <ValidateDataResult
                  file={uploadedFile}
                  onValidDone={(isValid, data) => {
                    setIsDataVilid(isValid);
                    setDataSource(data);
                  }}
                />
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

const ValidateDataResult = (props: IValidateDataResultProps) => {
  const [isLoading, setIsLoading] = useState(false);
  const [dataSource, setDataSource] = useState<any[]>([]);
  const [isOnlyShowErrData, setIsOnlyShowErrData] = useState(false);
  // console.log({
  //     dataSource
  // });

  const errData = useMemo(() => {
    return dataSource.filter((x) => x.ma_loi !== undefined && x.ma_loi !== "");
  }, [dataSource]);
  useEffect(() => {
    if (errData.length > 0 || dataSource.length <= 0) {
      props.onValidDone("error", []);
    } else {
      props.onValidDone(
        "success",
        dataSource.map((x) => ({ ...x, thue_vat: x.thue_suat }))
      );
    }
  }, [errData, dataSource]);
  useEffect(() => {
    handleRead();
  }, [props.file.url]);
  const handleRead = async () => {
    setIsLoading(true);
    const res = await hangHoaApi.validImport({
      ...props.file,
    });
    setIsLoading(false);
    if (res.is_success) {
      setDataSource(res.data);
    } else {
      NotifyHelper.Error("Error");
    }
  };
  return (
    <Box
      sx={{
        height: window.innerHeight - 300,
        overflow: "scroll",
        minWidth: "800px",
      }}
    >
      {isLoading && <PlaceHolder line_number={10} />}
      <DataTable
        // titleComponent={<Heading text='Danh sách hàng hóa' />}
        titleComponent={
          <>
            {errData.length > 0 && (
              <FormControl>
                <Checkbox
                  checked={isOnlyShowErrData}
                  onChange={(e) => {
                    setIsOnlyShowErrData(e.target.checked);
                  }}
                />
                <FormControl.Label>
                  Chỉ hiển thị các dòng không hợp lệ
                </FormControl.Label>

                <FormControl.Caption>
                  <FormControl.Validation variant="error">
                    Có {errData.length} dòng không hợp lệ
                  </FormControl.Validation>
                </FormControl.Caption>
              </FormControl>
            )}
          </>
        }
        data={isOnlyShowErrData ? errData : dataSource}
        height={window.innerHeight - 300}
        isLoading={isLoading}
        actionComponent={
          <>
            <Heading
              text={`Tổng số: ${dataSource.length} bản ghi`}
              size={eSize.medium}
            />
          </>
        }
        columns={
          dataSource.length > 0
            ? [
                {
                  header: "Kết quả",
                  field: "ma_loi",
                  rowHeader: false,
                  minWidth: "200px",
                  renderCell: (data: any) => {
                    return (
                      <>
                        {data.ma_loi && (
                          <FormControl.Validation variant="error">
                            <Box
                              sx={{ whiteSpace: "break-spaces" }}
                              className="limit2Line"
                            >
                              {data.ma_loi}
                            </Box>
                          </FormControl.Validation>
                        )}
                        {!data.ma_loi && (
                          <FormControl.Validation variant="success"></FormControl.Validation>
                        )}
                      </>
                    );
                  },
                },
                ...Object.keys(dataSource[0])
                  .filter((x) => x != "ma_loi")
                  .map((x) => {
                    return {
                      header: x,
                      field: x,
                      rowHeader: false,
                      // minWidth: "200px",
                    };
                  }),
              ]
            : []
        }
      />
    </Box>
  );
};

export default HangHoaImportModal;
