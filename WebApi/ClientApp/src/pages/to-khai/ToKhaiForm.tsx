import { ChevronLeftIcon, InfoIcon } from "@primer/octicons-react";
import {
  Box,
  Checkbox,
  Flash,
  FormControl,
  Octicon,
  Radio,
} from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { useHistory, useParams } from "react-router-dom";
import { TO_KHAI_API_PHAT_HANH, toKhaiApi } from "../../api/to-khai/toKhaiApi";
import KySoModal from "../../component-data/ky-so-modal";
import PrintToKhaiButton from "../../component-data/print-to-khai-button";
import SelectBoxCoQuanThue from "../../component-data/selectbox-co-quan-thue";
import Button from "../../component-ui/button";
import DateInput from "../../component-ui/date-input";
import Heading from "../../component-ui/heading";
import PaperFormGroup from "../../component-ui/paper-form-group";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { eLoaiToKhai } from "../../models/commons/eLoaiToKhai";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
import { IToKhaiAddOrEditModel } from "../../models/requests/to-khai/IToKhaiAddOrEditModel";
import { ICoQuanThue } from "../../models/responses/category/ICoQuanThue";
import { ITBSSPhatHanhPushNotifyModel } from "../../models/responses/hub/TBSSPhatHanhPushNotifyModel";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
import { IToKhaiCTS } from "../../models/responses/to-khai/IToKhaiCTS";
import { IUploadCerRespone } from "../../models/responses/upload/IUploadCerRespone";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import ToKhaiFormCTS from "./ToKhaiFormCTS";
import ToKhaiFormDaiDienPhapLuat from "./ToKhaiFormDaiDienPhapLuat";
import ToKhaiFormHinhThucGuiDuLieu from "./ToKhaiFormHinhThucGuiDuLieu";
import ToKhaiFormHinhThucHoaDon from "./ToKhaiFormHinhThucHoaDon";
import ToKhaiFormLoaiHoaDon from "./ToKhaiFormLoaiHoaDon";
import ToKhaiFormPhuongThucChuyenDuLieuHoaDon from "./ToKhaiFormPhuongThucChuyenDuLieuHoaDon";
import { ToKhaiTimeLineModal } from "./ToKhaiTimeLineModal";
import ToKhaiView from "./ToKhaiView";
import ToKhaiFormTTCP from "./ToKhaiFormTTCP";
import ToKhaiFormTTTN from "./ToKhaiFormTTTN";
const defaultTTTNObj = {
  TTCTN: "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM",
  MSTTCTN: "0103930279",
  TNgay: "2021-12-01",
  DNgay: "2030-12-31",
  isReadOnly: true,
};
const defaultTTCPObj = {
  TTCGP: "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM",
  MSTTCGP: "0103930279",
  TNgay: "2021-12-01",
  DNgay: "2030-12-31",
  isReadOnly: true,
};
export const ToKhaiForm = () => {
  const { _signalrConnected, createUUID, _signalrHubProxy } =
    useCommonContext();
  const history = useHistory();
  const { id: pId }: any = useParams();
  const { user } = useAuth();
  const id = parseInt(pId) ?? 0;
  const { checkAccesiableTo } = useCommonContext();
  const [loadingStatus, setLoadingStatus] = useState<
    "not_init" | "loading" | "load_success" | "load_err"
  >("not_init");
  const dispatch = useAppDispatch();
  const { status, toKhais } = useAppSelector((x) => x.toKhai.toKhaiReducer);
  const [isLoadingDone, setIsLoadingDone] = useState<boolean>(
    id > 0 ? false : true
  );

  const [isSaving, setIsSaving] = useState(false);
  const fakeValid: any = {};
  const [toKhaiViewModel, setToKhaiViewModel] = useState<IToKhaiAddOrEditModel>(
    {
      ...fakeValid,
      donvi_ma_dv: user?.donvi.ma_dv,
      nguoi_nop_thue: user?.donvi.ten_dv,
      mst: user?.donvi.mst,
      nguoi_lien_he: "",
      co_quan_thue: user?.donvi.donvi_chuquan,
      dia_chi_lien_he: user?.donvi.dia_chi,
      email_lien_he: user?.donvi.email,
      dien_thoai_lien_he: user?.donvi.dien_thoai,
      list_cts: [],
    }
  );

  const isAllowPhatHanh = useMemo(() => {
    return checkAccesiableTo(TO_KHAI_API_PHAT_HANH, "POST");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const [cerFiles, setCerFiles] = useState<IUploadCerRespone[]>([]);

  const [coQuanThueId, setCoQuanThueId] = useState(0);
  const [coQuanThue, setCoQuanThue] = useState<ICoQuanThue>();

  const [base64KySo, setBase64KySo] = useState("");
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);

  const { signalRConnectionServer } = useCommonContext();
  const [isShowPhatHanhResultModal, setIsShowPhatHanhResultModal] =
    useState(false);
  const [toKhaiPhatHanhPushNotifyModel, setToKhaiPhatHanhPushNotifyModel] =
    useState<ITBSSPhatHanhPushNotifyModel>();

  useEffect(() => {
    if (
      status === eReducerStatusBase.is_not_initialization ||
      status === eReducerStatusBase.is_need_reload
    ) {
      dispatch(rootAction.toKhai.toKhaiAction.loadStart());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [status]);
  useEffect(() => {
    if (signalRConnectionServer) {
      signalRConnectionServer.on("TOKHAI_HAS_RESULT", (message: any) => {
        onTBSSPhatHanhHasResult(message);
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [signalRConnectionServer]);
  const onTBSSPhatHanhHasResult = (message: ITBSSPhatHanhPushNotifyModel) => {
    if (message.id === id || true) {
      setIsShowPhatHanhResultModal(true);
      setToKhaiPhatHanhPushNotifyModel(message);
      if (toKhaiViewModel) {
        setToKhaiViewModel({
          ...toKhaiViewModel,
          to_khai_status_id: message.thong_bao_sai_sot_trang_thai_id,
        });
      }
    }
  };
  const handlePhatHanhAsync = async (signedtext: string) => {
    setIsSaving(true);
    const res = await toKhaiApi.phatHanh({
      signed_text: signedtext,
      id: id,
    });
    if (res.is_success) {
      NotifyHelper.Success("Success");
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
    setIsSaving(false);
  };
  const handleGetBase64KySo = async () => {
    setIsSaving(true);
    const res = await toKhaiApi.createBase64KySo(id);
    setIsSaving(false);
    if (res.is_success) {
      setBase64KySo(res.data);
      setIsShowKySoModal(true);
    } else {
      NotifyHelper.Error("Error");
    }
  };
  const handleKySoRemoteAsync = async () => {
    setIsSaving(true);
    const res = await toKhaiApi.kySoVaPhatHanhRemoteAsync(id);
    setIsSaving(false);
    if (res.is_success) {
    } else {
      NotifyHelper.Error(res?.message ?? "Error");
    }
  };
  const isCanSend = useMemo(() => {
    if (!isAllowPhatHanh) return false;
    if (toKhaiViewModel) {
      return true;
    }
    return false;
  }, [toKhaiViewModel, isAllowPhatHanh]);
  useEffect(() => {
    if (id > 0) {
      handleGetDetailAsync();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);
  useEffect(() => {
    if (toKhaiViewModel) {
      reset({
        ...toKhaiViewModel,
        ngay_lap: moment(toKhaiViewModel.ngay_lap).format("YYYY-MM-DD"),
        ngay_co_hieu_luc: moment(toKhaiViewModel.ngay_co_hieu_luc).format(
          "YYYY-MM-DD"
        ),
      });

      setCoQuanThueId(toKhaiViewModel.co_quan_thue_id);
      setCerFiles(
        toKhaiViewModel.list_cts.map((x) => {
          return {
            file_name: x.file_name,
            url: x.url,
            cer_info: {
              ...x,
            },
          };
        })
      );
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [toKhaiViewModel]);

  const {
    register,
    handleSubmit,
    control,
    watch,
    setValue,
    trigger,
    formState: { errors },
    reset,
    setError,
    setFocus,
  } = useForm<IToKhai>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...toKhaiViewModel,
    },
  });
  const noi_lap = watch("noi_lap");
  const loai_to_khai_id = watch("loai_to_khai_id");
  const is_camket = watch("is_camket");

  const handleGetDetailAsync = async () => {
    setLoadingStatus("loading");
    const res = await toKhaiApi.getViewModel(id);

    if (res.is_success) {
      setToKhaiViewModel(res.data);
      setLoadingStatus("load_success");
      setIsLoadingDone(true);
    } else {
      setLoadingStatus("load_err");
    }
  };
  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        console.log({
          data,
        });
        if (eventName === "SERVER") {
          const ketquas = data.split("|");
          const [returnCode, code, signedtext] = ketquas;

          if (signedtext === "CertInf") {
            const [nhaCungCap, serial, tuNgay, denNgay, subject] =
              ketquas.slice(3);
            let issuer = nhaCungCap;
            const match = nhaCungCap.match(/CN=([^,]+)/);
            if (match) {
              issuer = match[1];
            } else {
            }
            const data: any = {
              returnCode,
              code,
              signedtext,
              nhaCungCap,
              serial,
              tuNgay,
              denNgay,
              subject,
              issuer,
            };
            setCerFiles([
              ...cerFiles,
              {
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
              },
            ]);
            console.log({
              data,
            });
          }
        }
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [_signalrConnected, _signalrHubProxy]);

  useEffect(() => {
    const tokhaimoinhat = toKhais
      ?.filter((x) => x.to_khai_status_id === eToKhaiStatus.CQT_DONG_Y)
      .sort(
        (a, b) =>
          new Date(b?.ngay_lap).getTime() - new Date(a?.ngay_lap).getTime()
      )[0];

    if (tokhaimoinhat && id <= 0) {
      reset({
        ngay_lap: moment().format("YYYY-MM-DD"),
        loai_to_khai_id: eLoaiToKhai.THAY_DOI_THONG_TIN,
      });

      handleGetDetailLatestAsync();
    } else {
      reset({
        ngay_lap: moment().format("YYYY-MM-DD"),
        loai_to_khai_id: eLoaiToKhai.DANG_KY_MOI,
      });
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [toKhais, id]);

  useEffect(() => {
    if (loai_to_khai_id === eLoaiToKhai.DANG_KY_MOI) {
      const count =
        toKhais.filter((x) => x.loai_to_khai_id === loai_to_khai_id).length + 1;
      setValue("ma_to_khai", `${count < 10 ? `0${count}` : count}`);
      setTimeout(() => {
        trigger("ma_to_khai");
      }, 300);
    }
    if (loai_to_khai_id === eLoaiToKhai.THAY_DOI_THONG_TIN) {
      const count =
        toKhais.filter((x) => x.loai_to_khai_id === loai_to_khai_id).length + 1;
      setValue("ma_to_khai", `${count < 10 ? `0${count}` : count}`);
      setTimeout(() => {
        trigger("ma_to_khai");
      }, 300);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [toKhais, loai_to_khai_id]);
  const onSubmit = async (data: IToKhai) => {
    // console.log({
    //   onSubmit: data,
    // });

    ///validate email
    if (data.email_lien_he) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(data.email_lien_he)) {
        setError("email_lien_he", {
          type: "manual",
          message: "Email không đúng định dạng",
        });

        // 👇 Focus vào input bị lỗi
        setFocus("email_lien_he");
        return;
      }
    }

    const requestModel: IToKhaiAddOrEditModel = {
      ...data,

      nguoi_tao: "",
      cks_serial_no: "",
      cks_user_full_name: "",
      donvi_ma_dv: "",
      co_quan_thue_id: coQuanThue?.id ?? data.co_quan_thue_id,
      co_quan_thue: coQuanThue?.dia_chi ?? data.co_quan_thue,
      ma_cqt: coQuanThue?.ma_cqt ?? data.ma_cqt,
      phat_hanh_uuid: "",
      noi_lap: noi_lap,
      to_chuc_cap_giay_phep_json:
        data.to_chuc_cap_giay_phep_json ?? JSON.stringify([defaultTTCPObj]),
      to_chuc_truyen_nhan_json:
        data.to_chuc_truyen_nhan_json ?? JSON.stringify([defaultTTTNObj]),
      list_cts: cerFiles.map((x) => {
        const obj: IToKhaiCTS = {
          to_khai_id: id,
          id: 0,
          url: x.url,
          file_name: x.file_name,
          ...x.cer_info,
        };
        return obj;
      }),
    };

    setIsSaving(true);
    if (id <= 0) {
      const res = await toKhaiApi.insert(requestModel);
      setIsSaving(false);
      if (res.is_success) {
        NotifyHelper.Success("Thêm mới tờ khai thành công");
        history.push(`../../to-khai/${res.data.id}`);
      } else {
        NotifyHelper.Error(res.message ?? "Thêm mới tờ khai thất bại");
      }
    } else {
      const res = await toKhaiApi.update(requestModel);
      setIsSaving(false);
      if (res.is_success) {
        NotifyHelper.Success("Cập nhật tờ khai thành công");
        history.push(`../../to-khai/${res.data.id}`);
      } else {
        NotifyHelper.Error(res.message ?? "Cập nhật tờ khai thất bại");
      }
    }
  };

  const handleGetDetailByIdAsync = async (idtohkai: number) => {
    setLoadingStatus("loading");
    const res = await toKhaiApi.getViewModel(idtohkai);

    if (res.is_success) {
      setLoadingStatus("load_success");
      setCerFiles(
        res?.data?.list_cts?.map((x: any) => {
          return {
            file_name: x.file_name,
            url: x.url,
            cer_info: {
              ...x,
            },
          };
        })
      );

      setIsLoadingDone(true);
    } else {
      setLoadingStatus("load_err");
    }
  };

  const handleGetDetailLatestAsync = async () => {
    const tokhaimoinhat = toKhais
      ?.filter((x) => x.to_khai_status_id === eToKhaiStatus.CQT_DONG_Y)
      .sort(
        (a, b) =>
          new Date(b?.ngay_lap).getTime() - new Date(a?.ngay_lap).getTime()
      )[0];

    if (tokhaimoinhat) {
      await handleGetDetailByIdAsync(tokhaimoinhat.id);

      setCoQuanThueId(tokhaimoinhat?.co_quan_thue_id);
      setCoQuanThue({
        ten: tokhaimoinhat?.co_quan_thue,
        ma_cqt_ql: tokhaimoinhat?.ma_cqt,
        ma_cqt: tokhaimoinhat?.ma_cqt,
        id: tokhaimoinhat?.co_quan_thue_id,
        dia_chi: tokhaimoinhat?.co_quan_thue,
        ten_viet_tat: "",
        tinh: "",
        co_quan_thue_trang_thai_id: 0,
      });

      reset({
        loai_to_khai_id: eLoaiToKhai.THAY_DOI_THONG_TIN,

        //thông tin người nộp thuế
        nguoi_nop_thue: tokhaimoinhat?.nguoi_nop_thue,
        mst: tokhaimoinhat?.mst,
        // nguoi_lien_he: tokhaimoinhat?.nguoi_lien_he?.trim(),
        nguoi_lien_he: "",
        dia_chi_lien_he: tokhaimoinhat?.dia_chi_lien_he?.trim(),
        email_lien_he: tokhaimoinhat?.email_lien_he?.trim(),
        dien_thoai_lien_he: tokhaimoinhat?.dien_thoai_lien_he?.trim(),
        ho_chieu: tokhaimoinhat?.ho_chieu?.trim(),

        //đại diện pl
        dai_dien_phap_luat_ho_ten:
          tokhaimoinhat?.dai_dien_phap_luat_ho_ten?.trim(),
        dai_dien_phap_luat_dien_thoai:
          tokhaimoinhat?.dai_dien_phap_luat_dien_thoai?.trim(),
        dai_dien_phap_luat_dien_ngay_sinh:
          tokhaimoinhat?.dai_dien_phap_luat_dien_ngay_sinh,
        dai_dien_phap_luat_dien_gioi_tinh:
          tokhaimoinhat?.dai_dien_phap_luat_dien_gioi_tinh,
        dai_dien_phap_luat_dien_cccd:
          tokhaimoinhat?.dai_dien_phap_luat_dien_cccd?.trim(),

        //Hình thức hóa đơn
        is_hoadon_co_ma_cqt: tokhaimoinhat?.is_hoadon_co_ma_cqt ?? false,
        is_hoadon_khong_co_ma_cqt:
          tokhaimoinhat?.is_hoadon_khong_co_ma_cqt ?? false,
        is_hoadon_co_ma_cqt_mtt:
          tokhaimoinhat?.is_hoadon_co_ma_cqt_mtt ?? false,

        //hinh thức gửi dữ liệu
        is_doanh_nghiep_vvn_kho_khan:
          tokhaimoinhat?.is_doanh_nghiep_vvn_kho_khan ?? false,
        is_doanh_nghiep_vvn_khac:
          tokhaimoinhat?.is_doanh_nghiep_vvn_khac ?? false,

        //Phương thức chuyển dữ liệu
        is_chuyen_day_du_tung_hoadon:
          tokhaimoinhat?.is_chuyen_day_du_tung_hoadon ?? false,
        is_chuyen_theo_bang_tonghop:
          tokhaimoinhat?.is_chuyen_theo_bang_tonghop ?? false,

        //Loại hóa đơn
        is_ban_hang_du_tru_quoc_gia:
          tokhaimoinhat?.is_ban_hang_du_tru_quoc_gia ?? false,
        is_ban_tai_san_cong: tokhaimoinhat?.is_ban_tai_san_cong ?? false,
        is_sd_hoadon_gtgt: tokhaimoinhat?.is_sd_hoadon_gtgt ?? false,
        is_sd_hoadon_gtgt_bien_lai:
          tokhaimoinhat?.is_sd_hoadon_gtgt_bien_lai ?? false,
        is_sd_hoadon_banhang: tokhaimoinhat?.is_sd_hoadon_banhang ?? false,
        is_sd_hoadon_banhang_bien_lai:
          tokhaimoinhat?.is_sd_hoadon_banhang_bien_lai ?? false,
        is_sd_chungtu_giong_hoadon:
          tokhaimoinhat?.is_sd_chungtu_giong_hoadon ?? false,
        is_sd_hoadon_thuong_mai:
          tokhaimoinhat?.is_sd_hoadon_thuong_mai ?? false,
        is_sd_hoadon_khac: tokhaimoinhat?.is_sd_hoadon_khac ?? false,

        //Nơi lập
        noi_lap: tokhaimoinhat?.noi_lap,
        ngay_co_hieu_luc: moment(tokhaimoinhat?.ngay_co_hieu_luc).format(
          "YYYY-MM-DD"
        ),
      });
    }
  };

  if (
    toKhaiViewModel &&
    toKhaiViewModel.id &&
    toKhaiViewModel.to_khai_status_id !== eToKhaiStatus.TAO_MOI
  ) {
    return <ToKhaiView id={toKhaiViewModel.id} />;
  }

  return (
    <Box>
      {loadingStatus === "load_err" && (
        <Flash variant="danger">
          <Box
            sx={{
              display: "flex",
            }}
          >
            <Box>
              <Octicon icon={InfoIcon} />
            </Box>
            <Box>Không thể hiển thị tờ khai này.</Box>
          </Box>
        </Flash>
      )}
      {loadingStatus !== "load_err" && isLoadingDone && (
        <form onSubmit={handleSubmit(onSubmit)} noValidate={true}>
          <Box>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Box sx={{ width: "250px" }}>
                <Button
                  leadingVisual={ChevronLeftIcon}
                  size="large"
                  variant="invisible"
                  text="Quay lại"
                  sx={{
                    ml: -4,
                    backgroundColor: "#fff!important",
                  }}
                  onClick={() => {
                    history.push("/to-khai");
                  }}
                />
              </Box>
              <Heading
                text="Lập tờ khai Đăng ký/ Thay đổi thông tin sử dụng hóa đơn điện tử"
                sx={{ ml: 0, flex: 1 }}
              />
            </Box>
          </Box>
          <PaperFormGroup label="Loại tờ khai">
            <Box>
              <Controller
                control={control}
                name="loai_to_khai_id"
                rules={{
                  required: true,
                }}
                render={({ field }) => {
                  return (
                    <FormControl>
                      <FormControl.Label>Loại tờ khai</FormControl.Label>
                      <Box
                        sx={{
                          display: "grid",
                          gap: 2,
                          gridTemplateColumns: "1fr 1fr",
                          pt: 1,
                        }}
                      >
                        <FormControl>
                          <Radio
                            value="0"
                            checked={field.value === eLoaiToKhai.DANG_KY_MOI}
                            onChange={(e) => {
                              if (e.target.checked) {
                                field.onChange(eLoaiToKhai.DANG_KY_MOI);
                              }
                            }}
                          />
                          <FormControl.Label>Đăng ký mới</FormControl.Label>
                        </FormControl>
                        <FormControl>
                          <Radio
                            value="1"
                            checked={
                              field.value === eLoaiToKhai.THAY_DOI_THONG_TIN
                            }
                            onChange={(e) => {
                              if (e.target.checked) {
                                field.onChange(eLoaiToKhai.THAY_DOI_THONG_TIN);
                                handleGetDetailLatestAsync();
                              }
                            }}
                          />
                          <FormControl.Label>
                            Thay đổi thông tin
                          </FormControl.Label>
                        </FormControl>
                      </Box>
                      {errors && errors["loai_to_khai_id"] && (
                        <FormControl.Validation variant="error">
                          Vui lòng chọn Loại tờ khai
                        </FormControl.Validation>
                      )}
                    </FormControl>
                  );
                }}
              />
              {/* <RadioGroup name="defaultRadioGroup">
                                <RadioGroup.Label visuallyHidden>Choices</RadioGroup.Label>
                                <FormControl>
                                    <Radio value="1" checked={loaiToKhaiId === eLoaiToKhai.DANG_KY_MOI}
                                        onChange={(e) => {
                                            if (e.target.checked) {
                                                const id: number = eLoaiToKhai.DANG_KY_MOI;
                                                setLoaiToKhaiId(id)
                                            }
                                        }}
                                    />
                                    <FormControl.Label>Đăng ký mới</FormControl.Label>
                                </FormControl>
                                <FormControl>
                                    <Radio value="2" checked={loaiToKhaiId === eLoaiToKhai.THAY_DOI_THONG_TIN}
                                        onChange={(e) => {
                                            if (e.target.checked) {
                                                const id: number = eLoaiToKhai.THAY_DOI_THONG_TIN;
                                                setLoaiToKhaiId(id)
                                            }
                                        }}
                                    />
                                    <FormControl.Label>Thay đổi thông tin</FormControl.Label>
                                </FormControl>

                            </RadioGroup>
                            {
                                loaiToKhaiId === 0 &&
                                <FormControl.Validation id={"loaiToKhaiId"} variant="error">
                                    Vui lòng chọn Loại tờ khai
                                </FormControl.Validation>
                            } */}
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label="1. Thông tin kê khai">
            <Box>
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Số" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="ma_to_khai"
                      width={300}
                      required
                      validateMessage="Vui lòng điền Mã tờ khai"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl sx={{ ml: 3 }}>
                    <FormControl.Label>
                      <Text text="Ngày lập" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="ngay_lap"
                      type="date"
                      width={300}
                      required
                      validateMessage="Vui lòng điền Ngày lập"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Tên người nộp thuế" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="nguoi_nop_thue"
                    block
                    required
                    validateMessage="Vui lòng điền Người nộp thuế"
                    errors={errors}
                  />
                </FormControl>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Mã số thuế" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="mst"
                    block
                    required
                    validateMessage="Vui lòng điền Mã số thuế"
                    errors={errors}
                  />
                </FormControl>
                <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Người liên hệ" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_lien_he"
                      block
                      required
                      validateMessage="Vui lòng điền Người liên hệ"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl sx={{ ml: 3 }}>
                    <FormControl.Label>
                      <Text text="Cơ quan thuế" />
                    </FormControl.Label>

                    <SelectBoxCoQuanThue
                      value={coQuanThueId}
                      onValueChanged={(id, coQuanThue) => {
                        console.log(coQuanThue);

                        setCoQuanThue(coQuanThue);
                        setCoQuanThueId(id);
                        setValue("noi_lap", coQuanThue?.tinh ?? "");
                      }}
                    />
                  </FormControl>
                </Box>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Địa chỉ liên hệ" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="dia_chi_lien_he"
                    block
                    required
                    validateMessage="Vui lòng điền Địa chỉ liên hệ"
                    errors={errors}
                  />
                </FormControl>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr 1fr",
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Email" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="email_lien_he"
                      block
                      required
                      pattern={"/^[A-Z0-9._%+-]+@[A-Z0-9.-]+.[A-Z]{2,}$/i"}
                      validateMessage="Vui lòng điền Email liên hệ"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Điện thoại liên hệ" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="dien_thoai_lien_he"
                      block
                      required
                      validateMessage="Vui lòng điền Điện thoại liên hệ"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Hộ chiếu" />
                    </FormControl.Label>
                    <TextInput register={register} name="so_ho_chieu" block />
                  </FormControl>
                </Box>
              </Box>
              <ToKhaiFormDaiDienPhapLuat
                register={register}
                errors={errors}
                control={control}
              />
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label="2. Hình thức hóa đơn">
            <Box>
              <ToKhaiFormHinhThucHoaDon
                register={register}
                errors={errors}
                control={control}
                watch={watch}
                setValue={setValue}
              />
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label={`3. Hình thức gửi dữ liệu \nhóa đơn điện tử`}>
            <ToKhaiFormHinhThucGuiDuLieu
              register={register}
              errors={errors}
              control={control}
              watch={watch}
            />
          </PaperFormGroup>
          <PaperFormGroup
            label={`4. Phương thức chuyển dữ liệu \nhóa đơn điện tử`}
          >
            <ToKhaiFormPhuongThucChuyenDuLieuHoaDon
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
            />
          </PaperFormGroup>
          <PaperFormGroup label="5. Loại hóa đơn sử dụng">
            <ToKhaiFormLoaiHoaDon
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
            />
          </PaperFormGroup>
          <PaperFormGroup label="6. Danh sách chứng thư số sử dụng">
            <ToKhaiFormCTS
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
              setCerFiles={setCerFiles}
              cerFiles={cerFiles}
            />
          </PaperFormGroup>
          <PaperFormGroup label="7. Tổ chức cấp phép">
            <ToKhaiFormTTCP
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
            />
          </PaperFormGroup>
          <PaperFormGroup label="8. Tổ chức truyền nhận">
            <ToKhaiFormTTTN
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
            />
          </PaperFormGroup>
          <Box
            sx={{
              borderTopStyle: "solid",
              borderTopWidth: 1,
              borderTopColor: "border.default",
              mt: 4,
              pt: 4,
            }}
          >
            <Controller
              control={control}
              name="is_camket"
              rules={{
                required: true,
              }}
              render={({ field }) => {
                return (
                  <FormControl>
                    <Checkbox
                      checked={field.value}
                      onChange={(e) => {
                        field.onChange(e.target.checked);
                      }}
                    ></Checkbox>
                    <FormControl.Label sx={{ color: "red" }}>
                      Chúng tôi cam kết hoàn toàn chịu trách nhiệm trước pháp
                      luật về tính chính xác, trung thực của nội dung nêu trên
                      và thực hiện theo đúng quy định của pháp luật.
                    </FormControl.Label>
                  </FormControl>
                );
              }}
            />
          </Box>
          <Box
            sx={{
              borderTopStyle: "solid",
              borderTopWidth: 1,
              borderTopColor: "border.default",
              mt: 4,
              pt: 4,
              display: "flex",
            }}
          >
            <Box
              display={"grid"}
              sx={{
                gap: 2,
                flex: 1,
                ml: 5,
              }}
            >
              <FormControl>
                <FormControl.Label>Nơi lập</FormControl.Label>
                <TextInput
                  register={register}
                  name="noi_lap"
                  width={200}
                  required
                  validateMessage="Vui lòng điền Nơi lập"
                  errors={errors}
                />
              </FormControl>
              <Controller
                control={control}
                name="ngay_co_hieu_luc"
                rules={{
                  required: true,
                }}
                render={({ field }) => {
                  return (
                    <FormControl>
                      <FormControl.Label>Ngày có hiệu lực</FormControl.Label>
                      <DateInput
                        value={
                          field.value
                            ? moment(field.value).format("DD/MM/YYYY")
                            : undefined
                        }
                        onValueChanged={(text, date) => {
                          if (date) {
                            field.onChange(moment(date).format("YYYY-MM-DD"));
                          }
                        }}
                      />
                      {errors && errors["ngay_co_hieu_luc"] && (
                        <FormControl.Validation variant="error">
                          Vui lòng điền Ngày có hiệu lực
                        </FormControl.Validation>
                      )}
                    </FormControl>
                  );
                }}
              />
            </Box>
            <Box
              sx={{
                flex: 1,
                display: "flex",
                flexDirection: "row-reverse",
                mr: 5,
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                }}
              >
                <Text
                  text={`${noi_lap}, ngày ${moment().format(
                    "DD"
                  )} tháng ${moment().format("MM")} năm ${moment().format(
                    "YYYY"
                  )}`}
                />
                <Text
                  text="NGƯỜI NỘP THUẾ"
                  sx={{
                    fontSize: 15,
                    fontWeight: "bold",
                  }}
                />
              </Box>
            </Box>
          </Box>
          <Box
            sx={{
              borderTopStyle: "solid",
              borderTopWidth: 1,
              borderTopColor: "border.default",
              mt: 4,
              pt: 4,
              display: "flex",
            }}
          >
            <Box
              sx={{
                flex: 1,
              }}
            >
              <PrintToKhaiButton
                id={id}
                status={toKhaiViewModel?.to_khai_status_id}
              />
            </Box>
            <Box
              sx={{
                flex: 1,
                display: "flex",
                flexDirection: "row-reverse",
                mr: 5,
              }}
            >
              <Box display={"flex"}>
                <Button
                  text="Quay lại"
                  type="button"
                  disabled={!is_camket}
                  sx={{ mr: 2, minWidth: "100px" }}
                  size="large"
                  variant="invisible"
                  onClick={() => {
                    history.push("/to-khai");
                  }}
                />
                {(id === 0 ||
                  (toKhaiViewModel &&
                    toKhaiViewModel.to_khai_status_id ===
                      eToKhaiStatus.TAO_MOI)) && (
                  <>
                    <Button
                      text="Lưu"
                      isLoading={isSaving}
                      type="submit"
                      disabled={!is_camket}
                      sx={{ mr: 2, minWidth: "100px" }}
                      size="large"
                      variant="primary"
                      // tooltip="Vui lòng xác nhận Cam kết chịu trách nhiệm..."
                    />
                    <Button
                      text="Ký gửi cơ quan thuế"
                      isLoading={isSaving}
                      disabled={!isCanSend}
                      sx={{ minWidth: "100px" }}
                      variant="primary"
                      size="large"
                      tooltip="Bạn chỉ có thể gửi tờ khai sau khi đã ký số"
                      onClick={() => {
                        if (user) {
                          if (user.is_hsm_signing || user.is_remote_signing) {
                            handleKySoRemoteAsync();
                          } else {
                            handleGetBase64KySo();
                          }
                        }
                      }}
                    />
                  </>
                )}
              </Box>
            </Box>
          </Box>
        </form>
      )}
      {isShowKySoModal && (
        <KySoModal
          base64={base64KySo}
          onClose={() => {
            setIsShowKySoModal(false);
          }}
          onSuccess={(signedtext) => {
            setIsShowKySoModal(false);
            handlePhatHanhAsync(signedtext);
          }}
        />
      )}
      {isShowPhatHanhResultModal && toKhaiPhatHanhPushNotifyModel && (
        <ToKhaiTimeLineModal
          toKhaiId={id}
          onClose={() => {
            setIsShowPhatHanhResultModal(false);
          }}
        />
      )}
    </Box>
  );
};
