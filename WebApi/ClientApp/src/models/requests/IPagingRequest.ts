import { eSortMode } from "../commons/eSortMode";

export interface IPagingRequest {
    page_size?: number;
    page_index?: number;
    sort_by?: string;
    sort_mode?: eSortMode;
    search_key?: string;
}