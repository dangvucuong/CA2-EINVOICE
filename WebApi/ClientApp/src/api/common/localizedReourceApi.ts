import { apiGuestClient } from "../apiGuestClient"

export const localizedReourceApi = {
    getAll: (lan: string) => {
        return apiGuestClient.get(`localized-resource/${lan}`)
    }
}