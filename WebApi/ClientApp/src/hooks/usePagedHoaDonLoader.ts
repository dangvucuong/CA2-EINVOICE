import { useCallback, useEffect, useRef, useState } from "react";
import { NotifyHelper } from "../helpers/toast";
import { IHoaDonSelectPagingRequest } from "../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IBaseRespone } from "../models/responses/IBaseRespone";
import {
    IPagingResultSummary,
    getPagingSummary,
} from "../models/responses/IBasePagingRespone";
import { IHoaDon } from "../models/responses/hoa-don/IHoaDon";

const MAX_LOAD_RETRIES = 2;
const RETRY_DELAY_MS = 600;

/** Load danh sách HĐ phân trang trực tiếp qua API, có retry khi lỗi. */
export const usePagedHoaDonLoader = (
    filter: IHoaDonSelectPagingRequest,
    fetchFn: (
        filter: IHoaDonSelectPagingRequest
    ) => Promise<IBaseRespone>
) => {
    const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);
    const [pagingResult, setPagingResult] = useState<IPagingResultSummary>();
    const [isLoading, setIsLoading] = useState(false);
    const filterKey = JSON.stringify(filter);
    const retryCountRef = useRef(0);

    const loadData = useCallback(
        async (attempt = 0, cancelled?: () => boolean) => {
            if (cancelled?.()) {
                return;
            }
            setIsLoading(true);
            try {
                const res = await fetchFn(filter);
                if (cancelled?.()) {
                    return;
                }
                if (res.is_success) {
                    retryCountRef.current = 0;
                    setHoaDons(res.data.data);
                    setPagingResult(getPagingSummary(res.data));
                } else {
                    NotifyHelper.Error(
                        res.message ?? "Không thể tải danh sách hóa đơn"
                    );
                    if (attempt < MAX_LOAD_RETRIES) {
                        retryCountRef.current = attempt + 1;
                        window.setTimeout(
                            () => loadData(attempt + 1, cancelled),
                            RETRY_DELAY_MS
                        );
                        return;
                    }
                    setHoaDons([]);
                    setPagingResult(undefined);
                }
            } finally {
                if (!cancelled?.()) {
                    setIsLoading(false);
                }
            }
        },
        [filter, fetchFn]
    );

    useEffect(() => {
        retryCountRef.current = 0;
        let cancelled = false;
        const isCancelled = () => cancelled;
        loadData(0, isCancelled);
        return () => {
            cancelled = true;
        };
    }, [filterKey, loadData]);

    return {
        hoaDons,
        pagingResult,
        isLoading,
        reload: () => loadData(0),
    };
};
