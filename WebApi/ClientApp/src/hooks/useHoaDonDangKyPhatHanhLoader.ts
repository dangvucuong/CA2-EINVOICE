import { useEffect, useRef } from "react";
import { useAppDispatch } from "./useAppDispatch";
import { useAppSelector } from "./useAppSelector";
import { rootAction } from "../state/actions/rootAction";
import { eReducerStatusBase } from "../state/reducer-models/eReducerStatusBase";

const MAX_LOAD_RETRIES = 2;

/** Tải danh sách đăng ký phát hành; tự retry khi chưa khởi tạo hoặc lỗi load. */
export const useHoaDonDangKyPhatHanhLoader = () => {
    const dispatch = useAppDispatch();
    const retryCountRef = useRef(0);
    const { hoaDonDangKyPhatHanhs, status } = useAppSelector(
        (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer
    );

    useEffect(() => {
        if (status === eReducerStatusBase.is_loaded) {
            retryCountRef.current = 0;
            return;
        }

        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.loadStart());
            return;
        }

        if (
            status === eReducerStatusBase.is_load_err &&
            retryCountRef.current < MAX_LOAD_RETRIES
        ) {
            retryCountRef.current += 1;
            dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.loadStart());
        }
    }, [status, dispatch]);

    return {
        hoaDonDangKyPhatHanhs,
        status,
        isLoading: status === eReducerStatusBase.is_loading,
        isLoadError: status === eReducerStatusBase.is_load_err,
    };
};
