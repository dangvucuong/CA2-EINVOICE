export interface IToKhai {
  id: number;
  to_khai_status_id: number;
  loai_to_khai_id: number;
  ma_to_khai: string;
  ngay_lap: string;
  mst: string;
  donvi_ma_dv: string;
  nguoi_nop_thue: string;
  nguoi_lien_he: string;
  co_quan_thue: string;
  dia_chi_lien_he: string;
  email_lien_he: string;
  dien_thoai_lien_he: string;
  ho_chieu: string;
  is_hoadon_co_ma_cqt: boolean;
  is_hoadon_co_ma_cqt_mtt: boolean;
  is_hoadon_khong_co_ma_cqt: boolean;
  is_khong_phai_tra_tien_dich_vu: boolean;
  is_doanh_nghiep_vvn_kho_khan: boolean;
  is_doanh_nghiep_vvn_khac: boolean;

  is_chuyen_du_lieu_truc_tiep: boolean;
  is_chuyen_lieu_thong_qua_to_chuc: boolean;

  is_chuyen_day_du_tung_hoadon: boolean;
  is_chuyen_theo_bang_tonghop: boolean;

  is_sd_hoadon_gtgt: boolean;
  is_sd_hoadon_banhang: boolean;
  is_sd_hoadon_khac: boolean;
  is_sd_chungtu_giong_hoadon: boolean;

  is_ban_tai_san_cong: boolean;
  is_ban_hang_du_tru_quoc_gia: boolean;

  noi_lap: string;
  ngay_co_hieu_luc: string;
  cks_user_id: number;
  cks_serial_no: string;
  cks_user_full_name: string;
  is_camket: boolean;
  ngay_tao: string;
  nguoi_tao: string;
  co_quan_thue_id: number;
  ma_cqt: string;
  phat_hanh_uuid: string;

  dai_dien_phap_luat_ho_ten?: string;
  dai_dien_phap_luat_dien_thoai?: string;
  dai_dien_phap_luat_dien_cccd?: string;
  dai_dien_phap_luat_dien_ngay_sinh?: string;
  dai_dien_phap_luat_dien_gioi_tinh?: number;
  is_co_quan_xu_ly_tai_san_cong?: boolean;
  is_sd_hoadon_gtgt_bien_lai?: boolean;
  is_sd_hoadon_banhang_bien_lai?: boolean;
  is_sd_hoadon_thuong_mai?: boolean;

  to_chuc_cap_giay_phep_json?: string;
  to_chuc_truyen_nhan_json?: string;
  tam_ngung_su_dung?: string;

  tam_ngung_ten_to_chuc?: string;
  tam_ngung_mst?: string;
  tam_ngung_tu_ngay?: string;
  tam_ngung_den_ngay?: string;
  is_tam_ngung_su_dung?: boolean;
}
