import { apiClient } from "../apiClient";

export const contactStatusApi = {
    getAll: () => apiClient.get('contact-status')
}