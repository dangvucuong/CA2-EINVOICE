import {
  DownloadIcon,
  IssueClosedIcon,
  PencilIcon,
} from "@primer/octicons-react";
import { Box, Checkbox, Flash, FormControl, Link } from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useLocation, useParams } from "react-router-dom";
import { HOA_DON_PHATHANH_API, hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import { hoaDonKyLoApi } from "../../api/hoa-don/hoaDonKyLoApi";
import ButtonGipInfo from "../../component-data/btn-gip-info";
import DonViBanHangView from "../../component-data/don-vi-ban-hang-view";
import KySoModal from "../../component-data/ky-so-modal";
import PrintHoaDonButton from "../../component-data/print-hoa-don-button";
import SelectBoxLyDoDieuChinh from "../../component-data/selectbox-ly-do-dieu-chinh";
import TextInputMstKhachHang from "../../component-data/text-mst-khachhang-search";
import BackButton from "../../component-ui/back-button";
import Button from "../../component-ui/button";
import DateInput from "../../component-ui/date-input";
import FormGroupInline from "../../component-ui/form-group-inline";
import Heading from "../../component-ui/heading";
import PaperFormGroup from "../../component-ui/paper-form-group";
import PlaceHolder from "../../component-ui/place-holder";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { useLoaiHoaDonCT } from "../../hooks/useLoaiHoaDonCT";
import { useWindowSize } from "../../hooks/useWindowSize";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { eSize } from "../../models/commons/eSize";
import { IIHoaDonAddOrEditModel } from "../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { IHoaDonVM } from "../../models/responses/hoa-don/IHoaDonVM";
import { IHoaDonPhatHanhPushNotifyModel } from "../../models/responses/hub/IHoaDonPhatHanhPushNotifyModel";
import HoaDonGocInfoModal, { IHoaDonGocInfoValue } from "./HoaDonGocInfo";

import HoaDonPhatHanhResultModal from "./HoaDonPhatHanhResultModal";
import HoaDonView from "./HoaDonView";
import { axiosClient } from "../../api/axiosClient";
import SelectBoxLoaiChungTuPhatHanh from "../../component-data/selectbox-loai-chung-tu-phat-hanh/SelectBoxLoaiChungTuPhatHanh";
import SelectBoxMauSoChungTuPhatHanh from "../../component-data/selectbox-mau-so-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import { parseSoapResponse } from "../../helpers/common";
import XemChungTu from "../chung-tu/XemChungTu";

const ChungTuForm = () => {
  const { id: pId }: any = useParams();
  const location = useLocation();
  const { isMobile } = useWindowSize();
  const { checkAccesiableTo } = useCommonContext();
  const machungtu = pId ? parseInt(pId) : 0;
  const [chungtuViewModel, setchungtuViewModel] = useState<any>();
  const [hinhthucchungtu, setHinhThucChungTu] = useState(0);
  const [thongTinChungTuGoc, setThongTinChungTuGoc] = useState<any>(null);
  const [openModalXemChungTu, setOpenModalXemChungTu] = useState(false);
  const [isLoadHoaDonDone, setIsLoadHoaDonDone] = useState<boolean>(true);
  const { user } = useAuth();

  const fakeToByPassValid: any = {};

  const [formData, setFormData] = useState<any>({
    ...fakeToByPassValid,
    loai_chung_tu: "03/TNCN",
    mau_so_chung_tu: "03/TNCN",
    ca_nhan_cu_tru: false,
    ngay_chung_tu: moment(new Date()).format("YYYY-MM-DD"),
  });

  const history = useHistory();

  const [isSaving, setIsSaving] = useState(false);
  const [base64KySo, setBase64KySo] = useState("");
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);
  const [isKySoVaPhatHanh, setIsKySoVaPhatHanh] = useState(false);
  const { signalRConnectionServer } = useCommonContext();
  const [isShowPhatHanhResultModal, setIsShowPhatHanhResultModal] =
    useState(false);
  const [hoaDongPhatHanhPushNotifyModel, setHoaDongPhatHanhPushNotifyModel] =
    useState<IHoaDonPhatHanhPushNotifyModel>();

  useEffect(() => {
    if (signalRConnectionServer) {
      if (machungtu > 0) {
        signalRConnectionServer.on("THONG_DIEP_HAS_RESULT", (message: any) => {
          console.log({
            THONG_DIEP_HAS_RESULT: message,
          });
          onHoaDonPhatHanhHasResult(message);
        });
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [signalRConnectionServer, machungtu]);

  const onHoaDonPhatHanhHasResult = (
    message: IHoaDonPhatHanhPushNotifyModel,
  ) => {
    if (message.id === machungtu && machungtu > 0) {
      setIsShowPhatHanhResultModal(true);
      setHoaDongPhatHanhPushNotifyModel(message);
    }
  };

  useEffect(() => {
    // search?tinhchatct=2&mact_goc=27"
    const searchParams = new URLSearchParams(location.search);

    const tinhchatct = searchParams.get("tinhchatct");
    const mact_goc = searchParams.get("mact_goc");

    setHinhThucChungTu(tinhchatct ? parseInt(tinhchatct) : 0);

    // Nếu có mact_goc và machungtu = 0 thì lấy thông tin chứng từ gốc
    if (mact_goc && parseInt(mact_goc) > 0 && machungtu >= 0) {
      handleGetDetailAsync(mact_goc?.toString(), true);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  useEffect(() => {
    if (machungtu > 0) {
      handleGetDetailAsync(machungtu.toString());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [machungtu]);

  const {
    watch,
    register,
    handleSubmit,
    formState: { errors },
    clearErrors,
    setValue,
    getValues,
    trigger,
    setError,
    reset,
    control,
    setFocus,
  } = useForm<any>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...chungtuViewModel,
    },
  });

  const handleGetDetailAsync = async (
    machungtu: string,
    isChungTuGoc?: boolean,
  ) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <Laythongtinchungtu xmlns="http://tempuri.org/">
        <machungtu>${machungtu}</machungtu>
        <madonvi>${user?.donvi_ma_dv}</madonvi>
      </Laythongtinchungtu>
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
      const ttchungtu = parseRes.data[0];
      setchungtuViewModel(parseRes.data[0]);

      reset({
        nguoi_mua_mst: ttchungtu?.MasothueNNT,
        nguoi_mua_ten_donvi: ttchungtu?.TenNNT,
        nguoi_mua_email: ttchungtu?.EmailNNT,
        nguoi_mua_dien_thoai: ttchungtu?.DienthoaiNNT,
        nguoi_mua_dia_chi: ttchungtu?.DiachiNNT,
        nguoi_mua_cccd:
          ttchungtu?.SoCMND?.trim()?.length === 12 ? ttchungtu?.SoCMND : "",
        ho_chieu:
          ttchungtu?.SoCMND?.trim()?.length === 9 ? ttchungtu?.SoCMND : "",
        tu_thang: ttchungtu?.ThangTN,
        den_thang: ttchungtu?.Denthang,
        nam: ttchungtu?.NamTN,
        quoc_tich: ttchungtu?.QuoctichNNT,
        khoan_thu_nhap: ttchungtu?.ThunhapCN,
        tong_thu_nhap_chiu_thue: ttchungtu?.TongTNChiuthue,
        tong_thu_nhap_tinh_thue: ttchungtu?.TongTNTinhthue,
        thue_thu_nhap_ca_nhan: ttchungtu?.ThueTNCN,
        bao_hiem: ttchungtu?.Baohiem,
        khoan_dong_tu_thien: ttchungtu?.TThien,
      });

      setFormData({
        ...formData,
        loai_chung_tu: ttchungtu?.MSChungtu ?? "",
        mau_so_chung_tu: ttchungtu?.MSChungtu ?? "",
        ky_hieu_chung_tu: ttchungtu?.KHChungtu ?? "",
        ca_nhan_cu_tru: ttchungtu?.CanhanCT === 1,
        ngay_chung_tu: ttchungtu?.NgaylapCT,
      });

      if (isChungTuGoc) {
        setThongTinChungTuGoc({
          mau_so_chung_tu_goc: ttchungtu?.MSChungtu ?? "",
          ky_hieu_chung_tu_goc: ttchungtu?.KHChungtu ?? "",
          so_chung_tu_goc: ttchungtu?.Sochungtu ?? "",
          ngay_lap_chung_tu_goc: ttchungtu?.NgaylapCT ?? "",
          loai_chung_tu_goc: ttchungtu?.LoaiCTLienquan ?? "",
        });
      }
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const LaysoCT_update = async (
    mst: string,
    machungtu: string,
    kyhieu: string,
  ) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LaysoCT_update xmlns="http://tempuri.org/">
      <MasothueTC>${mst}</MasothueTC>
      <KHCTu>${kyhieu}</KHCTu>
      <mactu>${machungtu}</mactu>
    </LaysoCT_update>
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

      await handleGetDetailAsync(machungtu, true);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleKySoRemoteAsync = async () => {};
  const handleKySoVaPhatHanhRemoteAsync = async () => {};
  const handleUpdateKySoSuccess = async (signedtext: string) => {
    console.log(signedtext);
    // if (chungtuViewModel) {
    //   setIsSaving(true);

    //   setIsSaving(false);
    // }
  };

  const onSubmit = async (data: any) => {
    let isValid: boolean = true;
    // if ((formData?.loai_hoa_don_ct_id ?? 0) <= 0) {
    //   isValid = false;
    //   setError("loai_hoa_don_ct_id", {});
    // }
    // if ((formData?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? "") === "") {
    //   isValid = false;
    //   setError("hoa_don_dang_ky_phat_hanh_ky_hieu", {});
    // }
    // if ((formData?.hoa_don_dang_ky_phat_hanh_mau_so ?? "") === "") {
    //   isValid = false;
    //   setError("hoa_don_dang_ky_phat_hanh_mau_so", {});
    // }

    // const payload = getAddOrEditFormModel(data);

    // }
    // if (payload.nguoi_mua_mst !== undefined && payload.nguoi_mua_mst !== "") {
    //   if (!payload.nguoi_mua_email) {
    //     NotifyHelper.Error("Vui lòng điền Email người mua hàng");
    //     setError("nguoi_mua_email", {});
    //     isValid = false;
    //   }
    // }
    // if (payload.nguoi_mua_mst !== undefined && payload.nguoi_mua_mst !== "") {
    //   if (!payload.nguoi_mua_dia_chi) {
    //     NotifyHelper.Error("Vui lòng điền Địa chỉ người mua hàng");
    //     setError("nguoi_mua_dia_chi", {});
    //     isValid = false;
    //   }
    // }
    // payload.ngay_hoa_don = moment(formData.ngay_hoa_don ?? new Date()).format(
    //   "YYYY-MM-DD"
    // );

    if (data.nguoi_mua_mst !== undefined && data.nguoi_mua_mst !== "") {
      if (!data.nguoi_mua_ten_donvi) {
        NotifyHelper.Error("Vui lòng điền Tên người nộp thuế");
        setError("nguoi_mua_ten_donvi", {
          type: "manual",
          message: "Vui lòng điền Tên người nộp thuế",
        });
        isValid = false;
      }

      //check độ dài mst người mua không được lớn hơn 14
      if (data.nguoi_mua_mst?.trim()?.length > 14) {
        NotifyHelper.Error(
          "Mã số thuế người mua hàng không được vượt quá 14 ký tự",
        );
        setError("nguoi_mua_mst", {
          type: "manual",
          message: "Mã số thuế người mua hàng không được vượt quá 14 ký tự",
        });
        isValid = false;
      }
    }

    if (data.nguoi_mua_email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(data.nguoi_mua_email)) {
        NotifyHelper.Error("Email không đúng định dạng");
        setError("nguoi_mua_email", {
          type: "manual",
          message: "Email không đúng định dạng",
        });
        isValid = false;
        setFocus("nguoi_mua_email");
      }
    }

    if (data?.nguoi_mua_cccd) {
      // nếu quá 12 ký tự thì báo lỗi
      if (data.nguoi_mua_cccd?.length !== 12) {
        NotifyHelper.Error(
          "Căn cước công dân người mua hàng phải đúng 12 ký tự",
        );
        setError("nguoi_mua_cccd" as any, {
          type: "manual",
          message: "Căn cước công dân người mua hàng phải đúng 12 ký tự",
        });
        setFocus("nguoi_mua_cccd" as any);
        isValid = false;
      }
    }

    if (data?.ho_chieu) {
      // nếu quá 12 ký tự thì báo lỗi
      if (data.ho_chieu?.length !== 9) {
        NotifyHelper.Error("Số hộ chiếu người mua hàng phải đúng 9 ký tự");
        setError("ho_chieu" as any, {
          type: "manual",
          message: "Số hộ chiếu người mua hàng phải đúng 9 ký tự",
        });
        setFocus("ho_chieu" as any);
        isValid = false;
      }
    }

    if (!isValid) return;

    if (hinhthucchungtu === 0) {
      if (machungtu > 0) {
        await SuaChungTu(data);
      } else {
        await TaoChungTu(data);
      }
    } else {
      await TaoChungTuThayTheDieuChinh(data);
    }
  };

  const TaoChungTuThayTheDieuChinh = async (data: any) => {
    const soCCCD = data?.nguoi_mua_cccd ? data.nguoi_mua_cccd : data?.ho_chieu;

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <TaoChungTuThayTheDieuChinh xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <mau_so>03/TNCN</mau_so>
      <kyhieu>${formData?.ky_hieu_chung_tu}</kyhieu>
      <tenchungtu>Chứng từ khấu trừ thuế thu nhập cá nhân</tenchungtu>
      <ngaylap>${formData.ngay_chung_tu}</ngaylap>
      <mstnguoint>${data.nguoi_mua_mst?.trim()}</mstnguoint>
      <tennnt>${data.nguoi_mua_ten_donvi}</tennnt>
      <diachi>${data?.nguoi_mua_dia_chi}</diachi>
      <dienthoai>${data.nguoi_mua_dien_thoai}</dienthoai>
      <email>${data.nguoi_mua_email}</email>
      <cccd>${soCCCD}</cccd>
      <tuthang>${data.tu_thang}</tuthang>
      <denthang>${data.den_thang}</denthang>
      <nam>${data.nam}</nam>
      <quoctich>${data.quoc_tich}</quoctich>
      <khoanthunhap>${data.khoan_thu_nhap}</khoanthunhap>
      <canhancutru>${formData?.ca_nhan_cu_tru ? 1 : 0}</canhancutru>
      <tongthunhapchiuthue>${
        data?.tong_thu_nhap_chiu_thue
      }</tongthunhapchiuthue>
      <tongthunhaptinhthue>${
        data?.tong_thu_nhap_tinh_thue
      }</tongthunhaptinhthue>
      <thuetncn>${data?.thue_thu_nhap_ca_nhan}</thuetncn>
      <baohiem>${data?.bao_hiem}</baohiem>
      <tthien>${data?.khoan_dong_tu_thien}</tthien>
      <TinhchatCT>${hinhthucchungtu}</TinhchatCT>
      <LoaiCTLienquan>1</LoaiCTLienquan>
      <KHMSCTLienquan>${
        thongTinChungTuGoc?.mau_so_chung_tu_goc
      }</KHMSCTLienquan>
      <KHCTLienquan>${thongTinChungTuGoc?.ky_hieu_chung_tu_goc}</KHCTLienquan>
      <SoCTLienquan>${thongTinChungTuGoc?.so_chung_tu_goc}</SoCTLienquan>
      <NgaylapCTLienquan>${moment(
        thongTinChungTuGoc?.ngay_lap_chung_tu_goc,
      ).format("YYYY-MM-DD")}</NgaylapCTLienquan>
    </TaoChungTuThayTheDieuChinh>
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
      history.push(
        `../../chung-tu/form/${parseRes.data}?tinhchatct=${hinhthucchungtu}&mact_goc=${parseRes.data}`,
      );
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const TaoChungTu = async (data: any) => {
    const soCCCD = data?.nguoi_mua_cccd ? data.nguoi_mua_cccd : data?.ho_chieu;

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <TaoChungTu xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <mau_so>03/TNCN</mau_so>
      <kyhieu>${formData?.ky_hieu_chung_tu}</kyhieu>
      <tenchungtu>Chứng từ khấu trừ thuế thu nhập cá nhân</tenchungtu>
      <ngaylap>${formData.ngay_chung_tu}</ngaylap>
      <mstnguoint>${data.nguoi_mua_mst?.trim()}</mstnguoint>
      <tennnt>${data.nguoi_mua_ten_donvi}</tennnt>
      <diachi>${data?.nguoi_mua_dia_chi}</diachi>
      <dienthoai>${data.nguoi_mua_dien_thoai}</dienthoai>
      <email>${data.nguoi_mua_email}</email>
      <cccd>${soCCCD}</cccd>
      <tuthang>${data.tu_thang}</tuthang>
      <denthang>${data.den_thang}</denthang>
      <nam>${data.nam}</nam>
      <quoctich>${data.quoc_tich}</quoctich>
      <khoanthunhap>${data.khoan_thu_nhap}</khoanthunhap>
      <canhancutru>${formData?.ca_nhan_cu_tru ? 1 : 0}</canhancutru>
      <tongthunhapchiuthue>${
        data?.tong_thu_nhap_chiu_thue
      }</tongthunhapchiuthue>
      <tongthunhaptinhthue>${
        data?.tong_thu_nhap_tinh_thue
      }</tongthunhaptinhthue>
      <thuetncn>${data?.thue_thu_nhap_ca_nhan}</thuetncn>
      <baohiem>${data?.bao_hiem}</baohiem>
      <tthien>${data?.khoan_dong_tu_thien}</tthien>
    </TaoChungTu>
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

      history.push(`../../chung-tu/form/${parseRes.data}`);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const SuaChungTu = async (data: any) => {
    const soCCCD = data?.nguoi_mua_cccd ? data.nguoi_mua_cccd : data?.ho_chieu;
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <SuaChungTu xmlns="http://tempuri.org/">
      <machungtu>${machungtu}</machungtu>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <mau_so>03/TNCN</mau_so>
      <kyhieu>${formData?.ky_hieu_chung_tu}</kyhieu>
      <ngaylap>${formData.ngay_chung_tu}</ngaylap>
      <mstnguoint>${data.nguoi_mua_mst?.trim()}</mstnguoint>
      <tennnt>${data.nguoi_mua_ten_donvi}</tennnt>
      <diachi>${data?.nguoi_mua_dia_chi}</diachi>
      <dienthoai>${data.nguoi_mua_dien_thoai}</dienthoai>
      <email>${data.nguoi_mua_email}</email>
      <cccd>${soCCCD}</cccd>
      <tuthang>${data.tu_thang}</tuthang>
      <denthang>${data.den_thang}</denthang>
      <nam>${data.nam}</nam>
      <quoctich>${data.quoc_tich}</quoctich>
      <khoanthunhap>${data.khoan_thu_nhap}</khoanthunhap>
      <canhancutru>${formData?.ca_nhan_cu_tru ? 1 : 0}</canhancutru>
      <tongthunhapchiuthue>${
        data?.tong_thu_nhap_chiu_thue
      }</tongthunhapchiuthue>
      <tongthunhaptinhthue>${
        data?.tong_thu_nhap_tinh_thue
      }</tongthunhaptinhthue>
      <thuetncn>${data?.thue_thu_nhap_ca_nhan}</thuetncn>
      <baohiem>${data?.bao_hiem}</baohiem>
      <tthien>${data?.khoan_dong_tu_thien}</tthien>
    </SuaChungTu>
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

      history.push(`../../chung-tu/form/${parseRes.data}`);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleKySo = async () => {
    if (user) {
      await LaysoCT_update(
        user?.donvi_ma_dv,
        machungtu.toString(),
        formData?.ky_hieu_chung_tu,
      );
    }
    // setIsKySoVaPhatHanh(false);
  };

  const UpdateChungTuSauKy = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <UpdateChungTuSauKy xmlns="http://tempuri.org/">
        <xmlthongdiep>${values?.xmldaky}</xmlthongdiep>
        <trangthai>${values?.trangthai}</trangthai>
        <mst>${values?.mst}</mst>
        <machungtu>${values?.machungtu}</machungtu>
      </UpdateChungTuSauKy>
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
      NotifyHelper.Success("Ký số thành công");
      await handleGetDetailAsync(machungtu.toString());
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const GuichungtulenCQT = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <GuiChungTuCQT xmlns="http://tempuri.org/">
        <machungtu>${values?.machungtu}</machungtu>
        <madonvi>${values?.madonvi}</madonvi>
        <thongdiep>${values?.thongdiep}</thongdiep>
      </GuiChungTuCQT>
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
      NotifyHelper.Success(parseRes.message);
      await handleGetDetailAsync(machungtu.toString());
    } else {
      NotifyHelper.Error(parseRes.message ?? "Có lỗi xảy ra");
    }
  };

  // if (
  //   chungtuViewModel &&
  //   machungtu > 0 &&
  //   machungtu === chungtuViewModel?.MaCT &&
  //   chungtuViewModel.TinhtrangCT === eHoaDonTrangThai.DA_PHAT_HANH
  // ) {
  //   return <HoaDonView id={chungtuViewModel.id} />;
  // }

  // if (user && user.donvi && (user.donvi.to_khai_success_id ?? 0) === 0) {
  //   return (
  //     <Flash variant="danger">
  //       <b> Vui lòng tạo tờ khai trước!</b>
  //       <br />
  //       Chỉ được tạo hóa đơn sau khi đã có tờ khai được Cơ quan thuế chập nhận
  //     </Flash>
  //   );
  // }

  return (
    <Box>
      <BackButton />
      {!isLoadHoaDonDone && <PlaceHolder line_number={10} />}
      {isLoadHoaDonDone && (
        <form onSubmit={handleSubmit(onSubmit)}>
          {hinhthucchungtu !== 0 && (
            <Box sx={{}}>
              {thongTinChungTuGoc &&
                thongTinChungTuGoc?.ky_hieu_chung_tu_goc && (
                  <>
                    {hinhthucchungtu === 2 && (
                      <Heading
                        size={eSize.medium}
                        text={"LẬP CHỨNG TỪ ĐIỀU CHỈNH"}
                      />
                    )}
                    {hinhthucchungtu === 1 && (
                      <Heading
                        size={eSize.medium}
                        text={"LẬP CHỨNG TỪ THAY THẾ"}
                      />
                    )}
                    <PaperFormGroup label="Thông tin chứng từ gốc">
                      <Box
                        sx={{
                          display: "flex",
                        }}
                      >
                        <Box sx={{ fontSize: "15px" }}>
                          Ký hiệu mẫu số:{" "}
                          <b>{thongTinChungTuGoc?.mau_so_chung_tu_goc}</b>, Ký
                          hiệu chứng từ:{" "}
                          <b>{thongTinChungTuGoc?.ky_hieu_chung_tu_goc}</b>, Số
                          chứng từ: <b>{thongTinChungTuGoc?.so_chung_tu_goc}</b>
                          , Ngày chứng từ:{" "}
                          <b>
                            {moment(
                              thongTinChungTuGoc?.ngay_lap_chung_tu_goc,
                            ).format("DD/MM/YYYY")}
                          </b>
                          ;
                        </Box>
                      </Box>
                    </PaperFormGroup>
                  </>
                )}
            </Box>
          )}
          <Box
            sx={{
              borderBottomWidth: 1,
              borderBottomStyle: "solid",
              borderBottomColor: "border.default",
              pb: 3,
              mb: 3,
              mt: thongTinChungTuGoc ? 0 : -3,
            }}
          >
            <PaperFormGroup
              label="Chứng từ "
              isHideBorder={!thongTinChungTuGoc}
              style={{
                flexDirection: ["column", "column", "row"],
                mt: [0, 0, 4],
                pt: 4,
                gap: [2, 2, 0],
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  // borderBottomWidth: 1,
                  // borderBottomStyle: "solid",
                  // borderBottomColor: "border.default",
                  pb: [0, 0, 3],
                  flexDirection: ["column", "column", "row"],
                }}
              >
                <Box
                  sx={{
                    flex: 1,
                    display: "flex",
                    flexWrap: "wrap",
                  }}
                >
                  <FormControl sx={{ mr: 3, mb: 2 }}>
                    <FormControl.Label>Loại chứng từ</FormControl.Label>
                    <SelectBoxLoaiChungTuPhatHanh
                      value={formData?.loai_chung_tu}
                      onValueChanged={(value) => {
                        clearErrors("loai_chung_tu");
                        setFormData({
                          ...formData,
                          loai_chung_tu: value,
                        });
                      }}
                      isFormLap={true}
                    />
                    {errors && errors["loai_chung_tu"] && (
                      <FormControl.Validation variant="error">
                        Vui lòng chọn loại hóa đơn
                      </FormControl.Validation>
                    )}
                  </FormControl>
                  <Box sx={{ mr: 3, mb: 2 }}>
                    <FormControl>
                      <FormControl.Label>Mẫu số</FormControl.Label>
                      <SelectBoxMauSoChungTuPhatHanh
                        loai_chung_tu={formData?.loai_chung_tu ?? ""}
                        value={formData?.mau_so_chung_tu ?? ""}
                        onValueChanged={(value) => {
                          clearErrors("mau_so_chung_tu");
                          setFormData({
                            ...formData,
                            mau_so_chung_tu: value,
                          });
                        }}
                      />
                      {errors && errors["mau_so_chung_tu"] && (
                        <FormControl.Validation variant="error">
                          Vui lòng chọn mẫu số
                        </FormControl.Validation>
                      )}
                    </FormControl>
                  </Box>
                  <Box sx={{ mr: 3, mb: 2 }}>
                    <FormControl>
                      <FormControl.Label>Ký hiệu</FormControl.Label>
                      <SelectBoxKyHieuChungTuPhatHanh
                        value={formData?.ky_hieu_chung_tu ?? ""}
                        onValueChanged={(value) => {
                          clearErrors("ky_hieu_chung_tu");
                          setFormData({
                            ...formData,
                            ky_hieu_chung_tu: value,
                          });
                        }}
                        mau_so={formData?.mau_so_chung_tu}
                      />
                      {errors &&
                        errors["hoa_don_dang_ky_phat_hanh_ky_hieu"] && (
                          <FormControl.Validation variant="error">
                            Vui lòng chọn mẫu số
                          </FormControl.Validation>
                        )}
                    </FormControl>
                  </Box>
                </Box>
              </Box>
            </PaperFormGroup>
          </Box>
          <Box
            sx={{
              pt: 3,
            }}
          >
            <Box
              sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                flexDirection: "column",
                pb: [3, 3, 0],
              }}
            >
              <Heading
                text={"Chứng từ khấu trừ thuế thu nhập cá nhân"}
                size={window.innerWidth >= 768 ? eSize.large : eSize.medium}
              />

              <FormGroupInline label="Ngày chứng từ">
                <DateInput
                  name="ngay_chung_tu"
                  register={register}
                  required
                  value={
                    formData.ngay_chung_tu
                      ? moment(formData.ngay_chung_tu).format("DD/MM/YYYY")
                      : moment(new Date()).format("DD/MM/YYYY")
                  }
                  onValueChanged={(value, date) => {
                    // debugger
                    // setValue("ngay_hoa_don", moment(date).format("YYYY-MM-DD"))
                    setFormData({
                      ...formData,
                      ngay_chung_tu: moment(date).format("YYYY-MM-DD"),
                    });
                  }}
                />
              </FormGroupInline>
            </Box>
            <PaperFormGroup
              label="Đơn vị bán hàng"
              style={{
                flexDirection: ["column", "column", "row"],
                mt: [0, 0, 4],
                pt: [2, 2, 4],
                pb: [3, 3, 0],
                gap: [2, 2, 0],
              }}
            >
              {user?.donvi && <DonViBanHangView donvi={user?.donvi} />}
            </PaperFormGroup>
            <PaperFormGroup
              label="Đơn vị mua hàng"
              style={{
                flexDirection: ["column", "column", "row"],
                mt: [0, 0, 4],
                pt: [2, 2, 4],
                pb: [3, 3, 0],
                gap: [2, 2, 0],
              }}
            >
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <FormControl>
                  <FormControl.Label>
                    <Text text="Mã số thuế NNT" />
                  </FormControl.Label>
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: ["column", "row"],
                      gap: [2, 2, 0],
                      width: "100%",
                    }}
                  >
                    <TextInputMstKhachHang
                      register={register}
                      name="nguoi_mua_mst"
                      // required
                      validateMessage="Vui lòng điền Mã số thuế"
                      errors={errors}
                      value={getValues("nguoi_mua_mst") ?? ""}
                      onValueChanged={(data) => {
                        setValue("nguoi_mua_mst", data.text);
                        trigger("nguoi_mua_mst");
                        setValue(
                          "nguoi_mua_ten_donvi",
                          data.khach_hang?.ten_don_vi ?? "",
                        );
                        setValue(
                          "nguoi_mua_email",
                          data.khach_hang?.email ?? "",
                        );
                        setValue(
                          "nguoi_mua_ten",
                          data.khach_hang?.ten_khach_hang ?? "",
                        );
                        setValue(
                          "nguoi_mua_dia_chi",
                          data.khach_hang?.dia_chi ?? "",
                        );
                        // const x = getValues("nguoi_mua_mst")
                        // console.log({
                        //     data,
                        //     x
                        // });
                        // loaiTienInputRef.current.focus();
                      }}
                      sx={{
                        width: ["100%", 300],
                      }}
                    />
                    <Box sx={{ ml: [0, 0, 2] }}>
                      <ButtonGipInfo
                        mst={getValues("nguoi_mua_mst")}
                        onApply={(data) => {
                          setValue("nguoi_mua_ten_donvi", data?.ten_dv ?? "");
                          setValue("nguoi_mua_dia_chi", data?.dia_chi ?? "");
                        }}
                      />
                    </Box>
                  </Box>

                  <TextInput
                    sx={{
                      display: "none",
                    }}
                    register={register}
                    name="nguoi_mua_mst"
                    width={300}
                    // required
                    validateMessage="Vui lòng điền Mã số thuế"
                    errors={errors}
                  />
                </FormControl>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["1fr", "1fr 2fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tên người nộp thuế" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_ten_donvi"
                      // required={nguoi_mua_mst !== ""}
                      block
                      validateMessage="Vui lòng điền Đơn vị mua hàng"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Địa chỉ NNT" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_dia_chi"
                      // required={nguoi_mua_mst !== ""}
                      block
                      validateMessage="Vui lòng điền Địa chỉ"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["1fr", "1fr 1fr 1fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Quốc tịch" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="quoc_tich"
                      block
                      // required
                      validateMessage="Vui lòng điền quốc tịch"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Email" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_email"
                      // required={nguoi_mua_mst !== ""}
                      block
                      validateMessage="Vui lòng điền Email"
                      errors={errors}
                      onChange={(e) => {
                        const value = e.target.value.trim();
                        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

                        // Ưu tiên validate theo thứ tự: độ dài -> định dạng
                        if (value.length > 50) {
                          setError("nguoi_mua_email", {
                            type: "manual",
                            message: "Email không được vượt quá 50 ký tự",
                          });
                          return;
                        }

                        if (value && !emailRegex.test(value)) {
                          setError("nguoi_mua_email", {
                            type: "manual",
                            message: "Email không đúng định dạng",
                          });
                          return;
                        }

                        // ✅ Nếu hợp lệ -> clear error
                        clearErrors("nguoi_mua_email");
                      }}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Số điện thoại" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_dien_thoai"
                      block
                      // required
                      validateMessage="Vui lòng điền Số điện thoại"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["1fr", "1fr 1fr 1fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Số CCCD" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_cccd"
                      // required
                      block
                      validateMessage="Vui lòng điền Số Căn cước công dân"
                      errors={errors}
                      type="text"
                      onChange={(e) => {
                        if (e.target.value.length > 12) {
                          setError("nguoi_mua_cccd" as any, {
                            type: "manual",
                            message:
                              "Căn cước công dân phải đúng 12 ký tự (không bao gồm dấu cách)",
                          });
                        } else if (
                          e.target.value.length < 12 &&
                          e.target.value.length > 0
                        ) {
                          setError("nguoi_mua_cccd" as any, {
                            type: "manual",
                            message:
                              "Căn cước công dân phải đúng 12 ký tự (không bao gồm dấu cách)",
                          });
                        } else {
                          clearErrors("nguoi_mua_cccd" as any);
                        }
                      }}
                    />
                  </FormControl>

                  <FormControl>
                    <FormControl.Label>
                      <Text text="Số hộ chiếu" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="ho_chieu"
                      // required
                      block
                      validateMessage="Vui lòng điền Số hộ chiếu"
                      errors={errors}
                      type="text"
                      onChange={(e) => {
                        if (e.target.value.length > 9) {
                          setError("ho_chieu" as any, {
                            type: "manual",
                            message:
                              "Số hộ chiếu phải đúng 9 ký tự (không bao gồm dấu cách)",
                          });
                        } else if (
                          e.target.value.length < 9 &&
                          e.target.value.length > 0
                        ) {
                          setError("ho_chieu" as any, {
                            type: "manual",
                            message:
                              "Số hộ chiếu phải đúng 9 ký tự (không bao gồm dấu cách)",
                          });
                        } else {
                          clearErrors("ho_chieu" as any);
                        }
                      }}
                    />
                  </FormControl>
                </Box>

                <Box sx={{ mt: 2 }}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Cá nhân cư trú" />
                    </FormControl.Label>
                    <Checkbox
                      readOnly
                      checked={formData?.ca_nhan_cu_tru}
                      onChange={(e) => {
                        setFormData({
                          ...formData,
                          ca_nhan_cu_tru: e.target.checked,
                        });
                      }}
                      name="ca_nhan_cu_tru"
                    />
                  </FormControl>
                </Box>
              </Box>
            </PaperFormGroup>

            <PaperFormGroup
              label="Thuế thu nhập cá nhân khấu trừ"
              style={{
                flexDirection: ["column", "column", "row"],
                mt: [0, 0, 4],
                pt: [2, 2, 4],
                pb: [3, 3, 0],
                gap: [2, 2, 0],
              }}
            >
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["1fr", "1fr 1fr 1fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Từ tháng" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="tu_thang"
                      required
                      block
                      validateMessage="Vui lòng điền từ tháng"
                      errors={errors}
                    />
                  </FormControl>

                  <FormControl>
                    <FormControl.Label>
                      <Text text="Đến tháng" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="den_thang"
                      required
                      block
                      validateMessage="Vui lòng điền đến tháng"
                      errors={errors}
                    />
                  </FormControl>

                  <FormControl>
                    <FormControl.Label>
                      <Text text="Năm" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nam"
                      required
                      block
                      validateMessage="Vui lòng điền năm"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Khoản thu nhập" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="khoan_thu_nhap"
                      required
                      block
                      validateMessage="Vui lòng điền Khoản thu nhập"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Bảo hiểm" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="bao_hiem"
                      required
                      block
                      validateMessage="Vui lòng điền Bảo hiểm"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tổng thu nhập chịu thuế" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="tong_thu_nhap_chiu_thue"
                      required
                      block
                      validateMessage="Vui lòng điền Tổng thu nhập chịu thuế"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tổng thu nhập tính thuế" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="tong_thu_nhap_tinh_thue"
                      required
                      block
                      validateMessage="Vui lòng điền Tổng thu nhập tính thuế"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Thuế thu nhập cá nhân" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="thue_thu_nhap_ca_nhan"
                      required
                      block
                      validateMessage="Vui lòng điền Thuế thu nhập cá nhân"
                      errors={errors}
                    />
                  </FormControl>
                </Box>

                <Box sx={{}}>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Khoản đóng từ thiện, nhân đạo, khuyến học" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="khoan_dong_tu_thien"
                      required
                      block
                      validateMessage="Vui lòng điền Khoản đóng từ thiện, nhân đạo, khuyến học"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
              </Box>
            </PaperFormGroup>
          </Box>
          <Box
            sx={{
              borderTopStyle: "solid",
              borderTopWidth: 1,
              borderTopColor: "border.default",
              mt: 4,
              pt: 4,
              display: "flex",
              flexWrap: "wrap",
              flexDirection: isMobile ? "column" : "row",
              gap: 2,
            }}
          >
            <Box sx={{ flex: 1 }}>
              {machungtu > 0 && (
                <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                  <Button
                    text="Xem chứng từ"
                    variant="invisible"
                    size="medium"
                    onClick={() => {
                      setOpenModalXemChungTu(true);
                    }}
                  />
                </Box>
              )}
              {/* {hoaDonId <= 0 &&
                                <PreViewHoaDonButton
                                    data={getAddOrEditFormModel(watch())}
                                />
                            } */}
            </Box>
            <Box
              sx={{
                // flex: 1,
                display: "flex",
                flexWrap: "wrap",
                gap: 1,
                flexDirection: isMobile ? "column" : "row",
                mr: !isMobile ? 5 : 0,
              }}
            >
              <Button
                text="Hủy bỏ"
                type="button"
                sx={{ minWidth: "100px" }}
                size="large"
                variant="invisible"
                onClick={() => {
                  history.goBack();
                }}
              />
              {(chungtuViewModel?.TinhtrangCT === 1 || machungtu === 0) && (
                // {machungtu === 0 && (
                <Button
                  text="Lưu chứng từ"
                  type="submit"
                  sx={{ minWidth: "100px" }}
                  size="large"
                  isLoading={isSaving}
                />
              )}
              {machungtu > 0 && (
                <>
                  {user && chungtuViewModel?.TinhtrangCT === 1 && (
                    <Button
                      text="Ký số"
                      sx={{ minWidth: "100px" }}
                      variant="primary"
                      size="large"
                      type="button"
                      leadingVisual={IssueClosedIcon}
                      isLoading={isSaving}
                      onClick={handleKySo}
                      // disabled={
                      //   !isAllowPhatHanh ||
                      //   (chungtuViewModel?.TinhtrangCT ?? false)
                      // }
                    />
                  )}
                  {chungtuViewModel?.TinhtrangCT === 2 && (
                    <Button
                      text="Phát hành"
                      sx={{ minWidth: "100px" }}
                      // disabled={false}
                      // disabled={!isAllowPhatHanh}
                      variant="primary"
                      size="large"
                      type="button"
                      leadingVisual={IssueClosedIcon}
                      isLoading={isSaving}
                      onClick={() => {
                        // handlePhatHanhAsync("");
                        GuichungtulenCQT({
                          machungtu: machungtu,
                          madonvi: user?.donvi_ma_dv,
                          thongdiep: chungtuViewModel?.XMLChungtu,
                        });
                      }}
                      // tooltip='Bạn chỉ có thể gửi tờ khai sau khi đã ký số'
                    />
                  )}
                </>
              )}
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
            UpdateChungTuSauKy({
              xmldaky: signedtext,
              trangthai: 2,
              mst: user?.donvi_ma_dv,
              machungtu: machungtu,
            });
          }}
        />
      )}
      {isShowPhatHanhResultModal && hoaDongPhatHanhPushNotifyModel && (
        <HoaDonPhatHanhResultModal
          id={machungtu}
          data={hoaDongPhatHanhPushNotifyModel}
          onClose={() => {
            // setIsShowPhatHanhResultModal(false);
            // handleGetchungtuViewModel(machungtu);
          }}
        />
      )}

      {openModalXemChungTu && machungtu > 0 && (
        <XemChungTu
          isOpen={openModalXemChungTu}
          onClose={() => setOpenModalXemChungTu(false)}
          machungtu={machungtu?.toString()}
          user={user}
        />
      )}
    </Box>
  );
};

export default ChungTuForm;
