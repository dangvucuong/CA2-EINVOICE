import { PencilIcon, PlusIcon } from "@primer/octicons-react";
import { eToKhaiLogType } from "../models/commons/eToKhaiLogType";
import { eHoaDonLogType } from "../models/commons/eHoaDonLogType";


const hoaDonLogTypes = [
    {
        id: eToKhaiLogType.TAO_MOI,
        color: "#ffd78e",
        icon: PlusIcon
    },
    {
        id: eToKhaiLogType.CO_QUAN_THUE_PHAN_HOI,
        color: "#ffd78e",
        icon: PencilIcon
    },
    {
        id: eHoaDonLogType.CAP_NHAT,
        color: "#a4f287",
        icon: PencilIcon
    },
    
]

export const useHoaDonLogTypes = () => {
    return {
        hoaDonLogTypes: hoaDonLogTypes
    };
}