import { apiGuestClient } from "../apiGuestClient";

export const appConfigApi={
    getAll: () => apiGuestClient.get(`app-config`)
}