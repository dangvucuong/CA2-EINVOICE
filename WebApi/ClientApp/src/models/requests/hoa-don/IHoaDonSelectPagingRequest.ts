import { IPagingRequest } from "../IPagingRequest";

export interface IHoaDonSelectPagingRequest extends IPagingRequest {
    hoa_don_trang_thai_ids: number[],
    loai_hoa_don_ct_id: number,
    hoa_don_dang_ky_phat_hanh_mau_so: string,
    hoa_don_dang_ky_phat_hanh_ky_hieu: string,
    hoa_don_hinh_thuc_id?: number,
    tu_ngay?: string,
    den_ngay?: string,
    hoa_don_hinh_thuc_code?: string,
    nguoi_mua_mst?: string,
    ma_dai_ly?: string,
    tab?: string;
}