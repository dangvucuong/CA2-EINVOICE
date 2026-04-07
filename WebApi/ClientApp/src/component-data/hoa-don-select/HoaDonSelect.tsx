import { Box, Link } from "@primer/react";
import DataTableRemotePaging from "../../component-ui/data-table";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import SelectBoxLoaiHoaDonCTPhatHanh from "../selectbox-loai-hoa-don-ct-phat-hanh";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
import SelectBoxMauSoPhatHanh from "../selectbox-mau-so-phat-hanh";
import SelectBoxKyHieuPhatHanh from "../selectbox-ky-hieu-phat-hanh";
import TuNgayDenNgayInput from "../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import moment from "moment";
import { useEffect } from "react";
import HoaDonSort from "../../pages/hoa-don/HoaDonSort";
import HoaDonHinhThuc from "../hoa-don-hinh-thuc";
interface IHoaDonSelectProps {
  height?: number;
  isSingleMode?: boolean;
  onSelected: (ids: number[], hoaDon: IHoaDon[]) => void;
  addFilter?: {
    hoa_don_hinh_thuc_id?: number;
  };
}
const mainAction = rootAction.hoaDon.hoaDonSelectBoxAction;
const HoaDonSelect = (props: IHoaDonSelectProps) => {
  const { status, hoaDons, hoaDonSelectedIds, paging_res, filter } =
    useAppSelector((x) => x.hoaDon.hoaDonSelectBoxReducer);
  const dispatch = useAppDispatch();
  useEffect(() => {
    dispatch(mainAction.changeSelectedIds([]));
  }, []);

  // useEffect(() => {
  //   if (
  //     status === eReducerStatusBase.is_not_initialization ||
  //     status === eReducerStatusBase.is_need_reload
  //   ) {
  //     dispatch(mainAction.loadStart({ ...filter, ...props?.addFilter }));
  //   }
  // }, [status]);

  useEffect(() => {
    dispatch(mainAction.loadStart({ ...filter, ...props?.addFilter }));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filter]);

  return (
    <Box>
      <Box
        sx={{
          height: props.height ?? window.innerHeight - 200,
          overflowY: "auto",
          pt: 1,
        }}
      >
        <DataTableRemotePaging
          title={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
          subTitle={`Đã chọn: ${hoaDonSelectedIds?.length ?? 0}`}
          data={hoaDons}
          // height={props.height ?? window.innerHeight - 300}
          isLoading={status === eReducerStatusBase.is_loading}
          // exportEnable
          actionComponent={
            <>
              <Box>
                <HoaDonSort
                  sortBy={{
                    field:
                      (filter.sort_by ?? "") !== ""
                        ? filter.sort_by ?? ""
                        : "id",
                    mode: filter.sort_mode ?? eSortMode.DESC,
                  }}
                  onValueChanged={(data) => {
                    dispatch(
                      mainAction.changeFilter({
                        ...filter,
                        sort_by: data.field,
                        sort_mode: data.mode,
                      })
                    );
                  }}
                />
              </Box>
              <SelectBoxLoaiHoaDonCTPhatHanh
                isShowClearBtn
                value={filter.loai_hoa_don_ct_id}
                onValueChanged={(id) => {
                  dispatch(
                    mainAction.changeFilter({
                      ...filter,
                      loai_hoa_don_ct_id: id,
                      hoa_don_dang_ky_phat_hanh_mau_so: "",
                      hoa_don_dang_ky_phat_hanh_ky_hieu: "",
                    })
                  );
                }}
              />
              <Box>
                <SelectBoxMauSoPhatHanh
                  value={filter.hoa_don_dang_ky_phat_hanh_mau_so}
                  loai_hoa_don_ct_id={filter.loai_hoa_don_ct_id}
                  isAutoSelectIfHasOneItem
                  isShowClearBtn
                  onValueChanged={(id) => {
                    dispatch(
                      mainAction.changeFilter({
                        ...filter,
                        hoa_don_dang_ky_phat_hanh_mau_so: id,
                        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
                      })
                    );
                  }}
                />
              </Box>
              <Box>
                <SelectBoxKyHieuPhatHanh
                  value={filter.hoa_don_dang_ky_phat_hanh_ky_hieu}
                  isAutoSelectIfHasOneItem
                  isShowClearBtn
                  loai_hoa_don_ct_id={filter.loai_hoa_don_ct_id}
                  mau_so={filter.hoa_don_dang_ky_phat_hanh_mau_so}
                  onValueChanged={(id) => {
                    dispatch(
                      mainAction.changeFilter({
                        ...filter,
                        hoa_don_dang_ky_phat_hanh_ky_hieu: id,
                      })
                    );
                  }}
                />
              </Box>
              <Box>
                <TuNgayDenNgayInput
                  tu_ngay={filter.tu_ngay}
                  den_ngay={filter.den_ngay}
                  onValueChanged={(tu_ngay, den_ngay) => {
                    dispatch(
                      mainAction.changeFilter({
                        ...filter,
                        tu_ngay: tu_ngay,
                        den_ngay: den_ngay,
                      })
                    );
                  }}
                />
              </Box>
            </>
          }
          searchConfig={{
            enable: (hoaDonSelectedIds?.length ?? 0) <= 0,
            search_key: filter.search_key ?? "",
            onValueChanged: (key: string) => {
              dispatch(
                mainAction.changeFilter({
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
            mode: filter.sort_mode ?? eSortMode.ASC,
            onValueChanged: (key: string, sort_mode: eSortMode) => {
              // dispatch(mainAction.changeFilter({
              //     ...filter,
              //     sort_by: key,
              //     sort_mode: sort_mode
              // }))
            },
          }}
          paging={{
            onPageIndexChanged: (pageIndex) => {
              dispatch(
                mainAction.changeFilter({
                  ...filter,
                  page_index: pageIndex,
                })
              );
            },
            pageCount: paging_res?.page_count ?? 1,
            pageIndex: paging_res?.page_number ?? 1,
            pageSize: paging_res?.page_size ?? 1,
            totalCount: paging_res?.total_count ?? 1,
          }}
          selection={{
            mode: props.isSingleMode === true ? "single" : "multiple",
            keyExpr: "id",
            selectedRowKeys: hoaDonSelectedIds,
            onSelectionChanged: (keys) => {
              dispatch(mainAction.changeSelectedIds(keys));
              const hoaDonsSelecteds = hoaDons.filter((x) =>
                keys.includes(x.id)
              );
              props.onSelected(
                hoaDonsSelecteds.map((x) => x.id),
                hoaDonsSelecteds
              );
            },
          }}
          columns={[
            {
              header: "Id",
              field: "id",
              rowHeader: false,
              width: "50px",
              // sortBy: "alphanumeric"
            },

            {
              header: "Ký hiệu",
              field: "hoa_don_dang_ky_phat_hanh_ky_hieu",
              rowHeader: true,
              width: "100px",
              renderCell: (data: IHoaDon) => {
                return (
                  <Link target="_blank" href={`../../hoa-don/form/${data.id}`}>
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
            },
            {
              header: "Số HĐ",
              field: "ma_so_hoa_don",
              rowHeader: true,
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
          ]}
        />
      </Box>
    </Box>
  );
};

export default HoaDonSelect;
