import { apiClient } from "../apiClient";
export const WATERMARK_TEMPLATE_API_ENDPOIT = "watermark";
export const watermarkTemplateApi = {
    getAll: () => apiClient.get(`${WATERMARK_TEMPLATE_API_ENDPOIT}`),
   
}