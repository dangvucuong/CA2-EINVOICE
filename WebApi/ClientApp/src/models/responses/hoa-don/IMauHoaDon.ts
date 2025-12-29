export interface IMauHoaDon {
  id: number;
  loai_hoa_don_ct_template_id: number;
  donvi_ma_dv: string;
  name: string;
  so_qd?: string;
  ngay_qd?: string;
  logo_path: string;
  watermark_path: string;
  vien_path: string;
  xslt_path?: string;
  is_show_wattermark_inner_table?: boolean;
  logo_position?: string;
  is_locked?: boolean;
  is_active: boolean;
  watermark_opacity?: number;
  advanced_settings_json?: string;
}
