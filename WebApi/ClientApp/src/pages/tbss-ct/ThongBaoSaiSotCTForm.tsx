import { Box, FormControl } from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useParams } from "react-router-dom";
import {
  THONG_BAO_SAI_SOT_API_PHAT_HANH,
  thongBaoSaiSotApi,
} from "../../api/tbss/thongBaoSaiSotApi";
import KySoModal from "../../component-data/ky-so-modal";
import SelectBoxCoQuanThue from "../../component-data/selectbox-co-quan-thue";
import Button from "../../component-ui/button";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { ICoQuanThue } from "../../models/responses/category/ICoQuanThue";
import { ITBSSPhatHanhPushNotifyModel } from "../../models/responses/hub/TBSSPhatHanhPushNotifyModel";
import TBSSPhatHanhResultModal from "./TBSSPhatHanhResultModal";
import ThongBaoSaiSotFormHoaDon from "./ThongBaoSaiSotDSChungTu";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { coQuanThueApi } from "../../api/category/coQuanThueApi";
import ThongbaoSaiSotCTLoaiTB from "./ThongbaoSaiSotCTLoaiTB";
import DateInput from "../../component-ui/date-input";
import SelectBoxLoaiChungTuPhatHanh from "../../component-data/selectbox-loai-chung-tu-phat-hanh";
import ThongBaoSaiSotDSChungTu from "./ThongBaoSaiSotDSChungTu";
import SelectBoxMauSoChungTuPhatHanh from "../../component-data/selectbox-mau-so-chung-tu-phat-hanh";
import SelectBoxSoChungTuPhatHanh from "../../component-data/selectbox-so-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import { parseSoapResponse } from "../../helpers/common";
import { axiosClient } from "../../api/axiosClient";

const ThongBaoSaiSotCTForm = () => {
  const { user } = useAuth();
  const [viewDataModel, setViewDataModel] = useState<any>();
  const { checkAccesiableTo } = useCommonContext();
  const {
    register,
    clearErrors,
    setError,
    handleSubmit,
    reset,
    setValue,
    formState: { errors },
  } = useForm<any>({
    shouldUseNativeValidation: false,
    defaultValues: {},
  });
  const isAllowPhatHanh = useMemo(() => {
    return checkAccesiableTo(THONG_BAO_SAI_SOT_API_PHAT_HANH, "POST");
  }, []);
  const { id: pId }: any = useParams();
  const [thongBaoSaiSotId, setThongBaoSaiSotId] = useState<number>(
    parseInt(pId)
  );
  const [base64KySo, setBase64KySo] = useState("");

  const [coQuanThueId, setCoQuanThueId] = useState(0);
  const [coQuanThue, setCoQuanThue] = useState<ICoQuanThue>();
  const [tinhChatThongBaoId, setTinhChatThongBaoId] = useState(0);
  const [thongBaoSaiSotChiTiets, setThongBaoSaiSotChiTiets] = useState<any[]>(
    []
  );
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const history = useHistory();
  const { signalRConnectionServer } = useCommonContext();
  const [isShowPhatHanhResultModal, setIsShowPhatHanhResultModal] =
    useState(false);
  const [tbssPhatHanhPushNotifyModel, setTBSSPhatHanhPushNotifyModel] =
    useState<ITBSSPhatHanhPushNotifyModel>();
  const [dataForm, setDataForm] = useState<any>({
    loai_thong_bao: 1,
    loai_chung_tu_dien_tu: "",
    mau_so: "03/TNCN",
    ky_hieu: "",
    so_chung_tu: "",
  });

  const handleSearchSelectedAsync = async (id: number) => {
    const res = await coQuanThueApi.selectById(id);
    if (res.is_success) {
      setCoQuanThue(res.data);
      // console.log({
      //     CQT: res.data
      // });

      setValue("dia_danh", res.data?.tinh ?? "");
    }
  };

  useEffect(() => {
    if (signalRConnectionServer) {
      signalRConnectionServer.on("TBSS_HAS_RESULT", (message: any) => {
        // console.log({
        //     TBSS_HAS_RESULT: message
        // });
        onTBSSPhatHanhHasResult(message);
        // const notify: INotifyUser = JSON.parse(message);
        // setNewestNotify(notify);
        // handleGetTotalUnread();
        // setTimeout(() => {
        //     setNewestNotify(undefined)
        // }, 10000)
        // NotifyHelper.Success(notify.title)
        // console.log({
        //     message
        // });
      });
    }
  }, [signalRConnectionServer]);

  const onTBSSPhatHanhHasResult = (message: ITBSSPhatHanhPushNotifyModel) => {
    if (message.id === thongBaoSaiSotId || true) {
      setIsShowPhatHanhResultModal(true);
      setTBSSPhatHanhPushNotifyModel(message);
      if (viewDataModel) {
        setViewDataModel({
          ...viewDataModel,
          thong_bao_sai_sot_trang_thai_id:
            message.thong_bao_sai_sot_trang_thai_id,
        });
      }
    }
  };

  useEffect(() => {
    if (thongBaoSaiSotId > 0) {
      handleGetDetailAsync();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [thongBaoSaiSotId]);

  const handlePhatHanhAsync = async (signedtext: string) => {};

  const handleGetBase64KySo = async (madonvi: string, matbss: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <LayXmlTBSSChungTu xmlns="http://tempuri.org/">
        <madonvi>${madonvi}</madonvi>
        <matbss_ct>${matbss}</matbss_ct>
      </LayXmlTBSSChungTu>
    </soap12:Body>
  </soap12:Envelope>`;

    // setIsSaving(true);
    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );
    // setIsSaving(false);

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setBase64KySo(parseRes.data);
      setIsShowKySoModal(true);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleKySoRemoteAsync = async () => {};

  const onSubmit = async (data: any) => {
    let isValid: boolean = true;

    if (coQuanThueId <= 0 || !coQuanThue || !coQuanThue.ma_cqt) {
      setError("ma_cqt", {
        type: "manual",
      });
      isValid = false;
    }
    if (dataForm?.loai_chung_tu_dien_tu === "") {
      setError("loai_chung_tu_dien_tu", {
        type: "manual",
      });
      isValid = false;
    }
    if (dataForm?.loai_thong_bao <= 0) {
      setError("loai_thong_bao", {
        type: "manual",
      });
      isValid = false;
    }
    if (thongBaoSaiSotChiTiets.length <= 0) {
      setError("thong_bao_sai_sot_chi_tiets", {
        type: "manual",
      });
      isValid = false;
    }
    if (!isValid) return;

    const objTTChung = {
      PBan: "2.1.0",
      MSo: "04/SS-CTĐT",
      Ten: "Thông báo chứng từ điện tử đã lập sai",
      Loai: dataForm.loai_thong_bao,
      So: data.so_thong_bao,
      NTBCCQT: data.ngay_cap
        ? moment(data.ngay_cap, "DD/MM/YYYY").format("YYYY-MM-DD")
        : "",
      NTBao: moment().format("YYYY-MM-DD"),
      // MCQT: coQuanThue?.ma_cqt,
      // TCQT: coQuanThue?.ten,
      MCQT: "97100",
      TCQT: "CQT test 3",
      TNNT: user?.donvi.ten_dv,
      MST: user?.donvi.mst,
      DDanh: data.dia_danh,
      Taikhoan: user?.donvi.ma_dv,
      SerialNo: user?.serial_number,
    };

    const sjsonTTCTS = thongBaoSaiSotChiTiets.map((item, index) => ({
      STT: index + 1,
      KHMSCTu: item.mau_so,
      KHCTu: item.ky_hieu,
      SCTu: item.so_chung_tu,
      NLap: item.ngay_lap ? moment(item.ngay_lap).format("YYYY-MM-DD") : "",
      LCTDT: 1,
      LDo: item.ly_do,
    }));

    console.log(objTTChung);
    console.log(sjsonTTCTS);

    await TaoThongBaoSaiSot(
      JSON.stringify(objTTChung),
      JSON.stringify(sjsonTTCTS)
    );
  };

  const TaoThongBaoSaiSot = async (objTTChung: string, sjsonTTCTS: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <TaoThongBaoSaiSot xmlns="http://tempuri.org/">
     <sjsonTTChungTBSS>${objTTChung}</sjsonTTChungTBSS>
      <sjsonTTCT>${sjsonTTCTS}</sjsonTTCT>
    </TaoThongBaoSaiSot>
  </soap12:Body>
</soap12:Envelope>`;
    setIsSaving(true);

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
      // NotifyHelper.Success(parseRes.message);
      // history.push(`../../tbss-ct/${parseRes.data}`);

      setThongBaoSaiSotId(parseInt(parseRes.data));
      if (user) {
        handleGetBase64KySo(user?.donvi_ma_dv, parseRes?.data);
      }
    } else {
      NotifyHelper.Error(parseRes.message);
    }

    setIsSaving(false);
  };

  const handleGetDetailAsync = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Laythongtintbsschungtu xmlns="http://tempuri.org/">
      <matbss_ct>${thongBaoSaiSotId}</matbss_ct>
      <madonvi>${user?.donvi_ma_dv}</madonvi>
    </Laythongtintbsschungtu>
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
      const TBSSChungtu = parseRes.data?.TBSSChungtu[0];
      const TBSSChungtuchitiet = parseRes.data?.TBSSChungtuchitiet;

      setViewDataModel(TBSSChungtu);

      reset({
        dia_danh: TBSSChungtu.DDanh,
      });
      setDataForm({
        loai_thong_bao: TBSSChungtu.Loai,
      });

      setThongBaoSaiSotChiTiets(
        TBSSChungtuchitiet?.map((item: any) => ({
          id: 0,
          mau_so: item.KHMSCTu,
          ky_hieu: item.KHCTu,
          so_chung_tu: item.SCTu,
          ngay_lap: moment(item.NLap).format("YYYY-MM-DD"),
          ly_do: item.LDo,
        }))
      );
    } else {
    }
  };

  const UpdateChungTuTBSSSauKy = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <UpdateChungTuTBSSSauKy xmlns="http://tempuri.org/">
        <xmlthongdiep>${values?.xmldaky}</xmlthongdiep>
        <trangthai>${values?.trangthai}</trangthai>
        <madonvi>${values?.mst}</madonvi>
        <matbsschungtu>${values?.matbsschungtu}</matbsschungtu>
      </UpdateChungTuTBSSSauKy>
    </soap12:Body>
  </soap12:Envelope>`;

    setIsSaving(true);
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
    setIsSaving(false);

    if (parseRes.status === "success") {
      await GuiTBSSLenCQT({
        matbsschungtu: values?.matbsschungtu,
        madonvi: user?.donvi.ma_dv,
        signedtext: values?.xmldaky,
      });
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const GuiTBSSLenCQT = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <GuiTBSSLenCQT xmlns="http://tempuri.org/">
      <signedtext>${values?.signedtext}</signedtext>
      <madonvi>${values?.madonvi}</madonvi>
      <matbsschungtu>${values?.matbsschungtu}</matbsschungtu>
    </GuiTBSSLenCQT>
  </soap12:Body>
</soap12:Envelope>`;

    setIsSaving(true);
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
    setIsSaving(false);

    await handleGetDetailAsync();

    if (parseRes.status === "success") {
      NotifyHelper.Success(parseRes.message);
    } else {
      NotifyHelper.Error(parseRes.message ?? "Có lỗi xảy ra");
    }
  };

  return (
    <Box>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <FormControl>
            <FormControl.Label>
              <Text text="Cơ quan thuế" />
            </FormControl.Label>
            <SelectBoxCoQuanThue
              value={coQuanThueId}
              onValueChanged={(id: number, data?: ICoQuanThue) => {
                setCoQuanThueId(id);
                setCoQuanThue(data);
                setValue("dia_danh", data?.tinh ?? "");
                clearErrors("ma_cqt");
                clearErrors("dia_danh");
              }}
            />
            {errors && errors["ma_cqt"] && (
              <FormControl.Validation id={"ma_cqt"} variant="error">
                Vui lòng chọn cơ quan thuế
              </FormControl.Validation>
            )}
          </FormControl>
          <Box sx={{ display: "grid" }} gridTemplateColumns={"2fr 1fr 1fr"}>
            <FormControl>
              <FormControl.Label>
                <Text text="Tên người nộp thuế" />
              </FormControl.Label>
              <TextInput value={user?.donvi.ten_dv} block readOnly />
            </FormControl>
            <FormControl sx={{ ml: 3 }}>
              <FormControl.Label>
                <Text text="Mã số thuế" />
              </FormControl.Label>
              <TextInput value={user?.donvi.mst} block readOnly />
            </FormControl>
            <FormControl sx={{ ml: 3 }}>
              <FormControl.Label>
                <Text text="Địa danh" />
              </FormControl.Label>
              <TextInput
                name="dia_danh"
                block
                register={register}
                required
                validateMessage="Vui lòng điền Địa danh"
                errors={errors}
              />
            </FormControl>
          </Box>

          <FormControl>
            <FormControl.Label>
              <Text text="Loại thông báo" />
            </FormControl.Label>
            <ThongbaoSaiSotCTLoaiTB
              value={dataForm.loai_thong_bao}
              onValueChanged={(value) => {
                setDataForm({ ...dataForm, loai_thong_bao: value });
                // clearErrors("loai_hoa_don_dien_tu_id");
                clearErrors("loai_thong_bao");
              }}
            />
            {errors && errors["loai_thong_bao"] && (
              <FormControl.Validation id={"loai_thong_bao"} variant="error">
                Vui lòng chọn loại thông báo
              </FormControl.Validation>
            )}

            {dataForm.loai_thong_bao === 2 && (
              <Box
                sx={{ display: "grid", gap: 3 }}
                gridTemplateColumns={"1fr 1fr"}
              >
                <FormControl>
                  <FormControl.Label>
                    <Text text="Số thông báo" />
                  </FormControl.Label>
                  <TextInput
                    name="so_thong_bao"
                    register={register}
                    value={""}
                  />
                </FormControl>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Ngày thông báo" />
                  </FormControl.Label>
                  <DateInput
                    name="ngay_cap"
                    register={register}
                    required
                    value={moment(new Date()).format("DD/MM/YYYY")}
                    onValueChanged={(value, date) => {}}
                    width={"100%"}
                  />
                </FormControl>
              </Box>
            )}
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Loại chứng từ điện tử" />
            </FormControl.Label>
            <SelectBoxLoaiChungTuPhatHanh
              value={dataForm?.loai_chung_tu_dien_tu}
              onValueChanged={(value) => {
                setDataForm({
                  ...dataForm,
                  loai_chung_tu_dien_tu: value,
                });
                clearErrors("loai_chung_tu_dien_tu");
              }}
            />
            {errors && errors["loai_chung_tu_dien_tu"] && (
              <FormControl.Validation
                id={"loai_chung_tu_dien_tu"}
                variant="error"
              >
                Vui lòng chọn loại chứng từ điện tử
              </FormControl.Validation>
            )}
          </FormControl>

          <Box display={"flex"} sx={{ gap: 3, alignItems: "center" }}>
            <FormControl>
              <FormControl.Label>
                <Text text="Mẫu số" />
              </FormControl.Label>
              <SelectBoxMauSoChungTuPhatHanh
                value={dataForm?.mau_so}
                onValueChanged={(value) => {
                  setDataForm({ ...dataForm, mauso: value });
                  clearErrors("mau_so");
                }}
                loai_chung_tu={dataForm?.loai_chung_tu_dien_tu}
              />
              {errors && errors["mau_so"] && (
                <FormControl.Validation id={"mau_so"} variant="error">
                  Vui lòng chọn mẫu số
                </FormControl.Validation>
              )}
            </FormControl>

            <FormControl>
              <FormControl.Label>
                <Text text="Ký hiệu" />
              </FormControl.Label>
              <SelectBoxKyHieuChungTuPhatHanh
                value={dataForm?.ky_hieu}
                onValueChanged={(value) => {
                  setDataForm({ ...dataForm, ky_hieu: value });
                  clearErrors("ky_hieu");
                }}
                mau_so={dataForm?.mau_so}
              />
              {errors && errors["ky_hieu"] && (
                <FormControl.Validation id={"ky_hieu"} variant="error">
                  Vui lòng chọn ký hiệu
                </FormControl.Validation>
              )}
            </FormControl>

            <FormControl>
              <FormControl.Label>
                <Text text="Số chứng từ" />
              </FormControl.Label>
              <SelectBoxSoChungTuPhatHanh
                value={dataForm?.so_chung_tu}
                onValueChanged={(data: any) => {
                  setDataForm({ ...dataForm, so_chung_tu: data?.value });
                  clearErrors("so_chung_tu");
                }}
                mau_so={dataForm?.mau_so}
                ky_hieu={dataForm?.ky_hieu}
              />
              {errors && errors["so_chung_tu"] && (
                <FormControl.Validation id={"so_chung_tu"} variant="error">
                  Vui lòng chọn số chứng từ
                </FormControl.Validation>
              )}
            </FormControl>

            <Button
              size="medium"
              onClick={() => {
                if (dataForm?.so_chung_tu === "") {
                  return;
                }

                setThongBaoSaiSotChiTiets((prev) => {
                  if (
                    prev?.find(
                      (x) =>
                        x.mau_so === dataForm.mau_so &&
                        x.ky_hieu === dataForm.ky_hieu &&
                        x.so_chung_tu === dataForm.so_chung_tu
                    )
                  ) {
                    NotifyHelper.Error("Chứng từ đã được thêm");
                    return [...prev];
                  }
                  return [
                    ...prev,
                    {
                      id: 0,
                      mau_so: dataForm.mau_so,
                      ky_hieu: dataForm.ky_hieu,
                      so_chung_tu: dataForm.so_chung_tu,
                      ngay_lap: moment().format("YYYY-MM-DD"),
                      ly_do: "",
                    },
                  ];
                });
              }}
              text="Thêm chứng từ"
              sx={{
                mt: 4,
              }}
            />
          </Box>
        </Box>
        <Box
          sx={{
            mt: 3,
          }}
        >
          <ThongBaoSaiSotDSChungTu
            allowSelect={dataForm?.loai_chung_tu_dien_tu !== ""}
            data={thongBaoSaiSotChiTiets}
            onValueChanged={(data) => {
              setThongBaoSaiSotChiTiets(data);
              if (data.length > 0) {
                clearErrors("thong_bao_sai_sot_chi_tiets");
              } else {
                setError("thong_bao_sai_sot_chi_tiets", {
                  type: "manual",
                });
              }
            }}
          />
          {errors && errors["thong_bao_sai_sot_chi_tiets"] && (
            <FormControl.Validation
              id={"thong_bao_sai_sot_chi_tiets"}
              variant="error"
            >
              Vui lòng chọn tối thiểu một hóa đơn
            </FormControl.Validation>
          )}
        </Box>
        <ModalActions>
          <Button
            size="medium"
            onClick={() => {
              // history.goBack();
              history.push("../../tbss-ct");
            }}
            text="Đóng"
          />
          {viewDataModel && viewDataModel?.Trangthai === 1 && (
            <>
              <Button
                variant={thongBaoSaiSotId <= 0 ? "primary" : "default"}
                type="submit"
                size="medium"
                text={"Ký và gửi thông báo lên CQT"}
                isLoading={isSaving}
                // isLoading={status === eReducerStatusBase.is_saving}
              />
              {/* {thongBaoSaiSotId > 0 && (
                <Button
                  size="medium"
                  variant="primary"
                  isLoading={isSaving}
                  disabled={!isAllowPhatHanh}
                  onClick={() => {
                    if (user) {
                      handleGetBase64KySo(user?.donvi_ma_dv);
                    }
                  }}
                  text="Ký gửi Cơ quan thuế"
                />
              )} */}
            </>
          )}
          {!viewDataModel && (
            <Button
              variant={thongBaoSaiSotId <= 0 ? "primary" : "default"}
              type="submit"
              size="medium"
              text={"Cập nhật"}
              isLoading={isSaving}
              // isLoading={status === eReducerStatusBase.is_saving}
            />
          )}
        </ModalActions>
      </form>
      {isShowKySoModal && (
        <KySoModal
          base64={base64KySo}
          onClose={() => {
            setIsShowKySoModal(false);
          }}
          onSuccess={(signedtext) => {
            setIsShowKySoModal(false);
            // handleUpdateKySoSuccss(signedtext)

            UpdateChungTuTBSSSauKy({
              xmldaky: signedtext,
              trangthai: 2,
              mst: user?.donvi_ma_dv,
              matbsschungtu: thongBaoSaiSotId,
            });

            // handlePhatHanhAsync(signedtext);
          }}
        />
      )}
      {isShowPhatHanhResultModal && tbssPhatHanhPushNotifyModel && (
        <TBSSPhatHanhResultModal
          id={thongBaoSaiSotId}
          data={tbssPhatHanhPushNotifyModel}
          onClose={() => {
            setIsShowPhatHanhResultModal(false);
          }}
        />
      )}
    </Box>
  );
};

export default ThongBaoSaiSotCTForm;
