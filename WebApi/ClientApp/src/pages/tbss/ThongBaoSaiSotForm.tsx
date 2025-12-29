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
import SelectBoxLoaiHDDT from "../../component-data/selectbox-loai-hddt";
import SelectBoxTBSSTinhChat from "../../component-data/selectbox-tbss-tinhchat";
import Button from "../../component-ui/button";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { IThongBaoSaiSotAddOrEditRequest } from "../../models/requests/tbss/IThongBaoSaiSotAddOrEditRequest";
import { IBaseRespone } from "../../models/responses/IBaseRespone";
import { ICoQuanThue } from "../../models/responses/category/ICoQuanThue";
import { ITBSSPhatHanhPushNotifyModel } from "../../models/responses/hub/TBSSPhatHanhPushNotifyModel";
import { IThongBaoSaiSotChiTiet } from "../../models/responses/tbss/IThongBaoSaiSotChiTiet";
import TBSSPhatHanhResultModal from "./TBSSPhatHanhResultModal";
import ThongBaoSaiSotFormHoaDon from "./ThongBaoSaiSotFormHoaDon";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
import { coQuanThueApi } from "../../api/category/coQuanThueApi";

const ThongBaoSaiSotForm = () => {
  const { user } = useAuth();
  const dispatch = useAppDispatch();
    const [isShowImportModal, setIsShowImportModal] = useState(false);
  
  const [viewDataModel, setViewDataModel] =
    useState<IThongBaoSaiSotAddOrEditRequest>();
  const { checkAccesiableTo } = useCommonContext();
  const {
    register,
    clearErrors,
    setError,
    handleSubmit,
    reset,
    setValue,
    formState: { errors },
  } = useForm<IThongBaoSaiSotAddOrEditRequest>({
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
  const [loaiHoaDonDienTuId, setLoaiHoaDonDienTuId] = useState(1);
  const [tinhChatThongBaoId, setTinhChatThongBaoId] = useState(0);
  const [thongBaoSaiSotChiTiets, setThongBaoSaiSotChiTiets] = useState<
    IThongBaoSaiSotChiTiet[]
  >([]);
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const history = useHistory();
  const { signalRConnectionServer } = useCommonContext();
  const [isShowPhatHanhResultModal, setIsShowPhatHanhResultModal] =
    useState(false);
  const [tbssPhatHanhPushNotifyModel, setTBSSPhatHanhPushNotifyModel] =
    useState<ITBSSPhatHanhPushNotifyModel>();
  const { toKhais, status: toKhaiStatus } = useAppSelector(
    (x) => x.toKhai.toKhaiReducer
  );
  useEffect(() => {
    if (
      toKhaiStatus === eReducerStatusBase.is_not_initialization ||
      toKhaiStatus === eReducerStatusBase.is_need_reload
    ) {
      dispatch(rootAction.toKhai.toKhaiAction.loadStart());
    }
  }, [toKhaiStatus]);
  const toKhaiAccept = useMemo(() => {
    return toKhais
      .sort((a, b) => b.id - a.id)
      .find((x) => x.to_khai_status_id === eToKhaiStatus.CQT_DONG_Y);
  }, [toKhais]);
  useEffect(() => {
    if (toKhaiAccept) {
      setCoQuanThueId(toKhaiAccept?.co_quan_thue_id);
      handleSearchSelectedAsync(toKhaiAccept?.co_quan_thue_id);
    }
  }, [toKhaiAccept]);
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
      handleGetDataAsync();
    }
  }, [thongBaoSaiSotId]);
  const handleGetDataAsync = async () => {
    const res: IBaseRespone = await thongBaoSaiSotApi.getViewModel(
      thongBaoSaiSotId
    );
    if (res.is_success) {
      const data: IThongBaoSaiSotAddOrEditRequest = res.data;
      setLoaiHoaDonDienTuId(data.loai_hoa_don_dien_tu_id);
      setTinhChatThongBaoId(data.thong_bao_sai_sot_tinh_chat_id);
      reset({
        ...data,
        // ngay_lap: moment(toKhaiViewModel.ngay_lap).format("YYYY-MM-DD"),
        // ngay_co_hieu_luc: moment(toKhaiViewModel.ngay_co_hieu_luc).format("YYYY-MM-DD"),
      });
      setViewDataModel(data);
      setThongBaoSaiSotChiTiets(data.thong_bao_sai_sot_chi_tiets);
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
  };
  const handlePhatHanhAsync = async (signedtext: string) => {
    setIsSaving(true);
    const res = await thongBaoSaiSotApi.phatHanh({
      signed_text: signedtext,
      id: thongBaoSaiSotId,
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
    const res = await thongBaoSaiSotApi.createBase64KySo(thongBaoSaiSotId);
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
    const res = await thongBaoSaiSotApi.kySoVaPhatHanhRemoteAsync(
      thongBaoSaiSotId
    );
    setIsSaving(false);
    if (res.is_success) {
    } else {
      NotifyHelper.Error(res?.message ?? "Error");
    }
  };

  const onSubmit = async (data: any) => {
    let isValid: boolean = true;

    if (coQuanThueId <= 0 || !coQuanThue || !coQuanThue.ma_cqt) {
      setError("ma_cqt", {
        type: "manual",
      });
      isValid = false;
    }
    if (loaiHoaDonDienTuId <= 0) {
      setError("loai_hoa_don_dien_tu_id", {
        type: "manual",
      });
      isValid = false;
    }
    if (tinhChatThongBaoId <= 0) {
      setError("thong_bao_sai_sot_tinh_chat_id", {
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
    const res = await thongBaoSaiSotApi.insert({
      ...data,
      ma_cqt: coQuanThue?.ma_cqt,
      ten_cqt: coQuanThue?.ten,
      phien_ban: "2.1.0",
      ma_so: "04/SS-HĐĐT",
      ten_thong_bao: "Thông báo hóa đơn điện tử có sai sót",
      so_thong_bao: "",
      ten_nguoi_nop_thue: user?.donvi.ten_dv,
      ngay_thong_bao: moment().format("YYYY-MM-DD"),
      thong_bao_sai_sot_tinh_chat_id: tinhChatThongBaoId,
      loai_hoa_don_dien_tu_id: loaiHoaDonDienTuId,
      thong_bao_sai_sot_trang_thai_id: 1,
      thong_bao_sai_sot_chi_tiets: thongBaoSaiSotChiTiets,
      donvi_ma_dv: user?.donvi_ma_dv,
      ket_qua_phan_hoi: "",
    });
    if (res.is_success) {
      NotifyHelper.Success("Success");
      setThongBaoSaiSotId(res.data.id);
      history.push(`../../tbss/${res.data.id}`);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
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
          {/* <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}> */}
          <FormControl>
            <FormControl.Label>
              <Text text="Loại hóa đơn điện tử" />
            </FormControl.Label>
            <SelectBoxLoaiHDDT
              value={loaiHoaDonDienTuId}
              onValueChanged={(id) => {
                setLoaiHoaDonDienTuId(id);
                clearErrors("loai_hoa_don_dien_tu_id");
              }}
            />
            {errors && errors["loai_hoa_don_dien_tu_id"] && (
              <FormControl.Validation
                id={"loai_hoa_don_dien_tu_id"}
                variant="error"
              >
                Vui lòng chọn loại hóa đơn điện tử
              </FormControl.Validation>
            )}
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Tính chất thông báo" />
            </FormControl.Label>
            <SelectBoxTBSSTinhChat
              value={tinhChatThongBaoId}
              onValueChanged={(id) => {
                setTinhChatThongBaoId(id);
                clearErrors("thong_bao_sai_sot_tinh_chat_id");
              }}
            />
            {errors && errors["thong_bao_sai_sot_tinh_chat_id"] && (
              <FormControl.Validation
                id={"thong_bao_sai_sot_tinh_chat_id"}
                variant="error"
              >
                Vui lòng chọn Tính chất thông báo
              </FormControl.Validation>
            )}
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Lý do" />
            </FormControl.Label>
            <TextInput
              name="ly_do"
              block
              required
              maxLength={255}
              register={register}
              validateMessage="Vui lòng điền Lý do"
              errors={errors}
            />
            <FormControl.Caption>
              <Text text="Tối đa 255 ký tự" />
            </FormControl.Caption>
          </FormControl>
          {/* </Box> */}
        </Box>
        <Box
          sx={{
            mt: 3,
          }}
        >
          <ThongBaoSaiSotFormHoaDon
            allowSelect={loaiHoaDonDienTuId === 1}
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
              history.goBack();
            }}
            text="Đóng"
          />
          {viewDataModel &&
            viewDataModel.thong_bao_sai_sot_trang_thai_id === 1 && (
              <>
                <Button
                  variant={thongBaoSaiSotId <= 0 ? "primary" : "default"}
                  type="submit"
                  size="medium"
                  text={"Cập nhật"}
                  isLoading={isSaving}
                  // isLoading={status === eReducerStatusBase.is_saving}
                />
                {thongBaoSaiSotId > 0 && (
                  <Button
                    size="medium"
                    variant="primary"
                    isLoading={isSaving}
                    disabled={!isAllowPhatHanh}
                    onClick={() => {
                      if (user) {
                        if (user.is_hsm_signing || user.is_remote_signing) {
                          handleKySoRemoteAsync();
                        } else {
                          handleGetBase64KySo();
                        }
                      }
                    }}
                    text="Ký gửi Cơ quan thuế"
                  />
                )}
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

            handlePhatHanhAsync(signedtext);
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

export default ThongBaoSaiSotForm;
