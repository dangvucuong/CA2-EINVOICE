import { IHoaDonKyLoRequest } from "../../models/requests/hoa-don/IHoaDonKyLoRequest";
import { apiClient } from "../apiClient";

export const hoaDonKyLoApi = {

    createXmlKySos: (rq: IHoaDonKyLoRequest) => apiClient.post(`hoa-don-ky-lo/ky-so?notify=true`, rq),
    kySo: (id: number) => apiClient.post(`hoa-don-ky-so-remote/${id}/ky-so?notify=true`),
    kySoVaPhatHanh: (id: number) => apiClient.post(`hoa-don-ky-so-remote/${id}/ky-so-va-phat-hanh?notify=true`),


}