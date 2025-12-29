import { IHoaDonHangHoa } from "./IHoaDonHangHoa";

export interface IHoaDonHangHoaVM extends IHoaDonHangHoa {
    loai_hoa_don_ct_name: string;
    hoa_don_dang_ky_phat_hanh_mau_so: string;
    hoa_don_dang_ky_phat_hanh_ky_hieu: string;
    ma_so_hoa_don: string | null;
    ngay_hoa_don: string;
}