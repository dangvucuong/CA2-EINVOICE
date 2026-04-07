import { IHoaDon } from "../hoa-don/IHoaDon";
import { IBangTongHopDuLieu } from "./IBangTongHopDuLieu";

export interface IBangTongHopAddOrEditRequest extends IBangTongHopDuLieu {
    hoa_don_ids: number[]
    hoa_dons: IHoaDon[]
}