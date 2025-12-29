import { NotifyHelper } from "../../../helpers/toast";
import { ILoaiHoaDonCT } from "../../../models/responses/hoa-don/ILoaiHoaDonCT";
import { ILoaiHoaDonCTLoadError, ILoaiHoaDonCTLoadStart, ILoaiHoaDonCTLoadSuccess, eLoaiHoaDonCTActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTActionType";
import { baseAction } from "../baseAction";


export const loaiHoaDonCTAction = {
    loadStart: (): ILoaiHoaDonCTLoadStart =>
        baseAction(eLoaiHoaDonCTActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: ILoaiHoaDonCT[]): ILoaiHoaDonCTLoadSuccess =>
        baseAction(eLoaiHoaDonCTActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): ILoaiHoaDonCTLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eLoaiHoaDonCTActionTypeIds.LOAD_ERROR, message)
    }
   
}