import { IHoaDonSelectPagingRequest } from "../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { apiClient } from "../apiClient";
export const thongKeApi = {
    selectHoaDonPaging: (request: IHoaDonSelectPagingRequest) => apiClient.post(`thong-ke/hoa-don`, {
        ...request,
        tu_ngay: request.tu_ngay === "" ? undefined : request.tu_ngay,
        den_ngay: request.den_ngay === "" ? undefined : request.den_ngay,
    }
    ),
    selectHangHoaPaging: (request: IHoaDonSelectPagingRequest) => apiClient.post(`thong-ke/hang-hoa`, {
        ...request,
        tu_ngay: request.tu_ngay === "" ? undefined : request.tu_ngay,
        den_ngay: request.den_ngay === "" ? undefined : request.den_ngay,
    }
    ),
    exportBangKe: (request: IHoaDonSelectPagingRequest, file_name: string) => apiClient.download(`thong-ke/bang-ke/export`, file_name, {
        ...request,
        tu_ngay: request.tu_ngay === "" ? undefined : request.tu_ngay,
        den_ngay: request.den_ngay === "" ? undefined : request.den_ngay,
    }


    ),
    selectTopSoLuong: (request: any) => apiClient.post(`thong-ke/top/so-luong`, request),
    selectTopGiaTri: (request: any) => apiClient.post(`thong-ke/top/gia-tri`, request),
}