import { IPagingRequest } from "../IPagingRequest";

export interface IUserLoadByDonViRequest extends IPagingRequest {
    donvi_ma_dv: string
}