import { NotifyHelper } from "../../../helpers/toast";
import { ILoaiHoaDon } from "../../../models/responses/hoa-don/ILoaiHoaDon";
import { ILoaiHoaDonLoadError, ILoaiHoaDonLoadStart, ILoaiHoaDonLoadSuccess, eLoaiHoaDonActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonActionType";
import { baseAction } from "../baseAction";


export const loaiHoaDonAction = {
    loadStart: (): ILoaiHoaDonLoadStart =>
        baseAction(eLoaiHoaDonActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: ILoaiHoaDon[]): ILoaiHoaDonLoadSuccess =>
        baseAction(eLoaiHoaDonActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): ILoaiHoaDonLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eLoaiHoaDonActionTypeIds.LOAD_ERROR, message)
    }
   
}