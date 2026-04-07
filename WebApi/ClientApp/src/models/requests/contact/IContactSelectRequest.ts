import { IPagingRequest } from "../IPagingRequest";

export interface IContactSelectRequest extends IPagingRequest {
    contact_status_id: number | null;
}