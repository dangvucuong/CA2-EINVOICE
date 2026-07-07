import { DownloadIcon, EyeIcon, HistoryIcon } from "@primer/octicons-react";

import { Box, IconButton } from "@primer/react";
import axios from "axios";
import moment from "moment";
import { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { appInfo } from "../../../AppInfo";
import { thongKeApi } from "../../../api/hoa-don/thongKeApi";
import ExportToExcelBtn from "../../../component-data/export-excel-btn/ExportToExcelBtn";
import HoaDonHinhThuc from "../../../component-data/hoa-don-hinh-thuc";
import HoaDonStatus from "../../../component-data/hoa-don-status";
import SelectBoxHoaDonHinhThuc from "../../../component-data/selectbox-hoa-don-hinh-thuc";
import SelectBoxHoaDonTrangThaiMultiple from "../../../component-data/selectbox-hoa-don-trang-thai-multiple";
import Button from "../../../component-ui/button";
import DataTableRemotePaging from "../../../component-ui/data-table";
import Heading from "../../../component-ui/heading";
import { NotifyHelper } from "../../../helpers/toast";
import { eSortMode } from "../../../models/commons/eSortMode";
import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import {
  IPagingResultSummary,
  getPagingSummary,
} from "../../../models/responses/IBasePagingRespone";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { HoaDonTimelineModal } from "../../hoa-don/HoaDonTimelineModal";
import {
  thongKeHDDHuyTemplate,
  thongKeHDDieuChinhTemplate,
  thongKeHDThayTheTemplate,
} from "../../../helpers/excelteamplate";
import { useAuth } from "../../../hooks/useAuth";
import { hoaDonHinhThucs } from "../../../hooks/useHoaDonHinhThuc";
import { hoaDonTrangThais } from "../../../hooks/useHoaDonTrangThai";
interface IHoaDonListProps {
  tu_ngay?: string;
  den_ngay?: string;
  nguoi_mua_mst?: string;
  ma_dai_ly?: string;
  render_key?: string;

  hoa_don_trang_thai_ids?: number[]
  hoa_don_hinh_thuc_id?: number

  onFilterChanged?: (data:{
    hoa_don_trang_thai_ids?: number[]
    hoa_don_hinh_thuc_id?: number
  })=>void
}
const HoaDonList = (props: IHoaDonListProps) => {
  const history = useHistory();
  const [filter, setFilter] = useState<IHoaDonSelectPagingRequest>({
    hoa_don_trang_thai_ids: [],
    loai_hoa_don_ct_id: 0,
    hoa_don_dang_ky_phat_hanh_mau_so: "",
    hoa_don_dang_ky_phat_hanh_ky_hieu: "",
    hoa_don_hinh_thuc_id: 0,
    page_index: 0,
    page_size: 20,
    search_key: undefined,
    sort_by: "ma_so_hoa_don",
    sort_mode: eSortMode.DESC,
    tu_ngay: props.tu_ngay,
    den_ngay: props.den_ngay,
  });
  const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);
  const [pagingResult, setPagingResult] = useState<IPagingResultSummary>();
  const [isLoading, setIsLoading] = useState(false);
  const [isShowHistoryModal, setIsShowHistoryModal] = useState(false);
  const [hoaDonSelectedId, sethoaDonSelectedId] = useState(0);
  const [isExporting, setIsExporting] = useState(false);
  const { user } = useAuth();

useEffect(()=>{
    setFilter(x=>({
        ...x,
        hoa_don_trang_thai_ids: props.hoa_don_trang_thai_ids ?? [],
        hoa_don_hinh_thuc_id: props.hoa_don_hinh_thuc_id ?? 0,

        tu_ngay: props.tu_ngay,
        den_ngay: props.den_ngay,
        nguoi_mua_mst: props.nguoi_mua_mst,
        ma_dai_ly: props.ma_dai_ly
    }))
},[
    props.render_key,
    props.hoa_don_trang_thai_ids,
    props.hoa_don_hinh_thuc_id,
    props.tu_ngay,
    props.den_ngay,
    props.ma_dai_ly,
    props.nguoi_mua_mst
])

  useEffect(() => {
    console.log({
      filter,
    });

    handleGetDataAsync();
  }, [filter]);
  const handleGetDataAsync = async () => {
    setIsLoading(true);
    const res = await thongKeApi.selectHoaDonPaging(filter);
    setIsLoading(false);
    if (res.is_success) {
      setHoaDons(res.data.data);
      setPagingResult(getPagingSummary(res.data));
    } else {
      NotifyHelper.Error("Error");
    }
  };
  const handleExportBangKeAsync = async () => {
    setIsExporting(true);
    // const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

    const response = await axios.post(
      `${appInfo.baseApiURL}/thong-ke/bang-ke/export`,
      {
        ...filter,
        tu_ngay: filter.tu_ngay === "" ? undefined : filter.tu_ngay,
        den_ngay: filter.den_ngay === "" ? undefined : filter.den_ngay,
      },
      {
        headers: {
          Authorization: `Bearer ${localStorage.access_token}`,
          language: localStorage.getItem("language"),
        },
        responseType: "blob", // Important for handling binary data
      },
    );

    // Create a URL for the file blob
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;

    link.setAttribute(
      "download",
      `${user?.donvi.ma_dv}_${moment().format("DD-MM-YYYY")}.xlsx`,
    );
    document.body.appendChild(link);
    link.click();
    link.remove();
    // const res = await thongKeApi.exportBangKe({
    //     ...filter
    // }, "test.xlsx")
    setIsExporting(false);
  };

  const exportWithTemplate = async (data: any) => {
    console.log(data);

    if (filter?.hoa_don_hinh_thuc_id === 2) {
      await thongKeHDThayTheTemplate({
        data: data,
        fileName: "ThongKeHDThayThe",
        startDate: filter.tu_ngay,
        endDate: filter.den_ngay,
      });
    }

    if (filter?.hoa_don_hinh_thuc_id === 3) {
      await thongKeHDDieuChinhTemplate({
        data: data,
        fileName: "ThongKeHDDieuChinh",
        startDate: filter.tu_ngay,
        endDate: filter.den_ngay,
      });
    }

    if (filter?.hoa_don_hinh_thuc_id === 5) {
      await thongKeHDDHuyTemplate({
        data: data,
        fileName: "ThongKeHDDHuy",
        startDate: filter.tu_ngay,
        endDate: filter.den_ngay,
      });
    }
  };

  return (
    <Box>
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
          <Heading text="Danh sách hóa đơn" />
        </Box>
      </Box>
      <DataTableRemotePaging
        title={`Tổng số: ${(pagingResult?.total_count ?? 0).toLocaleString()}`}
        data={hoaDons}
        height={window.innerHeight - 100}
        isLoading={isLoading}
        exportEnable
        actionComponent={
          <>
          <SelectBoxHoaDonTrangThaiMultiple
    value={filter.hoa_don_trang_thai_ids ?? []}
    onValueChanged={(ids) => {

        setFilter({
            ...filter,
            hoa_don_trang_thai_ids: ids,
        });

        props.onFilterChanged?.({
            hoa_don_trang_thai_ids: ids
        })
    }}
/>
           

           <SelectBoxHoaDonHinhThuc
    value={filter.hoa_don_hinh_thuc_id ?? 0}
    onValueChanged={(id) => {

        setFilter({
            ...filter,
            hoa_don_hinh_thuc_id: id,
        });

        props.onFilterChanged?.({
            hoa_don_hinh_thuc_id: id
        })
    }}
            />            

            <ExportToExcelBtn
              fileName="hoa-don"
              text="Xuất danh sách"
              formatDataFunction={(data) => {
                return data.map((x: any) => {
                  return {
                    "Ký hiệu": x.hoa_don_dang_ky_phat_hanh_ky_hieu,
                    "Loại hóa đơn": x.ten_hoa_don,
                    "Ngày hóa đơn": moment(x.ngay_hoa_don).format("DD/MM/YYYY"),
                    "Mã số hóa đơn": x.ma_so_hoa_don,
                    "Người mua": x.nguoi_mua_ten_donvi
                      ? x.nguoi_mua_ten_donvi
                      : x.nguoi_mua_ten,
                    MST: x.nguoi_mua_mst,
                    "Địa chỉ người mua": x.nguoi_mua_dia_chi,
                    Email: x.nguoi_mua_email,
                    "Trước thuế": x.tong_tien_truong_thue,
                    Thuế: x.tong_tien_thue,
                    "Tiền phí": x.tong_tien_phi,
                    "Tiền giảm": x.giam_thue_thanh_tien,
                    "Tổng tiền": x.tong_tien_thanh_toan,
                    "Hình thức hóa đơn":
                      hoaDonHinhThucs.find(
                        (h) => h.id === x.hoa_don_hinh_thuc_id,
                      )?.name || "",
                    "Trạng thái hóa đơn":
                      hoaDonTrangThais.find(
                        (h) => h.id === x.hoa_don_trang_thai_id,
                      )?.name || "",
                    "Mã tra cứu": x.ma_tra_cuu,
                    "Kết quả": x.ket_qua_phat_hanh,
                    "Mã CQT cấp": x.phat_hanh_ma_ketqua_cqt,
                    "Mã ĐVNS": x.ma_dv_ngan_sach,
                    "Người mua CCCD": x.nguoi_mua_cccd,
                    "Loại tiền": x.loai_tien,
                    "Tỷ giá": x.ty_gia,
                    Link: x.link,
                  };
                });
              }}
              fetchDataPromise={() => {
                return new Promise((resolve, reject) => {
                  return thongKeApi
                    .selectHoaDonPaging({
                      ...filter,
                      page_index: 0,
                      page_size: pagingResult?.total_count,
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
              teamplate={
                filter?.hoa_don_hinh_thuc_id === 2 ||
                filter?.hoa_don_hinh_thuc_id === 3 ||
                filter?.hoa_don_hinh_thuc_id === 5
              }
              teamplateFunction={exportWithTemplate}
            />
            {/* <Link href="thong-ke/bang-ke/export"> */}
            <Button
              text="Xuất bảng kê"
              size="medium"
              leadingVisual={DownloadIcon}
              isLoading={isExporting}
              onClick={handleExportBangKeAsync}
            />
            {/* </Link> */}
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
                mode: filter.sort_mode ?? eSortMode.DESC,
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
            header: "Ký hiệu",
            field: "hoa_don_dang_ky_phat_hanh_ky_hieu",
            rowHeader: true,
            width: "100px",
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
            field: "so_hoa_don",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },
          {
            header: "MST người mua",
            field: "nguoi_mua_mst",
            rowHeader: false,
            width: "140px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Người mua",
            field: "nguoi_mua_ten_donvi",
            rowHeader: false,
            minWidth: "200px",
            renderCell: (x: IHoaDon) => {
              return (
                <Box>
                  {x.nguoi_mua_ten_donvi
                    ? x.nguoi_mua_ten_donvi
                    : x.nguoi_mua_ten}
                </Box>
              );
            },
            // sortBy: "alphanumeric"
          },
          {
            header: "Địa chỉ người mua",
            field: "nguoi_mua_dia_chi",
            rowHeader: false,
            minWidth: "220px",
            renderCell: (x: IHoaDon) => (
              <Box className="limit1Line">{x.nguoi_mua_dia_chi ?? ""}</Box>
            ),
          },
          {
            header: "Trước thuế",
            field: "tong_tien_truong_thue",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Thuế",
            field: "tong_tien_thue",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Tiền phí",
            field: "tong_tien_phi",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Tiền giảm",
            field: "giam_thue_thanh_tien",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Tổng tiền",
            field: "tong_tien_thanh_toan",
            rowHeader: false,
            width: "100px",
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
            header: "Hình thức HĐ",
            field: "hoa_don_hinh_thuc_id",
            rowHeader: false,
            width: "170px",
            renderCell: (cell: IHoaDon) => {
              return (
                <>
                  <HoaDonHinhThuc id={cell.hoa_don_hinh_thuc_id} />
                </>
              );
            },
          },
          {
            header: "Trạng thái",
            field: "hoa_don_trang_thai_id",
            rowHeader: false,
            width: "200px",
            renderCell: (cell: IHoaDon) => {
              return (
                <>
                  <HoaDonStatus id={cell.hoa_don_trang_thai_id} />
                </>
              );
            },
          },
          {
            header: "Nội dung phát hành",
            field: "ket_qua_phat_hanh",
            rowHeader: false,
            minWidth: "150px",
            renderCell: (data: IHoaDon) => {
              return <Box className="limit1Line">{data.ket_qua_phat_hanh}</Box>;
            },
          },
          {
            header: "Mã ĐVNS",
            field: "ma_dv_ngan_sach",
            rowHeader: false,
            width: "150px",
            // sortBy: "alphanumeric"
          },
          {
            header: "CCCD",
            field: "nguoi_mua_cccd",
            rowHeader: false,
            width: "150px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Mã CQT",
            field: "phat_hanh_ma_ketqua_cqt",
            rowHeader: false,
            width: "200px",
            // sortBy: "alphanumeric"
          },

          {
            header: "Loại tiền",
            field: "loai_tien",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },

          {
            header: "Tỷ giá",
            field: "ty_gia",
            rowHeader: false,
            width: "100px",
            // sortBy: "alphanumeric"
          },

          {
            id: "actions",
            header: "",
            renderCell: (row: any) => {
              return (
                <>
                  <Box
                    sx={{
                      mt: -2,
                      mb: -2,
                      display: "flex",
                    }}
                  >
                    <IconButton
                      aria-label={`Edit`}
                      title={`Edit`}
                      icon={EyeIcon}
                      variant="invisible"
                      onClick={() => {
                        history.push(`../../hoa-don/form/${row.id}`);
                      }}
                    />
                    <IconButton
                      aria-label={`Edit`}
                      title={`Edit`}
                      icon={HistoryIcon}
                      variant="invisible"
                      onClick={() => {
                        setIsShowHistoryModal(true);
                        sethoaDonSelectedId(row.id);
                      }}
                    />
                  </Box>
                </>
              );
            },
          },
        ]}
      />
      {isShowHistoryModal && hoaDonSelectedId && (
        <HoaDonTimelineModal
          hoaDonId={hoaDonSelectedId}
          onClose={() => {
            setIsShowHistoryModal(false);
          }}
        />
      )}
    </Box>
  );
};

export default HoaDonList;
