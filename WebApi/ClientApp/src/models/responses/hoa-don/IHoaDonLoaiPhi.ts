export interface IHoaDonLoaiPhi {
    id: number;
    stt: number;
    hoa_don_id: number;
    ten_le_phi: string;
    so_tien: number;
}
export const IsHoaDonLoaiPhiValid = (loaiPhi?: IHoaDonLoaiPhi) => {
    // if (!loaiPhi?.ten_le_phi) return false;
    // if (!loaiPhi?.so_tien) return false;
    // if ((loaiPhi?.stt ?? 0) <= 0) return false;
   
    return true;
}
