import { IProfileRespone } from "./IProfileRespone";

export interface ILoginRespone {
    token_info: ITokenInfo;
    profile: IProfileRespone;
    is_verify_cert?: boolean
}
export interface ITokenInfo {
    access_token: string;
    refresh_token: string;
}