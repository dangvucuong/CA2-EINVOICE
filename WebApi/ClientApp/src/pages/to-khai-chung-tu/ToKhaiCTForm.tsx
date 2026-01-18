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
import { Controller, set, useForm } from "react-hook-form";
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
import { useAuth } from "../../hooks/useAuth";
import { eLoaiToKhai } from "../../models/commons/eLoaiToKhai";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
import { IToKhaiAddOrEditModel } from "../../models/requests/to-khai/IToKhaiAddOrEditModel";
import { ICoQuanThue } from "../../models/responses/category/ICoQuanThue";
import { ITBSSPhatHanhPushNotifyModel } from "../../models/responses/hub/TBSSPhatHanhPushNotifyModel";
import { IUploadCerRespone } from "../../models/responses/upload/IUploadCerRespone";
import { ToKhaiTimeLineModal } from "./ToKhaiTimeLineModal";
import ToKhaiCTFormDoiTuong from "./ToKhaiCTFormDoiTuong";
import ToKhaiCTFormLoaiChungTu from "./ToKhaiCTFormLoaiChungTu";
import ToKhaiCTFormHinhThucChungTu from "./ToKhaiCTFormHinhThucChungTu";
import ToKhaiCTFormCTS from "./ToKhaiCTFormCTS";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import ToKhaiCTView from "./ToKhaiCTView";

export interface IToKhaiCT {
  id: number;
  to_khai_status_id: number;
  loai_to_khai_id: number;
  ma_to_khai: string;
  ngay_lap: string;
  mst: string;
  donvi_ma_dv: string;
  nguoi_nop_thue: string;
  nguoi_lien_he: string;
  co_quan_thue: string;
  dia_chi_lien_he: string;
  email_lien_he: string;
  dien_thoai_lien_he: string;
  dia_danh: string;

  is_to_chuc_ca_nhan_phat_hanh: boolean;
  is_co_quan_thue_phat_hanh: boolean;

  is_chung_tu_dien_tu_khau_tru_tncn: boolean;
  is_chung_tu_thue_thuong_mai_dien_tu: boolean;
  is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia: boolean;
  is_bien_thu_thue_phi_le_phi_in_san_menh_gia: boolean;
  is_bien_lai_thu_thue_phi_le_phi_ctt50: boolean;

  is_tren_cong_thong_tin_dien_tu_cua_cqt: boolean;
  is_chuyen_du_lieu_qua_tctn: boolean;
  is_chuyen_du_lieu_qua_tctn_duoc_uy_thac: boolean;

  cks_user_id: number;
  cks_serial_no: string;
  cks_user_full_name: string;
  is_camket: boolean;
  ngay_tao: string;
  nguoi_tao: string;
  co_quan_thue_id: number;
  ma_cqt: string;
  phat_hanh_uuid: string;
}

export const ToKhaiCTForm = () => {
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
  const [isLoadingDone, setIsLoadingDone] = useState<boolean>(
    id > 0 ? false : true,
  );

  const [isSaving, setIsSaving] = useState(false);
  const fakeValid: any = {};
  const [toKhaiViewModel, setToKhaiViewModel] = useState<any>({
    ...fakeValid,
    donvi_ma_dv: user?.donvi.ma_dv,
    nguoi_nop_thue: user?.donvi.ten_dv,
    mst: user?.donvi.mst,
    nguoi_lien_he: "",
    co_quan_thue: user?.donvi.donvi_chuquan,
    dia_chi_lien_he: user?.donvi.dia_chi,
    email_lien_he: user?.donvi.email,
    dien_thoai_lien_he: user?.donvi.dien_thoai,
    dia_danh: "",
    list_cts: [],
  });

  const isAllowPhatHanh = useMemo(() => {
    return checkAccesiableTo(TO_KHAI_API_PHAT_HANH, "POST");
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

  const GuitokhailenCQT = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <GuiToKhaiCQT xmlns="http://tempuri.org/">
      <Matokhai_CT>${values?.matokhaict}</Matokhai_CT>
      <madonvi>${values?.madonvi}</madonvi>
      <thongdiep>${values?.thongdiep}</thongdiep>
    </GuiToKhaiCQT>
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
      },
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

  const updateToKhaiSauKy = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <UpdateToKhaiSauKy xmlns="http://tempuri.org/">
      <xmlthongdiep>${values?.xmldaky}</xmlthongdiep>
      <trangthai>${values?.trangthai}</trangthai>
      <mst>${values?.mst}</mst>
      <matokhai>${values?.matokhai}</matokhai>
    </UpdateToKhaiSauKy>
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
      },
    );

    const parseRes = parseSoapResponse(res);
    setIsSaving(false);

    if (parseRes.status === "success") {
      await GuitokhailenCQT({
        matokhaict: values?.matokhai,
        madonvi: user?.donvi.ma_dv,
        thongdiep: values?.xmldaky,
      });
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleGetBase64KySo = async (mst: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayXmlTokhai xmlns="http://tempuri.org/">
      <mst>${mst}</mst>
      <matokhai>${id}</matokhai>
    </LayXmlTokhai>
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
      },
    );

    const parseRes = parseSoapResponse(res);

    setIsSaving(false);

    if (parseRes.status === "success") {
      setBase64KySo(parseRes.data);
      setIsShowKySoModal(true);
    } else {
      NotifyHelper.Error(parseRes.message);
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

  const {
    register,
    handleSubmit,
    control,
    watch,
    setValue,
    trigger,
    formState: { errors },
    reset,
  } = useForm<IToKhaiCT>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...toKhaiViewModel,
    },
  });
  const loai_to_khai_id = watch("loai_to_khai_id");
  const is_camket = watch("is_camket");

  const handleGetDetailAsync = async () => {
    setLoadingStatus("loading");
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Laythongtintokhai xmlns="http://tempuri.org/">
      <MatokhaiCT>${id}</MatokhaiCT>
      <madonvi>${user?.donvi_ma_dv}</madonvi>
    </Laythongtintokhai>
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
      const thongtintokhai = parseRes.data?.tokhai[0];
      const thongtincts = parseRes.data?.danhsachCTS;

      setToKhaiViewModel((prev: any) => {
        return {
          ...prev,
          thongtintokhai,
        };
      });

      setCerFiles(
        thongtincts?.map((x: any) => {
          return {
            cer_info: {
              ...x,
              serial_number: x?.Seri,
              not_after: x?.DNgay,
              not_before: x?.TNgay,
              issuer: x?.TTChuc,
            },
          };
        }),
      );

      reset({
        dia_danh: thongtintokhai?.DDanh,
        ngay_lap: moment(thongtintokhai?.NLap).format("YYYY-MM-DD"),
        loai_to_khai_id: thongtintokhai?.HThuc,
        nguoi_nop_thue: thongtintokhai?.TNNT,
        mst: thongtintokhai?.MST,
        nguoi_lien_he: thongtintokhai?.NLHe,
        co_quan_thue: thongtintokhai?.CQTQLy,
        dia_chi_lien_he: thongtintokhai?.DCLHe,
        email_lien_he: thongtintokhai?.DCTDTu,
        dien_thoai_lien_he: thongtintokhai?.DTLHe,
        is_to_chuc_ca_nhan_phat_hanh: thongtintokhai?.TCCNPHanh === 1,
        is_co_quan_thue_phat_hanh: thongtintokhai?.CQTPHanh === 1,
        is_chung_tu_dien_tu_khau_tru_tncn: thongtintokhai?.CTTNCNhan === 1,
        is_chung_tu_thue_thuong_mai_dien_tu: thongtintokhai?.CTKTTTMDTu === 1,
        is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia:
          thongtintokhai?.BLTPLPKIn === 1,
        is_bien_thu_thue_phi_le_phi_in_san_menh_gia:
          thongtintokhai?.BLTPLPIn === 1,
        is_bien_lai_thu_thue_phi_le_phi_ctt50: thongtintokhai?.BLTTPLPhi === 1,
        is_tren_cong_thong_tin_dien_tu_cua_cqt: thongtintokhai?.CDLQCCQT === 1,
        is_chuyen_du_lieu_qua_tctn: thongtintokhai?.CDLQTCTN === 1,
        is_chuyen_du_lieu_qua_tctn_duoc_uy_thac:
          thongtintokhai?.CDLQTCTNUT === 1,
        is_camket: true,
      });

      setCoQuanThueId(thongtintokhai?.co_quan_thue_id);

      setLoadingStatus("load_success");
      setIsLoadingDone(true);
    } else {
      setLoadingStatus("load_err");
    }
  };

  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        // console.log({
        //   data,
        // });
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
            // console.log({
            //   data,
            // });
          }
        }
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [_signalrConnected, _signalrHubProxy]);

  const onSubmit = async (data: IToKhaiCT) => {
    const objTTChung = {
      PBan: "2.1.0",
      MSo: "01/ĐKTĐ-CTĐT",
      Ten:
        loai_to_khai_id === 1
          ? "Tờ khai đăng ký sử dụng chứng từ điện tử"
          : "Tờ khai thay đổi thông tin sử dụng chứng từ điện tử",
      HThuc: loai_to_khai_id,
      TNNT: data.nguoi_nop_thue,
      MST: data.mst,
      CQTQLy: coQuanThue?.ten,
      MCQTQLy: coQuanThue?.ma_cqt,
      NLHe: data?.nguoi_lien_he,
      DCLHe: data?.dia_chi_lien_he,
      DCTDTu: data?.email_lien_he,
      DTLHe: data?.dien_thoai_lien_he,
      DDanh: data?.dia_danh,
      NLap: data?.ngay_lap,
      TCCNPHanh: data?.is_to_chuc_ca_nhan_phat_hanh ? 1 : 0,
      CQTPHanh: data?.is_co_quan_thue_phat_hanh ? 1 : 0,
      CTTNCNhan: data?.is_chung_tu_dien_tu_khau_tru_tncn ? 1 : 0,
      CTKTTTMDTu: data?.is_chung_tu_thue_thuong_mai_dien_tu ? 1 : 0,
      BLTPLPKIn: data?.is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia
        ? 1
        : 0,
      BLTPLPIn: data?.is_bien_thu_thue_phi_le_phi_in_san_menh_gia ? 1 : 0,
      BLTTPLPhi: data?.is_bien_lai_thu_thue_phi_le_phi_ctt50 ? 1 : 0,
      CDLQCCQT: data?.is_tren_cong_thong_tin_dien_tu_cua_cqt ? 1 : 0,
      CDLQTCTN: data?.is_chuyen_du_lieu_qua_tctn ? 1 : 0,
      CDLQTCTNUT: data?.is_chuyen_du_lieu_qua_tctn_duoc_uy_thac ? 1 : 0,
      Taikhoan: user?.donvi.ma_dv,
      SerialNo: cerFiles.length > 0 ? cerFiles[0].cer_info.serial_number : "",
      co_quan_thue_id: coQuanThueId,
    };

    if (!coQuanThue) {
      NotifyHelper.Error("Chưa chọn cơ quan thuế quản lý");
      return;
    }

    const sjsonTTCTS = cerFiles.map((item, index) => {
      const cer = item.cer_info;
      return {
        STT: index + 1,
        TTChuc: cer.issuer,
        Seri: cer.serial_number,
        TNgay: cer.not_before,
        DNgay: cer.not_after,
        HThuc: 1,
      };
    });

    // 2. Làm sạch objTTChung trước khi gửi
    const objTTChungClean = cleanObjectForSoap(objTTChung);

    setIsSaving(true);
    if (id <= 0) {
      await TaoToKhaiChungTu(
        JSON.stringify(objTTChungClean),
        JSON.stringify(sjsonTTCTS),
      );
    } else {
      await SuaToKhaiChungTu(
        JSON.stringify(objTTChungClean),
        JSON.stringify(sjsonTTCTS),
        id,
        user?.donvi.ma_dv,
      );
    }
    setIsSaving(false);
  };

  // Hàm thay thế các ký tự đặc biệt gây lỗi XML/SOAP
  const escapeSoapString = (str: string): string => {
    if (!str) return "";
    return str.replace(/[<>&'"]/g, (c) => {
      switch (c) {
        case "<":
          return "&lt;";
        case ">":
          return "&gt;";
        case "&":
          return "&amp;";
        case "'":
          return "&apos;";
        case '"':
          return "&quot;";
        default:
          return c;
      }
    });
  };

  // Hàm duyệt qua object để xử lý từng field
  const cleanObjectForSoap = <T extends Record<string, any>>(obj: T): T => {
    const newObj: any = { ...obj }; // Copy object để tránh mutate dữ liệu gốc

    Object.keys(newObj).forEach((key) => {
      const value = newObj[key];
      if (typeof value === "string") {
        // Trim khoảng trắng thừa và escape ký tự đặc biệt
        newObj[key] = escapeSoapString(value.trim()) as any;
      }
      // Nếu giá trị là null hoặc undefined thì gán về chuỗi rỗng (tuỳ logic SOAP của bạn)
      if (value === null || value === undefined) {
        newObj[key] = "" as any;
      }
    });

    return newObj;
  };

  const TaoToKhaiChungTu = async (objTTChung: string, sjsonTTCTS: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Taotokhai xmlns="http://tempuri.org/">
      <sjsonTTChungTK>${objTTChung}</sjsonTTChungTK>
      <sjsonTTCTS>${sjsonTTCTS}</sjsonTTCTS>
    </Taotokhai>
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
      NotifyHelper.Success(parseRes.message);
      history.push(`../../to-khai-chung-tu/${parseRes.data}`);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const SuaToKhaiChungTu = async (
    objTTChung: string,
    sjsonTTCTS: string,
    Matokhai_CT: number,
    madonvi: string | undefined,
  ) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Suatokhai  xmlns="http://tempuri.org/">
      <sjsonTTChungTK>${objTTChung}</sjsonTTChungTK>
      <sjsonTTCTS>${sjsonTTCTS}</sjsonTTCTS>
      <Matokhai_CT>${Matokhai_CT}</Matokhai_CT>
      <madonvi>${madonvi}</madonvi>
    </Suatokhai>
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
      NotifyHelper.Success(parseRes.message);
      history.push(`../../to-khai-chung-tu/${Matokhai_CT}`);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  if (
    toKhaiViewModel &&
    toKhaiViewModel?.thongtintokhai?.MatokhaiCT &&
    toKhaiViewModel?.thongtintokhai?.Trangthai !== eToKhaiStatus.TAO_MOI
  ) {
    return (
      <ToKhaiCTView matokhaiCT={toKhaiViewModel?.thongtintokhai?.MatokhaiCT} />
    );
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
                    history.goBack();
                  }}
                />
              </Box>
              <Heading
                text="Lập tờ khai Đăng ký/ Thay đổi thông tin sử dụng chứng từ điện tử"
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
                      defaultValue={moment().format("YYYY-MM-DD")}
                      // value={}
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
                        setCoQuanThue(coQuanThue);
                        console.log(id);

                        setCoQuanThueId(id);
                        // setValue("noi_lap", coQuanThue?.tinh ?? "");
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
                <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
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
                  <FormControl sx={{ ml: 3 }}>
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
                      <Text text="Địa danh" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="dia_danh"
                      block
                      required
                      validateMessage="Vui lòng điền Địa danh"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
              </Box>
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label="2. Đối tượng phát hành chứng từ điện tử">
            <Box>
              <ToKhaiCTFormDoiTuong
                register={register}
                errors={errors}
                control={control}
                watch={watch}
                setValue={setValue}
              />
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label="3. Loại hình sử dụng chứng từ điện tử">
            <Box>
              <ToKhaiCTFormLoaiChungTu
                register={register}
                errors={errors}
                control={control}
                watch={watch}
                setValue={setValue}
              />
            </Box>
          </PaperFormGroup>
          <PaperFormGroup label="4. Hình thức gửi dữ liệu chứng từ điện tử">
            <Box>
              <ToKhaiCTFormHinhThucChungTu
                register={register}
                errors={errors}
                control={control}
                watch={watch}
                setValue={setValue}
              />
            </Box>
          </PaperFormGroup>

          <PaperFormGroup label="6. Danh sách chứng thư số sử dụng">
            <ToKhaiCTFormCTS
              register={register}
              errors={errors}
              control={control}
              watch={watch}
              setValue={setValue}
              setCerFiles={setCerFiles}
              cerFiles={cerFiles}
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
              defaultValue={false}
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
              sx={{
                flex: 1,
              }}
            >
              {/* <PrintToKhaiButton id={id} /> */}
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
                    history.goBack();
                  }}
                />
                {(id === 0 ||
                  (toKhaiViewModel &&
                    (toKhaiViewModel as any).thongtintokhai?.Trangthai ===
                      1)) && (
                  <>
                    <Button
                      text="Lưu"
                      isLoading={isSaving}
                      type="submit"
                      disabled={!is_camket}
                      sx={{ mr: 2, minWidth: "100px" }}
                      size="large"
                      variant="primary"
                      tooltip="Vui lòng xác nhận Cam kết chịu trách nhiệm..."
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
                          handleGetBase64KySo(user?.donvi_ma_dv);
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
            updateToKhaiSauKy({
              xmldaky: signedtext,
              trangthai: 2,
              mst: user?.donvi_ma_dv,
              matokhai: id,
            });
          }}
        />
      )}
      {/* {isShowPhatHanhResultModal && toKhaiPhatHanhPushNotifyModel && (
        <ToKhaiTimeLineModal
          MatokhaiCT={id}
          onClose={() => {
            setIsShowPhatHanhResultModal(false);
          }}
        />
      )} */}
    </Box>
  );
};
