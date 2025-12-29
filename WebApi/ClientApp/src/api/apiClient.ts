import jwt_decode from 'jwt-decode';
import { axiosClient } from './axiosClient';
import { IBaseRespone } from '../models/responses/IBaseRespone';
import { appInfo } from '../AppInfo';
import { AxiosRequestConfig } from 'axios';

export const formatQueryString = (obj: any): string => {
    if (obj) {
        return Object.keys(obj)
            .filter(x => obj[x] != undefined)
            .map(key => {
                return `${key}=${encodeURIComponent(obj[key])}`;
            })
            .join('&');
        // return new URLSearchParams(obj).toString();
    }
    return "";
}

type jwt_decodeResult = {
    exp: any
}
let refreshTokenRequest: Promise<any> | null;

export const clearAccessToken = () => {
    localStorage.removeItem("access_token");
}

export const clearRefreshToken = () => {
    localStorage.removeItem("refresh_token");
}
export const saveAccessToken = (access_token: string) => {
    localStorage.setItem("access_token", access_token)
}

export const saveRefreshToken = (refresh_token: string) => {
    localStorage.setItem("refresh_token", refresh_token)
}


const checkIsTokenExpired = (): boolean => {
    // return true;
    try {
        const data: jwt_decodeResult = jwt_decode(localStorage.access_token)
        if (localStorage.access_token && data) {
            const exp = data.exp;
            if (parseInt(exp) > Math.floor(Date.now() / 1000)) {
                return false;
            } else {
                return true;
            }
        }
        return false;
    } catch (error) {
        return false;
    }
}

const refreshToken = () => {
    return new Promise(resolve => {
        setTimeout(async () => {
            try {
                // debugger
                const res: IBaseRespone = await axiosClient.post(`${appInfo.baseApiURL}/account/token/refresh`, {
                    access_token: localStorage.access_token,
                    refresh_token: localStorage.refresh_token
                });

                if (res.is_success) {
                    resolve(res.message)
                } else {
                    resolve("");
                }
            } catch (error) {
                debugger
                localStorage.removeItem("access_token");
                window.location.reload();
            }

        }, 1000);
    });
};
const checkRemoveTokenAfterLongTime = async () => {
    if (localStorage.getItem("access_token")) {
        const lastActiveTime = localStorage.getItem('last_active_time');
        const origin = window.location.origin;
        const loginUrl = `${origin}/login`;
        if (!lastActiveTime) {
            localStorage.removeItem("access_token");
            localStorage.removeItem("refresh_token");
            window.location.href = loginUrl;
        } else {
            var last_active_time = isNaN(parseInt(lastActiveTime)) ? 0 : parseInt(lastActiveTime)
            var currentTime = new Date().getTime();
            // console.log({
            //     last_active_time,
            //     currentTime
            // });
            const LOGOUT_AFTER_MILIS = 24 * 60 * 60 * 1000; //tính bằng mili giây
            const difference = Math.abs(currentTime - last_active_time); // Tính sự chênh lệch tuyệt đối giữa hai giá trị thời gian
            if (difference > LOGOUT_AFTER_MILIS) {
                localStorage.removeItem("access_token");
                localStorage.removeItem("refresh_token");
                window.location.href = loginUrl;
            }

        }
    }
}
const configIfTokenExpired = async () => {
    await checkRemoveTokenAfterLongTime();
    const isTokenExpired = checkIsTokenExpired();
    if (isTokenExpired) {
        refreshTokenRequest = refreshTokenRequest
            ? refreshTokenRequest
            : refreshToken();

        const newTokens = await refreshTokenRequest;
        refreshTokenRequest = null;
        if (newTokens === "") {
            return {
                is_success: false,
                message: "Token expired."
            }
        }
        const new_access_token = newTokens.split(' ')[0];
        const new_refresh_token = newTokens.split(' ')[1];
        localStorage.access_token = new_access_token;
        localStorage.refresh_token = new_refresh_token;

    }
}
const apiClient = {
    get: async (path: string): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {
            await configIfTokenExpired();
            const config = {
                headers: {
                    language: localStorage.getItem("language"),
                    Authorization: `Bearer ${localStorage.access_token}`,
                }
            }
            const res = await axiosClient.get<any, IBaseRespone>(url, config);
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return res;
        } catch (error: any) {
            if (error.response.status === 401) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    }
    ,
    post: async (path: string, data?: any, domain?: string): Promise<IBaseRespone> => {
        const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

        try {
            await configIfTokenExpired();
            const config = {
                headers: {
                    Authorization: `Bearer ${localStorage.access_token}`,
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.post<any, IBaseRespone>(url, data, config);
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return res;
        } catch (error: any) {
            //console.log('object', error);
            if (error.response.status === 403) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    },
    put: async (path: string, data?: any): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {
            await configIfTokenExpired();
            const config = {

                headers: {
                    Authorization: `Bearer ${localStorage.access_token}`,
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.put<any, IBaseRespone>(url, data, config);
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return res;
        } catch (error: any) {
            if (error.response.status === 403) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    },
    delete: async (path: string): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {
            await configIfTokenExpired();
            const config = {
                headers: {
                    Authorization: `Bearer ${localStorage.access_token}`,
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.delete<any, IBaseRespone>(url, config);
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return res;
        } catch (error: any) {
            if (error.response.status === 401) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    },
    upload: async (path: string, data?: any): Promise<IBaseRespone> => {
        const url = `${appInfo.baseApiURL}/${path}`
        try {
            await configIfTokenExpired();
            const config = {

                headers: {
                    Authorization: `Bearer ${localStorage.access_token}`, 'Content-Type': 'multipart/form-data',
                    language: localStorage.getItem("language"),
                }
            }
            const res = await axiosClient.post<any, IBaseRespone>(url, data, config);
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return res;
        } catch (error: any) {
            if (error.response.status == 401) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    },
    download: async (path: string, file_name: string, data?: any, domain?: string): Promise<IBaseRespone> => {
        const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

        try {
            await configIfTokenExpired();
            const config: AxiosRequestConfig = {
                headers: {
                    Authorization: `Bearer ${localStorage.access_token}`,
                    language: localStorage.getItem("language"),
                },
                responseType: "blob"
            }
            const res = await axiosClient.post(url, data, config);


            // Đọc tên file từ header 'Content-Disposition'
            // const contentDisposition = res?.headers['content-disposition'] ?? "";
            // const fileName = contentDisposition
            //     ? contentDisposition.split('filename=')[1].replace(/"/g, '')
            //     : 'exported_data.xlsx';  // Tên mặc định nếu không có header
            const fileName = "exported_data.xlsx";

            // Tạo URL tạm thời và tải file về
            const blob = new Blob([res.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.setAttribute('download', fileName);

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl); // Giải phóng URL sau khi tải

            localStorage.setItem("last_active_time", new Date().getTime().toString());
            return {
                status_code: 200,
                is_success: true,
                message: "File đã được tải về thành công."
            };
        } catch (error: any) {
            console.log(error);
            if (error.response.status === 403) {
                return {
                    status_code: parseInt(error.response.status),
                    is_success: false,
                    message: "Bạn không được phân quyền để thực hiện thao tác này. Vui lòng liên hệ Quản trị viên để được hỗ trợ."
                }
            } else {
                return {
                    status_code: error.response.data.status_code,
                    is_success: false,
                    message: error.response.data.message || "Có lỗi"
                };
            }

        }
    },
}
export { apiClient };
