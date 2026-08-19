import { IIHoaDonAddOrEditModel } from "../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { IHoaDonDeletesRequest } from "../../models/requests/hoa-don/IHoaDonDeletesRequest";
import { IHoaDonImportRequest } from "../../models/requests/hoa-don/IHoaDonImportRequest";
import { IHoaDonPhatHanhRequest } from "../../models/requests/hoa-don/IHoaDonPhatHanhRequest";
import { IHoaDonSelectPagingRequest } from "../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { apiClient } from "../apiClient";
export const HOA_DON_API = "hoa-don";
export const HOA_DON_PHATHANH_API = "hoa-don/phat-hanh";


const selectChoPhanHoiCQT = async (data: any) => {
    return await apiClient.post(
        "hoa-don/select-cho-phan-hoi-cqt",
        data
    );
};

const selectChuaGuiCQT = async (data: any) => {
  return await apiClient.post(
    "hoa-don/select-chua-gui-cqt",
    data
  );
};


export const hoaDonApi = {
    selectByDonViPaging: (request: IHoaDonSelectPagingRequest) => apiClient.post(`${HOA_DON_API}/select`, {
        ...request,
        tu_ngay: request.tu_ngay === "" ? undefined : request.tu_ngay,
        den_ngay: request.den_ngay === "" ? undefined : request.den_ngay,
    }
    ),
    getLogs: (hoaDonId: number) => apiClient.get(`${HOA_DON_API}/${hoaDonId}/log`),
    getPrintHtml: (hoaDonId: number, pageSize?: number, inChuyenDoi?: boolean) => apiClient.get(`${HOA_DON_API}/${hoaDonId}/print?page_size=${pageSize ?? 10}&${inChuyenDoi ? `chuyen_doi=${inChuyenDoi}` : ""}`),
    getPreviewHtml: (rq: IIHoaDonAddOrEditModel) => apiClient.put(`${HOA_DON_API}/preview`, rq),
    getBienBanHtml: (hoaDonId: number) => apiClient.get(`${HOA_DON_API}/${hoaDonId}/html-bien-ban`),
    getViewModel: (hoaDonId: number) => apiClient.get(`${HOA_DON_API}/${hoaDonId}`),
    save: (rq: IIHoaDonAddOrEditModel) => apiClient.post(`${HOA_DON_API}`, rq),
    delete: (id: number) => apiClient.delete(`${HOA_DON_API}/${id}`),
    createBase64KySo: (id: number) => apiClient.get(`${HOA_DON_API}/${id}/ky-so`),
    createBase64KySoNguoiBan: (id: number) => apiClient.get(`${HOA_DON_API}/${id}/ky-so-nguoi-ban`),
    createBase64KySoMttDongThoi: (id: number) => apiClient.get(`${HOA_DON_API}/${id}/ky-so-mtt-dong-thoi`),
    phatHanh: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${HOA_DON_API}/phat-hanh`, rq),
    updateKySoSuccess: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${HOA_DON_API}/${rq.id}/ky-so`, rq),
    readFromExcel: (rq: IHoaDonImportRequest) => apiClient.post(HOA_DON_API + "/import/valid", rq),
    importFromExcel: (rq: IHoaDonImportRequest) => apiClient.post(HOA_DON_API + "/import", rq),
    sendEmail: (rq: any) => apiClient.post(HOA_DON_API + "/send-email", rq),
    sendEmailCustom: (rq: any) => apiClient.post(HOA_DON_API + "/send-email-custom", rq),
    deletes: (rq: IHoaDonDeletesRequest) => apiClient.post(HOA_DON_API + "/deletes", rq),
    selectByIds: (rq: IHoaDonDeletesRequest) => apiClient.post(HOA_DON_API + "/select-by-ids", rq),
    searchByMaTraCuu: (key: string) => apiClient.get(HOA_DON_API + "/ma-tra-cuu/" + key),
    createViewLink: (hoaDonId: number) => apiClient.get(`${HOA_DON_API}/${hoaDonId}/link`),
    validateViewLink: (hoaDonId: number, hash: string) => apiClient.get(`${HOA_DON_API}/${hoaDonId}/link/validate?hash=${hash}`),
    createXmlKySos: (rq: IHoaDonDeletesRequest) => apiClient.post(`${HOA_DON_API}/ky-so-multiple`, rq),
    // update phan tach danh sách api theo 2 procerdure mới
    selectChoPhanHoiCQT,selectChuaGuiCQT,
    guiLaiCQT: (id: number) =>
    apiClient.post(
        `hoa-don/gui-lai-cqt/${id}`
    ),
    capNhatKetQuaCQT: (id: number) =>
        apiClient.post(`hoa-don/cap-nhat-ket-qua-cqt/${id}`),
    khoiPhucTrangThai: (id: number) =>
        apiClient.post(`hoa-don/khoi-phuc-trang-thai/${id}`),
}