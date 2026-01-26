import {
  CopyIcon,
  EyeIcon,
  FileIcon,
  GitCompareIcon,
  GitPullRequestClosedIcon,
  GitPullRequestIcon,
  HistoryIcon,
  KebabHorizontalIcon,
  MailIcon,
  PaperAirplaneIcon,
  PlusIcon,
  ShieldCheckIcon,
  ShieldIcon,
  ShieldSlashIcon,
  ShieldXIcon,
  TrashIcon,
  FileCodeIcon,
  DownloadIcon,
  LockIcon,
  PencilIcon,
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
import { useEffect, useMemo, useState } from "react";
import { Helmet } from "react-helmet";
import { Link, useHistory, useParams } from "react-router-dom";
import { HANG_HOA_API_ENDPOINT } from "../../api/category/hangHoaApi";
import Button from "../../component-ui/button";
import DataTableRemotePaging from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";

import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import ExportToExcelBtn from "../../component-data/export-excel-btn/ExportToExcelBtn";
import ConfirmModal from "../../component-ui/confirm-modal";
import { NotifyHelper } from "../../helpers/toast";
import HoaDonImportButton from "../hoa-don/HoaDonImportButton";
import HoaDonKySoPhatHanhMultiple from "../hoa-don/HoaDonKySoPhatHanhMultiple";
import HoaDonSendEmailModal from "../hoa-don/HoaDonSendEmailModal";
import { HoaDonTimelineModal } from "../hoa-don/HoaDonTimelineModal";
import HoaDonMayTinhTienFilter from "./HoaDonMayTinhTienFilter";
import HoaDonSort from "../hoa-don/HoaDonSort";
import { PrintHoaDonButtonActionListItem } from "../../component-data/print-hoa-don-button/PrintHoaDonButton";
const hoaDonAction = rootAction.hoaDon.hoaDonAction;
const HoaDonPage = () => {
  const { tab }: any = useParams();
  const history = useHistory();
  const confirm = useConfirm();
  const [isSaving, setIsSaving] = useState(false);

  const [isShowSendEmailConfirm, setisShowSendEmailConfirm] = useState(false);

  const {
    status,
    hoaDons,
    paging_res,
    isShowDeleteConfirm,
    hoaDonEditing,
    isShowLogModal,
    filter,
    hoaDonSelectedIds,
  } = useAppSelector((x) => x.hoaDon.hoaDonReducer);
  useEffect(() => {
    dispatch(
      hoaDonAction.changeFilter({
        hoa_don_trang_thai_ids: [],
        loai_hoa_don_ct_id: 0,
        hoa_don_dang_ky_phat_hanh_mau_so: "",
        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
        hoa_don_hinh_thuc_code: "M",
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC,
      }),
    );
  }, []);
  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT, "GET");
  }, []);
  const isCanNotEdit = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT, "PUT");
  }, []);
  const isCanNotDelete = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT + "/{id}", "DELETE");
  }, []);
  useEffect(() => {
    if (tab === "nhap") {
      dispatch(
        hoaDonAction.changeFilter({
          ...filter,
          hoa_don_hinh_thuc_code: "M",
          hoa_don_trang_thai_ids: [eHoaDonTrangThai.NHAP],
        }),
      );
    }
    if (!tab || tab === "da-phat-hanh") {
      dispatch(
        hoaDonAction.changeFilter({
          ...filter,
          hoa_don_hinh_thuc_code: "M",
          hoa_don_trang_thai_ids: [
            eHoaDonTrangThai.DA_PHAT_HANH,
            eHoaDonTrangThai.DA_GUI_CQT_CHUA_PHAN_HOI,
          ],
        }),
      );
    }
    if (tab === "cho-phat-hanh") {
      dispatch(
        hoaDonAction.changeFilter({
          ...filter,
          hoa_don_hinh_thuc_code: "M",
          hoa_don_trang_thai_ids: [eHoaDonTrangThai.CHUA_GUI_CQT],
        }),
      );
    }
    if (tab === "phat-hanh-loi") {
      dispatch(
        hoaDonAction.changeFilter({
          ...filter,
          hoa_don_hinh_thuc_code: "M",
          hoa_don_trang_thai_ids: [
            eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
            eHoaDonTrangThai.DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT,
            eHoaDonTrangThai.CHUA_CO_KET_QUA_PHAN_HOI,
            eHoaDonTrangThai.KHONG_HOP_LE,
            eHoaDonTrangThai.LOI_THONG_DIEP,
          ],
        }),
      );
    }
    if (tab === "da-huy") {
      dispatch(
        hoaDonAction.changeFilter({
          ...filter,
          hoa_don_hinh_thuc_code: "M",
          hoa_don_trang_thai_ids: [eHoaDonTrangThai.DA_HUY],
        }),
      );
    }
  }, [tab]);
  useEffect(() => {
    if (filter.hoa_don_trang_thai_ids.length > 0) {
      dispatch(
        hoaDonAction.loadStart({
          ...filter,
        }),
      );
    }
  }, [filter]);
  useEffect(() => {
    if (
      status === eReducerStatusBase.is_saved ||
      status === eReducerStatusBase.is_deleted
    ) {
      if (filter.hoa_don_trang_thai_ids.length > 0) {
        dispatch(
          hoaDonAction.loadStart({
            ...filter,
          }),
        );
      }
    }
  }, [status, filter]);
  const selectedEmailAddress = useMemo(() => {
    if (hoaDonSelectedIds && hoaDonSelectedIds?.length === 1) {
      // debugger
      const hoaDonSelectedId = hoaDonSelectedIds[0];
      const hoaDonSelected = hoaDons.find((x) => x.id === hoaDonSelectedId);
      if (hoaDonSelected) {
        return hoaDonSelected.nguoi_mua_email ?? "";
      }
    }
    return "";
  }, [hoaDonSelectedIds, hoaDons]);
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
    hoaDonId?: number,
  ) => {
    if (
      await confirm({
        content: `Bạn có chắc chắn muốn ${isHuyNoiBo ? "hủy nội bộ" : "xóa"} ${
          (hoaDonSelectedIds?.length ?? 0) > 0 ? "các" : ""
        } hóa đơn đã chọn`,
        title: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        cancelButtonContent: "Không",
        confirmButtonContent: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        confirmButtonType: "danger",
      })
    ) {
      setIsSaving(true);
      const res = await hoaDonApi.deletes({
        ids: hoaDonId ? [hoaDonId] : (hoaDonSelectedIds ?? []),
      });
      setIsSaving(false);
      if (res.is_success) {
        dispatch(hoaDonAction.changeSelectedId([]));
        NotifyHelper.Success("Success");
        dispatch(
          hoaDonAction.loadStart({
            ...filter,
          }),
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
        <title>Hóa đơn máy tính tiền</title>
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
              <Heading text="Danh sách hóa đơn máy tính tiền" />
            </Box>
            <Box
              sx={{
                display: "flex",
                gap: 2,
                justifyContent: "flex-end",
                flexDirection: ["column", "row"],
                alignItems: ["flex-end", "center"],
              }}
            >
              <HoaDonSort
                sortBy={{
                  field:
                    (filter.sort_by ?? "") !== ""
                      ? (filter.sort_by ?? "")
                      : "id",
                  mode: filter.sort_mode ?? eSortMode.DESC,
                }}
                onValueChanged={(data) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      sort_by: data.field,
                      sort_mode: data.mode,
                    }),
                  );
                }}
              />
              <Link
                to={{
                  pathname: "../../hoa-don/form/0",
                  state: { is_may_tinh_tien: true },
                }}
              >
                <Button
                  text="Thêm mới"
                  leadingVisual={PlusIcon}
                  variant="primary"
                  size="medium"
                />
              </Link>
              {/* <Button text="Nhập khẩu" leadingVisual={UploadIcon} size="medium"
                                sx={{ ml: 1 }}
                            /> */}
              <HoaDonImportButton
                onSuccess={() => {
                  dispatch(
                    hoaDonAction.loadStart({
                      ...filter,
                    }),
                  );
                }}
              />
            </Box>
          </Box>
          <Box id="tabs">
            <UnderlineNav aria-label="Repository">
              <UnderlineNav.Item
                as={Link}
                to={"../hoa-don-mtt/nhap"}
                aria-current={tab === "nhap" ? "page" : undefined}
                icon={ShieldIcon}
              >
                Hóa đơn nháp
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={"../hoa-don-mtt/da-phat-hanh"}
                aria-current={
                  !tab || tab === "da-phat-hanh" ? "page" : undefined
                }
                icon={ShieldCheckIcon}
              >
                Đã phát hành
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={"../hoa-don-mtt/cho-phat-hanh"}
                aria-current={tab === "cho-phat-hanh" ? "page" : undefined}
                icon={ShieldIcon}
              >
                Chưa gửi CQT
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={"../hoa-don-mtt/phat-hanh-loi"}
                aria-current={tab === "phat-hanh-loi" ? "page" : undefined}
                icon={ShieldSlashIcon}
              >
                Phát hành lỗi
              </UnderlineNav.Item>

              <UnderlineNav.Item
                as={Link}
                to={"../hoa-don-mtt/da-huy"}
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
            <DataTableRemotePaging
              title={`Tổng số: ${(
                paging_res?.total_count ?? 0
              ).toLocaleString()}`}
              subTitle={`Đã chọn: ${hoaDonSelectedIds?.length ?? 0}`}
              data={hoaDons}
              height={window.innerHeight - 100}
              isLoading={status === eReducerStatusBase.is_loading}
              exportEnable
              actionComponent={
                <>
                  {(hoaDonSelectedIds?.length ?? 0) <= 0 && (
                    <HoaDonMayTinhTienFilter
                      filter={filter}
                      onChanged={(filter) => {
                        dispatch(hoaDonAction.changeFilter(filter));
                      }}
                    />
                  )}
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
                    (!tab || tab === "nhap" || tab === "cho-phat-hanh") && (
                      <>
                        {tab !== "da-phat-hanh" && tab && (
                          <HoaDonKySoPhatHanhMultiple
                            ids={hoaDonSelectedIds ?? []}
                            onClose={() => {
                              dispatch(
                                hoaDonAction.loadStart({
                                  ...filter,
                                }),
                              );
                            }}
                            title={
                              tab === "cho-phat-hanh"
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
                          {/* <ActionList.LinkItem
                            target="_blank"
                            // href={`../../../api/hoa-don/pdfs?hoaDonIds=${hoaDonSelectedIds?.join(
                            //   ","
                            // )}`}
                            href={`${
                              process.env.VITE_API_BASE_URL
                            }/hoa-don/pdfs?hoaDonIds=${hoaDonSelectedIds?.join(
                              ","
                            )}`}
                          >
                            <ActionList.LeadingVisual>
                              <FileIcon />
                            </ActionList.LeadingVisual>
                            Tải pdf
                          </ActionList.LinkItem> */}
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
                                  "Không được tải xuống quá 20 hóa đơn cùng lúc",
                                );
                                return;
                              }
                              const url = `${
                                process.env.VITE_API_BASE_URL
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
                              // href={`../../../api/hoa-don/xmls?hoaDonIds=${hoaDonSelectedIds?.join(
                              //   ","
                              // )}`}
                              href={`${
                                process.env.VITE_API_BASE_URL
                              }/hoa-don/xmls?hoaDonIds=${hoaDonSelectedIds?.join(
                                ",",
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
                    fileName="hoa-don-mtt"
                    formatDataFunction={(data) => {
                      return data.map((x: IHoaDon) => {
                        return {
                          "Ký hiệu": x.hoa_don_dang_ky_phat_hanh_ky_hieu,
                          "Loại hóa đơn": x.ten_hoa_don,
                          "Ngày hóa đơn": moment(x.ngay_hoa_don).format(
                            "DD/MM/YYYY",
                          ),
                          "Mã số hóa đơn": x.ma_so_hoa_don,
                          "Người mua": x.nguoi_mua_ten_donvi,
                          MST: x.nguoi_mua_mst,
                          Email: x.nguoi_mua_email,
                          "Tổng tiền": x.tong_tien_thanh_toan,
                          "Mã tra cứu": x.ma_tra_cuu,
                          "Kết quả": x.ket_qua_phat_hanh,
                          "Mã CQT cấp": `${x.phat_hanh_ma_ketqua_cqt} ${
                            x.ma_so_hoa_don_mtt ?? ""
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
                    }),
                  );
                },
              }}
              sortConfig={{
                enable: false,
                field: filter.sort_by,
                mode: filter.sort_mode ?? eSortMode.ASC,
                onValueChanged: (key: string, sort_mode: eSortMode) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      sort_by: key,
                      sort_mode: sort_mode,
                    }),
                  );
                },
              }}
              paging={{
                onPageIndexChanged: (pageIndex) => {
                  dispatch(
                    hoaDonAction.changeFilter({
                      ...filter,
                      page_index: pageIndex,
                    }),
                  );
                },
                pageCount: paging_res?.page_count ?? 1,
                pageIndex: paging_res?.page_number ?? 1,
                pageSize: paging_res?.page_size ?? 1,
                totalCount: paging_res?.total_count ?? 1,
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
                  // sortBy: "alphanumeric"
                },
                {
                  id: "xem",
                  header: "Xem",
                  width: "50px",
                  renderCell: (row: any) => {
                    return (
                      <PrintHoaDonButtonActionListItem
                        id={row.id}
                        showText={false}
                        hoa_don_hinh_thuc_id={row.hoa_don_hinh_thuc_id}
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
                            onClick={() => {
                              history.push(`../../hoa-don-mtt/form/${row.id}`);
                            }}
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
                                    row,
                                  ),
                                );
                              }
                            }}
                          />
                        ) : (
                          <LockIcon />
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
                        <ActionMenu>
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
                                {tab === "nhap" && (
                                  <ActionList.Item
                                    onSelect={() => {
                                      history.push(
                                        `../../hoa-don-mtt/form/${row.id}`,
                                      );
                                    }}
                                  >
                                    <ActionList.LeadingVisual>
                                      <EyeIcon />
                                    </ActionList.LeadingVisual>
                                    Chi tiết hóa đơn
                                  </ActionList.Item>
                                )}

                                <ActionList.Item
                                  onSelect={() => {
                                    dispatch(
                                      rootAction.hoaDon.hoaDonAction.showLogModal(
                                        row,
                                      ),
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
                              </ActionList.Group>

                              {(tab === undefined ||
                                tab === "da-phat-hanh") && (
                                <>
                                  <ActionList.Divider />
                                  <ActionList.Group title="Điều chỉnh/ thay thế">
                                    <ActionList.Item
                                      // variant="danger"
                                      onSelect={() => {
                                        history.push(
                                          `../../hoa-don-mtt/form/0?hinh_thuc_id=3&hoa_don_goc_id=${row.id}`,
                                        );
                                      }}
                                    >
                                      <ActionList.LeadingVisual>
                                        <GitPullRequestIcon />
                                      </ActionList.LeadingVisual>
                                      Lập hóa đơn điều chỉnh
                                    </ActionList.Item>
                                    <ActionList.Item
                                      onSelect={() => {
                                        history.push(
                                          `../../hoa-don-mtt/form/0?hinh_thuc_id=2&hoa_don_goc_id=${row.id}`,
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
                                      onSelect={() => {
                                        history.push(
                                          `../../hoa-don-mtt/form/0?hinh_thuc_id=2&hoa_don_goc_id=${row.id}`,
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
                                      // variant="danger"
                                      onSelect={() => {
                                        dispatch(
                                          hoaDonAction.changeSelectedId([
                                            row.id,
                                          ]),
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
                                                                                history.push(`../../hoa-don-mtt/form/0?hinh_thuc_id=2&hoa_don_goc_id=${row.id}`)
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
                                            row,
                                          ),
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
                                tab === "phat-hanh-loi") && (
                                <>
                                  <ActionList.Divider />
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
                      <Link to={`../../hoa-don-mtt/form/${data.id}`}>
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
                  // sortBy: "alphanumeric"
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
                  // sortBy: "alphanumeric"
                },
                // {
                //     header: 'MST',
                //     field: 'nguoi_mua_mst',
                //     rowHeader: false,
                //     width: "140px",
                //     // sortBy: "alphanumeric"
                // },
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
                          {cell.nguoi_mua_mst} - {cell.nguoi_mua_email}
                        </Box>
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
                // {
                //     header: 'Hình thức HĐ',
                //     field: 'hoa_don_hinh_thuc_id',
                //     rowHeader: false,
                //     width: "170px",
                //     renderCell: (cell: IHoaDon) => {
                //         return (
                //             <>
                //                 <HoaDonHinhThuc id={cell.hoa_don_hinh_thuc_id} />
                //             </>
                //         )
                //     }
                // },
                // {
                //     header: 'Trạng thái',
                //     field: 'hoa_don_trang_thai_id',
                //     rowHeader: false,
                //     width: "200px",
                //     renderCell: (cell: IHoaDon) => {
                //         return (
                //             <>
                //                 <HoaDonStatus id={cell.hoa_don_trang_thai_id} />
                //             </>
                //         )
                //     }
                // },
                {
                  header: "Nội dung phát hành",
                  field: "ket_qua_phat_hanh",
                  rowHeader: false,
                  width: "250px",
                  renderCell: (data: IHoaDon) => {
                    return <Box>{data.ma_so_hoa_don_mtt ?? ""}</Box>;
                  },
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
              rootAction.hoaDon.hoaDonAction.deleteStart(
                hoaDonEditing?.id ?? 0,
              ),
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
