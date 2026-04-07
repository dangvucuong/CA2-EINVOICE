import {
  LockIcon,
  PencilIcon,
  PlusIcon,
  TrashIcon,
  UploadIcon,
} from "@primer/octicons-react";
import { Helmet } from "react-helmet";

import { Box, IconButton, Link as LinkHref, useConfirm } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import DataTableRemotePaging from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { eHoaDonHinhThuc } from "../../models/commons/eHoaDonHinhThuc";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHoaDonSelectPagingRequest } from "../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import {
  IPagingResultSummary,
  getPagingSummary,
} from "../../models/responses/IBasePagingRespone";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import { HoaDonTimelineModal } from "../hoa-don/HoaDonTimelineModal";
import HoaDonDieuChinhFilter from "./HoaDonDieuChinhFilter";
import HoaDonStatus from "../../component-data/hoa-don-status";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { rootAction } from "../../state/actions/rootAction";
import { hoaDonAction } from "../../state/actions/hoa-don/hoaDonAction";
import { useAppSelector } from "../../hooks/useAppSelector";
import ConfirmModal from "../../component-ui/confirm-modal";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";

const HoaDonDieuChinhPage = () => {
  const history = useHistory();
  const [filter, setFilter] = useState<IHoaDonSelectPagingRequest>({
    hoa_don_trang_thai_ids: [],
    loai_hoa_don_ct_id: 0,
    hoa_don_dang_ky_phat_hanh_mau_so: "",
    hoa_don_dang_ky_phat_hanh_ky_hieu: "",
    hoa_don_hinh_thuc_id: eHoaDonHinhThuc.HOA_DON_DIEU_CHINH,
    page_index: 0,
    page_size: 20,
    search_key: undefined,
    sort_by: "",
    sort_mode: eSortMode.DESC,
  });
  const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);
  const [pagingResult, setPagingResult] = useState<IPagingResultSummary>();
  const [isLoading, setIsLoading] = useState(false);
  const [isShowHistoryModal, setIsShowHistoryModal] = useState(false);
  const [hoaDonSelectedId, setHoaDonSelectedId] = useState(0);
  const confirm = useConfirm();
  const { status, isShowDeleteConfirm, hoaDonEditing } = useAppSelector(
    (x) => x.hoaDon.hoaDonReducer
  );

  useEffect(() => {
    handleGetDataAsync();
  }, [filter]);

  const handleGetDataAsync = async () => {
    setIsLoading(true);
    const res = await hoaDonApi.selectByDonViPaging(filter);
    setIsLoading(false);
    if (res.is_success) {
      setHoaDons(res.data.data);
      setPagingResult(getPagingSummary(res.data));
    } else {
      NotifyHelper.Error("Error");
    }
  };

  const handleDeletesHoaDon = async (
    isHuyNoiBo: boolean,
    hoaDonId?: number
  ) => {
    if (
      await confirm({
        content: `Bạn có chắc chắn muốn ${
          isHuyNoiBo ? "hủy nội bộ" : "xóa"
        } hóa đơn đã chọn`,
        title: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        cancelButtonContent: "Không",
        confirmButtonContent: `${isHuyNoiBo ? "Hủy nội bộ" : "Xóa hóa đơn"}`,
        confirmButtonType: "danger",
      })
    ) {
      // setIsSaving(true);
      const res = await hoaDonApi.deletes({
        ids: hoaDonId ? [hoaDonId] : [],
      });
      // setIsSaving(false);
      if (res.is_success) {
        NotifyHelper.Success("Success");
        dispatch(
          hoaDonAction.loadStart({
            ...filter,
          })
        );
      } else {
        NotifyHelper.Error(res.message ?? "Error");
      }
    }
  };

  const dispatch = useAppDispatch();
  return (
    <Box>
      <Helmet>
        <title>Hóa đơn điều chỉnh</title>
      </Helmet>
      <Box
        id="header"
        sx={{
          display: "flex",
        }}
      >
        <Box
          sx={{
            flex: 1,
          }}
        >
          <Heading text="Hóa đơn điều chỉnh" />
        </Box>
        <Box sx={{ display: "flex" }}>
          <Link to={"../../hoa-don-dieu-chinh/form/0?hinh_thuc_id=3"}>
            <Button
              text="Lập HĐ điều chỉnh"
              leadingVisual={PlusIcon}
              variant="primary"
              size="medium"
            />
          </Link>
          {/* <Button text="Nhập khẩu" leadingVisual={UploadIcon} size="medium"
                        sx={{ ml: 1 }}
                    /> */}
        </Box>
      </Box>
      <Box sx={{ mt: 3 }}>
        <DataTableRemotePaging
          title={`Tổng số: ${(
            pagingResult?.total_count ?? 0
          ).toLocaleString()}`}
          data={hoaDons}
          height={window.innerHeight - 100}
          isLoading={isLoading}
          exportEnable
          actionComponent={
            <>
              <HoaDonDieuChinhFilter filter={filter} onChanged={setFilter} />
            </>
          }
          searchConfig={{
            enable: true,
            onValueChanged: (key: string) => {
              setFilter({
                ...filter,
                page_index: 0,
                search_key: key,
              });
            },
          }}
          sortConfig={{
            enable: false,
            field: filter.sort_by,
            mode: filter.sort_mode ?? eSortMode.ASC,
            onValueChanged: (key: string, sort_mode: eSortMode) => {
              setFilter({
                ...filter,
                sort_by: key,
                sort_mode: sort_mode,
              });
            },
          }}
          paging={{
            onPageIndexChanged: (pageIndex) => {
              setFilter({
                ...filter,
                page_index: pageIndex,
              });
            },
            pageCount: pagingResult?.page_count ?? 1,
            pageIndex: pagingResult?.page_number ?? 1,
            pageSize: pagingResult?.page_size ?? 1,
            totalCount: pagingResult?.total_count ?? 1,
          }}
          columns={[
            {
              header: "Id",
              field: "id",
              rowHeader: false,
              width: "80px",
              // sortBy: "alphanumeric"
            },
            {
              id: "edit",
              header: "Sửa",
              width: "50px",
              renderCell: (row: any) => {
                return (
                  <>
                    {row?.hoa_don_trang_thai_id === eHoaDonTrangThai.NHAP ? (
                      <IconButton
                        as={LinkHref}
                        href={`../../hoa-don/form/${row.id}`}
                        icon={PencilIcon}
                        aria-label="Edit"
                        variant="invisible"
                        size="small"
                      />
                    ) : (
                      <></>
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
                    {row?.hoa_don_trang_thai_id === eHoaDonTrangThai.NHAP &&
                    row?.ma_so_hoa_don <= 0 ? (
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
                      <></>
                    )}
                  </>
                );
              },
            },

            {
              header: "Trạng thái",
              field: "hoa_don_trang_thai_id",
              rowHeader: true,
              width: "150px",
              renderCell: (data: IHoaDon) => {
                return <HoaDonStatus id={data.hoa_don_trang_thai_id} />;
              },
              // sortBy: "alphanumeric"
            },
            {
              header: "Ký hiệu",
              field: "hoa_don_dang_ky_phat_hanh_ky_hieu",
              rowHeader: true,
              width: "100px",
              renderCell: (data: IHoaDon) => {
                return (
                  <Link to={`../../hoa-don-dieu-chinh/form/${data.id}`}>
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
                  <Box>{moment(cell.ngay_hoa_don).format("DD/MM/YYYY")}</Box>
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

            {
              header: "Nội dung phát hành",
              field: "ket_qua_phat_hanh",
              rowHeader: false,
              width: "300px",
              renderCell: (data: IHoaDon) => {
                return (
                  // <Box className="limit1Line">
                  <Box>
                    {/* <HoaDonStatus id={data.hoa_don_trang_thai_id} /> */}
                    {data.phat_hanh_ma_ketqua_cqt && (
                      <Box
                        sx={{
                          display: "flex",
                          flexWrap: "wrap",
                        }}
                      >
                        <b>Mã CQT: </b>{" "}
                        <Box sx={{ color: "green" }}>
                          {data.phat_hanh_ma_ketqua_cqt}
                        </Box>
                      </Box>
                    )}
                    {data.ket_qua_phat_hanh}
                  </Box>
                );
              },
            },
            {
              header: "Hóa đơn gốc",
              field: "ket_qua_phat_hanh",
              rowHeader: false,
              width: "300px",
              renderCell: (data: IHoaDon) => {
                return (
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                    }}
                  >
                    {data.hoa_don_id_goc > 0 && (
                      <LinkHref
                        target="_blank"
                        href={`../../hoa-don-dieu-chinh/form/${data.hoa_don_id_goc}`}
                      >
                        <Box>
                          Mẫu số:{" "}
                          <b>{data.hoa_don_dang_ky_phat_hanh_mau_so_goc}</b>, Ký
                          hiệu:{" "}
                          <b>{data.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}</b>,
                          Số HĐ: <b>{data.ma_so_hoa_don_goc}</b>
                        </Box>
                      </LinkHref>
                    )}
                    {data.hoa_don_id_goc <= 0 && (
                      <Box>
                        Mẫu số:{" "}
                        <b>{data.hoa_don_dang_ky_phat_hanh_mau_so_goc}</b>, Ký
                        hiệu:{" "}
                        <b>{data.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}</b>, Số
                        HĐ: <b>{data.ma_so_hoa_don_goc}</b>
                      </Box>
                    )}

                    <Box
                      sx={{
                        fontSize: "12px",
                        color: "fg.muted",
                      }}
                    >
                      Ngày hóa đơn:{" "}
                      <b>
                        {data.ngay_hoa_don_goc
                          ? moment(data.ngay_hoa_don_goc).format("DD/MM/YYYY")
                          : ""}
                      </b>
                    </Box>
                  </Box>
                );
              },
            },
            {
              header: "Mã đơn vị quan hệ ngân sách",
              field: "ma_dv_ngan_sach",
              rowHeader: false,
              width: "150px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số CCCD",
              field: "nguoi_mua_cccd",
              rowHeader: false,
              width: "150px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Tổng tiến chiết khấu",
              field: "tong_tien_chiet_khau",
              rowHeader: false,
              width: "150px",
              renderCell: (cell: IHoaDon) => {
                return (
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                    }}
                  >
                    <Box>
                      <b>{cell.tong_tien_chiet_khau.toLocaleString()}</b>
                    </Box>
                  </Box>
                );
              },
              // sortBy: "alphanumeric"
            },
          ]}
        />
      </Box>
      {isShowHistoryModal && hoaDonSelectedId && (
        <HoaDonTimelineModal
          hoaDonId={hoaDonSelectedId}
          onClose={() => {
            setIsShowHistoryModal(false);
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
    </Box>
  );
};

export default HoaDonDieuChinhPage;
