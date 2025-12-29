export interface IHoaDon {
  id: number;
  phien_ban: string;
  ten_hoa_don: string;
  hoa_don_dang_ky_phat_hanh_mau_so: string;
  hoa_don_dang_ky_phat_hanh_ky_hieu: string;
  loai_hoa_don_ct_id: number;
  so_hoa_don: string;
  ma_so_hoa_don: string;
  ngay_hoa_don: string;
  loai_tien: string;
  ty_gia: number;
  chinhanh_code: string;
  nguoi_ban_mst: string;
  nguoi_ban_ten_donvi: string;
  nguoi_ban_dia_chi: string;
  nguoi_ban_stk: string;
  nguoi_ban_ngan_hang: string;
  nguoi_ban_dien_thoai: string;
  nguoi_ban_fax: string;
  nguoi_ban_email: string;
  nguoi_ban_website: string;
  nguoi_mua_mst: string;
  nguoi_mua_ten_donvi: string;
  nguoi_mua_ten: string;
  nguoi_mua_dia_chi: string;
  nguoi_mua_stk: string;
  nguoi_mua_ngan_hang: string;
  nguoi_mua_dien_thoai: string;
  nguoi_mua_fax: string;
  nguoi_mua_email: string;
  nguoi_mua_website: string;
  hinh_thuc_tt: string;
  hoa_don_trang_thai_id: number;
  tong_tien_truong_thue: number;
  tong_tien_thue: number;
  tong_tien_phi: number;
  tong_tien_thanh_toan: number;
  tong_tien_chiet_khau: number;
  tong_tien_chu: string;
  ma_tra_cuu: string;
  qr_code: string;
  hoa_don_hinh_thuc_id: number;

  hoa_don_id_goc: number;
  hoa_don_nghi_dinh_id: number;
  hoa_don_nghi_dinh_id_goc: number;

  hoa_don_dang_ky_phat_hanh_mau_so_goc: string;
  hoa_don_dang_ky_phat_hanh_ky_hieu_goc: string;
  ma_so_hoa_don_goc?: number;
  ngay_hoa_don_goc?: string;

  ket_qua_phat_hanh: string;
  hoa_don_ly_do_dieu_chinh_id: number;
  is_ky_so_succes: boolean;

  xuat_kho_vc_lenh_dieu_dong_noi_bo: string;

  ma_dv_ngan_sach?: string;
  xuat_kho_dl_hop_dong_kinh_te_so: string;
  xuat_kho_dl_hop_dong_ngay: string;

  xuat_kho_hop_dong_so: string;
  xuat_kho_nguoi_xuat_hang: string;
  xuat_kho_nguoi_van_chuyen: string;
  xuat_kho_phuong_tien_van_chuyen: string;
  phat_hanh_ma_ketqua_cqt: string;
  xuat_kho_dia_chi: string;

  hoa_don_ids_thaythe_dieuchinh?: string;
  ma_dai_ly?: string;
  ten_dai_ly?: string;
  ma_so_hoa_don_mtt?: string;
  so_tien_tang_giam?: number;
  so_tien_tang_giam_tien_hang?: number;
  so_tien_tang_giam_tien_thue?: number;
  giam_thue_phan_tram?: number;
  giam_thue_ty_le?: number;
  giam_thue_thanh_tien?: number;
  invoice_id?: string;
  ly_do_dieu_chinh?: string;

  // Thông tin khác
  thong_tin_khac?: any;
  NgayDenNgayDi?: string;
  TenTau?: string;
  SoThamChieu?: string;
  NoiDiNoiDen?: string;
}
