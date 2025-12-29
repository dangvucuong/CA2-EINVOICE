import { IMauHoaDon } from "./IMauHoaDon";

export interface IMauHoaDonVM extends IMauHoaDon{
    loai_hoa_don_ct_template_name: string;
    loai_hoa_don_ct_name: string;
    loai_hoa_don_ct_id: number
}