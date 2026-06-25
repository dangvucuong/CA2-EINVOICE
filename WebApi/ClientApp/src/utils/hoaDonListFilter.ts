import moment from "moment";
import { eHoaDonTrangThai } from "../models/commons/eHoaDonTrangThai";
import { eSortMode } from "../models/commons/eSortMode";
import { IHoaDonSelectPagingRequest } from "../models/requests/hoa-don/IHoaDonSelectPagingRequest";

export const getDefaultDateRange = () => ({
    tu_ngay: moment().startOf("month").format("YYYY-MM-DD"),
    den_ngay: moment().format("YYYY-MM-DD"),
});

export const getTrangThaiIdsForTab = (tab?: string): number[] => {
    switch (tab) {
        case "nhap":
            return [eHoaDonTrangThai.NHAP];
        case "cho-phat-hanh":
            return [eHoaDonTrangThai.CHUA_GUI_CQT];
        case "chua-gui-cqt":
            return [];
        case "phat-hanh-loi":
            return [
                eHoaDonTrangThai.DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT,
                eHoaDonTrangThai.CHUA_CO_KET_QUA_PHAN_HOI,
                eHoaDonTrangThai.KHONG_HOP_LE,
                eHoaDonTrangThai.LOI_THONG_DIEP,
            ];
        case "da-huy":
            return [eHoaDonTrangThai.DA_HUY];
        case "da-phat-hanh":
        default:
            return [
                eHoaDonTrangThai.DA_PHAT_HANH,
                eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
                eHoaDonTrangThai.DA_GUI_CQT_CHUA_PHAN_HOI,
            ];
    }
};

export const canLoadHoaDonList = (
    tab: string | undefined,
    filter: IHoaDonSelectPagingRequest
): boolean =>
    filter.hoa_don_trang_thai_ids.length > 0 || tab === "chua-gui-cqt";

export const buildHoaDonListFilterForTab = (
    tab: string | undefined,
    isMtt: boolean,
    current?: Partial<IHoaDonSelectPagingRequest>
): IHoaDonSelectPagingRequest => {
    const { tu_ngay, den_ngay } = getDefaultDateRange();
    return {
        hoa_don_trang_thai_ids: getTrangThaiIdsForTab(tab),
        loai_hoa_don_ct_id: 0,
        hoa_don_dang_ky_phat_hanh_mau_so: "",
        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
        hoa_don_hinh_thuc_code: isMtt ? "M" : undefined,
        page_index: 0,
        page_size: current?.page_size ?? 20,
        search_key: undefined,
        sort_by: current?.sort_by ?? "ma_so_hoa_don",
        sort_mode: current?.sort_mode ?? eSortMode.DESC,
        tu_ngay: current?.tu_ngay ?? tu_ngay,
        den_ngay: current?.den_ngay ?? den_ngay,
    };
};
