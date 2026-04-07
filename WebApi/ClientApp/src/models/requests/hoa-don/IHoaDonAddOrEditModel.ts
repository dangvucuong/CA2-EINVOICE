import { IHoaDon } from "../../responses/hoa-don/IHoaDon";
import { IHoaDonHangHoa } from "../../responses/hoa-don/IHoaDonHangHoa";
import { IHoaDonLoaiPhi } from "../../responses/hoa-don/IHoaDonLoaiPhi";

export interface IIHoaDonAddOrEditModel extends IHoaDon {
    hoang_hoas: IHoaDonHangHoa[],
    loai_phis: IHoaDonLoaiPhi[]
}