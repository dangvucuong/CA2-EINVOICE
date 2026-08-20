import {
  DownloadIcon,
  IssueClosedIcon,
  PencilIcon,
  PlusIcon,
  QuestionIcon,
} from "@primer/octicons-react";
import {
  Box,
  Checkbox,
  Flash,
  FormControl,
  IconButton,
  Link,
  useConfirm,
} from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useLocation, useParams } from "react-router-dom";
import { HOA_DON_PHATHANH_API, hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import { hoaDonKyLoApi } from "../../api/hoa-don/hoaDonKyLoApi";
import ButtonGipInfo from "../../component-data/btn-gip-info";
import DonViBanHangView from "../../component-data/don-vi-ban-hang-view";
import KySoModal from "../../component-data/ky-so-modal";
import PrintHoaDonBienBanButton from "../../component-data/print-hoa-don-bien-ban-button";
import PrintHoaDonButton from "../../component-data/print-hoa-don-button";
import SelectBoxHoaDon from "../../component-data/selectbox-hoa-don";
import SelectBoxKyHieuPhatHanh from "../../component-data/selectbox-ky-hieu-phat-hanh";
import SelectBoxLoaiHoaDonCTPhatHanh from "../../component-data/selectbox-loai-hoa-don-ct-phat-hanh";
import SelectBoxLoaiTien from "../../component-data/selectbox-loai-tien";
import SelectBoxLyDoDieuChinh from "../../component-data/selectbox-ly-do-dieu-chinh";
import SelectBoxMauSoPhatHanh from "../../component-data/selectbox-mau-so-phat-hanh";
import TextInputMaDaiLySearch from "../../component-data/text-ma-dai-ly-search";
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
import {
  isNguoiMuaCccd,
  validateAndNormalizeNguoiMuaBuyer,
} from "../../helpers/nguoiMuaMstHelper";
import { useAuth } from "../../hooks/useAuth";
import { useLoaiHoaDonCT } from "../../hooks/useLoaiHoaDonCT";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { useWindowSize } from "../../hooks/useWindowSize";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { eLyDoDieuChinh } from "../../models/commons/eLyDoDieuChinh";
import { eSize } from "../../models/commons/eSize";
import { IIHoaDonAddOrEditModel } from "../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import {
  IHoaDonHangHoa,
  IsHoaDonHangHoaValid,
} from "../../models/responses/hoa-don/IHoaDonHangHoa";
import { IsHoaDonLoaiPhiValid } from "../../models/responses/hoa-don/IHoaDonLoaiPhi";
import { IHoaDonVM } from "../../models/responses/hoa-don/IHoaDonVM";
import { IHoaDonPhatHanhPushNotifyModel } from "../../models/responses/hub/IHoaDonPhatHanhPushNotifyModel";
import HangHoaDieuChinhThue from "./HangHoaDieuChinhThue";
import HoaDonGocInfoModal, { IHoaDonGocInfoValue } from "./HoaDonGocInfo";
import HoaDonHangHoaList, {
  getThanhTien,
  getTongTienData,
} from "./HoaDonHangHoaList";
import HoaDonPhatHanhResultModal from "./HoaDonPhatHanhResultModal";
import HoaDonView from "./HoaDonView";
import PhieuXuatKhoDaiLySubForm from "./PhieuXuatKhoDaiLySubForm";
import PhieuXuatKhoVanChuyenSubForm from "./PhieuXuatKhoVanChuyenSubForm";
import { appInfo } from "../../AppInfo";
import { ConvertTienChu, toIsoDateOrEmpty } from "../../helpers/common";
import HoaDonBanTaiSanCongSubForm from "./HoaDonBanTaiSanCongSubForm";

const HoaDonForm = () => {
  const { id: pId }: any = useParams();
  const location = useLocation();
  const { isMobile } = useWindowSize();
  const { checkAccesiableTo } = useCommonContext();
  const hoaDonId = pId ? parseInt(pId) : 0;
  const [hoaDonViewModel, setHoaDonViewModel] = useState<IHoaDonVM>();
  const [hinhThucHoaDonId, setHinhThucHoaDonId] = useState(1);
  const [isOpenHoaDonGocModal, setIsOpenHoaDonGocModal] = useState(false);
  const [thongTinHoaDonGoc, setThongTinHoaDonGoc] =
    useState<IHoaDonGocInfoValue>();
  const [isLoadHoaDonDone, setIsLoadHoaDonDone] = useState<boolean>(
    hoaDonId > 0 ? false : true,
  );
  const confirm = useConfirm();
  const [hoaDonGocId, setHoaDonGocId] = useState(0);
  const [hangHoasGoc, setHangHoasGoc] = useState<IHoaDonHangHoa[]>([]);
  const [tongTienChu, setTongTienChu] = useState("");
  const [isSavedForKySo, setIsSavedForKySo] = useState(false);

  const isAllowPhatHanh = useMemo(() => {
    return checkAccesiableTo(HOA_DON_PHATHANH_API, "POST");
    // return true;
  }, []);

  const viewModelRequestRef = useRef(0);
  const skipKySoResetRef = useRef(false);

  const resolveFormPath = (id: number) => {
    const path = location.pathname;
    if (path.includes("/hoa-don-mtt/form")) return `/hoa-don-mtt/form/${id}`;
    if (path.includes("/hoa-don-dieu-chinh/form"))
      return `/hoa-don-dieu-chinh/form/${id}`;
    if (path.includes("/hoa-don-thay-the/form"))
      return `/hoa-don-thay-the/form/${id}`;
    return `/hoa-don/form/${id}`;
  };

  useEffect(() => {
    if (
      hinhThucHoaDonId !== 1 &&
      !thongTinHoaDonGoc &&
      isLoadHoaDonDone &&
      hoaDonGocId <= 0
    ) {
      setIsOpenHoaDonGocModal(true);
    }
  }, [hinhThucHoaDonId, thongTinHoaDonGoc, isLoadHoaDonDone, hoaDonGocId]);

  // Nếu vào từ header để lập hóa đơn mới thì set loại hóa đơn
  useEffect(() => {
    if (location.state && (location.state as any)?.from === "header") {
      const loai_hoa_don_ct_id = (location.state as any).value;
      setValue("loai_hoa_don_ct_id", loai_hoa_don_ct_id);
      setFormData((prev) => ({
        ...prev,
        loai_hoa_don_ct_id: loai_hoa_don_ct_id,
      }));
      clearErrors("loai_hoa_don_ct_id");
      trigger("loai_hoa_don_ct_id");
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location.state]);

  const { user } = useAuth();
  const fakeToByPassValid: any = {};
  const [formData, setFormData] = useState<IHoaDon>({
    ...fakeToByPassValid,
    hinh_thuc_tt: "Tiền mặt",
    ty_gia: 0,
    ngay_hoa_don: moment().format("YYYY-MM-DD"),
  });

  const isDieuChinhThue =
    formData?.hoa_don_ly_do_dieu_chinh_id === eLyDoDieuChinh.DIEU_CHINH_THUE
      ? true
      : false;
  const { loaiHoaDonCT } = useLoaiHoaDonCT(formData.loai_hoa_don_ct_id);
  useHoaDonDangKyPhatHanhLoader();

  // const { text: ngayThangNam } = useNgayThangNam();
  // const [hangHoas, setHangHoas] = useState<any[]>([{}]);
  const [hangHoas, setHangHoas] = useState<any[]>([
    {
      ten_hang: "Hàng hóa dịch vụ 1",
      don_vi_tinh: "Cái",
      so_luong: 1,
      don_gia: "0",
      thue_vat: "0%",
      hang_hoa_tinh_chat_id: 1,
      stt: 1,
      ma_hang: "HH001",
      dvt: "Cái",
      ty_le_chiet_khau: 0,
    },
  ]);
  const [loaiPhis, setLoaiPhis] = useState<any[]>([]);
  const history = useHistory();
  const [loaiTien, setLoaiTien] = useState("VND");
  const [giam_thue_ty_le, setGiam_thue_ty_le] = useState(-1);

  const [isSaving, setIsSaving] = useState(false);
  const [base64KySo, setBase64KySo] = useState("");
  const [base64BienBan, setBase64BienBan] = useState("");
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);
  const [isKySoVaPhatHanh, setIsKySoVaPhatHanh] = useState(false);
  const [isMttPhatHanh, setIsMttPhatHanh] = useState(false);
  const { signalRConnectionServer } = useCommonContext();
  const [isShowPhatHanhResultModal, setIsShowPhatHanhResultModal] =
    useState(false);
  const [hoaDongPhatHanhPushNotifyModel, setHoaDongPhatHanhPushNotifyModel] =
    useState<IHoaDonPhatHanhPushNotifyModel>();
  const CKM = useMemo(() => {
    if (formData && formData.hoa_don_dang_ky_phat_hanh_ky_hieu) {
      // 
      if (formData.hoa_don_dang_ky_phat_hanh_ky_hieu.substring(3, 4) === "M")
        return "M";
      if (formData.hoa_don_dang_ky_phat_hanh_ky_hieu.substring(0, 1) === "C")
        return "C";
      if (formData.hoa_don_dang_ky_phat_hanh_ky_hieu.substring(0, 1) === "K")
        return "K";
    }
    return "";
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formData.hoa_don_dang_ky_phat_hanh_ky_hieu]);

  const loaiHoaDonPhatHanhRef = useRef<HTMLDivElement>(null);
  const kyHieuPhatHanhRef = useRef<HTMLDivElement>(null);

  const mauSoPhatHanhRef = useRef<HTMLDivElement>(null);
  const hinhThucDieuChinhRef = useRef<HTMLDivElement>(null);

  // console.log({
  //     CKM
  // });

  const isHoaDonBanHang = useMemo(() => {
    return loaiHoaDonCT?.loai_hoa_don_id === 9;
  }, [loaiHoaDonCT]);
  const isPhieuXuatKhoVanChuyen = useMemo(() => {
    return loaiHoaDonCT?.id === 9;
  }, [loaiHoaDonCT]);
  const isPhieuXuatKhoDaiLy = useMemo(() => {
    return loaiHoaDonCT?.id === 10;
  }, [loaiHoaDonCT]);
  const isHoaDonGTGT = useMemo(() => {
    return loaiHoaDonCT?.id === 1;
  }, [loaiHoaDonCT]);

  const isHoaDonBanTaiSanCong = useMemo(() => {
    return loaiHoaDonCT?.id === 3;
  }, [loaiHoaDonCT]);

  useEffect(() => {
    if (signalRConnectionServer) {
      if (hoaDonId > 0) {
        signalRConnectionServer.on("THONG_DIEP_HAS_RESULT", (message: any) => {
          console.log({
            THONG_DIEP_HAS_RESULT: message,
          });
          onHoaDonPhatHanhHasResult(message);
        });
      }
    }
  }, [signalRConnectionServer, hoaDonId]);

  const onHoaDonPhatHanhHasResult = (
    message: IHoaDonPhatHanhPushNotifyModel,
  ) => {
    if (message.id === hoaDonId && hoaDonId > 0) {
      setIsShowPhatHanhResultModal(true);
      setHoaDongPhatHanhPushNotifyModel(message);
    }
  };

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const hinh_thuc_id: any = searchParams.get("hinh_thuc_id");
    const hoa_don_goc_id: any = searchParams.get("hoa_don_goc_id");
    const copy_id: any = searchParams.get("copy_id");
    setHinhThucHoaDonId(hinh_thuc_id ? parseInt(hinh_thuc_id) : 1);

    setHoaDonGocId(hoa_don_goc_id ? parseInt(hoa_don_goc_id) : 0);

    if (copy_id && copy_id > 0) {
      copyHoaDonAsync(copy_id);
    }
  }, [location.search]);

  useEffect(() => {
    if (!skipKySoResetRef.current) {
      setIsSavedForKySo(false);
    }
    skipKySoResetRef.current = false;
    if (hoaDonId > 0) {
      handleGetHoaDonViewModel(hoaDonId);
    }
  }, [hoaDonId]);
  useEffect(() => {
    if (hoaDonId === 0 && hoaDonGocId > 0) {
      handleGetHoaDonViewModel(hoaDonGocId);
    }
  }, [hoaDonId, hoaDonGocId]);

  useEffect(() => {
    if (hoaDonViewModel) {
      // console.log({
      //     hoaDonViewModel
      // });

      setFormData({
        ...hoaDonViewModel,
        IsHdPhiThueQuan: hoaDonViewModel.thong_tin_bo_sungs?.is_hd_phi_thue_quan
          ? 1
          : 0,
      });

      if (hoaDonId > 0) {
        setHinhThucHoaDonId(hoaDonViewModel.hoa_don_hinh_thuc_id);
        if (
          hoaDonViewModel.hoa_don_hinh_thuc_id === 3 &&
          hoaDonViewModel.hoa_don_id_goc > 0
        ) {
          hoaDonApi.getViewModel(hoaDonViewModel.hoa_don_id_goc).then((gocRes) => {
            if (gocRes.is_success) {
              setHangHoasGoc(gocRes.data.hang_hoas ?? []);
            }
          });
        }
        setThongTinHoaDonGoc({
          hoa_don_dang_ky_phat_hanh_ky_hieu_goc:
            hoaDonViewModel.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
          hoa_don_dang_ky_phat_hanh_mau_so_goc:
            hoaDonViewModel.hoa_don_dang_ky_phat_hanh_mau_so_goc,
          hoaDonGocId: hoaDonViewModel.hoa_don_id_goc,
          ma_so_hoa_don_goc:
            hoaDonViewModel.ma_so_hoa_don_goc?.toString() ?? "",
          ngay_hoa_don_goc: hoaDonViewModel.ngay_hoa_don_goc ?? "",
          hoa_don_nghi_dinh_id_goc: hoaDonViewModel.hoa_don_nghi_dinh_id,
        });
      }
      setHangHoas(hoaDonViewModel.hang_hoas);
      setLoaiPhis(hoaDonViewModel.loai_phis);
      setLoaiTien(hoaDonViewModel.loai_tien);
      reset({
        ...hoaDonViewModel,
        nguoi_mua_mst: hoaDonViewModel.nguoi_mua_mst?.toString() ?? "",
        nguoi_mua_cccd: hoaDonViewModel.nguoi_mua_cccd?.toString() ?? "",
        NgayDenNgayDi: hoaDonViewModel?.thong_tin_khac?.NgayDenNgayDi,
        TenTau: hoaDonViewModel?.thong_tin_khac?.TenTau,
        SoThamChieu: hoaDonViewModel?.thong_tin_khac?.SoThamChieu,
        NoiDiNoiDen: hoaDonViewModel?.thong_tin_khac?.NoiDiNoiDen,
        // ngay_lap: moment(toKhaiViewModel.ngay_lap).format("YYYY-MM-DD"),
        // ngay_co_hieu_luc: moment(toKhaiViewModel.ngay_co_hieu_luc).format("YYYY-MM-DD"),
        SoQuyetDinh: hoaDonViewModel.thong_tin_bo_sungs?.so_quyet_dinh,
        NgayQuyetDinh: toIsoDateOrEmpty(
          hoaDonViewModel.thong_tin_bo_sungs?.ngay_quyet_dinh,
        ),
        CoQuanBanHanhQD:
          hoaDonViewModel.thong_tin_bo_sungs?.co_quan_ban_hanh_qd,
        HinhThucBan: hoaDonViewModel.thong_tin_bo_sungs?.hinh_thuc_ban,
        DiaDiemVCHangDen:
          hoaDonViewModel.thong_tin_bo_sungs?.dia_diem_vc_hang_den,

        TgianVCHangDenTu: toIsoDateOrEmpty(
          hoaDonViewModel.thong_tin_bo_sungs?.tgian_vc_hang_den_tu,
        ),
        TgianVCHangDenDen: toIsoDateOrEmpty(
          hoaDonViewModel.thong_tin_bo_sungs?.tgian_vc_hang_den_den,
        ),
      });
    }
  }, [hoaDonViewModel, hoaDonId]);

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
  } = useForm<IHoaDonVM>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...hoaDonViewModel,
      ty_gia: hoaDonViewModel?.ty_gia ?? 0,
      hinh_thuc_tt: hoaDonViewModel?.hinh_thuc_tt ?? "Tiền mặt/ Chuyển khoản",
    },
  });

  const nguoiMuaMstValue = watch("nguoi_mua_mst");

  const copyHoaDonAsync = async (id: number) => {
    const res = await hoaDonApi.getViewModel(id);
    if (res.is_success) {
      setIsLoadHoaDonDone(true);
      if (hoaDonId === 0) {
        setHoaDonViewModel({
          ...res.data,
          id: 0,
          ngay_hoa_don: moment(new Date()).format("YYYY-MM-DD"),
          is_ky_so_succes: false,
          hoa_don_trang_thai_id: 0,
          phat_hanh_ma_ketqua_cqt: "",
          ket_qua_phat_hanh: "",
          phat_hanh_uuid: "",
          user_id_phathanh: 0,
          invoice_id: undefined,
          nguoi_mua_mst: res.data.nguoi_mua_mst?.toString() ?? "",
          nguoi_mua_cccd: res.data.nguoi_mua_cccd?.toString() ?? "",
          hang_hoas: res.data.hang_hoas.map((x: any) => {
            return {
              ...x,
              id: 0,
            };
          }),
          loai_phis: res.data.loai_phis.map((x: any) => ({
            ...x,
            id: 0,
          })),
        });
        setGiam_thue_ty_le(res.data.giam_thue_thanh_tien ?? -1);

        setIsOpenHoaDonGocModal(false);
      }
    } else {
      setIsLoadHoaDonDone(true);
      NotifyHelper.Error(res?.message ?? "Không thể tải dữ liệu hóa đơn gốc");
    }
  };

  const handleGetHoaDonViewModel = async (
    id: number,
    forceEditMode = false,
  ) => {
    const requestId = ++viewModelRequestRef.current;
    const res = await hoaDonApi.getViewModel(id);
    if (requestId !== viewModelRequestRef.current) {
      return;
    }
    if (res.is_success) {
      setIsLoadHoaDonDone(true);

      const loadAsGocTemplate =
        !forceEditMode &&
        hoaDonId === 0 &&
        hoaDonGocId > 0 &&
        id === hoaDonGocId;

      if (loadAsGocTemplate) {
        const isCreatingDieuChinh =
          hoaDonGocId > 0 && hinhThucHoaDonId === 3;
        const gocHangHoas = res.data.hang_hoas.map((x: any) => ({
          ...x,
          id: 0,
        }));
        if (isCreatingDieuChinh) {
          setHangHoasGoc(gocHangHoas);
        }
        setHoaDonViewModel({
          ...res.data,
          id: 0,
          is_ky_so_succes: false,
          hoa_don_trang_thai_id: 0,
          phat_hanh_ma_ketqua_cqt: "",
          ket_qua_phat_hanh: "",
          phat_hanh_uuid: "",
          user_id_phathanh: 0,
          invoice_id: undefined,
          xuat_kho_dia_chi: res?.data?.xuat_kho_dia_chi?.split("|")[1] ?? "",
          ngay_hoa_don: moment(new Date()).format("YYYY-MM-DD"),
          hang_hoas: isCreatingDieuChinh
            ? [
                {
                  hang_hoa_tinh_chat_id: 1,
                  ma_hang: "",
                  ten_hang: "",
                  dvt: "",
                  so_luong: 0,
                  don_gia: 0,
                  thanh_tien: 0,
                  ty_le_chiet_khau: 0,
                  thue_vat: "10%",
                },
              ]
            : gocHangHoas,
          loai_phis: res.data.loai_phis.map((x: any) => ({
            ...x,
            id: 0,
          })),
        });
        setThongTinHoaDonGoc({
          hoa_don_dang_ky_phat_hanh_ky_hieu_goc:
            res.data.hoa_don_dang_ky_phat_hanh_ky_hieu,
          hoa_don_dang_ky_phat_hanh_mau_so_goc:
            res.data.hoa_don_dang_ky_phat_hanh_mau_so,
          hoaDonGocId: res.data.id,
          ma_so_hoa_don_goc: res.data.ma_so_hoa_don?.toString() ?? "",
          ngay_hoa_don_goc: res.data.ngay_hoa_don
            ? moment(res.data.ngay_hoa_don).format("YYYY-MM-DD")
            : "",
          hoa_don_nghi_dinh_id_goc: res.data.hoa_don_nghi_dinh_id ?? 0,
        });
        setGiam_thue_ty_le(res.data.giam_thue_ty_le ?? -1);
        setIsOpenHoaDonGocModal(false);
      } else {
        const effectiveId = forceEditMode
          ? id
          : hoaDonId > 0
            ? hoaDonId
            : id;
        setHoaDonViewModel({
          ...res.data,
          id: effectiveId,
          hang_hoas: res.data.hang_hoas.map((x: any) => {
            return {
              ...x,
              id: 0,
            };
          }),
          loai_phis: res.data.loai_phis.map((x: any) => ({
            ...x,
            id: 0,
          })),
          xuat_kho_dia_chi: res?.data?.xuat_kho_dia_chi?.split("|")[1] ?? "",
        });
        setGiam_thue_ty_le(res.data.giam_thue_ty_le ?? -1);
      }
    } else {
      setIsLoadHoaDonDone(true);
      NotifyHelper.Error(res?.message ?? "Không thể tải dữ liệu hóa đơn gốc");
    }
  };
  const openKySoModalFromResponse = (res: any) => {
    const data = res.data;
    if (typeof data === "string") {
      setBase64KySo(data);
      setIsShowKySoModal(true);
    } else if (typeof data === "object" && data !== null) {
      const { hoa_don_base64, bien_ban_base64 } = data;
      setBase64KySo(hoa_don_base64);
      setBase64BienBan(bien_ban_base64);
      setIsShowKySoModal(true);
    } else {
      NotifyHelper.Error("Dữ liệu không hợp lệ.");
    }
  };

  const handleGetBase64KySoNguoiBan = async () => {
    setIsSaving(true);
    const res = await hoaDonApi.createBase64KySoNguoiBan(hoaDonId);
    setIsSaving(false);
    if (res.is_success) {
      openKySoModalFromResponse(res);
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };

  const handleGetBase64KySoMttDongThoi = async () => {
    const isKhacNgay =
      moment(new Date()).format("YYYY-MM-DD") !==
      moment(formData.ngay_hoa_don ?? new Date()).format("YYYY-MM-DD");
    if (isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
              </p>
            </div>
          ),
          title: "Lưu ý",
          cancelButtonContent: "Không ký",
          confirmButtonContent: "Tiếp tục ký số",
          confirmButtonType: "danger",
        }))
      ) {
        return;
      }
    }

    setIsSaving(true);
    const res = await hoaDonApi.createBase64KySoMttDongThoi(hoaDonId);
    setIsSaving(false);
    if (res.is_success) {
      openKySoModalFromResponse(res);
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };

  const handleGetBase64KySo = async () => {
    const isKhacNgay =
      moment(new Date()).format("YYYY-MM-DD") !==
      moment(formData.ngay_hoa_don ?? new Date()).format("YYYY-MM-DD");
    if (isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
              </p>
            </div>
          ),
          title: "Lưu ý",
          cancelButtonContent: "Không ký",
          confirmButtonContent: "Tiếp tục ký số",
          confirmButtonType: "danger",
        }))
      ) {
        return;
      }
    }

    setIsSaving(true);
    const res = await hoaDonApi.createBase64KySo(hoaDonId);

    setIsSaving(false);
    if (res.is_success) {
      openKySoModalFromResponse(res);
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };
  
  const handleKySoRemoteAsync = async () => {
    const isKhacNgay =
      moment(new Date()).format("YYYY-MM-DD") !==
      moment(formData.ngay_hoa_don ?? new Date()).format("YYYY-MM-DD");
    if (isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
              </p>
            </div>
          ),
          title: "Lưu ý",
          cancelButtonContent: "Không ký",
          confirmButtonContent: "Tiếp tục ký số",
          confirmButtonType: "danger",
        }))
      ) {
        return;
      }
    }
    setIsSaving(true);
    const res = await hoaDonKyLoApi.kySo(hoaDonId);
    setIsSaving(false);
    if (res.is_success) {
      //reload
      handleGetHoaDonViewModel(hoaDonId);
      NotifyHelper.Success("Ký số thành công");
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };

  const handleMttPhatHanhRemoteAsync = async () => {
    const isKhacNgay =
      moment(new Date()).format("YYYY-MM-DD") !==
      moment(formData.ngay_hoa_don ?? new Date()).format("YYYY-MM-DD");
    if (isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
              </p>
            </div>
          ),
          title: "Lưu ý",
          cancelButtonContent: "Không ký",
          confirmButtonContent: "Tiếp tục ký số",
          confirmButtonType: "danger",
        }))
      ) {
        return;
      }
    }
    setIsSaving(true);
    const res = await hoaDonKyLoApi.phatHanhMtt(hoaDonId);
    setIsSaving(false);
    if (res.is_success) {
      handleGetHoaDonViewModel(hoaDonId);
      NotifyHelper.Success("Phát hành hóa đơn thành công");
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };

  const handleKySoVaPhatHanhRemoteAsync = async () => {
    const isKhacNgay =
      moment(new Date()).format("YYYY-MM-DD") !==
      moment(formData.ngay_hoa_don ?? new Date()).format("YYYY-MM-DD");
    if (isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
              </p>
            </div>
          ),
          title: "Lưu ý",
          cancelButtonContent: "Không ký",
          confirmButtonContent: "Tiếp tục ký số",
          confirmButtonType: "danger",
        }))
      ) {
        return;
      }
    }
    setIsSaving(true);
    const res = await hoaDonKyLoApi.kySoVaPhatHanh(hoaDonId);
    setIsSaving(false);
    if (res.is_success) {
      //reload
      handleGetHoaDonViewModel(hoaDonId);
      NotifyHelper.Success("Ký số và gửi phát hành thành công");
    } else {
      NotifyHelper.Error(res?.message ?? "Có lỗi không xác định");
    }
  };

  const handleUpdateKySoSuccss = async (
    signedtext: string,
    bienBanSignedText?: string,
  ) => {
    if (hoaDonViewModel) {
      setIsSaving(true);
      const res = await hoaDonApi.updateKySoSuccess({
        signed_text: signedtext,
        bienBanSignedText: bienBanSignedText,
        id: hoaDonId,
      });
      if (res.is_success) {
        if (!(isKySoVaPhatHanh && CKM === "M")) {
          NotifyHelper.Success(
            CKM === "M" ? "Đã ký số người bán" : "Đã ký số",
          );
        }
        setHoaDonViewModel({
          ...hoaDonViewModel,
          is_ky_so_succes: true,
        });
        handleGetHoaDonViewModel(hoaDonId);

        if (isKySoVaPhatHanh) {
          handlePhatHanhAsync(signedtext, bienBanSignedText, true);
        }
      } else {
        NotifyHelper.Error(res.message ?? "Có lỗi");
      }
      setIsSaving(false);
    }
  };

  const handlePhatHanhAsync = async (
    signedtext: string,
    bienBanSignedText?: string,
    isKySoVaPhatHanhFlow?: boolean,
  ) => {
    setIsSaving(true);
    const res = await hoaDonApi.phatHanh({
      signed_text: signedtext,
      bienBanSignedText: bienBanSignedText,
      id: hoaDonId,
    });
    if (res.is_success) {
      handleGetHoaDonViewModel(hoaDonId);
      if (isKySoVaPhatHanhFlow) {
        NotifyHelper.Success("Ký số và phát hành thành công");
      }
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
    setIsSaving(false);
  };

  const handleSetHangHoaDonGiaAm = async (
    hoa_don_ly_do_dieu_chinh_id: number,
  ) => {
    if (hoa_don_ly_do_dieu_chinh_id === 2) {
      //điều chỉnh giảm
      //set hết đơn giá về âm
      setHangHoas(
        hangHoas.map((x, i) => {
          const don_gia = x.don_gia > 0 ? -1 * x.don_gia : x.don_gia;
          return {
            ...x,
            don_gia: don_gia,
            thanh_tien: getThanhTien(x.so_luong, don_gia, x.ty_le_chiet_khau),
          };
        }),
      );
    } else {
      //set hết đơn giá về dương
      setHangHoas(
        hangHoas.map((x, i) => {
          const don_gia = x.don_gia < 0 ? -1 * x.don_gia : x.don_gia;
          return {
            ...x,
            don_gia: don_gia,
            thanh_tien: getThanhTien(x.so_luong, don_gia, x.ty_le_chiet_khau),
          };
        }),
      );
    }
  };

  const getAddOrEditFormModel = (data: any): IIHoaDonAddOrEditModel => {
    if (hoaDonId === 0) {
      delete data.id;
    }

    const newHangHoas = hangHoas.map((item) => ({
      ...item,
      ma_hang: item.ma_hang ?? "",
      don_gia: item.don_gia ?? "0",
      so_luong: item.so_luong === "" ? 0 : (item.so_luong ?? 0),
      thanh_tien: item.thanh_tien === "" ? 0 : (item.thanh_tien ?? 0),
    }));

    const _hangHoas = isDieuChinhThue
      ? newHangHoas.map((h) => ({
          ...h,
          so_luong: 0,
          don_gia: 0,
          thanh_tien: 0,
        }))
      : newHangHoas;
    const tongTienData = getTongTienData(
      _hangHoas,
      loaiTien || "VND",
      giam_thue_ty_le,
      loaiPhis,
    );

    ///map lai hàng hóa, nếu stt là "" thì bỏ luôn trường stt
    const mappedHangHoas = _hangHoas.map((item) => {
      const newItem: any = { ...item };
      if (newItem.stt === "") {
        delete newItem.stt;
      }
      return newItem;
    });

    if (
      formData.loai_hoa_don_ct_id === 9 ||
      formData.loai_hoa_don_ct_id === 10
    ) {
      data.xuat_kho_dia_chi = `${user?.donvi?.dia_chi}|${
        data?.xuat_kho_dia_chi?.trim() ?? ""
      }`;
    } else {
      data.xuat_kho_dia_chi = "";
    }

    if (formData.loai_hoa_don_ct_id === 1) {
      data.thong_tin_khac_json = JSON.stringify({
        LoaiHD: "HDDVVTai",
        NgayDenNgayDi: data?.NgayDenNgayDi,
        TenTau: data?.TenTau,
        SoThamChieu: data?.SoThamChieu,
        NoiDiNoiDen: data?.NoiDiNoiDen,
      });

      delete data?.NgayDenNgayDi;
      delete data?.TenTau;
      delete data?.SoThamChieu;
      delete data?.NoiDiNoiDen;
      delete data?.thong_tin_khac;
    }

    const tongTienChuKhongDong =
      tongTienData?.tong_thanh_tien > 0 && tongTienChu === "Không đồng"
        ? ConvertTienChu(data.tong_tien_thue, loaiTien)
        : tongTienChu;

    const so_tien_tang_giam = data.so_tien_tang_giam ?? 0;
    const so_tien_tang_giam_tien_hang = data.so_tien_tang_giam_tien_hang ?? 0;
    const so_tien_tang_giam_tien_thue = data.so_tien_tang_giam_tien_thue ?? 0;

    return {
      ...data,
      ...thongTinHoaDonGoc,
      hoa_don_id_goc: thongTinHoaDonGoc ? thongTinHoaDonGoc.hoaDonGocId : 0,
      ten_hoa_don: loaiHoaDonCT?.name,
      donvi_ma_dv: user?.donvi_ma_dv,
      loai_tien: loaiTien,
      loai_hoa_don_ct_id: formData?.loai_hoa_don_ct_id,
      hoa_don_dang_ky_phat_hanh_ky_hieu:
        formData?.hoa_don_dang_ky_phat_hanh_ky_hieu,
      hoa_don_dang_ky_phat_hanh_mau_so:
        formData?.hoa_don_dang_ky_phat_hanh_mau_so,
      hoa_don_hinh_thuc_id: hinhThucHoaDonId,

      hoa_don_ly_do_dieu_chinh_id: formData.hoa_don_ly_do_dieu_chinh_id,
      nguoi_mua_email: data?.nguoi_mua_email?.trim() ?? "",
      nguoi_mua_mst: data?.nguoi_mua_mst?.toString().trim() ?? "",
      nguoi_mua_cccd: data?.nguoi_mua_cccd?.toString().trim() ?? "",
      nguoi_mua_ten: data?.nguoi_mua_ten?.trim() ?? "",

      // tong_tien_chu: isDieuChinhThue
      //   ? ConvertTienChu(data.tong_tien_thue, loaiTien)
      //   : tongTienChuKhongDong,

      tong_tien_chu: isDieuChinhThue
        ? ConvertTienChu(data.tong_tien_thue, loaiTien)
        : ConvertTienChu(
            tongTienData.tong_thanh_tien +
              so_tien_tang_giam +
              so_tien_tang_giam_tien_hang +
              so_tien_tang_giam_tien_thue,
            loaiTien,
          ),
      tong_tien_truong_thue: isDieuChinhThue
        ? 0
        : (tongTienData?.tong_thanh_tien ?? 0) -
          (tongTienData?.vats_total ?? 0),
      tong_tien_thue: isDieuChinhThue
        ? data.tong_tien_thue
        : (tongTienData?.vats_total ?? 0),
      tong_tien_phi: loaiPhis.map((x) => x.so_tien).reduce((a, b) => a + b, 0),
      tong_tien_thanh_toan: isDieuChinhThue
        ? data.tong_tien_thue
        : (tongTienData?.tong_thanh_tien ?? 0),
      giam_thue_ty_le: giam_thue_ty_le > 0 ? giam_thue_ty_le : 0,
      giam_thue_thanh_tien: tongTienData?.tienGiamThueTheoNghiDinh ?? 0,
      ngay_hoa_don_goc: thongTinHoaDonGoc?.ngay_hoa_don_goc
        ? thongTinHoaDonGoc?.ngay_hoa_don_goc
        : undefined,
      hoang_hoas: mappedHangHoas,
      loai_phis: loaiPhis.map((x, idx) => ({
        ...x,
        stt: idx + 1,
      })),

      // Hóa đơn bổ sung thông tin
      IsHdPhiThueQuan: formData.IsHdPhiThueQuan ?? 0,
      IsHdBanTaiSanCong: isHoaDonBanTaiSanCong ? 1 : 0,
      SoQuyetDinh: data?.SoQuyetDinh?.trim() ?? "",
      NgayQuyetDinh: data?.NgayQuyetDinh
        ? moment(data?.NgayQuyetDinh).format("YYYY-MM-DD")
        : "",
      CoQuanBanHanhQD: data?.CoQuanBanHanhQD?.trim() ?? "",
      HinhThucBan: data?.HinhThucBan?.trim() ?? "",
      DiaDiemVCHangDen: data?.DiaDiemVCHangDen?.trim() ?? "",
      TgianVCHangDenTu: data?.TgianVCHangDenTu
        ? moment(data?.TgianVCHangDenTu).format("YYYY-MM-DD")
        : "",
      TgianVCHangDenDen: data?.TgianVCHangDenDen
        ? moment(data?.TgianVCHangDenDen).format("YYYY-MM-DD")
        : "",
    };
  };

  const onSubmit = async (data: any) => {
    let isValid: boolean = true;
    if ((formData?.loai_hoa_don_ct_id ?? 0) <= 0) {
      isValid = false;
      setError("loai_hoa_don_ct_id", {});
      if (loaiHoaDonPhatHanhRef.current) {
        loaiHoaDonPhatHanhRef.current.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }
    }
    if ((formData?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? "") === "") {
      isValid = false;
      setError("hoa_don_dang_ky_phat_hanh_ky_hieu", {});

      if (loaiHoaDonPhatHanhRef.current) {
        loaiHoaDonPhatHanhRef.current.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }
    }
    if ((formData?.hoa_don_dang_ky_phat_hanh_mau_so ?? "") === "") {
      isValid = false;
      setError("hoa_don_dang_ky_phat_hanh_mau_so", {});
      if (mauSoPhatHanhRef.current) {
        mauSoPhatHanhRef.current.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }
    }

    if (
      (formData?.hoa_don_ly_do_dieu_chinh_id ?? 0) === 0 &&
      hinhThucHoaDonId === 3
    ) {
      isValid = false;
      setError("hoa_don_ly_do_dieu_chinh_id", {});
      if (hinhThucDieuChinhRef.current) {
        hinhThucDieuChinhRef.current.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }
    }

    if (hangHoas.find((x) => !IsHoaDonHangHoaValid(x))) {
      isValid = false;
      setError("hang_hoas", {});
    } else {
      clearErrors("hang_hoas");
    }
    // console.log({
    //     loaiPhis
    // });

    if (loaiPhis.find((x) => !IsHoaDonLoaiPhiValid(x))) {
      // isValid = false;
      // setError("loai_phis", {})
    } else {
      clearErrors("loai_phis");
    }

    const payload = getAddOrEditFormModel(data);

    const nguoiMuaValidation = validateAndNormalizeNguoiMuaBuyer({
      nguoi_mua_mst: payload.nguoi_mua_mst,
      nguoi_mua_cccd: data.nguoi_mua_cccd,
      nguoi_mua_ten: payload.nguoi_mua_ten,
      nguoi_mua_ten_donvi: payload.nguoi_mua_ten_donvi,
      nguoi_mua_dia_chi: payload.nguoi_mua_dia_chi,
    });

    if (!nguoiMuaValidation.isValid) {
      NotifyHelper.Error(nguoiMuaValidation.message ?? "Thông tin người mua không hợp lệ");
      if (nguoiMuaValidation.field) {
        setError(nguoiMuaValidation.field as any, {
          type: "manual",
          message: nguoiMuaValidation.message,
        });
        setFocus(nguoiMuaValidation.field as any);
      }
      isValid = false;
    } else if (nguoiMuaValidation.normalized) {
      payload.nguoi_mua_mst = nguoiMuaValidation.normalized.nguoi_mua_mst ?? "";
      clearErrors("nguoi_mua_mst");
      clearErrors("nguoi_mua_ten_donvi");
      clearErrors("nguoi_mua_dia_chi");
    }

    if (payload.nguoi_mua_dia_chi && payload.nguoi_mua_dia_chi.length > 400) {
      NotifyHelper.Error(
        "Địa chỉ người mua hàng không được vượt quá 400 ký tự",
      );
      setError("nguoi_mua_dia_chi", {
        type: "manual",
        message: "Địa chỉ người mua hàng không được vượt quá 400 ký tự",
      });
      isValid = false;
    }

    if (payload.nguoi_mua_email) {
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

    if (payload?.nguoi_mua_ten) {
      //check độ dài không được lớn hơn 100
      if (payload.nguoi_mua_ten?.length > 100) {
        NotifyHelper.Error("Tên người mua hàng không được vượt quá 100 ký tự");
        setError("nguoi_mua_ten", {
          type: "manual",
          message: "Tên người mua hàng không được vượt quá 100 ký tự",
        });
        isValid = false;
      }
    }

    if (payload?.ma_dv_ngan_sach) {
      // nếu quá 7 ký tự thì báo lỗi
      if (payload.ma_dv_ngan_sach?.length !== 7) {
        NotifyHelper.Error("Mã đơn vị ngân sách phải đúng 7 ký tự");
        setError("ma_dv_ngan_sach", {
          type: "manual",
          message: "Mã đơn vị ngân sách phải đúng 7 ký tự",
        });
        isValid = false;
      }
    }

    if (data?.nguoi_mua_cccd && !isNguoiMuaCccd(data.nguoi_mua_cccd)) {
      NotifyHelper.Error(
        "Căn cước công dân người mua hàng phải đúng 12 chữ số",
      );
      setError("nguoi_mua_cccd" as any, {
        type: "manual",
        message: "Căn cước công dân người mua hàng phải đúng 12 chữ số",
      });
      setFocus("nguoi_mua_cccd" as any);
      isValid = false;
    }

    if (payload?.nguoi_mua_stk) {
      //check độ dài không được lớn hơn 30
      if (payload.nguoi_mua_stk?.length > 30) {
        NotifyHelper.Error(
          "Số tài khoản người mua hàng không được vượt quá 30 ký tự",
        );
        setError("nguoi_mua_stk", {});
        isValid = false;
      }
    }

    if (payload?.nguoi_mua_ngan_hang) {
      //check độ dài không được lớn hơn 400
      if (payload.nguoi_mua_ngan_hang?.length > 400) {
        NotifyHelper.Error(
          "Ngân hàng người mua hàng không được vượt quá 400 ký tự",
        );
        setError("nguoi_mua_ngan_hang", {});
        isValid = false;
      }
    }

    //check ma_hang không lớn hơn 50 ký tự
    // tên hàng hóa không được lớn hơn 500 ký tự
    // đơn vị tính không được lớn hơn 50 ký tự
    payload?.hoang_hoas?.forEach((hh, index) => {
      if (hh?.ma_hang && hh.ma_hang.length > 50) {
        NotifyHelper.Error(
          `Mã hàng hóa dịch vụ dòng ${index + 1} không được vượt quá 50 ký tự`,
        );
        setError("hang_hoas", {});
        isValid = false;
      }
      if (hh?.ten_hang && hh.ten_hang.length > 500) {
        NotifyHelper.Error(
          `Tên hàng hóa dịch vụ dòng ${index + 1} không được vượt quá 500 ký tự`,
        );
        setError("hang_hoas", {});
        isValid = false;
      }
      if (hh?.dvt && hh.dvt.length > 50) {
        NotifyHelper.Error(
          `Đơn vị tính dòng ${index + 1} không được vượt quá 50 ký tự`,
        );
        setError("hang_hoas", {});
        isValid = false;
      }
    });

    if (!isValid) return;

    payload.ngay_hoa_don = moment(formData.ngay_hoa_don ?? new Date()).format(
      "YYYY-MM-DD",
    );

    setIsSaving(true);

    const res = await hoaDonApi.save(payload);

    setIsSaving(false);
    if (res.is_success) {
      const newId = res.data?.id ?? hoaDonId;
      if (newId > 0) {
        skipKySoResetRef.current = true;
        const formPath = resolveFormPath(newId);
        history.replace(formPath);
        await handleGetHoaDonViewModel(newId, true);
      }
      setIsSavedForKySo(true);
      NotifyHelper.Success("Success");
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi không xác định");
    }
  };
  // console.log({
  //     hoaDonViewModel,
  //     hoaDonId,
  //     pId,
  //     thongTinHoaDonGoc,
  //     hinhThucHoaDonId
  // });

  if (
    hoaDonViewModel &&
    hoaDonId > 0 &&
    hoaDonId === hoaDonViewModel.id &&
    hoaDonViewModel.hoa_don_trang_thai_id === eHoaDonTrangThai.DA_PHAT_HANH
  ) {
    return (
      <HoaDonView id={hoaDonViewModel.id} hinhThucHoaDonId={hinhThucHoaDonId} />
    );
  }
  if (user && user.donvi && (user.donvi.to_khai_success_id ?? 0) === 0) {
    return (
      <Flash variant="danger">
        <b> Vui lòng tạo tờ khai trước!</b>
        <br />
        Chỉ được tạo hóa đơn sau khi đã có tờ khai được Cơ quan thuế chập nhận
      </Flash>
    );
  }

  return (
    <Box>
      <BackButton />
      {!isLoadHoaDonDone && <PlaceHolder line_number={10} />}
      {isLoadHoaDonDone && (
        <form onSubmit={handleSubmit(onSubmit)}>
          {hinhThucHoaDonId !== 1 && (
            <Box
              sx={
                {
                  // display: "flex",
                  // borderBottomWidth: 1,
                  // borderBottomStyle: "solid",
                  // borderBottomColor: "border.default",
                  // pb: 3,
                  // mb: 3
                }
              }
            >
              {thongTinHoaDonGoc &&
                thongTinHoaDonGoc.hoa_don_dang_ky_phat_hanh_ky_hieu_goc && (
                  <>
                    {hinhThucHoaDonId === 3 && (
                      <Box
                        style={{
                          display: "flex",
                          alignItems: "center",
                          gap: 8,
                        }}
                      >
                        <Heading
                          size={eSize.medium}
                          text={"LẬP HÓA ĐƠN ĐIỀU CHỈNH"}
                        />
                        <IconButton icon={QuestionIcon} aria-label="Help" />
                      </Box>
                    )}
                    {hinhThucHoaDonId === 2 && (
                      <Heading
                        size={eSize.medium}
                        text={"LẬP HÓA ĐƠN THAY THẾ"}
                      />
                    )}
                    <PaperFormGroup label="Thông tin hóa đơn gốc">
                      <Box
                        sx={{
                          display: "flex",
                        }}
                      >
                        <Box sx={{ fontSize: "15px" }}>
                          Ký hiệu mẫu số:{" "}
                          <b>
                            {
                              thongTinHoaDonGoc.hoa_don_dang_ky_phat_hanh_mau_so_goc
                            }
                          </b>
                          , Ký hiệu hóa đơn:{" "}
                          <b>
                            {
                              thongTinHoaDonGoc.hoa_don_dang_ky_phat_hanh_ky_hieu_goc
                            }
                          </b>
                          , Số hóa đơn:{" "}
                          <b>{thongTinHoaDonGoc.ma_so_hoa_don_goc}</b>, Ngày hóa
                          đơn:{" "}
                          <b>
                            {moment(thongTinHoaDonGoc.ngay_hoa_don_goc).format(
                              "DD/MM/YYYY",
                            )}
                          </b>
                          ;
                        </Box>
                        <Box sx={{ ml: 2, mt: "-2px" }}>
                          <Button
                            variant="invisible"
                            leadingVisual={PencilIcon}
                            onClick={() => {
                              setIsOpenHoaDonGocModal(true);
                            }}
                          />
                        </Box>
                      </Box>
                    </PaperFormGroup>

                    {hinhThucHoaDonId === 2 && (
                      <PaperFormGroup label="Lý do thay thế">
                        <Box
                          ref={hinhThucDieuChinhRef}
                          sx={{
                            display: "flex",
                            gap: 2,
                            alignItems: "center",
                          }}
                        >
                          <Box sx={{ flex: 1 }}>
                            <FormControl>
                              <TextInput
                                name="ly_do_dieu_chinh"
                                block
                                register={register}
                                required
                                errors={errors}
                                validateMessage="Vui lòng điền lý do thay thế"
                              />
                            </FormControl>
                          </Box>
                        </Box>
                      </PaperFormGroup>
                    )}
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
              mt: thongTinHoaDonGoc ? 0 : -3,
            }}
          >
            <PaperFormGroup
              label="Hóa đơn"
              isHideBorder={!thongTinHoaDonGoc}
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
                  ref={loaiHoaDonPhatHanhRef}
                >
                  <FormControl sx={{ mr: 3, mb: 2 }}>
                    <FormControl.Label>Loại hóa đơn</FormControl.Label>
                    <SelectBoxLoaiHoaDonCTPhatHanh
                      value={formData?.loai_hoa_don_ct_id ?? 0}
                      onValueChanged={(id) => {
                        clearErrors("loai_hoa_don_ct_id");

                        setFormData({
                          ...formData,
                          loai_hoa_don_ct_id: id,
                          hoa_don_dang_ky_phat_hanh_mau_so: "",
                        });
                      }}
                    />
                    {errors && errors["loai_hoa_don_ct_id"] && (
                      <FormControl.Validation variant="error">
                        Vui lòng chọn loại hóa đơn
                      </FormControl.Validation>
                    )}
                  </FormControl>
                  <Box sx={{ mr: 3, mb: 2 }} ref={mauSoPhatHanhRef}>
                    <FormControl>
                      <FormControl.Label>Mẫu số</FormControl.Label>
                      <SelectBoxMauSoPhatHanh
                        isAutoSelectIfHasOneItem
                        loai_hoa_don_ct_id={formData?.loai_hoa_don_ct_id ?? 0}
                        value={formData?.hoa_don_dang_ky_phat_hanh_mau_so ?? ""}
                        onValueChanged={(id) => {
                          clearErrors("hoa_don_dang_ky_phat_hanh_mau_so");
                          setFormData({
                            ...formData,
                            hoa_don_dang_ky_phat_hanh_mau_so: id,
                            hoa_don_dang_ky_phat_hanh_ky_hieu: "",
                          });
                        }}
                      />
                      {errors && errors["hoa_don_dang_ky_phat_hanh_mau_so"] && (
                        <FormControl.Validation variant="error">
                          Vui lòng chọn mẫu số
                        </FormControl.Validation>
                      )}
                    </FormControl>
                  </Box>
                  <Box sx={{ mr: 3, mb: 2 }} ref={kyHieuPhatHanhRef}>
                    <FormControl>
                      <FormControl.Label>Ký hiệu</FormControl.Label>
                      <SelectBoxKyHieuPhatHanh
                        loai_hoa_don_ct_id={formData?.loai_hoa_don_ct_id ?? 0}
                        mau_so={
                          formData?.hoa_don_dang_ky_phat_hanh_mau_so ?? ""
                        }
                        value={
                          formData?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? ""
                        }
                        isAutoSelectIfHasOneItem
                        onValueChanged={(id) => {
                          clearErrors("hoa_don_dang_ky_phat_hanh_ky_hieu");
                          setFormData({
                            ...formData,
                            hoa_don_dang_ky_phat_hanh_ky_hieu: id,
                          });
                        }}
                        isShowKyHieuTheoNam={hinhThucHoaDonId === 1}
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
                {hoaDonId === 0 && (
                  <Box>
                    <FormControl>
                      <FormControl.Label>
                        Sao chép từ hóa đơn cũ
                      </FormControl.Label>
                      <SelectBoxHoaDon
                        placeHolder="Thêm hóa đơn từ danh sách"
                        value={0}
                        leadingVisual={PlusIcon}
                        onValueChanged={(ids, hoa_dons) => {
                          if (hoa_dons) {
                            if (hoa_dons.length > 1) {
                              NotifyHelper.Error("Vui lòng chỉ chọn 1 hóa đơn");
                            } else {
                              copyHoaDonAsync(hoa_dons[0].id);
                            }
                          }
                        }}
                      />
                    </FormControl>
                  </Box>
                )}
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
                text={loaiHoaDonCT?.name.toUpperCase() ?? ""}
                size={window.innerWidth >= 768 ? eSize.large : eSize.medium}
              />
              {/* <Text text={ngayThangNam} sx={{ color: 'fg.muted' }} /> */}

              <FormGroupInline label="Ngày hóa đơn">
                {/* <TextInput type='date' value={formData.ngay_hoa_don ? moment(formData.ngay_hoa_don).format("YYYY-MM-DD") : undefined}
                                    name='ngay_hoa_don'
                                    register={register}
                                    required
                                    onChange={(e) => {
                                        setFormData({
                                            ...formData,
                                            ngay_hoa_don: moment(e.target.value).format("YYYY-MM-DD")
                                        })
                                    }}
                                /> */}
                <DateInput
                  name="ngay_hoa_don"
                  register={register}
                  required
                  value={
                    formData.ngay_hoa_don
                      ? moment(formData.ngay_hoa_don).format("DD/MM/YYYY")
                      : undefined
                  }
                  onValueChanged={(value, date) => {
                    // 
                    // setValue("ngay_hoa_don", moment(date).format("YYYY-MM-DD"))
                    setFormData({
                      ...formData,
                      ngay_hoa_don: moment(date).format("YYYY-MM-DD"),
                    });
                  }}
                />
              </FormGroupInline>

              {isHoaDonBanHang && (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 2,
                    mt: 2,
                  }}
                >
                  <Checkbox
                    checked={formData.IsHdPhiThueQuan === 1}
                    onChange={(e) => {
                      // IsHdPhiThueQuan
                      setFormData({
                        ...formData,
                        IsHdPhiThueQuan: e.target.checked ? 1 : 0,
                      });
                    }}
                  />
                  <Box
                    sx={{
                      ml: 1,
                      fontWeight: "400",
                      fontSize: "14px",
                    }}
                  >
                    Dành cho tổ chức, cá nhân trong khu phi thuế quan
                  </Box>
                </Box>
              )}
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
              {isPhieuXuatKhoVanChuyen && (
                <Box>
                  <PhieuXuatKhoVanChuyenSubForm
                    register={register}
                    errors={errors}
                  />
                </Box>
              )}
              {isPhieuXuatKhoDaiLy && (
                <Box>
                  <PhieuXuatKhoDaiLySubForm
                    register={register}
                    errors={errors}
                  />
                </Box>
              )}
              {isHoaDonBanTaiSanCong && (
                <Box>
                  <HoaDonBanTaiSanCongSubForm
                    register={register}
                    errors={errors}
                  />
                </Box>
              )}
            </PaperFormGroup>

            {hinhThucHoaDonId === 3 && (
              <PaperFormGroup label="Hình thức điều chỉnh">
                <Box
                  ref={hinhThucDieuChinhRef}
                  sx={{
                    display: "flex",
                    gap: 2,
                    alignItems: "center",
                  }}
                >
                  <Box sx={{ mt: 3 }}>
                    <SelectBoxLyDoDieuChinh
                      value={formData.hoa_don_ly_do_dieu_chinh_id}
                      onValueChanged={(id) => {
                        clearErrors("hoa_don_ly_do_dieu_chinh_id");
                        setFormData({
                          ...formData,
                          hoa_don_ly_do_dieu_chinh_id: id,
                        });
                        handleSetHangHoaDonGiaAm(id);
                      }}
                      errors={errors}
                      register={register}
                    />
                    {errors && errors["hoa_don_ly_do_dieu_chinh_id"] && (
                      <FormControl.Validation variant="error">
                        Vui lòng chọn hình thức điều chỉnh
                      </FormControl.Validation>
                    )}
                  </Box>
                  <Box sx={{ flex: 1 }}>
                    <FormControl>
                      <FormControl.Label>Lý do điều chỉnh</FormControl.Label>
                      <TextInput
                        name="ly_do_dieu_chinh"
                        block
                        register={register}
                        required
                        errors={errors}
                        validateMessage="Vui lòng điền lý do điều chỉnh"
                      />
                    </FormControl>
                  </Box>
                </Box>
              </PaperFormGroup>
            )}
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
                    <Text text="Mã số thuế" />
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
                      value={nguoiMuaMstValue ?? ""}
                      onValueChanged={(data) => {
                        setValue("nguoi_mua_mst", data.text);
                        trigger("nguoi_mua_mst");
                        setValue(
                          "nguoi_mua_ten_donvi",
                          data.khach_hang?.ten_don_vi ?? "",
                        );
                        setValue(
                          "ma_dv_ngan_sach",
                          data.khach_hang?.ma_dv_ngan_sach ?? "",
                        );
                        setValue(
                          "nguoi_mua_email",
                          data.khach_hang?.email ?? "",
                        );
                        setValue(
                          "nguoi_mua_stk",
                          data.khach_hang?.stk?.split("|")[0] ?? "",
                        );
                        if (data.khach_hang?.stk) {
                          setValue(
                            "nguoi_mua_ngan_hang",
                            data.khach_hang?.stk?.split("|")[
                              data.khach_hang?.stk?.split("|").length - 1
                            ],
                          );
                        } else {
                          setValue("nguoi_mua_ngan_hang", "");
                        }
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
                        mst={nguoiMuaMstValue ?? ""}
                        onApply={(data) => {
                          setValue("nguoi_mua_ten_donvi", data?.ten_dv ?? "");
                          setValue("nguoi_mua_dia_chi", data?.dia_chi ?? "");

                          console.log({
                            data,
                          });
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
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length > 14) {
                        setError("nguoi_mua_mst", {
                          type: "manual",
                          message:
                            "Mã số thuế người mua không được vượt quá 14 ký tự",
                        });
                      } else if (
                        value &&
                        !isNguoiMuaCccd(value) &&
                        /[^\d-]/.test(value)
                      ) {
                        setError("nguoi_mua_mst", {
                          type: "manual",
                          message:
                            "Mã số thuế chỉ được chứa số và dấu gạch ngang (-)",
                        });
                      } else {
                        clearErrors("nguoi_mua_mst");
                      }
                    }}
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
                      <Text text="Đơn vị mua hàng" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_ten_donvi"
                      // required={nguoi_mua_mst !== ""}
                      block
                      validateMessage="Vui lòng điền Đơn vị mua hàng"
                      errors={errors}
                      onChange={(e) => {
                        if (e.target.value.length > 400) {
                          setError("nguoi_mua_ten_donvi", {
                            type: "manual",
                            message:
                              "Tên đơn vị mua hàng không được vượt quá 400 ký tự",
                          });
                        } else {
                          clearErrors("nguoi_mua_ten_donvi");
                        }
                      }}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Địa chỉ" />
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
                      <Text text="Họ tên người mua" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="nguoi_mua_ten"
                      block
                      // required
                      validateMessage="Vui lòng điền Họ tên người mua"
                      errors={errors}
                      onChange={(e) => {
                        if (e.target.value.length > 100) {
                          setError("nguoi_mua_ten", {
                            type: "manual",
                            message:
                              "Tên người mua không được vượt quá 100 ký tự",
                          });
                        } else {
                          clearErrors("nguoi_mua_ten");
                        }
                      }}
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
                      onChange={(e) => {
                        if (e.target.value.length > 20) {
                          setError("nguoi_mua_dien_thoai", {
                            type: "manual",
                            message:
                              "Số điện thoại không được vượt quá 20 ký tự",
                          });
                        } else {
                          clearErrors("nguoi_mua_dien_thoai");
                        }
                      }}
                    />
                  </FormControl>
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["1fr", "1fr 1fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Mã đại lý" />
                    </FormControl.Label>
                    <Box sx={{ display: "flex", width: "100%" }}>
                      <TextInputMaDaiLySearch
                        register={register}
                        name="ma_dai_ly"
                        // block
                        // required
                        // validateMessage='Vui lòng điền Mã số thuế'
                        errors={errors}
                        value={getValues("ma_dai_ly")}
                        onValueChanged={(data) => {
                          setValue("ma_dai_ly", data.text);
                          setValue("ten_dai_ly", data.dai_ly?.ten_dai_ly ?? "");
                        }}
                        sx={{
                          width: ["100%", 300],
                        }}
                      />
                    </Box>
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tên đại lý" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="ten_dai_ly"
                      block
                      // required
                      // validateMessage='Vui lòng điền Số điện thoại'
                      errors={errors}
                    />
                  </FormControl>
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: [
                      "1fr",
                      CKM === "M" ? "1fr 1fr 1fr" : "1fr",
                    ],
                    gap: 2,
                  }}
                >
                  {/* {CKM === "M" && ( */}
                  <Box
                    sx={{
                      display: "grid",
                      gridTemplateColumns: ["1fr 1fr 1fr"],
                      gap: 2,
                    }}
                  >
                    <FormControl>
                      <FormControl.Label>
                        <Text text="Căn cước công dân" />
                      </FormControl.Label>
                      <TextInput
                        register={register}
                        name="nguoi_mua_cccd"
                        // required
                        block
                        validateMessage="Vui lòng điền Số Căn cước công dân"
                        errors={errors}
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
                        <Text text="Mã đơn vị quan hệ ngân sách" />
                      </FormControl.Label>
                      <TextInput
                        register={register}
                        name="ma_dv_ngan_sach"
                        // required
                        block
                        // validateMessage="Vui lòng điền Số Căn cước công dân"
                        errors={errors}
                        onChange={(e) => {
                          if (e.target.value.length > 7) {
                            setError("ma_dv_ngan_sach", {
                              type: "manual",
                              message: "Mã đơn vị ngân sách phải đúng 7 ký tự",
                            });
                          } else if (
                            e.target.value.length < 7 &&
                            e.target.value.length > 0
                          ) {
                            setError("ma_dv_ngan_sach", {
                              type: "manual",
                              message: "Mã đơn vị ngân sách phải đúng 7 ký tự",
                            });
                          } else {
                            clearErrors("ma_dv_ngan_sach");
                          }
                        }}
                      />
                    </FormControl>

                    <FormControl>
                      <FormControl.Label>
                        <Text text="Hộ chiếu" />
                      </FormControl.Label>
                      <TextInput
                        register={register}
                        name="so_ho_chieu"
                        // required
                        block
                        // validateMessage="Vui lòng điền Số Căn cước công dân"
                        errors={errors}
                        // onChange={(e) => {
                        //   if (e.target.value.length > 7) {
                        //     setError("ma_dv_ngan_sach", {
                        //       type: "manual",
                        //       message: "Mã đơn vị ngân sách phải đúng 7 ký tự",
                        //     });
                        //   } else if (
                        //     e.target.value.length < 7 &&
                        //     e.target.value.length > 0
                        //   ) {
                        //     setError("ma_dv_ngan_sach", {
                        //       type: "manual",
                        //       message: "Mã đơn vị ngân sách phải đúng 7 ký tự",
                        //     });
                        //   } else {
                        //     clearErrors("ma_dv_ngan_sach");
                        //   }
                        // }}
                      />
                    </FormControl>
                  </Box>

                  {/* )} */}

                  <Box
                    sx={{
                      display: "grid",
                      gridTemplateColumns: ["1fr 1fr"],
                      gap: 2,
                    }}
                  >
                    <FormControl>
                      <FormControl.Label>
                        <Text text="Số tài khoản" />
                      </FormControl.Label>
                      <TextInput
                        register={register}
                        name="nguoi_mua_stk"
                        // required
                        block
                        validateMessage="Vui lòng điền Số tài khoản"
                        errors={errors}
                        onChange={(e) => {
                          if (e.target.value.length > 30) {
                            setError("nguoi_mua_stk", {
                              type: "manual",
                              message:
                                "Số tài khoản không được vượt quá 30 ký tự",
                            });
                          } else {
                            clearErrors("nguoi_mua_stk");
                          }
                        }}
                      />
                    </FormControl>
                    <FormControl>
                      <FormControl.Label>
                        <Text text="Ngân hàng" />
                      </FormControl.Label>
                      <TextInput
                        register={register}
                        name="nguoi_mua_ngan_hang"
                        // required
                        block
                        validateMessage="Vui lòng Ngân hàng"
                        errors={errors}
                        onChange={(e) => {
                          if (e.target.value.length > 400) {
                            setError("nguoi_mua_ngan_hang", {
                              type: "manual",
                              message:
                                "Ngân hàng không được vượt quá 400 ký tự",
                            });
                          } else {
                            clearErrors("nguoi_mua_ngan_hang");
                          }
                        }}
                      />
                    </FormControl>
                  </Box>
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: ["5fr 2fr 2fr", "1fr 1fr 1fr"],
                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Hình thức thanh toán" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="hinh_thuc_tt"
                      required
                      block
                      validateMessage="Vui lòng điền Hình thức thanh toán"
                      errors={errors}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Loại tiền" />
                    </FormControl.Label>
                    <SelectBoxLoaiTien
                      value={loaiTien}
                      onValueChanged={(value) => {
                        setLoaiTien(value);
                        if (value !== "VND") {
                          setValue("so_tien_tang_giam", 0);
                          setValue("so_tien_tang_giam_tien_hang", 0);
                          setValue("so_tien_tang_giam_tien_thue", 0);
                        }
                      }}
                    />
                    {/* <TextInput
                                            register={register}
                                            name='loai_tien'
                                            required
                                            block
                                            // ref={loaiTienInputRef}
                                            validateMessage='Vui lòng Loại tiền'
                                            errors={errors}
                                        /> */}
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tỷ giá" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="ty_gia"
                      // required
                      type="number"
                      step={"0.01"}
                      block
                      validateMessage="Vui lòng Loại tiền"
                      errors={errors}
                    />
                  </FormControl>
                </Box>
              </Box>
            </PaperFormGroup>

            {isHoaDonGTGT && (
              <PaperFormGroup
                label="Thêm thông tin bổ sung của dịch vụ vận tải"
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
                    gridTemplateColumns: ["1fr", "1fr 1fr"],

                    gap: 2,
                  }}
                >
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Ngày đến - ngày đi" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      name="NgayDenNgayDi"
                      block
                      onChange={(e) => {}}
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Tên tàu" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      block
                      onChange={(e) => {}}
                      name="TenTau"
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Số tham chiếu" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      block
                      onChange={(e) => {}}
                      name="SoThamChieu"
                    />
                  </FormControl>
                  <FormControl>
                    <FormControl.Label>
                      <Text text="Nơi đi - nơi đến" />
                    </FormControl.Label>
                    <TextInput
                      register={register}
                      block
                      onChange={(e) => {}}
                      name="NoiDiNoiDen"
                    />
                  </FormControl>
                </Box>
              </PaperFormGroup>
            )}

            <Box
              sx={{
                borderTopWidth: 1,
                borderTopStyle: "solid",
                borderTopColor: "border.default",
                mt: [2, 2, 3],
                pt: [2, 2, 3],
              }}
            >
              {isDieuChinhThue && (
                <HangHoaDieuChinhThue
                  hangHoas={hangHoas}
                  control={control}
                  watch={watch}
                  error={errors}
                  giam_thue_ty_le={giam_thue_ty_le}
                  onGiamThueTyLeChanged={setGiam_thue_ty_le}
                  tienTe={loaiTien}
                  isHoaDonBanHang={isHoaDonBanHang}
                  isSoAm={formData.hoa_don_ly_do_dieu_chinh_id === 2}
                  onValueChanged={(hangHoas) => {
                    setHangHoas(hangHoas);
                    if (hangHoas.find((x) => !IsHoaDonHangHoaValid(x))) {
                      setError("hang_hoas", {});
                    } else {
                      clearErrors("hang_hoas");
                    }
                  }}
                />
              )}
              {!isDieuChinhThue && (
                <HoaDonHangHoaList
                  hangHoas={hangHoas}
                  control={control}
                  watch={watch}
                  error={errors}
                  giam_thue_ty_le={giam_thue_ty_le}
                  onGiamThueTyLeChanged={setGiam_thue_ty_le}
                  limit={
                    formData.hoa_don_ly_do_dieu_chinh_id === 20 ? 1 : undefined
                  }
                  tienTe={loaiTien}
                  isHoaDonBanHang={isHoaDonBanHang}
                  isSoAm={false}
                  isDieuChinh={hinhThucHoaDonId === 3}
                  hangHoasGoc={hangHoasGoc}
                  onValueChanged={(hangHoas) => {
                    setHangHoas(hangHoas);
                    if (hangHoas.find((x) => !IsHoaDonHangHoaValid(x))) {
                      setError("hang_hoas", {});
                    } else {
                      clearErrors("hang_hoas");
                    }
                  }}
                  loaiPhis={loaiPhis}
                  onValueChangedLoaiPhis={(loaiPhis) => {
                    setLoaiPhis(loaiPhis);
                    if (loaiPhis.find((x) => !IsHoaDonLoaiPhiValid(x))) {
                      setError("loai_phis", {});
                    } else {
                      clearErrors("loai_phis");
                    }
                  }}
                  loaiTien={loaiTien}
                  setTongTienChu={(data) => {
                    setTongTienChu(data);
                  }}
                  tongTienChu={tongTienChu}
                  hoa_don_dang_ky_phat_hanh_mau_so={
                    formData.hoa_don_dang_ky_phat_hanh_mau_so
                  }
                />
              )}
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
              flexWrap: "wrap",
              flexDirection: isMobile ? "column" : "row",
              gap: 2,
            }}
          >
            <Box sx={{ flex: 1 }}>
              {hoaDonId > 0 && (
                <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                  <PrintHoaDonButton id={hoaDonId} />
                  <PrintHoaDonBienBanButton id={hoaDonId} hoaDon={formData} />
                  <Link
                    href={`${appInfo.baseApiURL}/hoa-don/${hoaDonId}/download`}
                  >
                    <Button
                      text="Tải xuống XML"
                      variant="invisible"
                      size="medium"
                      leadingVisual={DownloadIcon}
                    />
                  </Link>
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
              {hoaDonViewModel?.is_ky_so_succes !== true && (
                <Button
                  text="Lưu hóa đơn"
                  type="submit"
                  sx={{ minWidth: "100px" }}
                  size="large"
                  isLoading={isSaving}
                  disabled={hoaDonViewModel?.is_ky_so_succes ?? false}
                />
              )}
              {hoaDonId > 0 && isSavedForKySo && (
                <>
                  {CKM === "M" && (
                    <>
                      {hoaDonViewModel?.is_ky_so_succes !== true && (
                        <>
                          {user &&
                            !user.is_hsm_signing &&
                            !user.is_remote_signing && (
                              <Button
                                text="Ký số người bán"
                                sx={{ minWidth: "100px" }}
                                variant="primary"
                                size="large"
                                type="button"
                                leadingVisual={IssueClosedIcon}
                                isLoading={isSaving}
                                onClick={() => {
                                  setIsMttPhatHanh(false);
                                  setIsKySoVaPhatHanh(false);
                                  handleGetBase64KySoNguoiBan();
                                }}
                                disabled={!isAllowPhatHanh}
                              />
                            )}
                          {user &&
                            (user.is_hsm_signing || user.is_remote_signing) && (
                              <Button
                                text="Ký số người bán"
                                sx={{ minWidth: "100px" }}
                                variant="primary"
                                size="large"
                                type="button"
                                leadingVisual={IssueClosedIcon}
                                isLoading={isSaving}
                                onClick={() => {
                                  setIsMttPhatHanh(false);
                                  setIsKySoVaPhatHanh(false);
                                  handleKySoRemoteAsync();
                                }}
                                disabled={!isAllowPhatHanh}
                              />
                            )}
                        </>
                      )}
                      {hoaDonViewModel?.is_ky_so_succes !== true &&
                        (hoaDonViewModel?.hoa_don_trang_thai_id ===
                          eHoaDonTrangThai.NHAP ||
                          hoaDonViewModel?.hoa_don_trang_thai_id ===
                            eHoaDonTrangThai.CHUA_GUI_CQT) && (
                          <>
                            {user &&
                              !user.is_hsm_signing &&
                              !user.is_remote_signing && (
                                <Button
                                  text="Ký số và phát hành"
                                  sx={{ minWidth: "100px" }}
                                  disabled={!isAllowPhatHanh}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    setIsKySoVaPhatHanh(true);
                                    handleGetBase64KySoMttDongThoi();
                                  }}
                                />
                              )}
                            {user &&
                              (user.is_hsm_signing ||
                                user.is_remote_signing) && (
                                <Button
                                  text="Ký số và phát hành"
                                  sx={{ minWidth: "100px" }}
                                  disabled={!isAllowPhatHanh}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    setIsKySoVaPhatHanh(true);
                                    handleKySoVaPhatHanhRemoteAsync();
                                  }}
                                />
                              )}
                          </>
                        )}
                      {hoaDonViewModel?.is_ky_so_succes === true &&
                        hoaDonViewModel?.hoa_don_trang_thai_id ===
                          eHoaDonTrangThai.CHUA_GUI_CQT && (
                        <>
                          {user &&
                            !user.is_hsm_signing &&
                            !user.is_remote_signing && (
                              <Button
                                text="Phát hành hóa đơn"
                                sx={{ minWidth: "100px" }}
                                disabled={!isAllowPhatHanh}
                                variant="primary"
                                size="large"
                                type="button"
                                leadingVisual={IssueClosedIcon}
                                isLoading={isSaving}
                                onClick={() => {
                                  setIsMttPhatHanh(true);
                                  setIsKySoVaPhatHanh(false);
                                  handleGetBase64KySo();
                                }}
                              />
                            )}
                          {user &&
                            (user.is_hsm_signing || user.is_remote_signing) && (
                              <Button
                                text="Phát hành hóa đơn"
                                sx={{ minWidth: "100px" }}
                                disabled={!isAllowPhatHanh}
                                variant="primary"
                                size="large"
                                type="button"
                                leadingVisual={IssueClosedIcon}
                                isLoading={isSaving}
                                onClick={() => {
                                  setIsMttPhatHanh(true);
                                  handleMttPhatHanhRemoteAsync();
                                }}
                              />
                            )}
                        </>
                      )}
                    </>
                  )}
                  {(CKM === "C" || CKM === "K") && (
                    <>
                      {hoaDonViewModel?.is_ky_so_succes !== true &&
                        hoaDonViewModel?.hoa_don_trang_thai_id ===
                          eHoaDonTrangThai.NHAP && (
                          <>
                            {user &&
                              !user.is_hsm_signing &&
                              !user.is_remote_signing && (
                                <Button
                                  text="Ký số"
                                  sx={{ minWidth: "100px" }}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    handleGetBase64KySo();
                                    setIsKySoVaPhatHanh(false);
                                  }}
                                  disabled={!isAllowPhatHanh}
                                />
                              )}
                            {user &&
                              (user.is_hsm_signing ||
                                user.is_remote_signing) && (
                                <Button
                                  text="Ký số"
                                  sx={{ minWidth: "100px" }}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    handleKySoRemoteAsync();
                                    setIsKySoVaPhatHanh(false);
                                  }}
                                  disabled={!isAllowPhatHanh}
                                />
                              )}
                            {user &&
                              !user.is_hsm_signing &&
                              !user.is_remote_signing && (
                                <Button
                                  text="Ký số và phát hành"
                                  sx={{ minWidth: "100px" }}
                                  disabled={!isAllowPhatHanh}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    handleGetBase64KySo();
                                    setIsKySoVaPhatHanh(true);
                                  }}
                                />
                              )}
                            {user &&
                              (user.is_hsm_signing ||
                                user.is_remote_signing) && (
                                <Button
                                  text="Ký số và phát hành"
                                  sx={{ minWidth: "100px" }}
                                  disabled={!isAllowPhatHanh}
                                  variant="primary"
                                  size="large"
                                  type="button"
                                  leadingVisual={IssueClosedIcon}
                                  isLoading={isSaving}
                                  onClick={() => {
                                    setIsMttPhatHanh(false);
                                    setIsKySoVaPhatHanh(true);
                                    handleKySoVaPhatHanhRemoteAsync();
                                  }}
                                />
                              )}
                          </>
                        )}
                      {hoaDonViewModel?.is_ky_so_succes === true && (
                        <Button
                          text="Phát hành"
                          sx={{ minWidth: "100px" }}
                          disabled={!isAllowPhatHanh}
                          variant="primary"
                          size="large"
                          type="button"
                          leadingVisual={IssueClosedIcon}
                          isLoading={isSaving}
                          onClick={() => {
                            handlePhatHanhAsync("");
                          }}
                        />
                      )}
                    </>
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
          base64BienBan={base64BienBan}
          onClose={() => {
            setIsShowKySoModal(false);
          }}
          onSuccess={(signedtext, bienBanSignedText) => {
            setIsShowKySoModal(false);
            if (CKM === "M" && isMttPhatHanh) {
              handlePhatHanhAsync(signedtext, bienBanSignedText);
            } else {
              handleUpdateKySoSuccss(signedtext, bienBanSignedText);
            }
          }}
        />
      )}
      {isShowPhatHanhResultModal && hoaDongPhatHanhPushNotifyModel && (
        <HoaDonPhatHanhResultModal
          id={hoaDonId}
          data={hoaDongPhatHanhPushNotifyModel}
          onClose={() => {
            setIsShowPhatHanhResultModal(false);
            handleGetHoaDonViewModel(hoaDonId);
          }}
        />
      )}
      {isOpenHoaDonGocModal && (
        <HoaDonGocInfoModal
          value={thongTinHoaDonGoc}
          onSubmit={(data, hoaDon) => {
            setThongTinHoaDonGoc(data);
            setIsOpenHoaDonGocModal(false);
            if (hoaDon) {
              handleGetHoaDonViewModel(hoaDon.id);
            }
          }}
        />
      )}
    </Box>
  );
};

export default HoaDonForm;
