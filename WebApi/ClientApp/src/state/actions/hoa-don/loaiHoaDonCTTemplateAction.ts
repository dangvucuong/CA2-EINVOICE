import { NotifyHelper } from "../../../helpers/toast";
import { ILoaiHoaDonCTTemplate } from "../../../models/responses/hoa-don/ILoaiHoaDonCTTemplate";
import { ILoaiHoaDonCTTemplateLoadError, ILoaiHoaDonCTTemplateLoadStart, ILoaiHoaDonCTTemplateLoadSuccess, eLoaiHoaDonCTTemplateActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTTemplateActionType";
import { baseAction } from "../baseAction";


export const loaiHoaDonCTTemplateAction = {
    loadStart: (): ILoaiHoaDonCTTemplateLoadStart =>
        baseAction(eLoaiHoaDonCTTemplateActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: ILoaiHoaDonCTTemplate[]): ILoaiHoaDonCTTemplateLoadSuccess =>
        baseAction(eLoaiHoaDonCTTemplateActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): ILoaiHoaDonCTTemplateLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eLoaiHoaDonCTTemplateActionTypeIds.LOAD_ERROR, message)
    }
   
}