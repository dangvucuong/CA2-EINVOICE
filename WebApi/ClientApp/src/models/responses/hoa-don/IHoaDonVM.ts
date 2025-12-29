import { IHoaDon } from "./IHoaDon";
import { IHoaDonHangHoa } from "./IHoaDonHangHoa";
import { IHoaDonLoaiPhi } from "./IHoaDonLoaiPhi";

export interface IHoaDonVM extends IHoaDon {
    hang_hoas: IHoaDonHangHoa[]
    loai_phis: IHoaDonLoaiPhi[]
}