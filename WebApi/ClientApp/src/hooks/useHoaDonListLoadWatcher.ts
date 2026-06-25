import { useEffect, useRef } from "react";
import { NotifyHelper } from "../helpers/toast";
import { IHoaDonSelectPagingRequest } from "../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { rootAction } from "../state/actions/rootAction";
import { eReducerStatusBase } from "../state/reducer-models/eReducerStatusBase";
import { canLoadHoaDonList } from "../utils/hoaDonListFilter";
import { useAppDispatch } from "./useAppDispatch";
import { useAppSelector } from "./useAppSelector";

const hoaDonAction = rootAction.hoaDon.hoaDonAction;
const MAX_LOAD_RETRIES = 2;

/** Theo dõi lỗi load danh sách HĐ qua saga và tự retry. */
export const useHoaDonListLoadWatcher = (
    tab: string | undefined,
    filter: IHoaDonSelectPagingRequest,
    applyListFilter: <T extends object>(f: T) => T
) => {
    const dispatch = useAppDispatch();
    const { status } = useAppSelector((x) => x.hoaDon.hoaDonReducer);
    const retryCountRef = useRef(0);
    const lastErrorShownRef = useRef(false);

    useEffect(() => {
        if (status === eReducerStatusBase.is_loaded) {
            retryCountRef.current = 0;
            lastErrorShownRef.current = false;
            return;
        }

        if (status !== eReducerStatusBase.is_load_err) {
            return;
        }

        if (!lastErrorShownRef.current) {
            NotifyHelper.Error("Không thể tải danh sách hóa đơn");
            lastErrorShownRef.current = true;
        }

        if (
            retryCountRef.current < MAX_LOAD_RETRIES &&
            canLoadHoaDonList(tab, filter)
        ) {
            retryCountRef.current += 1;
            dispatch(
                hoaDonAction.loadStart({
                    ...applyListFilter(filter),
                    tab,
                })
            );
        }
    }, [status, tab, filter, dispatch, applyListFilter]);
};
