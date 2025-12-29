import { IDonVi } from "../category/IDonVi";
import { IApi } from "../user/IApi";
import { IMenuViewModel } from "../user/IMenu";
import { IRole } from "../user/IRole";

export interface IProfileRespone {
    user_id: number;
    donvi_ma_dv: string;
    serial_number: string;
    serial_remote_signing_numner?: string;
    is_serial_remote_signing_verified?: boolean;
    username: string;
    full_name: string;
    email: string;
    menus: IMenuViewModel[];
    apis: IApi[];
    roles: IRole[];
    donvi: IDonVi;
    is_hsm_signing: boolean;
    is_remote_signing: boolean;
}

