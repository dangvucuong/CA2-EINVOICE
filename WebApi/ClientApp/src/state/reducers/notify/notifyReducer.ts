import { INotifyActionType, eNotifyActionTypeIds } from "../../action-types/notify/INotifyActionType";
import { INotifyReducer } from "../../reducer-models/notify/INotifyReducer";
const iniState: INotifyReducer = {

}
export const notifyReducer = (state: INotifyReducer = iniState, action: INotifyActionType): INotifyReducer => {
    switch (action.type) {
        case eNotifyActionTypeIds.LOAD_SUMMARY_SUCCESS:
            return {
                ...state,
                notifySummary: action.payload
            }

        default:
            return state;
    }
}