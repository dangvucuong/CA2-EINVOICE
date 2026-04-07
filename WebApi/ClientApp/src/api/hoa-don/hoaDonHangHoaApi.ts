import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import { apiClient } from "../apiClient";
export const HOA_DON_HANG_HOA_API = "hoa-don-hang-hoa";
export const hoaDonHangHoaApi = {
    readFromExcel: (rq: IUploadRespone) => apiClient.post(HOA_DON_HANG_HOA_API + "/import/valid", rq)
}