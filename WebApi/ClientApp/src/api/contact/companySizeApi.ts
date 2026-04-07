import { apiGuestClient } from "../apiGuestClient";

export const companySizeApi = {
    getAll: () => apiGuestClient.get('company-size')
}

