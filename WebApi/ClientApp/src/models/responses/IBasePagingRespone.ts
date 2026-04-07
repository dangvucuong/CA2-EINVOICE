
export interface IPagingRespone<T> extends IPagingResultSummary {
    data: T;

}
export interface IPagingResultSummary {
    total_count: number;
    page_count: number;
    page_number: number;
    page_size: number;
}
export const getPagingSummary = (paging_res: IPagingResultSummary): IPagingResultSummary => {
    const obj: IPagingResultSummary = {
        total_count: paging_res.total_count,
        page_count: paging_res.page_count,
        page_number: paging_res.page_number,
        page_size: paging_res.page_size
    }
    return obj;
}
