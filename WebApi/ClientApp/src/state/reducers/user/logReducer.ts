import { eSortMode } from "../../../models/commons/eSortMode";
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone";
import { ILogActionType, eLogActionTypeIds } from "../../action-types/user/ILogActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { ILogReducer } from "../../reducer-models/user/ILogReducer";
const iniState: ILogReducer = {
    status: eReducerStatusBase.is_not_initialization,
    logs: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const logReducer = (state: ILogReducer = iniState, action: ILogActionType): ILogReducer => {
    switch (action.type) {
        case eLogActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eLogActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                logs: action.payload.data,
                paging_res: getPagingSummary(action.payload)

            }
        case eLogActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        case eLogActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }

        default:
            return state;
    }

}