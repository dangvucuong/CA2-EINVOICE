

import { DownloadIcon } from '@primer/octicons-react';
import { Box } from '@primer/react';
import axios from 'axios';
import moment from "moment";
import { useEffect, useState } from "react";
import { appInfo } from '../../../AppInfo';
import { thongKeApi } from "../../../api/hoa-don/thongKeApi";
import ExportToExcelBtn from '../../../component-data/export-excel-btn/ExportToExcelBtn';
import Button from '../../../component-ui/button';
import DataTableRemotePaging from "../../../component-ui/data-table";
import Heading from "../../../component-ui/heading";
import { NotifyHelper } from "../../../helpers/toast";
import { eSortMode } from "../../../models/commons/eSortMode";
import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IPagingResultSummary, getPagingSummary } from "../../../models/responses/IBasePagingRespone";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { IHoaDonHangHoaVM } from '../../../models/responses/hoa-don/IHoaDonHangHoaVM';
import { HoaDonTimelineModal } from "../../hoa-don/HoaDonTimelineModal";
interface IHangHoaListProps {
    tu_ngay?: string,
    den_ngay?: string,
    nguoi_mua_mst?: string,
    ma_dai_ly?: string,
    render_key?: string,

    hoa_don_trang_thai_ids?: number[]
    hoa_don_hinh_thuc_id?: number
}
const tinhChatHangHoaDictionary: any = {
    1: "Hàng hoa, dịch vụ",
    2: "Khuyến mại",
    3: "Chiết khấu",
    4: "Ghi chú, diễn giải"
}
const HangHoaList = (props: IHangHoaListProps) => {
    const [isExporting, setIsExporting] = useState(false);
    const [isExportingChiTiet, setIsExportingChiTiet] = useState(false);

    const [filter, setFilter] = useState<IHoaDonSelectPagingRequest>({
        hoa_don_trang_thai_ids: [],
        loai_hoa_don_ct_id: 0,
        hoa_don_dang_ky_phat_hanh_mau_so: "",
        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
        hoa_don_hinh_thuc_id: 0,
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC,
        tu_ngay: props.tu_ngay,
        den_ngay: props.den_ngay
    });
    const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);
    const [pagingResult, setPagingResult] = useState<IPagingResultSummary>();
    const [isLoading, setIsLoading] = useState(false);
    const [isShowHistoryModal, setIsShowHistoryModal] = useState(false);
    const [hoaDonSelectedId, sethoaDonSelectedId] = useState(0);
    
  useEffect(() => {
    setFilter(x => ({
        ...x,
        tu_ngay: props.tu_ngay,
        den_ngay: props.den_ngay,
        nguoi_mua_mst: props.nguoi_mua_mst,
        ma_dai_ly: props.ma_dai_ly,

        hoa_don_trang_thai_ids: props.hoa_don_trang_thai_ids ?? [],
        hoa_don_hinh_thuc_id: props.hoa_don_hinh_thuc_id ?? 0
    }))
}, [
    props.tu_ngay,
    props.den_ngay,
    props.ma_dai_ly,
    props.nguoi_mua_mst,
    props.render_key,
    props.hoa_don_trang_thai_ids,
    props.hoa_don_hinh_thuc_id
])


    useEffect(() => {
        console.log({
            filter
        });

        handleGetDataAsync();
    }, [filter])
    
    const handleGetDataAsync = async () => {
        setIsLoading(true)
        const res = await thongKeApi.selectHangHoaPaging(filter)
        setIsLoading(false)
        if (res.is_success) {
            setHoaDons(res.data.data)
            setPagingResult(getPagingSummary(res.data))
        } else {
            NotifyHelper.Error("Error")
        }
    }
    const handleExportBaoCaoAsync = async () => {
        setIsExporting(true)
        // const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

        const response = await axios.post(`${appInfo.baseApiURL}/thong-ke/hang-hoa/export`,
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
                responseType: 'blob', // Important for handling binary data
            });

        // Create a URL for the file blob
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'downloaded_file.xlsx'); // File name
        document.body.appendChild(link);
        link.click();
        link.remove();
        // const res = await thongKeApi.exportBangKe({
        //     ...filter
        // }, "test.xlsx")
        setIsExporting(false)
    }
    const handleExportBaoCaoChiTietAsync = async () => {
        setIsExportingChiTiet(true)
        // const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

        const response = await axios.post(`${appInfo.baseApiURL}/thong-ke/hang-hoa/export/chi-tiet`,
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
                responseType: 'blob', // Important for handling binary data
            });

        // Create a URL for the file blob
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'downloaded_file.xlsx'); // File name
        document.body.appendChild(link);
        link.click();
        link.remove();
        // const res = await thongKeApi.exportBangKe({
        //     ...filter
        // }, "test.xlsx")
        setIsExportingChiTiet(false)
    }
    return (
        <Box>
            <Box id="header" sx={{
                display: "flex"
            }}>
                <Box sx={{
                    flex: 1
                }}>
                    <Heading text="Danh sách hàng hóa" />
                </Box>
            </Box>
            <DataTableRemotePaging
                title={`Tổng số: ${(pagingResult?.total_count ?? 0).toLocaleString()}`}
                data={hoaDons}
                height={window.innerHeight - 100}
                isLoading={isLoading}
                exportEnable
                actionComponent={<>
                    <Box sx={{ display: "flex" }}>
                        <Box sx={{ mr: 1 }}>
                            <Button text="Xuất báo cáo tổng hợp"
                                size="medium"
                                leadingVisual={DownloadIcon}
                                isLoading={isExporting}
                                onClick={handleExportBaoCaoAsync}
                            />
                        </Box>
                        <Box sx={{ mr: 1 }}>
                            <Button text="Xuất báo cáo chi tiết"
                                size="medium"
                                leadingVisual={DownloadIcon}
                                isLoading={isExportingChiTiet}
                                onClick={handleExportBaoCaoChiTietAsync}
                            />
                        </Box>
                        <ExportToExcelBtn
                            fileName="hang-hoa"
                            formatDataFunction={(data) => {
                                return data.map((x: IHoaDonHangHoaVM) => {
                                    return {
                                        "Ký hiệu": x.hoa_don_dang_ky_phat_hanh_ky_hieu,
                                        "Loại hóa đơn": x.loai_hoa_don_ct_name,
                                        "Ngày hóa đơn": moment(x.ngay_hoa_don).format("DD/MM/YYYY"),
                                        "Mã số hóa đơn": x.ma_so_hoa_don,
                                        "STT": x.stt,
                                        "Tính chất": tinhChatHangHoaDictionary[x.hang_hoa_tinh_chat_id.toString()],
                                        "Mã hàng": x.ma_hang,
                                        "Tên hàng": x.ten_hang,
                                        "ĐVT": x.dvt,
                                        "Số lượng": x.so_luong,
                                        "Chiết khấu": x.ty_le_chiet_khau,
                                        "Thuế suất": x.thue_vat,
                                        "Thành tiền": x.thanh_tien,


                                    }
                                })
                            }}
                            fetchDataPromise={() => {
                                return new Promise((resolve, reject) => {
                                    return thongKeApi.selectHangHoaPaging({
                                        ...filter,
                                        page_index: 0,
                                        page_size: pagingResult?.total_count
                                    }).then(res => {
                                        if (res.is_success) {
                                            resolve(res.data.data);
                                        } else {
                                            NotifyHelper.Error(res.message ?? "Error")
                                            resolve(undefined);
                                        }
                                    })
                                });
                            }}
                        />
                    </Box>
                </>
                }
                searchConfig={{
                    enable: true,
                    onValueChanged: (key: string) => {
                        setFilter({
                            ...filter,
                            page_index: 0,
                            search_key: key
                        })
                    }
                }}
                sortConfig={{
                    enable: false,
                    field: filter.sort_by,
                    mode: filter.sort_mode ?? eSortMode.ASC,
                    onValueChanged: (key: string, sort_mode: eSortMode) => {
                        setFilter({
                            ...filter,
                            sort_by: key,
                            sort_mode: sort_mode
                        })
                    }
                }}
                paging={{
                    onPageIndexChanged: (pageIndex) => {
                        setFilter({
                            ...filter,
                            page_index: pageIndex
                        })
                    },
                    pageCount: pagingResult?.page_count ?? 1,
                    pageIndex: pagingResult?.page_number ?? 1,
                    pageSize: pagingResult?.page_size ?? 1,
                    totalCount: pagingResult?.total_count ?? 1
                }}

                columns={[
                    {
                        header: 'Id',
                        field: 'id',
                        rowHeader: false,
                        width: "80px"
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Loại HĐ',
                        field: 'loai_hoa_don_ct_name',
                        // rowHeader: true,
                        width: "200px",
                        // sortBy: "alphanumeric"
                    },

                    {
                        header: 'Ký hiệu',
                        field: 'hoa_don_dang_ky_phat_hanh_mau_so',
                        rowHeader: false,
                        minWidth: "150px",
                        renderCell: (x: any) => {
                            return (
                                <b>{x.hoa_don_dang_ky_phat_hanh_mau_so}{x.hoa_don_dang_ky_phat_hanh_ky_hieu}_{x.ma_so_hoa_don}</b>
                            )
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Ngày HĐ',
                        field: 'ngay_hoa_don',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (cell: IHoaDon) => {
                            return (
                                <Box>{moment(cell.ngay_hoa_don).format("DD/MM/YYYY")}</Box>
                            )
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'STT',
                        field: 'stt',
                        rowHeader: false,
                        minWidth: "80px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Tính chất',
                        field: 'hang_hoa_tinh_chat_id',
                        rowHeader: false,
                        minWidth: "100px",
                        renderCell: (cell: any) => {
                            return <>{tinhChatHangHoaDictionary[cell.hang_hoa_tinh_chat_id.toString()]}</>
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Mã hàng',
                        field: 'ma_hang',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Tên hàng',
                        field: 'ten_hang',
                        rowHeader: false,
                        minWidth: "200px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'ĐVT',
                        field: 'dvt',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Số lượng',
                        field: 'so_luong',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Đơn giá',
                        field: 'don_gia',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Thuế suất',
                        field: 'thue_vat',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Chiết khấu',
                        field: 'ty_le_chiet_khau',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Thành tiền',
                        field: 'thanh_tien',
                        rowHeader: false,
                        minWidth: "100px",
                        // sortBy: "alphanumeric"
                    },
                ]}
            />
            {isShowHistoryModal && hoaDonSelectedId &&
                <HoaDonTimelineModal
                    hoaDonId={hoaDonSelectedId}
                    onClose={() => {
                        setIsShowHistoryModal(false)
                    }}
                />
            }
        </Box>
    );
};

export default HangHoaList;