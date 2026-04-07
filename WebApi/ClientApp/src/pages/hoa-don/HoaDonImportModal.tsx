import { DownloadIcon } from "@primer/octicons-react";
import { Box, Checkbox, FormControl, Link, Radio, SubNav } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { appInfo } from "../../AppInfo";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Files from "../../component-data/files/Files";
import SelectBoxKyHieuPhatHanh from "../../component-data/selectbox-ky-hieu-phat-hanh";
import SelectBoxLoaiHoaDonCTPhatHanh from "../../component-data/selectbox-loai-hoa-don-ct-phat-hanh";
import SelectBoxMauSoPhatHanh from "../../component-data/selectbox-mau-so-phat-hanh";
import Steps from "../../component-data/steps";
import { IStepData } from "../../component-data/steps/Steps";
import Upload from "../../component-data/upload";
import Button from "../../component-ui/button";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import { NotifyHelper } from "../../helpers/toast";
import { useLoaiHoaDonCT } from "../../hooks/useLoaiHoaDonCT";
import { eSize } from "../../models/commons/eSize";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import PlaceHolder from "../../component-ui/place-holder";

interface IValidateDataResultProps {
  file: IUploadRespone;
  template: "" | "hoc_phi" | "nuoc";
  onValidDone: (isValid: "" | "success" | "error", dataSource: any[]) => void;
}
interface IHoaDonImportModalProps {
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
  {
    id: 3,
    name: "Kiểm tra",
    is_active: false,
  },
  {
    id: 4,
    name: "Import",
    is_active: false,
  },
];
const HoaDonImportModal = (props: IHoaDonImportModalProps) => {
  const [stepId, setStepId] = useState(1);
  const [isSaving, setIsSaving] = useState(false);

  const [uploadedFile, setUploadedFile] = useState<IUploadRespone>();
  const [isDataVilid, setIsDataVilid] = useState<"" | "success" | "error">("");
  const [dataSource, setDataSource] = useState<any[]>([]);
  const [loaiHoaDonChiTietId, setLoaiHoaDonChiTietId] = useState(0);
  const [mauSo, setMauSo] = useState("");
  const [kyHieu, setKyHieu] = useState("");
  const [template, setTemplate] = useState<"" | "hoc_phi" | "nuoc">("");
  // 0 là đa luồng, 1 là tuần tự
  const [importType, setImportType] = useState<"0" | "1">("0");

  const width = useMemo(() => {
    if (stepId === 1) {
      return "xlarge";
    }
    if (stepId === 2) {
      return "xlarge";
    }
    // if (stepId === 3) {
    //     return "xlarge";
    // }
    if (stepId === 4) {
      return "xlarge";
    }
    return "90%%";
  }, [stepId]);
  const { loaiHoaDonCT } = useLoaiHoaDonCT(loaiHoaDonChiTietId);
  const handleSubmit = async () => {
    if (isDataVilid !== "success") {
      NotifyHelper.Warning("Vui lòng đảm bảo dữ liệu hợp lệ");
    } else {
      if (uploadedFile) {
        setIsSaving(true);
        const res = await hoaDonApi.importFromExcel({
          ...uploadedFile,
          hoa_don_dang_ky_phat_hanh_ky_hieu: kyHieu,
          hoa_don_dang_ky_phat_hanh_mau_so: mauSo,
          loai_hoa_don_ct_id: loaiHoaDonChiTietId,
          ten_hoa_don: loaiHoaDonCT?.name ?? "",
          template: template,
          importType: importType,
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
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <FormControl>
                  <FormControl.Label>Loại hóa đơn</FormControl.Label>
                  <SelectBoxLoaiHoaDonCTPhatHanh
                    value={loaiHoaDonChiTietId}
                    onValueChanged={(id) => {
                      setLoaiHoaDonChiTietId(id);
                    }}
                  />
                </FormControl>
                <Box>
                  <FormControl>
                    <FormControl.Label>Mẫu số</FormControl.Label>
                    <SelectBoxMauSoPhatHanh
                      isAutoSelectIfHasOneItem
                      loai_hoa_don_ct_id={loaiHoaDonChiTietId}
                      value={mauSo}
                      onValueChanged={(id) => {
                        setMauSo(id);
                      }}
                    />
                  </FormControl>
                </Box>
                <Box>
                  <FormControl>
                    <FormControl.Label>Ký hiệu</FormControl.Label>
                    <SelectBoxKyHieuPhatHanh
                      loai_hoa_don_ct_id={loaiHoaDonChiTietId}
                      mau_so={mauSo}
                      value={kyHieu}
                      isAutoSelectIfHasOneItem
                      onValueChanged={(id) => {
                        setKyHieu(id);
                      }}
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
                      <SubNav.Link
                        selected={template === "hoc_phi"}
                        sx={{
                          cursor: "pointer",
                        }}
                        onClick={() => {
                          setTemplate("hoc_phi");
                        }}
                      >
                        Học phí
                      </SubNav.Link>
                      <SubNav.Link
                        selected={template === "nuoc"}
                        sx={{
                          cursor: "pointer",
                        }}
                        onClick={() => {
                          setTemplate("nuoc");
                        }}
                      >
                        Nước
                      </SubNav.Link>
                    </SubNav.Links>
                  </SubNav>
                </Box>
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
                      setStepId(3);
                    }}
                    //acept=".xls, .xlsx"
                    accept=".xls, .xlsx"
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
                    <Box
                      sx={{
                        display: "flex",
                        gap: 2,
                        width: "100%",
                        justifyContent: "center",
                      }}
                    >
                      <Link
                        href={`${appInfo.baseApiURL.replace(
                          "/api",
                          "",
                        )}/Template/Template-import-hoa-don.xlsx`}
                        target="_blank"
                      >
                        <Button
                          text="Tải file mẫu"
                          size="medium"
                          variant="invisible"
                          leadingVisual={DownloadIcon}
                        />
                      </Link>

                      <Link
                        href={`${appInfo.baseApiURL.replace(
                          "/api",
                          "",
                        )}/Template/Template-import-hoa-don-hh-dac-trung.xlsx`}
                        target="_blank"
                      >
                        <Button
                          text="Tải file mẫu hàng hóa đặc trưng"
                          size="medium"
                          variant="invisible"
                          leadingVisual={DownloadIcon}
                        />
                      </Link>
                    </Box>
                  )}
                  {template === "hoc_phi" && (
                    <Link
                      href={`${appInfo.baseApiURL.replace(
                        "/api",
                        "",
                      )}/Template/Template-import-hoa-don-hoc-phi.xlsx`}
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
                  {template === "nuoc" && (
                    <Link
                      href={`${appInfo.baseApiURL.replace(
                        "/api",
                        "",
                      )}/Template/Template-import-hoa-don-nuoc.xlsx`}
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
            {stepId === 3 && uploadedFile && (
              <Box>
                <Box
                  sx={{
                    display: "flex",
                    gap: 2,
                    pt: 1,
                    width: "auto",
                  }}
                >
                  <Box
                    display="flex"
                    alignItems="center"
                    sx={{
                      gap: 2,
                    }}
                  >
                    <Checkbox
                      value="1"
                      checked={importType === "1"}
                      onChange={(e) => {
                        setImportType(e.target.checked ? "1" : "0");
                      }}
                    />
                    <Text
                      text="Đẩy tuần tự dữ liệu excel."
                      sx={{
                        display: "block",
                      }}
                    ></Text>
                  </Box>
                </Box>
                <ValidateDataResult
                  file={uploadedFile}
                  template={template}
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
            {stepId === 1 && (
              <Button
                variant="primary"
                type="button"
                text="Tiếp tục"
                onClick={() => {
                  console.log({
                    loaiHoaDonChiTietId,
                    mauSo,
                    kyHieu,
                  });

                  if (loaiHoaDonChiTietId > 0 && mauSo != "" && kyHieu != "") {
                    setStepId(2);
                  } else {
                    NotifyHelper.Error("Vui lòng chọn đầy đủ dữ liệu");
                  }
                }}
              />
            )}
            {stepId === 3 && (
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
        dataSource.map((x) => ({ ...x, thue_vat: x.thue_suat })),
      );
    }
  }, [errData, dataSource]);
  useEffect(() => {
    handleRead();
  }, [props.file.url]);
  const handleRead = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.readFromExcel({
      ...props.file,
      hoa_don_dang_ky_phat_hanh_ky_hieu: "",
      hoa_don_dang_ky_phat_hanh_mau_so: "",
      loai_hoa_don_ct_id: 0,
      template: props.template,
      ten_hoa_don: "",
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
        // actionComponent={
        //   <>
        //     <Heading
        //       text={`Tổng số: ${dataSource.length} bản ghi`}
        //       size={eSize.medium}
        //     />
        //   </>
        // }
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

export default HoaDonImportModal;
