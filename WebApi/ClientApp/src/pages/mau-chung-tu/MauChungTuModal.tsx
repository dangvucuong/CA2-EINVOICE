import { useEffect, useState } from "react";
import Modal from "../../component-ui/modal";
import { set, useForm } from "react-hook-form";
import { Box, FormControl, Select } from "@primer/react";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import moment from "moment";
import { useAuth } from "../../hooks/useAuth";
import Button from "../../component-ui/button";
import UploadImage from "./UploadImage";
import { axiosClient } from "../../api/axiosClient";
import { memo } from "react";
import { NotifyHelper } from "../../helpers/toast";
import { parseSoapResponse } from "../../helpers/common";

function MauChungTuModal({
  openModal,
  onClose,
  onRefresh,
  dataEdit,
}: {
  openModal: boolean;
  onClose: () => void;
  onRefresh: () => void;
  dataEdit: any;
}) {
  const {
    register,
    handleSubmit,
    control,
    watch,
    setValue,
    trigger,
    formState: { errors },
    reset,
    getValues,
  } = useForm<any>({
    shouldUseNativeValidation: false,
    defaultValues: {},
  });
  const { user } = useAuth();
  const [dsmauhienthi, setDsmauhienthi] = useState<any[]>([]);
  const [loaiChungTu, setLoaiChungTu] = useState("");
  const [mauhienthi, setMauhienthi] = useState("");
  const [logo, setLogo] = useState({
    base64: "",
    fileName: "",
  });
  const [anhnen, setAnhnen] = useState({
    base64: "",
    fileName: "",
  });
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (!openModal) return;

    // Nếu có dữ liệu chỉnh sửa thì reset theo dataEdit, ngược lại reset trắng
    if (dataEdit) {
      reset({
        so_quyet_dinh: dataEdit?.SoQD,
        ngay_quyet_dinh: dataEdit?.NgayQD
          ? moment(dataEdit?.NgayQD).format("YYYY-MM-DD")
          : moment().format("YYYY-MM-DD"),
        mau_so: dataEdit?.Mauso,
        ngay_tao_mau: dataEdit?.ThoigianPH
          ? moment(dataEdit?.ThoigianPH).format("YYYY-MM-DD")
          : moment().format("YYYY-MM-DD"),
      });
      setLoaiChungTu(dataEdit?.MaloaiHD);
    } else {
      reset({
        mau_so: "03/TNCN",
      });
      setLoaiChungTu("03/TNCN");
    }

    GetTemplateFiles("");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [openModal, dataEdit]);

  const handleLoaiChungTuChange = (value: string) => {
    setLoaiChungTu(value);
    setValue("mau_so", value);
  };

  const onSubmit = async (data: any) => {
    const payload = {
      ten_cong_ty: data.ten_cong_ty,
      mau_so: data.mau_so,
      ngay_tao_mau: data.ngay_tao_mau,
      so_quyet_dinh: data.so_quyet_dinh,
      ngay_quyet_dinh: data.ngay_quyet_dinh,
      loai_chung_tu: loaiChungTu,
      mau_hien_thi: dsmauhienthi?.find((x) => x.Filepath === mauhienthi),
      logo: logo,
      anh_nen: anhnen,
    };

    setIsSaving(true);

    if (dataEdit) {
      await SuaMauChungTu({
        ...payload,
        idmau: dataEdit?.IDMau,
      });
    } else {
      await TaoMauChungTu(payload);
    }

    setIsSaving(false);
  };

  const TaoMauChungTu = async (payload: any) => {
    const logoPath = await uploadLogoMau({
      base64: logo.base64,
      fileName: logo.fileName,
    });

    const anhnenPath = await uploadBgMau({
      base64: anhnen.base64,
      fileName: anhnen.fileName,
    });

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <TaoMauChungTu xmlns="http://tempuri.org/">
      <NgayQD>${moment(payload?.ngay_quyet_dinh).format("YYYY-MM-DD")}</NgayQD>
      <khmauso>${payload?.mau_so}</khmauso>
      <tenhd>${"Chứng từ khấu trừ thuế thu nhập cá nhân theo ND70"}</tenhd>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <ctbg>${anhnenPath}</ctbg>
      <ctlogo>${logoPath}</ctlogo>
      <cmbTemplateText>${payload?.mau_hien_thi?.name}</cmbTemplateText>
      <cmbTemplateValue>${payload?.mau_hien_thi?.Filepath}</cmbTemplateValue>
      <loaichungtu>${payload?.mau_so}</loaichungtu>
      <soquyetdinh>${payload?.so_quyet_dinh}</soquyetdinh>
    </TaoMauChungTu>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      onRefresh();
      onClose();
      NotifyHelper.Success(parseRes.message);
    }
  };

  const SuaMauChungTu = async (payload: any) => {
    const logoPath = await uploadLogoMau({
      base64: logo.base64,
      fileName: logo.fileName,
    });

    const anhnenPath = await uploadBgMau({
      base64: anhnen.base64,
      fileName: anhnen.fileName,
    });

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <SuaMauChungTu  xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <ctbg>${anhnenPath}</ctbg>
      <ctlogo>${logoPath}</ctlogo>
      <soquyetdinh>${payload?.so_quyet_dinh}</soquyetdinh>
      <NgayQD>${moment(payload?.ngay_quyet_dinh).format("YYYY-MM-DD")}</NgayQD>
      <idmau>${dataEdit?.idMauHD}</idmau>
    </SuaMauChungTu>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      onRefresh();
      onClose();
      NotifyHelper.Success(parseRes.message);
    }
  };

  const XemTruocMau = async () => {
    setIsSaving(true);

    let logoPath = "";
    let anhnenPath = "";

    if (dataEdit) {
      logoPath = dataEdit?.Logo;
      anhnenPath = dataEdit?.Nen;
    } else {
      logoPath = await uploadLogoMau({
        base64: logo.base64,
        fileName: logo.fileName,
      });
      anhnenPath = await uploadBgMau({
        base64: anhnen.base64,
        fileName: anhnen.fileName,
      });
    }

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <XemTruocMau xmlns="http://tempuri.org/">
      <loaichungtu>${loaiChungTu}</loaichungtu>
      <id_chitiet>${15}</id_chitiet>
      <mauhienthi>${mauhienthi}</mauhienthi>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <tenchungtu>${
        dsmauhienthi?.find((x) => x.Filepath === mauhienthi)?.name
      }</tenchungtu>
      <ctbg>${anhnenPath}</ctbg>
      <ctlogo>${logoPath}</ctlogo>
    </XemTruocMau>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      document.getElementById("preview-mauct")!.innerHTML = parseRes.data || "";
    } else {
      NotifyHelper.Error(parseRes.message);
    }
    setIsSaving(false);
  };

  const GetTemplateFiles = async (folderName?: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <GetTemplateFiles xmlns="http://tempuri.org/">
      <folderName>${folderName}</folderName>
    </GetTemplateFiles>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setDsmauhienthi(
        Array.isArray(parseRes.data) ? parseRes.data : [parseRes.data],
      );
      setMauhienthi(
        Array.isArray(parseRes.data) && parseRes.data.length > 0
          ? parseRes.data[0].Filepath
          : "",
      );
    }
  };

  const uploadLogoMau = async ({
    base64,
    fileName,
  }: {
    base64: string;
    fileName: string;
  }) => {
    if (!base64 || !fileName) return "";

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <UploadLogoMau xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <base64File>${base64}</base64File>
      <fileName>${fileName}</fileName>
    </UploadLogoMau>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      return parseRes.data;
    }
  };

  const uploadBgMau = async ({
    base64,
    fileName,
  }: {
    base64: string;
    fileName: string;
  }) => {
    if (!base64 || !fileName) return "";

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <UploadBgMau  xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <base64File>${base64}</base64File>
      <fileName>${fileName}</fileName>
    </UploadBgMau>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      return parseRes.data;
    }
  };

  console.log(dataEdit);

  return (
    <Modal
      isOpen={openModal}
      onClose={onClose}
      title={"Thêm mới mẫu chứng từ"}
      sx={{ width: window.innerWidth - 200 }}
    >
      <Box>
        <p>Thông tin mẫu chứng từ</p>

        <Box
          sx={{
            padding: "0 20px",
          }}
        >
          <form onSubmit={handleSubmit(onSubmit)} noValidate={true}>
            <Box
              sx={{
                display: "grid",
                gap: 20,
                gridTemplateColumns: "1fr 1fr",
              }}
            >
              <Box sx={{ "& > * + *": { mt: 2 } }}>
                <Box>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tên công ty" />
                    </FormControl.Label>
                    <TextInput
                      width={"100%"}
                      register={register}
                      name="ten_cong_ty"
                      readOnly
                      validateMessage="Vui lòng điền Tên công ty"
                      errors={errors}
                      value={user?.donvi?.ten_dv}
                    />
                  </FormControl>
                </Box>

                <Box sx={{ mt: 3 }}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Loại chứng từ" />
                    </FormControl.Label>
                    <Select
                      width={"100%"}
                      block
                      value={loaiChungTu}
                      onChange={(e: any) =>
                        handleLoaiChungTuChange(e.target.value)
                      }
                      disabled={dataEdit ? true : false}
                    >
                      <Select.Option value="03/TNCN">
                        Chứng từ khấu trừ thuế thu nhập cá nhân theo ND70
                      </Select.Option>
                    </Select>
                  </FormControl>
                </Box>

                <Box>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Chọn mẫu hiển thị" />
                    </FormControl.Label>
                    <Select
                      block
                      value={mauhienthi}
                      disabled={dataEdit ? true : false}
                      onChange={(e) => {
                        setMauhienthi(e.target.value);
                      }}
                    >
                      {dsmauhienthi?.map((item: any, index: number) => (
                        <Select.Option key={index} value={item.Filepath}>
                          {item.name}
                        </Select.Option>
                      ))}
                    </Select>
                  </FormControl>
                </Box>

                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: 20,
                    mt: 4,
                  }}
                >
                  <UploadImage
                    id="upload-bg"
                    noImageText="Chọn ảnh nền"
                    onChangeValue={(value, fileName) =>
                      setAnhnen({ base64: value, fileName })
                    }
                    defaultImage={
                      dataEdit
                        ? process.env.REACT_APP_URL_CHUNG_TU + dataEdit?.Nen
                        : undefined
                    }
                  />
                  <UploadImage
                    id="upload-logo"
                    noImageText="Chọn logo"
                    onChangeValue={(value, fileName) =>
                      setLogo({ base64: value, fileName })
                    }
                    defaultImage={
                      dataEdit
                        ? process.env.REACT_APP_URL_CHUNG_TU + dataEdit?.Logo
                        : undefined
                    }
                  />
                </Box>
              </Box>

              <Box sx={{ "& > * + *": { mt: 2 } }}>
                <Box>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Mẫu số hóa đơn" />
                    </FormControl.Label>
                    <TextInput
                      width={"100%"}
                      register={register}
                      name="mau_so"
                      readOnly
                      validateMessage="Vui lòng điền Mẫu số hóa đơn"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Ngày tạo mẫu:" />
                    </FormControl.Label>

                    <TextInput
                      register={register}
                      name="ngay_tao_mau"
                      type="date"
                      width={"100%"}
                      required
                      validateMessage="Vui lòng điền Ngày tạo mẫu"
                      errors={errors}
                      defaultValue={moment().format("YYYY-MM-DD")}
                      disabled={dataEdit ? true : false}
                      // value={}
                    />
                  </FormControl>
                </Box>

                <Box sx={{ display: "flex", gap: 3 }}>
                  <Box>
                    <FormControl>
                      <FormControl.Label>
                        <Text text="Số quyết định" />
                      </FormControl.Label>
                      <TextInput
                        width={"100%"}
                        register={register}
                        name="so_quyet_dinh"
                        validateMessage="Vui lòng điền Số quyết định"
                        errors={errors}
                        type="number"
                      />
                    </FormControl>
                  </Box>
                  <Box>
                    <FormControl>
                      <FormControl.Label>
                        <Text text="Ngày quyết định:" />
                      </FormControl.Label>

                      <TextInput
                        register={register}
                        name="ngay_quyet_dinh"
                        type="date"
                        width={"100%"}
                        required
                        validateMessage="Vui lòng điền Ngày quyết định"
                        errors={errors}
                        defaultValue={moment().format("YYYY-MM-DD")}
                      />
                    </FormControl>
                  </Box>
                </Box>

                <Box
                  sx={{ display: "flex", justifyContent: "flex-end", mt: 4 }}
                >
                  <Button
                    text="Lưu"
                    isLoading={isSaving}
                    type="submit"
                    sx={{ mr: 2, minWidth: "100px" }}
                    size="large"
                    variant="primary"
                  />

                  <Button
                    text="Xem trước"
                    isLoading={isSaving}
                    sx={{ mr: 2, minWidth: "100px" }}
                    size="large"
                    variant="primary"
                    onClick={XemTruocMau}
                  />
                </Box>
              </Box>
            </Box>
          </form>
        </Box>
      </Box>

      <Box>
        <p>Xem trước</p>

        <Box
          id="preview-mauct"
          sx={{
            "& > *": {
              position: "relative",
            },
          }}
        ></Box>
      </Box>
    </Modal>
  );
}

export default memo(MauChungTuModal);
