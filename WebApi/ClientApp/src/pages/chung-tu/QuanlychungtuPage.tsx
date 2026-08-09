import {
  CopyIcon,
  DownloadIcon,
  EyeIcon,
  FileCodeIcon,
  FileIcon,
  GitCompareIcon,
  GitPullRequestClosedIcon,
  GitPullRequestIcon,
  HistoryIcon,
  IssueClosedIcon,
  KebabHorizontalIcon,
  MailIcon,
  PaperAirplaneIcon,
  PencilIcon,
  PlusIcon,
  ShieldCheckIcon,
  ShieldIcon,
  ShieldSlashIcon,
  ShieldXIcon,
  SyncIcon,
  TrashIcon,
  WorkflowIcon,
} from "@primer/octicons-react";

import {
  ActionList,
  ActionMenu,
  Box,
  IconButton,
  UnderlineNav,
  useConfirm,
} from "@primer/react";
import moment from "moment";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Helmet } from "react-helmet";
import { Link, useHistory, useParams } from "react-router-dom";
import { HOA_DON_API } from "../../api/hoa-don/hoaDonApi";
import ExportToExcelBtn from "../../component-data/export-excel-btn/ExportToExcelBtn";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import DataTableRemotePaging from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import HoaDonImportButton from "./ChungTuImportButton";

import ChungTuFilter from "./ChungTuFilter";
import { useAuth } from "../../hooks/useAuth";
import { formatXml, parseSoapResponse } from "../../helpers/common";
import {
  checkChungTuThayTheDieuChinh,
  hasToKhaiChungTuChapNhan,
  layDanhSachToKhaiChungTu,
  validateDieuChinhChungTu,
  validateThayTheChungTu,
} from "../../helpers/toKhaiChungTuHelper";
import { axiosClient } from "../../api/axiosClient";
import Modal from "../../component-ui/modal";
import XemChungTu from "./XemChungTu";
import { PrintIcon } from "../../component-ui/icon";
import { NhatKyChungTuModal } from "./NhatKyChungTuModal";
import KySoModal from "../../component-data/ky-so-modal";
import GuiChungTuLenCQTModal from "./GuiChungTuLenCQTModal";
import ChungTuKySoPhatHanhMultiple from "./ChungTuKySoPhatHanhMultiple";
import {
  buildChungTuDownloadZipUrl,
  MAX_CHUNG_TU_DOWNLOAD,
} from "../../helpers/chungTuDownloadHelper";

const tabData = [
  {
    name: "Chứng từ nháp",
    value: "nhap",
    api: "LayDanhSachChungTuNhap",
    excel_api: "XuatDanhSachChungTuNhap",
  },
  {
    name: "Chứng từ đã ký",
    value: "da-ky",
    api: "LayDanhSachChungTuDaKy",
    excel_api: "XuatDanhSachChungTuDaKy",
  },
  {
    name: "Chứng từ đã gửi CQT",
    value: "da-gui-cqt",
    api: "LayDanhSachChungTuDaGuiCQT",
    excel_api: "XuatDanhSachChungTuDaGuiCQT",
  },
];

const QuanlychungtuPage = () => {
  const { tab }: any = useParams();
  const history = useHistory();
  const confirm = useConfirm();
  const [loading, setLoading] = useState(false);

  const [isShowSendEmailConfirm, setisShowSendEmailConfirm] = useState(false);
  const [hoaDonActionMenuOpenId, setHoaDonActionMenuOpenId] = useState(0);
  const [dataDetail, setDataDetail] = useState<any>(null);
  const [openModalXemChungTu, setOpenModalXemChungTu] = useState(false);
  const [xemChungTuInChuyenDoi, setXemChungTuInChuyenDoi] = useState(false);
  const [openHistoryModal, setOpenHistoryModal] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [base64KySo, setBase64KySo] = useState("");
  const [isShowKySoModal, setIsShowKySoModal] = useState(false);
  const [hoaDonSelectedIds, setHoaDonSelectedIds] = useState<number[]>([]);

  const { status, filter, isShowDeleteConfirm } = useAppSelector(
    (x) => x.hoaDon.hoaDonReducer
  );
  const { user } = useAuth();
  const [dataFileter, setDataFilter] = useState({
    loai_chung_tu: "",
    mau_so: "03/TNCN",
    ky_hieu: "",
    tu_ngay: "",
    den_ngay: "",
  });
  const [dataTable, setDataTable] = useState<any>([]);
  const [pagination, setPagination] = useState({
    pageIndex: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0,
  });
  const [openXMLModal, setOpenXMLModal] = useState(false);
  const [xmlContent, setXMLContent] = useState("");
  const [openConFirmDeleteModal, setOpenConFirmDeleteModal] = useState(false);
  const [openConFirmRemoveModal, setOpenConFirmRemoveModal] = useState(false);
  const [openGuiChungTuModal, setOpenGuiChungTuModal] = useState(false);
  const [isSending, setIsSending] = useState(false);

  const handleDownloadChungTuZip = useCallback(
    (type: "pdf" | "xml") => {
      const ids = hoaDonSelectedIds ?? [];
      if (ids.length === 0) return;
      if (ids.length > MAX_CHUNG_TU_DOWNLOAD) {
        NotifyHelper.Error(
          `Không được tải xuống quá ${MAX_CHUNG_TU_DOWNLOAD} chứng từ cùng lúc`
        );
        return;
      }
      const madonvi =
        user?.donvi_ma_dv || user?.donvi?.ma_dv || "";
      if (!madonvi) {
        NotifyHelper.Error("Không xác định được mã đơn vị.");
        return;
      }
      const url = buildChungTuDownloadZipUrl(type, ids, madonvi);
      window.open(url, "_blank");
    },
    [hoaDonSelectedIds, user]
  );


  const { checkAccesiableTo } = useCommonContext();

  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(HOA_DON_API, "GET");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    LayDanhSachChungTu({
      ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
      loaiTimKiem: 0,
      mau_so: dataFileter.mau_so,
      ky_hieu: dataFileter.ky_hieu,
      tu_ngay: dataFileter.tu_ngay,
      den_ngay: dataFileter.den_ngay,
      soct: "",
      matracuu: "",
      madonvi: user?.donvi?.ma_dv,
      pageIndex: 1,
      pageSize: pagination.pageSize,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tab]);

  const LayDanhSachChungTu = async (payload?: any) => {
    const apiName =
      tabData.find((x) => x.value === tab)?.api ?? "LayDanhSachChungTuNhap";

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <${apiName} xmlns="http://tempuri.org/">
      <loaiTimKiem>${0}</loaiTimKiem>
      <mauso>${payload?.mau_so ?? "03/TNCN"}</mauso>
      <kyhieu>${payload?.ky_hieu}</kyhieu>
      <tungay>${payload?.tu_ngay}</tungay>
      <denngay>${payload?.den_ngay}</denngay>
      <soct></soct>
      <matracuu></matracuu>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <pageIndex>${payload?.pageIndex ?? 1}</pageIndex>
      <pageSize>${payload?.pageSize ?? pagination.pageSize}</pageSize>
    </${apiName}>
  </soap12:Body>
</soap12:Envelope>`;

    setLoading(true);

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
      const newData = parseRes.data?.map((item: any, index: number) => ({
        ...item,
        MaCT:
          item?.MaCT !== undefined && item?.MaCT !== null
            ? Number(item.MaCT)
            : item?.MaCT,
        PhanbietCTValue: item?.PhanbietCT,
        TinhtrangCTText: GetTinhtrangCT(item?.TinhtrangCT),
        PhanbietCT:
          tab === "da-gui-cqt"
            ? item?.GhichuCT
            : PhanbietChungtu(
              item?.PhanbietCT,
              item?.SoCTLienquan,
              item?.KHMSCTLienquan,
              item?.KHCTLienquan
            ),
      }));

      setDataTable(newData);
      setPagination((prev) => ({
        ...prev,
        totalCount: parseRes.pagination?.totalRecords ?? 0,
        pageIndex: parseRes.pagination?.pageIndex ?? 1,
        totalPages: parseRes.pagination?.totalPages ?? 0,
      }));
    } else {
      NotifyHelper.Error(parseRes.message);
    }

    setLoading(false);
  };

  const XuatExcelDataDanhSachChungTu = async (payload?: any) => {
    const apiName =
      tabData.find((x) => x.value === tab)?.excel_api ??
      "XuatDanhSachChungTuNhap";

    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <${apiName} xmlns="http://tempuri.org/">
      <loaiTimKiem>${0}</loaiTimKiem>
      <mauso>03/TNCN</mauso>
      <kyhieu>${payload?.ky_hieu}</kyhieu>
      <tungay>${payload?.tu_ngay}</tungay>
      <denngay>${payload?.den_ngay}</denngay>
      <soct></soct>
      <matracuu></matracuu>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
    </${apiName}>
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

    return parseRes;
  };

  const GetTinhtrangCT = useCallback((status: number): string => {
    switch (status) {
      case 1:
        return "Mới lập";
      case 2:
        return "Đã ký";
      case 33:
        return "Đã gửi CQT";
      case 6:
        return "Đã hủy";
      case 3:
        return "Đã in chuyển đổi";
      default:
        return "";
    }
  }, []);

  const PhanbietChungtu = useCallback(
    (
      status: number,
      sochungtulq: string,
      mschungtulq: string,
      khchungtulq: string
    ): string => {
      switch (status) {
        case 0:
          return "Chứng từ mới";
        case 1:
          return `Thay thế cho CT số: ${sochungtulq}, Mẫu số: ${mschungtulq}, Ký hiệu: ${khchungtulq}`;
        case 2:
          return `Điều chỉnh cho CT số: ${sochungtulq}, Mẫu số: ${mschungtulq}, Ký hiệu: ${khchungtulq}`;
        default:
          return "";
      }
    },
    []
  );

  const ensureToKhaiChungTuChapNhan = async (): Promise<boolean> => {
    const parseRes = await layDanhSachToKhaiChungTu(user?.donvi?.ma_dv);
    if (parseRes.status !== "success") {
      NotifyHelper.Error(parseRes.message ?? "Không thể kiểm tra tờ khai chứng từ");
      return false;
    }
    if (!hasToKhaiChungTuChapNhan(parseRes.data)) {
      NotifyHelper.Error(
        "Chỉ được tạo chứng từ sau khi đã có tờ khai được Cơ quan thuế chấp nhận",
      );
      return false;
    }
    return true;
  };

  const handleMoFormThemMoi = async () => {
    const canCreate = await ensureToKhaiChungTuChapNhan();
    if (canCreate) {
      history.push("../../chung-tu/form/0");
    }
  };

  const handleLapChungTuThayTheDieuChinh = async (
    row: any,
    loaiChungTu: number,
  ) => {
    const validateMessage =
      loaiChungTu === 2
        ? validateDieuChinhChungTu(row)
        : validateThayTheChungTu(row);
    if (validateMessage) {
      NotifyHelper.Error(validateMessage);
      return;
    }

    const parseRes = await checkChungTuThayTheDieuChinh(user?.donvi?.ma_dv, {
      mau_so: row?.MSChungtu,
      ky_hieu: row?.KHChungtu,
      so_chung_tu_goc: row?.Sochungtu,
      loai_chung_tu: loaiChungTu,
    });

    if (parseRes.status === "success") {
      history.push(
        `../../chung-tu/form/0?tinhchatct=${loaiChungTu}&mact_goc=${parseRes.data}`,
      );
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleSendEmail = async () => {
    await confirm({
      content: "Bạn có chắc chắn muốn gửi email tới người mua",
      title: "Gửi Email",
      cancelButtonContent: "Đóng",
      confirmButtonContent: "Gửi email",
      confirmButtonType: "primary",
    });
  };


  const GuiMailChungTu = async (machungtu: number) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                 xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                 xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <GuiMailCT xmlns="http://tempuri.org/">
      <machungtu>${machungtu}</machungtu>
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
    </GuiMailCT>
  </soap12:Body>
</soap12:Envelope>`;

    setIsSending(true);

    try {
      const res: string = await axiosClient.post(
        process.env.REACT_APP_API_CHUNG_TU as string,
        soap,
        {
          headers: {
            "Content-Type": "text/xml; charset=utf-8"
          }
        }
      );

      const parseRes = parseSoapResponse(res);
      console.log("result sentmail", parseRes);
      if (parseRes === 1) {
        NotifyHelper.Success("Gửi email thành công");
      } else {
        NotifyHelper.Error(parseRes.message);
      }

    } catch (err) {
      NotifyHelper.Error("Không gọi được service gửi mail");
    }

    setIsSending(false);
  };

  const handleChangePage = async (pageIndex: number) => {
    await LayDanhSachChungTu({
      ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
      loaiTimKiem: 0,
      mau_so: dataFileter.mau_so,
      ky_hieu: dataFileter.ky_hieu,
      tu_ngay: dataFileter.tu_ngay,
      den_ngay: dataFileter.den_ngay,
      soct: "",
      matracuu: "",
      madonvi: user?.donvi?.ma_dv,
        pageIndex: pageIndex + 1,
    pageSize: pagination.pageSize
    });
  };

  const XoaNhapChungTu = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <XoaNhapChungTu xmlns="http://tempuri.org/">
        <mact>${dataDetail?.MaCT}</mact>
        <madv>${user?.donvi_ma_dv}</madv>
      </XoaNhapChungTu>
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
      setOpenConFirmDeleteModal(false);
      await LayDanhSachChungTu({
        ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
        loaiTimKiem: 0,
        mau_so: dataFileter.mau_so,
        ky_hieu: dataFileter.ky_hieu,
        tu_ngay: dataFileter.tu_ngay,
        den_ngay: dataFileter.den_ngay,
        soct: "",
        matracuu: "",
        madonvi: user?.donvi?.ma_dv,
        pageIndex: 1,
      });
      NotifyHelper.Success(parseRes.message);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const HuyChungTu = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <HuyChungTu xmlns="http://tempuri.org/">
        <machungtu>${dataDetail?.MaCT}</machungtu>
        <kyhieuchungtu>${dataDetail?.KHChungtu}</kyhieuchungtu>
        <madonvi>${user?.donvi_ma_dv}</madonvi>
      </HuyChungTu>
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
      setOpenConFirmRemoveModal(false);
      await LayDanhSachChungTu({
        ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
        loaiTimKiem: 0,
        mau_so: dataFileter.mau_so,
        ky_hieu: dataFileter.ky_hieu,
        tu_ngay: dataFileter.tu_ngay,
        den_ngay: dataFileter.den_ngay,
        soct: "",
        matracuu: "",
        madonvi: user?.donvi?.ma_dv,
        pageIndex: 1,
      });
      NotifyHelper.Success(parseRes.message);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const Laythongtinchungtugoc = async (data: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
  <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
    <soap12:Body>
      <LayThongTinchungTuGoc xmlns="http://tempuri.org/">
        <madonvi>${user?.donvi?.ma_dv}</madonvi>
        <mausogoc>${data?.KHMSCTLienquan}</mausogoc>
        <kyhieugoc>${data?.KHCTLienquan}</kyhieugoc>
        <soctgoc>${data?.SoCTLienquan}</soctgoc>
      </LayThongTinchungTuGoc>
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
      // Lấy được mã chứng từ gốc rồi thì chuyển qua trang form
      history.push(
        `../../chung-tu/form/${data.MaCT}?tinhchatct=${data?.TinhchatCT}&mact_goc=${parseRes.data?.MaCT}`
      );
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const handleKySo = async (data: any) => {
    if (user) {
      setIsShowKySoModal(true);
      // await LaysoCT_update(user?.donvi_ma_dv, data?.MaCT?.toString(), "CT/25E");
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
      }
    );

    const parseRes = parseSoapResponse(res);
    setIsSaving(false);

    if (parseRes.status === "success") {
      NotifyHelper.Success("Ký số thành công");

      LayDanhSachChungTu({
        ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
        loaiTimKiem: 0,
        mau_so: dataFileter.mau_so,
        ky_hieu: dataFileter.ky_hieu,
        tu_ngay: dataFileter.tu_ngay,
        den_ngay: dataFileter.den_ngay,
        soct: "",
        matracuu: "",
        madonvi: user?.donvi?.ma_dv,
        pageIndex: 1,
      });
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
        </GuiChungTuCQT>
      </soap12:Body>
    </soap12:Envelope>`;

    setIsSending(true);
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
    setIsSending(false);

    if (parseRes.status === "success") {
      NotifyHelper.Success(parseRes.message);
      setOpenGuiChungTuModal(false);
      LayDanhSachChungTu({
        ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
        loaiTimKiem: 0,
        mau_so: dataFileter.mau_so,
        ky_hieu: dataFileter.ky_hieu,
        tu_ngay: dataFileter.tu_ngay,
        den_ngay: dataFileter.den_ngay,
        soct: "",
        matracuu: "",
        madonvi: user?.donvi?.ma_dv,
        pageIndex: 1,
      });
    } else {
      setOpenGuiChungTuModal(false);
      NotifyHelper.Error(parseRes.message ?? "Có lỗi xảy ra");
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Chứng từ</title>
      </Helmet>

      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <Box>
          <Box
            id="header"
            sx={{
              display: "flex",
              flexDirection: ["column", "column", "row"],
              gap: 2,
            }}
          >
            <Box
              sx={{
                flex: 1,
              }}
            >
              <Heading text="Danh sách chứng từ" />
            </Box>
            <Box
              sx={{
                display: "flex",
                gap: 1,
                justifyContent: "flex-end",
                flexDirection: ["column", "column", "row"],
                alignItems: ["flex-end", "flex-end", "center"],
              }}
            >
              {/* <HoaDonSort
                sortBy={{
                  field:
                    (filter.sort_by ?? "") !== "" ? filter.sort_by ?? "" : "id",
                  mode: filter.sort_mode ?? eSortMode.DESC,
                }}
                onValueChanged={(data) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      sort_by: data.field,
                      sort_mode: data.mode,
                    })
                  );
                }}
              /> */}
              <Box sx={{ display: "flex", gap: 1 }}>
                <Button
                  text="Thêm mới"
                  leadingVisual={PlusIcon}
                  variant="primary"
                  size="medium"
                  onClick={handleMoFormThemMoi}
                />
                <HoaDonImportButton
                  onBeforeOpen={ensureToKhaiChungTuChapNhan}
                  onSuccess={() => {
                    LayDanhSachChungTu({
                      ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
                      loaiTimKiem: 0,
                      mau_so: dataFileter.mau_so,
                      ky_hieu: dataFileter.ky_hieu,
                      tu_ngay: dataFileter.tu_ngay,
                      den_ngay: dataFileter.den_ngay,
                      soct: "",
                      matracuu: "",
                      madonvi: user?.donvi?.ma_dv,
                      pageIndex: 1,
                    });
                  }}
                />
              </Box>
            </Box>
          </Box>
          <Box id="tabs">
            <UnderlineNav aria-label="Repository">
              <UnderlineNav.Item
                as={Link}
                to="../chung-tu/nhap"
                aria-current={tab === "nhap" ? "page" : undefined}
                icon={ShieldIcon}
              >
                Chứng từ nháp
              </UnderlineNav.Item>
              <UnderlineNav.Item
                as={Link}
                to={"../chung-tu/da-ky"}
                aria-current={!tab || tab === "da-ky" ? "page" : undefined}
                icon={ShieldCheckIcon}
              >
                Chứng từ đã ký
              </UnderlineNav.Item>
              <UnderlineNav.Item
                as={Link}
                to={"../chung-tu/da-gui-cqt"}
                aria-current={tab === "da-gui-cqt" ? "page" : undefined}
                icon={ShieldIcon}
              >
                Chứng từ đã gửi CQT
              </UnderlineNav.Item>
            </UnderlineNav>
          </Box>

          <Box sx={{ mt: 3, display: "flex", justifyContent: "flex-end" }}>
            <Box sx={{ display: "flex", gap: 1, flexWrap: "wrap", mb: 1 }}>
              {(hoaDonSelectedIds?.length ?? 0) > 0 && (
                <ActionMenu>
                  <ActionMenu.Button
                    size="small"
                    leadingVisual={DownloadIcon}
                  >
                    Tải xuống
                  </ActionMenu.Button>
                  <ActionMenu.Overlay>
                    <ActionList showDividers>
                      <ActionList.Item
                        onSelect={() => handleDownloadChungTuZip("pdf")}
                      >
                        <ActionList.LeadingVisual>
                          <FileIcon />
                        </ActionList.LeadingVisual>
                        Tải pdf
                      </ActionList.Item>
                      {tab !== "nhap" && (
                        <ActionList.Item
                          onSelect={() => handleDownloadChungTuZip("xml")}
                        >
                          <ActionList.LeadingVisual>
                            <FileCodeIcon />
                          </ActionList.LeadingVisual>
                          Tải xml gửi CQT
                        </ActionList.Item>
                      )}
                    </ActionList>
                  </ActionMenu.Overlay>
                </ActionMenu>
              )}

              {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                (!tab || tab === "nhap" || tab === "cho-phat-hanh") && (
                  <>
                    {tab !== "da-phat-hanh" && tab && (
                      <ChungTuKySoPhatHanhMultiple
                        ids={hoaDonSelectedIds ?? []}
                        // isKhacNgay={hoaDonKhacNgay != undefined}
                        onClose={() => {
                          setHoaDonSelectedIds([]);
                          LayDanhSachChungTu({
                            ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
                            loaiTimKiem: 0,
                            mau_so: dataFileter.mau_so,
                            ky_hieu: dataFileter.ky_hieu,
                            tu_ngay: dataFileter.tu_ngay,
                            den_ngay: dataFileter.den_ngay,
                            soct: "",
                            matracuu: "",
                            madonvi: user?.donvi?.ma_dv,
                            pageIndex: 1,
                          });
                        }}
                      // title={
                      //   tab === "cho-phat-hanh"
                      //     ? "Gửi cấp mã Thuế"
                      //     : "Ký số và gửi cấp mã"
                      // }
                      />
                    )}
                    {/* <Button
                    text="Gửi HĐ nháp"
                    leadingVisual={MailIcon}
                    onClick={handleSendEmail}
                    isLoading={isSaving}
                  /> */}
                  </>
                )}

              <Button
                text="Refresh"
                leadingVisual={SyncIcon}
                onClick={() => {
                  setHoaDonSelectedIds([]);
                  setDataFilter({
                    ...dataFileter,
                    ky_hieu: "",
                    tu_ngay: "",
                    den_ngay: "",
                  });

                  LayDanhSachChungTu({
                    ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
                    loaiTimKiem: 0,
                    mau_so: dataFileter.mau_so,
                    ky_hieu: "",
                    tu_ngay: "",
                    den_ngay: "",
                    soct: "",
                    matracuu: "",
                    madonvi: user?.donvi?.ma_dv,
                    pageIndex: 1,
                  });
                }}
                size="medium"
              />
              <ChungTuFilter
                dataFilter={dataFileter}
                setValueFilter={async (data) => {
                  setDataFilter(data);
                  setHoaDonSelectedIds([]);
                }}
                loadData={async (
                  changes: Partial<{
                    mau_so?: string;
                    ky_hieu?: string;
                    tu_ngay?: string;
                    den_ngay?: string;
                  }>
                ) => {
                  const payload = {
                    loaiTimKiem: 0,
                    mau_so: dataFileter.mau_so,
                    ky_hieu: dataFileter.ky_hieu,
                    tu_ngay: dataFileter.tu_ngay,
                    den_ngay: dataFileter.den_ngay,
                    soct: "",
                    matracuu: "",
                    madonvi: user?.donvi?.ma_dv,
                    pageIndex: 1,
                  };
                  await LayDanhSachChungTu({
                    ...payload,
                    ...changes, // ghi đè nhiều trường cùng lúc
                  });
                }}
              />

              <ExportToExcelBtn
                fileName={`Danh_sach_chung_tu`}
                formatDataFunction={(data) => {
                  return data.map((x) => {
                    return {
                      "Mã chứng từ": x?.MaCT,
                      "Ngày lập": moment(x?.NgayLapCT).format("DD/MM/YYYY"),
                      "Số chứng từ": x?.Sochungtu,
                      "Mã số thuế": x?.MasothueNNT,
                      "Tên người nộp thuế": x?.TenNNT,
                      "Số CMND/CCCD": x?.SoCMND,
                      "Thuế TNCN": x?.ThueTNCN,
                      "Tính chất":
                        tab === "da-gui-cqt"
                          ? x?.GhichuCT
                          : PhanbietChungtu(
                            x?.PhanbietCT,
                            x?.SoCTLienquan,
                            x?.KHMSCTLienquan,
                            x?.KHCTLienquan
                          ),
                      "Trạng thái": GetTinhtrangCT(x?.TinhtrangCT),
                      "Trạng thái gửi CQT": x?.TrangthaiguiCQT,
                      "Lý do": x?.DSLDo ? formatXml(x?.DSLDo) : "",
                    };
                  });
                }}
                fetchDataPromise={() => {
                  return new Promise((resolve, reject) => {
                    return XuatExcelDataDanhSachChungTu({
                      ///0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu
                      loaiTimKiem: 0,
                      mau_so: dataFileter.mau_so,
                      ky_hieu: dataFileter.ky_hieu,
                      tu_ngay: dataFileter.tu_ngay,
                      den_ngay: dataFileter.den_ngay,
                    }).then((res) => {
                      if (res.status === "success") {
                        resolve(res.data);
                      } else {
                        NotifyHelper.Error(res.message ?? "Error");
                        resolve(undefined);
                      }
                    });
                  });
                }}
              />
            </Box>
          </Box>

          <Box sx={{ mt: 3 }}>
            <DataTableRemotePaging
              title={`Tổng số: ${(
                pagination.totalCount ?? 0
              ).toLocaleString()}`}
              subTitle={`Đã chọn: ${hoaDonSelectedIds?.length ?? 0}`}
              data={dataTable}
              height={window.innerHeight - 100}
              isLoading={loading}
              // exportEnable

              // searchConfig={{
              //   enable: (hoaDonSelectedIds?.length ?? 0) <= 0,
              //   onValueChanged: (key: string) => {
              //     dispatch(
              //       hoaDonAction.changeFilter({
              //         ...filter,
              //         page_index: 0,
              //         search_key: key,
              //       })
              //     );
              //   },
              // }}
              sortConfig={{
                enable: false,
                field: filter.sort_by,
                mode: filter.sort_mode ?? eSortMode.ASC,
                onValueChanged: (key: string, sort_mode: eSortMode) => {
                  // dispatch(
                  //   hoaDonAction.changeFilter({
                  //     ...filter,
                  //     sort_by: key,
                  //     sort_mode: sort_mode,
                  //   })
                  // );
                },
              }}
              paging={{
                onPageIndexChanged: (pageIndex) => {
                  handleChangePage(pageIndex);
                },
                pageCount: pagination.totalPages,
                pageIndex: pagination.pageIndex - 1,
                pageSize: pagination.pageSize,
                totalCount: pagination.totalCount,

                // thêm 2 dòng này
                pageSizeItems: [10, 20, 50, 100, 200],
                onPageSizeChanged: (size) => {
                  setPagination(prev => ({
                    ...prev,
                    pageSize: size,
                    pageIndex: 1
                  }));

                  LayDanhSachChungTu({
                    loaiTimKiem: 0,
                    mau_so: dataFileter.mau_so,
                    ky_hieu: dataFileter.ky_hieu,
                    tu_ngay: dataFileter.tu_ngay,
                    den_ngay: dataFileter.den_ngay,
                    soct: "",
                    matracuu: "",
                    madonvi: user?.donvi?.ma_dv,
                    pageIndex: 1,
                    pageSize: size
                  });
                }
              }}
              selection={{
                mode: "multiple",
                keyExpr: "MaCT",
                selectedRowKeys: hoaDonSelectedIds,
                onSelectionChanged: (keys) => {
                  setHoaDonSelectedIds(keys);
                },
              }}
              columns={[
                {
                  id: "actions",
                  header: "",
                  width: "50px",
                  renderCell: (row: any) => {
                    return (
                      <>
                        <ActionMenu
                          open={
                            hoaDonActionMenuOpenId <= 0
                              ? undefined
                              : hoaDonActionMenuOpenId === row.id
                          }
                        >
                          <ActionMenu.Anchor>
                            <IconButton
                              icon={KebabHorizontalIcon}
                              aria-label="Open menu"
                              variant="invisible"
                            />
                          </ActionMenu.Anchor>
                          <ActionMenu.Overlay width="small">
                            <ActionList showDividers>
                              <ActionList.Group title="Xem">
                                <ActionList.Item
                                  // onSelect={() => {
                                  //     handlePrintAsync()
                                  // }}
                                  onClick={() => {
                                    setDataDetail(row);
                                    setXemChungTuInChuyenDoi(false);
                                    setOpenModalXemChungTu(true);
                                  }}
                                >
                                  <ActionList.LeadingVisual>
                                    <EyeIcon />
                                  </ActionList.LeadingVisual>
                                  Xem chứng từ
                                </ActionList.Item>
                                {((tab === "da-ky" && row?.TinhtrangCT === 2) ||
                                  (tab === "da-gui-cqt" &&
                                    [2, 3, 33].includes(row?.TinhtrangCT))) && (
                                  <ActionList.Item
                                    onClick={() => {
                                      setDataDetail(row);
                                      setXemChungTuInChuyenDoi(true);
                                      setOpenModalXemChungTu(true);
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <WorkflowIcon />
                                    </ActionList.LeadingVisual>
                                    {row?.TinhtrangCT === 3
                                      ? "In lại chuyển đổi"
                                      : "In chuyển đổi"}
                                  </ActionList.Item>
                                )}
                                {tab === "nhap" && (
                                  <>
                                    <ActionList.Item
                                      onSelect={() => {
                                        if (row?.TinhchatCT === 0) {
                                          history.push(
                                            `../../chung-tu/form/${row.MaCT}`
                                          );
                                        } else {
                                          Laythongtinchungtugoc(row);
                                        }
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <PencilIcon />
                                      </ActionList.LeadingVisual>
                                      Sửa chứng từ
                                    </ActionList.Item>

                                    {/* <ActionList.Item
                                      onSelect={() => {
                                        setDataDetail(row);
                                        handleKySo(row);
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <IssueClosedIcon />
                                      </ActionList.LeadingVisual>
                                      Ký số
                                    </ActionList.Item> */}
                                  </>
                                )}

                                {tab === "da-ky" && row?.TinhtrangCT === 2 && (
                                  <ActionList.Item
                                    onSelect={() => {
                                      // GuichungtulenCQT({
                                      //   machungtu: row?.MaCT,
                                      //   madonvi: user?.donvi_ma_dv,
                                      // });
                                      setDataDetail(row);
                                      setOpenGuiChungTuModal(true);
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <PaperAirplaneIcon />
                                    </ActionList.LeadingVisual>
                                    Gửi cơ quan thuế
                                  </ActionList.Item>
                                )}

                                {tab === "da-gui-cqt" && (
                                  <ActionList.Item
                                    onSelect={() => {
                                      setDataDetail(row);
                                      setOpenHistoryModal(true);
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <HistoryIcon />
                                    </ActionList.LeadingVisual>
                                    Nhật ký truyền nhận
                                  </ActionList.Item>
                                )}
                              </ActionList.Group>

                              {tab === "da-gui-cqt" && (
                                <>
                                  <ActionList.Divider />
                                  <ActionList.Group title="Điều chỉnh/ thay thế">
                                    <ActionList.Item
                                      onSelect={() => {
                                        handleLapChungTuThayTheDieuChinh(row, 2);
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitPullRequestIcon />
                                      </ActionList.LeadingVisual>
                                      Lập chứng từ điều chỉnh
                                    </ActionList.Item>
                                    <ActionList.Item
                                      onSelect={() => {
                                        handleLapChungTuThayTheDieuChinh(row, 1);
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitCompareIcon />
                                      </ActionList.LeadingVisual>
                                      Lập chứng từ thay thế
                                    </ActionList.Item>
                                    <ActionList.Item
                                      variant="danger"
                                      disabled
                                      onSelect={() => {
                                        history.push(
                                          `../../chung-tu/form/0?hinh_thuc_id=2&hoa_don_goc_id=${row.id}`
                                        );
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitPullRequestClosedIcon />
                                      </ActionList.LeadingVisual>
                                      Thông báo sai sót
                                    </ActionList.Item>
                                    <ActionList.Divider />

                                    <ActionList.Item
                                      onSelect={async () => {

                                        setDataDetail(row);
                                        await confirm({
                                          title: "Gửi email",
                                          content: "Bạn có chắc muốn gửi email chứng từ này?",
                                          confirmButtonContent: "Gửi",
                                          cancelButtonContent: "Huỷ",
                                        });

                                        await GuiMailChungTu(row.MaCT);
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <PaperAirplaneIcon />
                                      </ActionList.LeadingVisual>
                                      Gửi email
                                    </ActionList.Item>
                                  </ActionList.Group>
                                </>
                              )}
                              {tab === "nhap" && (
                                <>
                                  <ActionList.Divider />
                                  <ActionList.Item
                                    variant="danger"
                                    // disabled={isCanNotDelete}
                                    onSelect={() => {
                                      setDataDetail(row);
                                      setOpenConFirmDeleteModal(true);
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <TrashIcon />
                                    </ActionList.LeadingVisual>
                                    Xóa chứng từ nháp
                                  </ActionList.Item>
                                </>
                              )}
                              {tab === "da-ky" && (
                                <>
                                  <ActionList.Divider />
                                  <ActionList.Item
                                    variant="danger"
                                    // disabled={isCanNotDelete}
                                    onSelect={() => {
                                      setDataDetail(row);
                                      setOpenConFirmRemoveModal(true);
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <TrashIcon />
                                    </ActionList.LeadingVisual>
                                    Hủy nội bộ
                                  </ActionList.Item>
                                </>
                              )}
                            </ActionList>
                          </ActionMenu.Overlay>
                        </ActionMenu>
                      </>
                    );
                  },
                },
                // {
                //   header: "Ký hiệu",
                //   field: "hoa_don_dang_ky_phat_hanh_ky_hieu",
                //   rowHeader: true,
                //   width: "100px",
                //   renderCell: (data: IHoaDon) => {
                //     return (
                //       <Link to={`../../hoa-don/form/${data.id}`}>
                //         {data.hoa_don_dang_ky_phat_hanh_ky_hieu}
                //       </Link>
                //     );
                //   },
                //   // sortBy: "alphanumeric"
                // },

                {
                  header: "Mã CT",
                  field: "MaCT",
                  rowHeader: false,
                  width: "100px",
                },
                {
                  header: "Ngày lập",
                  field: "NgaylapCT",
                  rowHeader: false,
                  width: "100px",
                  renderCell: (cell: any) => {
                    return (
                      <Box>{moment(cell.NgaylapCT).format("DD/MM/YYYY")}</Box>
                    );
                  },
                  // sortBy: "alphanumeric"
                },
                {
                  header: "Số chứng từ",
                  field: "Sochungtu",
                  rowHeader: false,
                  width: "100px",
                },
                {
                  header: "Mã số thuế",
                  field: "MasothueNNT",
                  rowHeader: false,
                  width: "150px",
                },
                {
                  header: "Tên NNT",
                  field: "TenNNT",
                  rowHeader: false,
                  width: "200px",
                },
                {
                  header: "Số CMND/CCCD",
                  field: "SoCMND",
                  rowHeader: false,
                  width: "160px",
                },
                {
                  header: "Thuế TNCN",
                  field: "ThueTNCN",
                  rowHeader: false,
                  width: "100px",
                },
                // {
                //   header: "Mã tra cứu",
                //   field: "Matracuu",
                //   rowHeader: false,
                //   width: "100px",
                //   // sortBy: "alphanumeric"
                // },
                {
                  header: "Tính chất",
                  field: "PhanbietCT",
                  rowHeader: false,
                  width: "200px",
                  // sortBy: "alphanumeric"
                },
                {
                  header: "Trạng thái",
                  field: "TinhtrangCTText",
                  rowHeader: false,
                  width: "120px",
                  // sortBy: "alphanumeric"
                },

                {
                  header: "Trạng thái gửi CQT",
                  field: "TrangthaiguiCQT",
                  rowHeader: false,
                  // sortBy: "alphanumeric"
                },
                {
                  header: "Lý do",
                  field: "DSLDo",
                  rowHeader: false,
                  width: "200px",
                  // sortBy: "alphanumeric",
                  renderCell: (cell: any) => {
                    if (!cell?.DSLDo || cell?.DSLDo.trim() === "") {
                      return <></>; // Hoặc return null để không hiển thị gì
                    }
                    return (
                      <div
                        style={{ whiteSpace: "normal", cursor: "pointer" }}
                        onClick={() => {
                          const prettyXml = formatXml(cell?.DSLDo);

                          setXMLContent(prettyXml);
                          // 3. set state
                          setOpenXMLModal(true);
                        }}
                      >
                        <p
                          style={{
                            color: "blue",
                          }}
                        >
                          Xem lý do
                        </p>
                      </div>
                    );
                  },
                },
              ]}
            />
          </Box>
        </Box>
      )}
      {/* {isShowLogModal && (
        <HoaDonTimelineModal
          hoaDonId={hoaDonEditing?.id}
          onClose={() => {
            dispatch(rootAction.hoaDon.hoaDonAction.closeLogModal());
          }}
        />
      )} */}
      {openConFirmDeleteModal && (
        <ConfirmModal
          onCancel={() => {
            setOpenConFirmDeleteModal(false);
          }}
          type="danger"
          title="Xóa hóa đơn nháp"
          text="Bạn có chắc chắn muốn xóa hóa đơn này?"
          isSaving={status === eReducerStatusBase.is_deleting}
          onConfirm={() => {
            XoaNhapChungTu();
          }}
        />
      )}

      {openConFirmRemoveModal && (
        <ConfirmModal
          onCancel={() => {
            setOpenConFirmRemoveModal(false);
          }}
          type="danger"
          title="Hủy nội bộ"
          text="Bạn có chắc chắn muốn hủy chứng từ này?"
          isSaving={status === eReducerStatusBase.is_deleting}
          onConfirm={() => {
            HuyChungTu();
          }}
        />
      )}
      {/* {isShowKySoModal && (
        <KySoModal
          base64={base64KySo}
          onClose={() => {
            setIsShowKySoModal(false);
          }}
          onSuccess={(signedtext) => {
            // console.log(signedtext);

            setIsShowKySoModal(false);
            UpdateChungTuSauKy({
              xmldaky: signedtext,
              trangthai: 2,
              mst: user?.donvi_ma_dv,
              machungtu: dataDetail?.MaCT,
            });
          }}
        />
      )} */}

      {/* {isShowSendEmailConfirm &&
        hoaDonSelectedIds &&
        hoaDonSelectedIds.length === 1 && (
          <HoaDonSendEmailModal
            defaultEmail={selectedEmailAddress}
            id={hoaDonSelectedIds[0]}
            onClose={() => {
              setisShowSendEmailConfirm(false);
            }}
            onSuccess={() => {
              setisShowSendEmailConfirm(false);
            }}
          />
        )} */}

      {openXMLModal && (
        <Modal
          title="XML thông điệp"
          onClose={() => {
            setOpenXMLModal(false);
            // Đóng modal
          }}
          isOpen={openXMLModal}
          width="600px"
        >
          <Box
            padding={2}
            overflowY="auto"
            width={"800px"}
            maxHeight={window.innerHeight - 200}
            height={window.innerHeight - 200}
            fontFamily="monospace"
            fontSize={12}
            border="1px solid #ddd"
            borderRadius={4}
            bg="#f9f9f9"
          >
            <pre>{xmlContent}</pre>
          </Box>
        </Modal>
      )}

      {openModalXemChungTu && dataDetail && (
        <XemChungTu
          isOpen={openModalXemChungTu}
          onClose={() => {
            setOpenModalXemChungTu(false);
            setXemChungTuInChuyenDoi(false);
          }}
          machungtu={dataDetail?.MaCT}
          user={user}
          inChuyenDoi={xemChungTuInChuyenDoi}
          onInChuyenDoiApplied={() => {
            LayDanhSachChungTu({
              loaiTimKiem: 0,
              pageIndex: pagination.pageIndex,
              pageSize: pagination.pageSize,
            });
          }}
        />
      )}

      {openHistoryModal && dataDetail && (
        <NhatKyChungTuModal
          MaCT={dataDetail?.MaCT}
          onClose={() => {
            setOpenHistoryModal(false);
          }}
        />
      )}

      {openGuiChungTuModal && dataDetail && (
        <GuiChungTuLenCQTModal
          onClose={() => {
            setOpenGuiChungTuModal(false);
          }}
          GuichungtulenCQT={() => {
            GuichungtulenCQT({
              machungtu: dataDetail?.MaCT,
              madonvi: user?.donvi_ma_dv,
            });
          }}
          isSending={isSending}
        />
      )}
    </Box>
  );
};

export default QuanlychungtuPage;
