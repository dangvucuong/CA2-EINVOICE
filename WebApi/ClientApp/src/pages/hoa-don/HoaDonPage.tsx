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
  KebabHorizontalIcon,
  LockIcon,
  MailIcon,
  PaperAirplaneIcon,
  PencilIcon,
  PlusIcon,
  ShieldCheckIcon,
  ShieldIcon,
  ShieldSlashIcon,
  ShieldXIcon,
  TrashIcon,
} from "@primer/octicons-react";

import {
  ActionList,
  ActionMenu,
  Box,
  IconButton,
  Label,
  Link as LinkHref,
  UnderlineNav,
  useConfirm,
} from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useState } from "react";
import { Helmet } from "react-helmet";
import { Link, useHistory, useParams } from "react-router-dom";
import { HOA_DON_API, hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import ExportToExcelBtn from "../../component-data/export-excel-btn/ExportToExcelBtn";
import HoaDonHinhThuc from "../../component-data/hoa-don-hinh-thuc";
import HoaDonStatus from "../../component-data/hoa-don-status";
import { PrintHoaDonButtonActionListItem } from "../../component-data/print-hoa-don-button/PrintHoaDonButton";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import DataTableRemotePaging from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import HoaDonFilter from "./HoaDonFilter";
import HoaDonMayTinhTienFilter from "../hoa-don-may-tien-tien/HoaDonMayTinhTienFilter";
import HoaDonImportButton from "./HoaDonImportButton";
import HoaDonKySoPhatHanhMultiple from "./HoaDonKySoPhatHanhMultiple";
import HoaDonSendEmailModal from "./HoaDonSendEmailModal";
import HoaDonSort from "./HoaDonSort";
import { HoaDonTimelineModal } from "./HoaDonTimelineModal";
import { useWindowSize } from "../../hooks/useWindowSize";
import { useHoaDonTrangThaiAllReport } from "../../hooks/useHoaDonTrangThaiAllReport";
import ViewHoaDonButtonActionListItem from "../../component-data/view-hoa-don-modal";

const hoaDonAction = rootAction.hoaDon.hoaDonAction;

export interface IHoaDonPageProps {
  variant?: "default" | "mtt";
}

const HoaDonPage = ({ variant = "default" }: IHoaDonPageProps) => {
  const isMtt = variant === "mtt";
  const routeSlug = isMtt ? "hoa-don-mtt" : "hoa-don";
  const applyListFilter = <T extends object>(f: T): T =>
    isMtt ? { ...f, hoa_don_hinh_thuc_code: "M" } : f;

  const { tab }: any = useParams();

  const history = useHistory();
  const confirm = useConfirm();
  const { dataReport, handleSelectReport } = useHoaDonTrangThaiAllReport();

  const [isSaving, setIsSaving] = useState(false);
  const [hoaDonsDieuChinhThayThe, setHoaDonsDieuChinhThayThe] = useState<
    IHoaDon[]
  >([]);
  const [isShowSendEmailConfirm, setisShowSendEmailConfirm] = useState(false);
  const [hoaDonActionMenuOpenId, setHoaDonActionMenuOpenId] = useState(0);
  const { isMobile } = useWindowSize();
  const today = moment(new Date()).format("YYYY-MM-DD");

  const [choPhanHoiCount, setChoPhanHoiCount] = useState(0);
  const [chuaGuiCQTCount, setChuaGuiCQTCount] = useState(0);

  const {
    status,
    hoaDons,
    filter,
    paging_res,
    isShowDeleteConfirm,
    hoaDonEditing,
    isShowLogModal,
    hoaDonSelectedIds,
  } = useAppSelector((x) => x.hoaDon.hoaDonReducer);



  const hoaDonKhacNgay = hoaDons
    .filter((x) => (hoaDonSelectedIds ?? []).includes(x.id))
    .find((x) => moment(x.ngay_hoa_don).format("YYYY-MM-DD") !== today);

  // check xem các hóa đơn được chọn có cùng 1 ngày hay không
  const isHoaDonCungNgay = hoaDons
    .filter((x) => (hoaDonSelectedIds ?? []).includes(x.id))
    .every(
      (x) =>
        moment(x.ngay_hoa_don).format("YYYY-MM-DD") ===
        moment(
          hoaDons.find((y) => (hoaDonSelectedIds ?? []).includes(y.id))
            ?.ngay_hoa_don
        ).format("YYYY-MM-DD")
    );

  useEffect(() => {
    dispatch(
      hoaDonAction.changeFilter({
        hoa_don_trang_thai_ids: [],
        loai_hoa_don_ct_id: 0,
        hoa_don_dang_ky_phat_hanh_mau_so: "",
        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
        hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "ma_so_hoa_don",
        sort_mode: eSortMode.DESC,
      })
    );
  }, []);
  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const selectedEmailAddress = useMemo(() => {
    if (hoaDonSelectedIds && hoaDonSelectedIds?.length === 1) {
      // 
      const hoaDonSelectedId = hoaDonSelectedIds[0];
      const hoaDonSelected = hoaDons.find((x) => x.id === hoaDonSelectedId);
      if (hoaDonSelected) {
        return hoaDonSelected.nguoi_mua_email ?? "";
      }
    }
    return "";
  }, [hoaDonSelectedIds, hoaDons]);
  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(HOA_DON_API, "GET");
  }, []);

  const getHoaDonIdsDieuChinhThayThe = () => {
    let result: number[] = [];
    hoaDons.forEach((x) => {
      if (x.hoa_don_ids_thaythe_dieuchinh) {
        x.hoa_don_ids_thaythe_dieuchinh.split(",").forEach((k) => {
          if (k && k !== "") {
            const kId = parseInt(k);
            if (!isNaN(kId) && !result.includes(kId)) {
              result.push(kId);
            }
          }
        });
      }
    });
    return result.filter((x) => x > 0);
  };
  const hoa_don_ids_thaythe_dieuchinh = getHoaDonIdsDieuChinhThayThe();
  const hoa_don_ids_thaythe_dieuchinh_string = JSON.stringify(
    hoa_don_ids_thaythe_dieuchinh
  );
  useEffect(() => {
    const hoa_don_ids_thaythe_dieuchinh = JSON.parse(
      hoa_don_ids_thaythe_dieuchinh_string
    );
    if (hoa_don_ids_thaythe_dieuchinh.length > 0) {
      handleGetThongTinHoaDonDieuChinhThayTheAsync();
    } else {
      setHoaDonsDieuChinhThayThe([]);
    }
  }, [hoa_don_ids_thaythe_dieuchinh_string]);
  const handleGetThongTinHoaDonDieuChinhThayTheAsync = async () => {
    const res = await hoaDonApi.selectByIds({
      ids: hoa_don_ids_thaythe_dieuchinh,
    });
    if (res.is_success) {
      setHoaDonsDieuChinhThayThe(res.data);
    } else {
      setHoaDonsDieuChinhThayThe([]);
      NotifyHelper.Error("Không tải được thông tin hóa đơn điều chỉnh");
    }
  };

  useEffect(() => {
    if (tab === "nhap") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
            hoa_don_trang_thai_ids: [eHoaDonTrangThai.NHAP],
          })
        )
      );
    }
    if (!tab || tab === "da-phat-hanh") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
            hoa_don_trang_thai_ids: [
              eHoaDonTrangThai.DA_PHAT_HANH,
              eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
              eHoaDonTrangThai.DA_GUI_CQT_CHUA_PHAN_HOI,
            ],
          })
        )
      );
    }
    if (tab === "cho-phat-hanh") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_trang_thai_ids: [eHoaDonTrangThai.CHUA_GUI_CQT],
          })
        )
      );
    }
    if (tab === "chua-gui-cqt") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_trang_thai_ids: [],
          })
        )
      );
    }
    if (tab === "phat-hanh-loi") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
            hoa_don_trang_thai_ids: [
              eHoaDonTrangThai.DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT,
              eHoaDonTrangThai.CHUA_CO_KET_QUA_PHAN_HOI,
              eHoaDonTrangThai.KHONG_HOP_LE,
              eHoaDonTrangThai.LOI_THONG_DIEP,
            ],
          })
        )
      );
    }
    if (tab === "da-huy") {
      dispatch(
        hoaDonAction.changeFilter(
          applyListFilter({
            ...filter,
            hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
            hoa_don_trang_thai_ids: [eHoaDonTrangThai.DA_HUY],
          })
        )
      );
    }
  }, [tab]);




  useEffect(() => {
    if (filter.hoa_don_trang_thai_ids.length > 0 || tab === "chua-gui-cqt") {
      dispatch(
        hoaDonAction.loadStart({
          ...applyListFilter(filter),
          tab,
        })
      );
    }
  }, [filter]);

  useEffect(() => {
    if (tab === "cho-phat-hanh") {
      setChoPhanHoiCount(
        paging_res?.total_count ?? 0
      );
    }

    if (tab === "chua-gui-cqt") {
      setChuaGuiCQTCount(
        paging_res?.total_count ?? 0
      );
    }
  }, [paging_res?.total_count, tab]);



  useEffect(() => {
    if (
      status === eReducerStatusBase.is_saved ||
      status === eReducerStatusBase.is_deleted
    ) {
      if (filter.hoa_don_trang_thai_ids.length > 0 || tab === "chua-gui-cqt") {
        dispatch(
          hoaDonAction.loadStart({
            ...applyListFilter(filter),
            tab,
          })
        );
      }
    }
  }, [status, filter]);

  const handleSendEmail = async () => {
    if ((hoaDonSelectedIds?.length ?? 0) >= 2) {
      if (
        await confirm({
          content: "Bạn có chắc chắn muốn gửi email tới người mua",
          title: "Gửi Email",
          cancelButtonContent: "Đóng",
          confirmButtonContent: "Gửi email",
          confirmButtonType: "primary",
        })
      ) {
        setIsSaving(true);
        const res = await hoaDonApi.sendEmail({
          ids: hoaDonSelectedIds,
        });
        setIsSaving(false);
        if (res.is_success) {
          dispatch(hoaDonAction.changeSelectedId([]));
          NotifyHelper.Success("Success");
        } else {
          NotifyHelper.Error(res.message ?? "Error");
        }
      }
    } else {
      setisShowSendEmailConfirm(true);
    }
  };

  const handleDeletesHoaDon = async (
    isHuyNoiBo: boolean,
    hoaDonId?: number
  ) => {
    const hoaDonSelected = hoaDons.filter((x) =>
      hoaDonSelectedIds?.includes(x.id)
    );

    const checkHoaDonCoMa = hoaDonSelected.find(
      (x) => x?.ma_so_hoa_don && +x?.ma_so_hoa_don > 0
    );

    if (checkHoaDonCoMa && !isHuyNoiBo) {
      await confirm({
        content: `Không được xóa hóa đơn đã có số`,
        title: `Xóa hóa đơn`,
        //css cho thẻ cha của cancel btn display = none
        cancelButtonContent: "",
        confirmButtonContent: "Đóng",
      });
      return;
    }

    if (
      await confirm({
        content: `Bạn có chắc chắn muốn ${isHuyNoiBo ? "hủy nội bộ" : "xóa"} ${(hoaDonSelectedIds?.length ?? 0) > 0 ? "các" : ""
          } hóa đơn đã chọn`,
        title: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        cancelButtonContent: "Không",
        confirmButtonContent: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        confirmButtonType: "danger",
      })
    ) {
      setIsSaving(true);
      const res = await hoaDonApi.deletes({
        ids: hoaDonId ? [hoaDonId] : hoaDonSelectedIds ?? [],
      });
      setIsSaving(false);
      if (res.is_success) {
        dispatch(hoaDonAction.changeSelectedId([]));
        NotifyHelper.Success("Success");
        dispatch(
          hoaDonAction.loadStart({
            ...applyListFilter(filter),
            tab,
          })
        );
      } else {
        NotifyHelper.Error(res.message ?? "Error");
      }
    }
  };

  const handleCreateViewLink = async (id: number) => {
    const res = await hoaDonApi.createViewLink(id);
    if (res.is_success) {
      // navigator.clipboard.writeText(res.data);
      navigator.clipboard.writeText(res.data);
      NotifyHelper.Success("Đã copy link");
    }
  };

  return (
    <Box>
      <Helmet>
        <title>{isMtt ? "Hóa đơn máy tính tiền" : "Hóa đơn"}</title>
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
              <Heading
                text={
                  isMtt
                    ? "Danh sách hóa đơn máy tính tiền"
                    : "Danh sách hóa đơn"
                }
              />
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
              <HoaDonSort
                sortBy={{
                  field:
                    (filter.sort_by ?? "") !== ""
                      ? filter.sort_by ?? ""
                      : "ma_so_hoa_don",
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
              />
              <Box sx={{ display: "flex", gap: 1 }}>
                <Link to={`../../${routeSlug}/form/0`}>
                  <Button
                    text="Lập hóa đơn mới"
                    leadingVisual={PlusIcon}
                    variant="primary"
                    size="medium"
                  />
                </Link>
                <HoaDonImportButton
                  onSuccess={() => {
                    dispatch(
                      hoaDonAction.loadStart({
                        ...applyListFilter(filter),
                        tab,
                      })
                    );
                  }}
                />
              </Box>
            </Box>
          </Box>

          <Box id="tabs">
            <UnderlineNav aria-label="Repository">
              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/nhap`}
                aria-current={tab === "nhap" ? "page" : undefined}
                icon={ShieldIcon}
              >
                Hóa đơn nháp
              </UnderlineNav.Item>
              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/da-phat-hanh`}
                aria-current={
                  !tab || tab === "da-phat-hanh" ? "page" : undefined
                }
                icon={ShieldCheckIcon}
              >
                Đã phát hành
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/cho-phat-hanh`}
                aria-current={tab === "cho-phat-hanh" ? "page" : undefined}
                icon={ShieldIcon}
              >
                <Box sx={{ display: "flex", gap: 2 }}>
                  Chờ phản hồi CQT

                  <Label variant="danger">
                    {choPhanHoiCount}
                  </Label>
                </Box>
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/chua-gui-cqt`}
                aria-current={tab === "chua-gui-cqt" ? "page" : undefined}
                icon={ShieldIcon}
              >
                <Box sx={{ display: "flex", gap: 2 }}>
                  Chưa gửi được CQT

                  <Label variant="danger">
                    {chuaGuiCQTCount}
                  </Label>
                </Box>
              </UnderlineNav.Item>


              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/phat-hanh-loi`}
                aria-current={tab === "phat-hanh-loi" ? "page" : undefined}
                icon={ShieldSlashIcon}
              >
                Phát hành lỗi
              </UnderlineNav.Item>
              <UnderlineNav.Item
                as={Link}
                to={`../${routeSlug}/da-huy`}
                aria-current={tab === "da-huy" ? "page" : undefined}
                icon={ShieldXIcon}
              >
                Hóa đơn đã hủy
              </UnderlineNav.Item>
              {/* <UnderlineNav.Item>Hóa đơn đã phát hành</UnderlineNav.Item> */}
              {/* <UnderlineNav.Item>Hóa đơn đã hủy</UnderlineNav.Item> */}
            </UnderlineNav>
          </Box>

          <Box sx={{ mt: 3 }}>
            {isMobile && (
              <Box sx={{ display: "flex", gap: 1, flexWrap: "wrap", mb: 1 }}>
                {(hoaDonSelectedIds?.length ?? 0) <= 0 &&
                  (isMtt ? (
                    <HoaDonMayTinhTienFilter
                      filter={filter}
                      onChanged={(f) =>
                        dispatch(hoaDonAction.changeFilter(applyListFilter(f)))
                      }
                    />
                  ) : (
                    <HoaDonFilter />
                  ))}
                {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                  (!tab || tab === "da-phat-hanh") && (
                    <>
                      <Button
                        text="Gửi HĐ cho khách hàng"
                        leadingVisual={MailIcon}
                        variant="primary"
                        onClick={handleSendEmail}
                        isLoading={isSaving}
                      />
                    </>
                  )}

                {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                  (tab === "nhap") && (
                    <>
                      {(tab === "nhap") && (
                        <HoaDonKySoPhatHanhMultiple
                          ids={hoaDonSelectedIds ?? []}
                          isKhacNgay={hoaDonKhacNgay != undefined}
                          isHoaDonCungNgay={isHoaDonCungNgay}
                          onClose={() => {
                            dispatch(
                              hoaDonAction.loadStart({
                                ...applyListFilter(filter),
                                tab,
                              })
                            );
                          }}
                          title={
                            tab === "chua-gui-cqt"
                              ? "Gửi cấp mã Thuế"
                              : "Ký số và gửi cấp mã"
                          }
                        />
                      )}

                      <Button
                        text="Gửi HĐ nháp"
                        leadingVisual={MailIcon}
                        onClick={handleSendEmail}
                        isLoading={isSaving}
                      />
                    </>
                  )}
                {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                  (tab === "nhap" ||
                    tab === "cho-phat-hanh" ||
                    tab === "phat-hanh-loi") && (
                    <>
                      <Button
                        text={tab === "nhap" ? "Xóa hóa đơn" : "Hủy nội bộ"}
                        leadingVisual={TrashIcon}
                        variant="danger"
                        onClick={() => {
                          handleDeletesHoaDon(tab !== "nhap");
                        }}
                        isLoading={isSaving}
                      />
                    </>
                  )}
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
                        <ActionList.LinkItem
                          target="_blank"
                          // href={`../../../api/hoa-don/pdfs?hoaDonIds=${hoaDonSelectedIds?.join(
                          //   ","
                          // )}`}
                          href={`${process.env.REACT_APP_API_BASE_URL
                            }/hoa-don/pdfs?hoaDonIds=${hoaDonSelectedIds?.join(
                              ","
                            )}`}
                        >
                          <ActionList.LeadingVisual>
                            <FileIcon />
                          </ActionList.LeadingVisual>
                          Tải pdf
                        </ActionList.LinkItem>
                        {tab !== "nhap" && (
                          <ActionList.LinkItem
                            target="_blank"
                            // href={`../../../api/hoa-don/xmls?hoaDonIds=${hoaDonSelectedIds?.join(
                            //   ","
                            // )}`}
                            href={`${process.env.REACT_APP_API_BASE_URL
                              }/hoa-don/xmls?hoaDonIds=${hoaDonSelectedIds?.join(
                                ","
                              )}`}
                          >
                            <ActionList.LeadingVisual>
                              <FileCodeIcon />
                            </ActionList.LeadingVisual>
                            Tải xml gửi CQT
                          </ActionList.LinkItem>
                        )}
                      </ActionList>
                    </ActionMenu.Overlay>
                  </ActionMenu>
                )}
                <ExportToExcelBtn
                  fileName={isMtt ? "hoa-don-mtt" : "hoa-don"}
                  formatDataFunction={(data) => {
                    return data.map((x: IHoaDon) => {
                      return {
                        "Ký hiệu": x.hoa_don_dang_ky_phat_hanh_ky_hieu,
                        "Loại hóa đơn": x.ten_hoa_don,
                        "Ngày hóa đơn": moment(x.ngay_hoa_don).format(
                          "DD/MM/YYYY"
                        ),
                        "Mã số hóa đơn": x.ma_so_hoa_don,
                        "Người mua": x.nguoi_mua_ten_donvi,
                        MST: x.nguoi_mua_mst,
                        Email: x.nguoi_mua_email,
                        "Tổng tiền": x.tong_tien_thanh_toan,
                        "Mã tra cứu": x.ma_tra_cuu,
                        "Kết quả": x.ket_qua_phat_hanh,
                        "Mã CQT cấp": `${x.phat_hanh_ma_ketqua_cqt} ${x.ma_so_hoa_don_mtt ?? ""
                          }`,
                      };
                    });
                  }}
                  fetchDataPromise={() => {
                    return new Promise((resolve, reject) => {
                      return hoaDonApi
                        .selectByDonViPaging({
                          ...filter,
                          page_index: 0,
                          page_size: paging_res?.total_count,
                        })
                        .then((res) => {
                          if (res.is_success) {
                            resolve(res.data.data);
                          } else {
                            NotifyHelper.Error(res.message ?? "Error");
                            resolve(undefined);
                          }
                        });
                    });
                  }}
                />
              </Box>
            )}
            <DataTableRemotePaging
              title={`Tổng số: ${(
                paging_res?.total_count ?? 0
              ).toLocaleString()}`}
              subTitle={`Đã chọn: ${hoaDonSelectedIds?.length ?? 0}`}
              data={hoaDons}
              height={window.innerHeight - 100}
              isLoading={status === eReducerStatusBase.is_loading}
              // exportEnable
              actionComponent={
                <>
                  {!isMobile && (
                    <Box sx={{ display: "flex", gap: 1 }}>
                      {(hoaDonSelectedIds?.length ?? 0) <= 0 &&
                        (isMtt ? (
                          <HoaDonMayTinhTienFilter
                            filter={filter}
                            onChanged={(f) =>
                              dispatch(
                                hoaDonAction.changeFilter(applyListFilter(f))
                              )
                            }
                          />
                        ) : (
                          <HoaDonFilter />
                        ))}
                      {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                        (!tab || tab === "da-phat-hanh") && (
                          <>
                            <Button
                              text="Gửi HĐ cho khách hàng"
                              leadingVisual={MailIcon}
                              variant="primary"
                              onClick={handleSendEmail}
                              isLoading={isSaving}
                            />
                          </>
                        )}
                      {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                        (tab === "nhap") && (
                          <>
                            {(tab === "nhap") && (
                              <HoaDonKySoPhatHanhMultiple
                                ids={hoaDonSelectedIds ?? []}
                                isKhacNgay={hoaDonKhacNgay != undefined}
                                isHoaDonCungNgay={isHoaDonCungNgay}
                                onClose={() => {
                                  dispatch(
                                    hoaDonAction.loadStart({
                                      ...applyListFilter(filter),
                                      tab,
                                    })
                                  );
                                }}
                                title={
                                  tab === "chua-gui-cqt"
                                    ? "Gửi cấp mã Thuế"
                                    : "Ký số và gửi cấp mã"
                                }
                              />
                            )}

                            <Button
                              text="Gửi HĐ nháp"
                              leadingVisual={MailIcon}
                              onClick={handleSendEmail}
                              isLoading={isSaving}
                            />
                          </>
                        )}
                      {(hoaDonSelectedIds?.length ?? 0) > 0 &&
                        (tab === "nhap" ||
                          tab === "cho-phat-hanh" ||
                          tab === "phat-hanh-loi") && (
                          <>
                            <Button
                              text={
                                tab === "nhap" ? "Xóa hóa đơn" : "Hủy nội bộ"
                              }
                              leadingVisual={TrashIcon}
                              variant="danger"
                              onClick={() => {
                                handleDeletesHoaDon(tab !== "nhap");
                              }}
                              isLoading={isSaving}
                            />
                          </>
                        )}
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
                                // target="_blank"
                                // href={`../../../api/hoa-don/pdfs?hoaDonIds=${hoaDonSelectedIds?.join(
                                //   ","
                                // )}`}
                                onSelect={() => {
                                  const ids = hoaDonSelectedIds ?? [];
                                  if (!ids || ids.length === 0) return;
                                  if (ids.length > 20) {
                                    NotifyHelper.Error(
                                      "Không được tải xuống quá 20 hóa đơn cùng lúc"
                                    );
                                    return;
                                  }
                                  const url = `${process.env.REACT_APP_API_BASE_URL
                                    }/hoa-don/pdfs?hoaDonIds=${ids.join(",")}`;
                                  window.open(url, "_blank");
                                }}
                              >
                                <ActionList.LeadingVisual>
                                  <FileIcon />
                                </ActionList.LeadingVisual>
                                Tải pdf
                              </ActionList.Item>
                              {tab !== "nhap" && (
                                <ActionList.LinkItem
                                  target="_blank"
                                  href={`${process.env.REACT_APP_API_BASE_URL
                                    }/hoa-don/xmls?hoaDonIds=${hoaDonSelectedIds?.join(
                                      ","
                                    )}`}
                                >
                                  <ActionList.LeadingVisual>
                                    <FileCodeIcon />
                                  </ActionList.LeadingVisual>
                                  Tải xml gửi CQT
                                </ActionList.LinkItem>
                              )}
                            </ActionList>
                          </ActionMenu.Overlay>
                        </ActionMenu>
                      )}
                      <ExportToExcelBtn
                        fileName={isMtt ? "hoa-don-mtt" : "hoa-don"}
                        formatDataFunction={(data) => {
                          return data.map((x: IHoaDon) => {
                            return {
                              "Ký hiệu": x.hoa_don_dang_ky_phat_hanh_ky_hieu,
                              "Loại hóa đơn": x.ten_hoa_don,
                              "Ngày hóa đơn": moment(x.ngay_hoa_don).format(
                                "DD/MM/YYYY"
                              ),
                              "Mã số hóa đơn": x.ma_so_hoa_don,
                              "Người mua": x.nguoi_mua_ten_donvi,
                              MST: x.nguoi_mua_mst,
                              Email: x.nguoi_mua_email,
                              "Tổng tiền": x.tong_tien_thanh_toan,
                              "Mã tra cứu": x.ma_tra_cuu,
                              "Kết quả": x.ket_qua_phat_hanh,
                              "Mã CQT cấp": `${x.phat_hanh_ma_ketqua_cqt} ${x.ma_so_hoa_don_mtt ?? ""
                                }`,
                            };
                          });
                        }}
                        fetchDataPromise={() => {
                          return new Promise((resolve, reject) => {
                            return hoaDonApi
                              .selectByDonViPaging({
                                ...filter,
                                page_index: 0,
                                page_size: paging_res?.total_count,
                              })
                              .then((res) => {
                                if (res.is_success) {
                                  resolve(res.data.data);
                                } else {
                                  NotifyHelper.Error(res.message ?? "Error");
                                  resolve(undefined);
                                }
                              });
                          });
                        }}
                      />
                    </Box>
                  )}
                </>
              }
              searchConfig={{
                enable: (hoaDonSelectedIds?.length ?? 0) <= 0,
                onValueChanged: (key: string) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      page_index: 0,
                      search_key: key,
                    })
                  );
                },
              }}
              sortConfig={{
                enable: false,
                field: filter.sort_by,
                mode: filter.sort_mode ?? eSortMode.DESC,
                onValueChanged: (key: string, sort_mode: eSortMode) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      sort_by: key,
                      sort_mode: sort_mode,
                    })
                  );
                },
              }}
              paging={{
                onPageIndexChanged: (pageIndex) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      page_index: pageIndex,
                    })
                  );
                },
                pageCount: paging_res?.page_count ?? 1,
                pageIndex: paging_res?.page_number ?? 1,
                pageSize: paging_res?.page_size ?? 1,
                totalCount: paging_res?.total_count ?? 1,
                pageSizeItems: [10, 20, 50, 100, 200],
                onPageSizeChanged: (size) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      page_size: size,
                      page_index: 0,
                    })
                  );
                },
              }}
              selection={
                !tab ||
                  tab === "da-phat-hanh" ||
                  tab === "nhap" ||
                  tab === "cho-phat-hanh" ||
                  tab === "phat-hanh-loi"
                  ? {
                    mode: "multiple",
                    keyExpr: "id",
                    selectedRowKeys: hoaDonSelectedIds,
                    onSelectionChanged: (keys) => {
                      dispatch(hoaDonAction.changeSelectedId(keys));
                    },
                  }
                  : undefined
              }
              columns={[
                {
                  header: "Id",
                  field: "id",
                  rowHeader: false,
                  width: "50px",
                  // header: <Box>123</Box>
                  // sortBy: "alphanumeric"
                },
                {
                  id: "xem",
                  header: "Xem",
                  width: "50px",
                  renderCell: (row: any) => {
                    return (
                      <ViewHoaDonButtonActionListItem
                        id={row.id}
                        showText={false}
                        hinhThucHoaDonId={row.hoa_don_hinh_thuc_id}
                      />
                    );
                  },
                },
                {
                  id: "edit",
                  header: "Sửa",
                  width: "50px",
                  renderCell: (row: any) => {
                    return (
                      <>
                        {tab === "nhap" ? (
                          <IconButton
                            as={LinkHref}
                            href={`../../${routeSlug}/form/${row.id}`}
                            icon={PencilIcon}
                            aria-label="Edit"
                            variant="invisible"
                            size="small"
                          />
                        ) : (
                          <LockIcon />
                        )}
                      </>
                    );
                  },
                },
                {
                  id: "delete",
                  header: "Xóa",
                  width: "50px",
                  renderCell: (row: any) => {
                    return (
                      <>
                        {tab === "nhap" && row?.ma_so_hoa_don <= 0 ? (
                          <IconButton
                            icon={TrashIcon}
                            aria-label="Delete"
                            variant="invisible"
                            size="small"
                            onClick={() => {
                              if (
                                row?.hoa_don_trang_thai_id ===
                                eHoaDonTrangThai.NHAP &&
                                row?.ma_so_hoa_don > 0
                              ) {
                                handleDeletesHoaDon(true, row.id);
                              } else {
                                dispatch(
                                  rootAction.hoaDon.hoaDonAction.showDeleteConfirm(
                                    row
                                  )
                                );
                              }
                            }}
                          />
                        ) : (
                          <Box
                            sx={{
                              display: "flex",
                              justifyContent: "center",
                              width: "100%",
                            }}
                          >
                            <LockIcon />
                          </Box>
                        )}
                      </>
                    );
                  },
                },
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
                                <PrintHoaDonButtonActionListItem
                                  id={row.id}
                                  onClose={() => {
                                    setHoaDonActionMenuOpenId(0);
                                  }}
                                  onOpenedModal={() => {
                                    setHoaDonActionMenuOpenId(row.id);
                                  }}
                                  hoa_don_hinh_thuc_id={
                                    row.hoa_don_hinh_thuc_id
                                  }
                                />
                                {tab === "nhap" && (
                                  <ActionList.Item
                                    onSelect={() => {
                                      history.push(
                                        `../../${routeSlug}/form/${row.id}`
                                      );
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <PencilIcon />
                                    </ActionList.LeadingVisual>
                                    Sửa hóa đơn
                                  </ActionList.Item>
                                )}
                                <ActionList.Item
                                  onSelect={() => {
                                    dispatch(
                                      rootAction.hoaDon.hoaDonAction.showLogModal(
                                        row
                                      )
                                    );
                                  }}
                                >
                                  <ActionList.LeadingVisual>
                                    <HistoryIcon />
                                  </ActionList.LeadingVisual>
                                  Lịch sử hóa đơn
                                </ActionList.Item>
                                <ActionList.Item
                                  onSelect={() => {
                                    handleCreateViewLink(row.id);
                                  }}
                                >
                                  <ActionList.LeadingVisual>
                                    <CopyIcon />
                                  </ActionList.LeadingVisual>
                                  Sao chép link xem hóa đơn
                                </ActionList.Item>
                                <ActionList.Item
                                  onSelect={() => {
                                    history.push(
                                      `../../${routeSlug}/form/0?copy_id=${row.id}`
                                    );
                                  }}
                                >
                                  <ActionList.LeadingVisual>
                                    <CopyIcon />
                                  </ActionList.LeadingVisual>
                                  Sao chép sang hóa đơn mới
                                </ActionList.Item>
                              </ActionList.Group>

                              {(!tab || tab === "da-phat-hanh") && (
                                <>
                                  <ActionList.Divider />
                                  <ActionList.Group title="Điều chỉnh/ thay thế">
                                    <ActionList.Item
                                      disabled={
                                        // hoa_don_id_goc ==0
                                        row.hoa_don_hinh_thuc_id === 2 ||
                                        row.hoa_don_hinh_thuc_id === 3 ||
                                        row.hoa_don_hinh_thuc_id === 6
                                      }
                                      // variant="danger"
                                      onSelect={() => {
                                        history.push(
                                          `../../${routeSlug}/form/0?hinh_thuc_id=3&hoa_don_goc_id=${row.id}`
                                        );
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitPullRequestIcon />
                                      </ActionList.LeadingVisual>
                                      Lập hóa đơn điều chỉnh
                                    </ActionList.Item>
                                    <ActionList.Item
                                      disabled={
                                        row.hoa_don_hinh_thuc_id === 3 ||
                                        row.hoa_don_hinh_thuc_id === 4 ||
                                        row.hoa_don_hinh_thuc_id === 6
                                      }
                                      onSelect={() => {
                                        history.push(
                                          `../../${routeSlug}/form/0?hinh_thuc_id=2&hoa_don_goc_id=${row.id}`
                                        );
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitCompareIcon />
                                      </ActionList.LeadingVisual>
                                      Lập hóa đơn thay thế
                                    </ActionList.Item>
                                    <ActionList.Item
                                      variant="danger"
                                      disabled={
                                        row.hoa_don_hinh_thuc_id === 0 &&
                                        row.phat_hanh_ma_ketqua_cqt
                                      }
                                      onSelect={() => {
                                        history.push(`../../tbss/0`, {
                                          hoa_don: {
                                            hoa_don_dang_ky_phat_hanh_mau_so:
                                              row.hoa_don_dang_ky_phat_hanh_mau_so,
                                            hoa_don_dang_ky_phat_hanh_ky_hieu:
                                              row.hoa_don_dang_ky_phat_hanh_ky_hieu,
                                            ma_so_hoa_don: row.ma_so_hoa_don,
                                            ma_cqt_cap:
                                              row.phat_hanh_ma_ketqua_cqt,
                                            ngay_hoa_don: moment(
                                              row.ngay_hoa_don
                                            ).format("DD/MM/YYYY"),
                                          },
                                        });
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitPullRequestClosedIcon />
                                      </ActionList.LeadingVisual>
                                      Thông báo sai sót
                                    </ActionList.Item>
                                    <ActionList.Divider />
                                    <ActionList.Item
                                      // variant="danger"
                                      onSelect={() => {
                                        dispatch(
                                          hoaDonAction.changeSelectedId([
                                            row.id,
                                          ])
                                        );
                                        setisShowSendEmailConfirm(true);
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <PaperAirplaneIcon />
                                      </ActionList.LeadingVisual>
                                      Gửi email
                                    </ActionList.Item>
                                    {/* <ActionList.Item
                                                                            // variant="danger"
                                                                            onSelect={() => {
                                                                                history.push(`../../${routeSlug}/form/${row.id}`)
                                                                            }}>
                                                                            <ActionList.LeadingVisual>
                                                                                <WorkflowIcon />
                                                                            </ActionList.LeadingVisual>
                                                                            In chuyển đổi
                                                                        </ActionList.Item> */}
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
                                      if (
                                        row?.hoa_don_trang_thai_id ===
                                        eHoaDonTrangThai.NHAP &&
                                        row?.ma_so_hoa_don > 0
                                      ) {
                                        handleDeletesHoaDon(true, row.id);
                                      } else {
                                        dispatch(
                                          rootAction.hoaDon.hoaDonAction.showDeleteConfirm(
                                            row
                                          )
                                        );
                                      }
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <TrashIcon />
                                    </ActionList.LeadingVisual>

                                    {row?.hoa_don_trang_thai_id ===
                                      eHoaDonTrangThai.NHAP &&
                                      row?.ma_so_hoa_don > 0
                                      ? "Hủy nội bộ"
                                      : "Xóa hóa đơn nháp"}
                                  </ActionList.Item>

                                </>
                              )}
                              {(tab === "cho-phat-hanh" ||
                                tab === "phat-hanh-loi" || tab === "chua-gui-cqt") && (
                                  <>
                                    <ActionList.Divider />

                                    {tab === "chua-gui-cqt" && (
                                      <>
                                        <ActionList.Divider />

                                        <ActionList.Item
                                          onSelect={async () => {

                                            if (
                                              !(await confirm({
                                                title: "Gửi lại CQT",
                                                content:
                                                  "Bạn có chắc chắn muốn gửi lại hóa đơn lên CQT?",
                                                confirmButtonContent: "Gửi lại",
                                                cancelButtonContent: "Đóng",
                                                confirmButtonType: "primary",
                                              }))
                                            ) {
                                              return;
                                            }

                                            setIsSaving(true);

                                            try {
                                              const res = await hoaDonApi.guiLaiCQT(
                                                row.id
                                              );

                                              if (res.is_success) {
                                                NotifyHelper.Success(
                                                  "Gửi lại CQT thành công"
                                                );

                                                dispatch(
                                                  hoaDonAction.loadStart({
                                                    ...applyListFilter(filter),
                                                    tab,
                                                  })
                                                );
                                              } else {
                                                NotifyHelper.Error(
                                                  res.message ?? "Gửi lại CQT thất bại"
                                                );
                                              }
                                            } finally {
                                              setIsSaving(false);
                                            }
                                          }}
                                        >
                                          <ActionList.LeadingVisual>
                                            <PaperAirplaneIcon />
                                          </ActionList.LeadingVisual>

                                          Gửi lại CQT
                                        </ActionList.Item>
                                      </>
                                    )}


                                    <ActionList.Item
                                      variant="danger"
                                      // disabled={isCanNotDelete}
                                      onSelect={() => {
                                        handleDeletesHoaDon(true, row.id);
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
                {
                  header: "Ký hiệu",
                  field: "hoa_don_dang_ky_phat_hanh_ky_hieu",
                  rowHeader: true,
                  width: "100px",
                  renderCell: (data: IHoaDon) => {
                    return (
                      <Link to={`../../${routeSlug}/form/${data.id}`}>
                        {data.hoa_don_dang_ky_phat_hanh_ky_hieu}
                      </Link>
                    );
                  },
                  // sortBy: "alphanumeric"
                },

                {
                  header: "Loại HĐ",
                  field: "ten_hoa_don",
                  rowHeader: false,
                  minWidth: "200px",
                },
                {
                  header: "Ngày HĐ",
                  field: "ngay_hoa_don",
                  rowHeader: false,
                  width: "100px",
                  renderCell: (cell: IHoaDon) => {
                    return (
                      <Box>
                        {moment(cell.ngay_hoa_don).format("DD/MM/YYYY")}
                      </Box>
                    );
                  },
                  // sortBy: "alphanumeric"
                },
                {
                  header: "Số HĐ",
                  field: "ma_so_hoa_don",
                  rowHeader: false,
                  width: "100px",
                },
                {
                  header: "Hình thức",
                  field: "hoa_don_hinh_thuc_id",
                  rowHeader: false,
                  width: "200px",
                  renderCell: (cell: IHoaDon) => {
                    return (
                      <>
                        <HoaDonHinhThuc id={cell.hoa_don_hinh_thuc_id} />
                      </>
                    );
                  },
                },

                {
                  header: "Người mua",
                  field: "nguoi_mua_ten_donvi",
                  rowHeader: false,
                  minWidth: "300px",
                  renderCell: (cell: IHoaDon) => {
                    return (
                      <Box
                        sx={{
                          display: "flex",
                          flexDirection: "column",
                        }}
                      >
                        <Box>{cell.nguoi_mua_ten_donvi}</Box>

                        <Box
                          sx={{
                            fontSize: "12px",
                            color: "fg.muted",
                          }}
                        >
                          {cell.nguoi_mua_mst
                            ? cell.nguoi_mua_mst
                            : cell.nguoi_mua_ten}{" "}
                          &nbsp; {cell.nguoi_mua_email}
                        </Box>
                        {(cell.ten_dai_ly || cell.ma_dai_ly) && (
                          <Box
                            sx={{
                              fontSize: "12px",
                              color: "fg.muted",
                            }}
                          >
                            Đại lý: {cell.ma_dai_ly ?? ""} -{" "}
                            {cell.ten_dai_ly ?? ""}
                          </Box>
                        )}
                      </Box>
                    );
                  },
                },

                {
                  header: "Nội dung phát hành",
                  field: "ket_qua_phat_hanh",
                  rowHeader: false,
                  width: "300px",
                  renderCell: (data: IHoaDon) => {
                    const _hoaDonThayTheDieuChinhIds =
                      data.hoa_don_ids_thaythe_dieuchinh
                        ? data.hoa_don_ids_thaythe_dieuchinh.split(",")
                        : [];
                    const _hoaDonThayTheDieuChinhs =
                      hoaDonsDieuChinhThayThe.filter((x) =>
                        _hoaDonThayTheDieuChinhIds.includes(x.id.toString())
                      );
                    return (
                      // <Box className="limit1Line">
                      <Box>
                        {tab === "da-phat-hanh" &&
                          (data.hoa_don_trang_thai_id ===
                            eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU ||
                            data.hoa_don_trang_thai_id ===
                            eHoaDonTrangThai.DA_GUI_CQT_CHUA_PHAN_HOI) && (
                            <Box>
                              Chưa có phản hồi kết quả kiểm tra dữ liệu từ CQT
                            </Box>
                          )}
                        {tab === "phat-hanh-loi" && (
                          <HoaDonStatus id={data.hoa_don_trang_thai_id} />
                        )}
                        {/* {tab === "da-huy" && (
                          <HoaDonHinhThuc id={data.hoa_don_hinh_thuc_id} />
                        )} */}
                        {(data.phat_hanh_ma_ketqua_cqt ||
                          data.ma_so_hoa_don_mtt) && (
                            <Box
                              sx={{
                                display: "flex",
                                flexWrap: "wrap",
                              }}
                            >
                              <b>Mã CQT: </b>{" "}
                              <Box sx={{ color: "green" }}>
                                {data.phat_hanh_ma_ketqua_cqt}{" "}
                                {data.ma_so_hoa_don_mtt ?? ""}
                              </Box>
                            </Box>
                          )}
                        {data.ket_qua_phat_hanh}
                        {_hoaDonThayTheDieuChinhs.length > 0 && (
                          <Box sx={{ mt: 1 }}>
                            {_hoaDonThayTheDieuChinhs.map((x) => {
                              return (
                                <Box>
                                  <LinkHref href={`../../${routeSlug}/form/${x.id}`}>
                                    <Box>
                                      {x.hoa_don_hinh_thuc_id === 2
                                        ? "Thay thế bởi"
                                        : ""}
                                      {x.hoa_don_hinh_thuc_id === 3
                                        ? "Điều chỉnh bởi"
                                        : ""}
                                      &nbsp;
                                      {x.hoa_don_dang_ky_phat_hanh_mau_so}
                                      {x.hoa_don_dang_ky_phat_hanh_ky_hieu}_
                                      {x.ma_so_hoa_don}
                                    </Box>
                                  </LinkHref>
                                </Box>
                              );
                            })}
                          </Box>
                        )}
                      </Box>
                    );
                  },
                },
                {
                  header: "Tổng tiền",
                  field: "tong_tien_thanh_toan",
                  rowHeader: false,
                  width: "100px",
                  renderCell: (cell: IHoaDon) => {
                    return (
                      <Box
                        sx={{
                          display: "flex",
                          flexDirection: "column",
                        }}
                      >
                        <Box>
                          <b>{cell.tong_tien_thanh_toan.toLocaleString()}</b>
                        </Box>
                      </Box>
                    );
                  },
                  // sortBy: "alphanumeric"
                },
                {
                  header: "Mã tra cứu",
                  field: "ma_tra_cuu",
                  rowHeader: false,
                  width: "150px",
                  // sortBy: "alphanumeric"
                },
              ]}
            />
          </Box>
        </Box>
      )}
      {isShowLogModal && hoaDonEditing && (
        <HoaDonTimelineModal
          hoaDonId={hoaDonEditing?.id}
          onClose={() => {
            dispatch(rootAction.hoaDon.hoaDonAction.closeLogModal());
          }}
        />
      )}
      {isShowDeleteConfirm && hoaDonEditing && (
        <ConfirmModal
          onCancel={() => {
            dispatch(rootAction.hoaDon.hoaDonAction.closeDeleteConfirm());
          }}
          type="danger"
          title="Xóa hóa đơn nháp"
          text="Bạn có chắc chắn muốn xóa hóa đơn này?"
          isSaving={status === eReducerStatusBase.is_deleting}
          onConfirm={() => {
            dispatch(
              rootAction.hoaDon.hoaDonAction.deleteStart(hoaDonEditing?.id ?? 0)
            );
          }}
        />
      )}
      {isShowSendEmailConfirm &&
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
        )}
    </Box>
  );
};

export default HoaDonPage;
