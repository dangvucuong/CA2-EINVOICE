import { apiClient } from "../apiClient";
const uploadApi = {
    upload: (file: File) => {
        let formData = new FormData();
        formData.append("form_file", file);
        return apiClient.upload(`upload`, formData)
    },
    uploadCert: (file: File) => {
        let formData = new FormData();
        formData.append("form_file", file);
        return apiClient.upload(`upload/cert`, formData)
    },
    createLink: (url: string) => {
        return apiClient.post(`link`, { url })
    }

}
export { uploadApi };
