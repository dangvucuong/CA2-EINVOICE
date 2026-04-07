import { eHoaDonLogType } from "../models/commons/eHoaDonLogType";
import { PaperAirplaneIcon, PlusIcon, ShieldCheckIcon, PencilIcon , KeyIcon, ShieldSlashIcon} from "@primer/octicons-react";


const toKhaiLogTypes = [
    {
        id: eHoaDonLogType.TAO_MOI,
        color: "#ffd78e",
        icon: PlusIcon
    },
    {
        id: eHoaDonLogType.CAP_NHAT,
        color: "#ffd78e",
        icon: PencilIcon
    },
    {
        id: eHoaDonLogType.KY,
        color: "#a4f287",
        icon: KeyIcon
    },
    {
        id: eHoaDonLogType.GUI_CQT,
        color: "#a2eeef",
        icon: PaperAirplaneIcon
    },
    {
        id: eHoaDonLogType.CQT_TU_CHOI,
        color: "#d73a4a",
        icon: ShieldSlashIcon
    },
    {
        id: eHoaDonLogType.CQT_DONG_Y,
        color: "#0cf478",
        icon: ShieldCheckIcon
    }
]

export const useToKhaiLogTypesHook = () => {
    return {
        toKhaiLogTypes: toKhaiLogTypes
    };
}