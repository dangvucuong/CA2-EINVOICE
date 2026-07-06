import { IHoaDonPhatHanhRequest } from "../../models/requests/hoa-don/IHoaDonPhatHanhRequest";
import { IBangTongHopAddOrEditRequest } from "../../models/responses/bang-tong-hop/IBangTongHopAddOrEditRequest";
import { apiClient } from "../apiClient";
export const BANG_TONG_HOP_API = "bang-tong-hop";
export const bangTongHopApi = {
    getByDonVi: () => apiClient.get(`${BANG_TONG_HOP_API}`),
    getUsedHoaDonIds: () => apiClient.get(`${BANG_TONG_HOP_API}/used-hoa-don-ids`),
    selectHoaDonForTongHop: (tu_ngay?: string, den_ngay?: string) => {
        const params = new URLSearchParams();
        if (tu_ngay) params.append("tu_ngay", tu_ngay);
        if (den_ngay) params.append("den_ngay", den_ngay);
        const qs = params.toString();
        return apiClient.get(
            `${BANG_TONG_HOP_API}/hoa-don-tong-hop${qs ? `?${qs}` : ""}`,
        );
    },
    getLogs: (id: number) => apiClient.get(`${BANG_TONG_HOP_API}/${id}/log`),
    save: (rq: IBangTongHopAddOrEditRequest) => apiClient.post(`${BANG_TONG_HOP_API}`, rq),
    selectViewById: (id: number) => apiClient.get(`${BANG_TONG_HOP_API}/${id}`),
    delete: (id: number) => apiClient.delete(`${BANG_TONG_HOP_API}/${id}`),
    createBase64KySo: (id: number) => apiClient.get(`${BANG_TONG_HOP_API}/${id}/ky-so`),
    phatHanh: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${BANG_TONG_HOP_API}/phat-hanh`, rq),
    updateKySoSuccess: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${BANG_TONG_HOP_API}/${rq.id}/ky-so`, rq),
    
}