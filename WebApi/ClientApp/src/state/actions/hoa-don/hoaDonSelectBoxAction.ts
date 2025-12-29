import { NotifyHelper } from "../../../helpers/toast"
import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest"
import { IHoaDonPaging } from "../../../models/responses/hoa-don/IHoaDonPaging"
import { IHoaDonSelectBoxChangeFilter, IHoaDonSelectBoxChangeSelectedIds, IHoaDonSelectBoxLoadError, IHoaDonSelectBoxLoadStart, IHoaDonSelectBoxLoadSuccess, eHoaDonSelectBoxActionTypeIds } from "../../action-types/hoa-don/IHoaDonSelectBoxActionType"
import { baseAction } from "../baseAction"

export const hoaDonSelectBoxAction = {
    loadStart: (rq: IHoaDonSelectPagingRequest): IHoaDonSelectBoxLoadStart => baseAction(eHoaDonSelectBoxActionTypeIds.LOAD_START, rq),
    loadSuccess: (data: IHoaDonPaging): IHoaDonSelectBoxLoadSuccess => baseAction(eHoaDonSelectBoxActionTypeIds.LOAD_SUCCESS, data),
    loadError: (m: string): IHoaDonSelectBoxLoadError => {
        NotifyHelper.Error(m)
        return baseAction(eHoaDonSelectBoxActionTypeIds.LOAD_ERROR, m);
    },
    changeFilter: (rq: IHoaDonSelectPagingRequest): IHoaDonSelectBoxChangeFilter => baseAction(eHoaDonSelectBoxActionTypeIds.CHANGE_FILTER, rq),
    changeSelectedIds: (ids: number[]): IHoaDonSelectBoxChangeSelectedIds => baseAction(eHoaDonSelectBoxActionTypeIds.CHANGE_SELECTED_ID, ids),
}