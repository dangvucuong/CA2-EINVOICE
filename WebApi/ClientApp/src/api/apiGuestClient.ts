
import { appInfo } from '../AppInfo';
import { IBaseRespone } from '../models/responses/IBaseRespone';
import { axiosClient } from './axiosClient';
import { getApiErrorMessage, getApiStatusCode } from './apiErrorHelper';

const handleApiError = (error: any): IBaseRespone => ({
    status_code: getApiStatusCode(error),
    is_success: false,
    message: getApiErrorMessage(error)
});

const apiGuestClient = {
    get: async (path: string): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {

            const config = {
                headers: {
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.get<any, IBaseRespone>(url, config);
            return res;
        } catch (error: any) {
            return handleApiError(error);
        }
    }
    ,
    post: async (path: string, data?: any, domain?: string): Promise<IBaseRespone> => {
        const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

        try {
            const config = {
                headers: {
                    language: localStorage.getItem("language"),
                },
                timeout: 1000 * 60 * 10
            }
            const res = await axiosClient.post<any, IBaseRespone>(url, data, config);
            return res;
        } catch (error: any) {
            return handleApiError(error);
        }
    },
    put: async (path: string, data?: any): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {
            const config = {

                headers: {
                    language: localStorage.getItem("language"),
                },
                timeout: 1000 * 60 * 10
            }
            const res = await axiosClient.put<any, IBaseRespone>(url, data, config);
            return res;
        } catch (error: any) {
            return handleApiError(error);
        }
    },
    delete: async (path: string): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {

            const config = {
                headers: {
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.delete<any, IBaseRespone>(url, config);
            return res;
        } catch (error: any) {
            return handleApiError(error);
        }
    },

}
export { apiGuestClient };
