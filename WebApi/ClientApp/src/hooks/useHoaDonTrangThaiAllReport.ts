import { useEffect } from "react";
import { rootAction } from "../state/actions/rootAction";
import { useAppDispatch } from "./useAppDispatch";
import { useAppSelector } from "./useAppSelector";
import { eReducerStatusBase } from "../state/reducer-models/eReducerStatusBase";


export const useHoaDonTrangThaiAllReport = () => {
    const { status, data } = useAppSelector(x => x.dashBoard.trangThaiReportAll)
    const dispatch = useAppDispatch();
    const handleSelectReport = () => {
        dispatch(rootAction.dashBoard.trangThaiReportLoadAllStart())
    }
    useEffect(() => {
        // debugger
        if (status === eReducerStatusBase.is_need_reload || status === eReducerStatusBase.is_not_initialization) {
            handleSelectReport();
        }
    }, [status])
    return {
        dataReport: data,
        handleSelectReport
    }
}