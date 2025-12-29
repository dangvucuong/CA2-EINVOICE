export interface IBangTongHopDuLieu  {
    id: number;
    donvi_ma_dv: string;
    bang_tong_hop_du_lieu_type: string;
    ngay: string;
    thang: number;
    quy: number;
    nam: number;
    ky_du_lieu: string;
    is_lan_dau: boolean;
    // bo_sung_lan_thu: number;
    so_thu_tu_lan_bo_sung: number;
    phat_hanh_uuid?: string;
    user_id_phathanh?: string;
    thoi_gian_gui?: string;
    bang_tong_hop_du_lieu_trang_thai_id: number;
    bang_tong_hop_du_lieu_loai_hang_hoa_id: number;
    ket_qua_phat_hanh: string;
    so_luong_hoa_don: number;
}