import { IUser } from "./IUser";

export interface IUserEditModel extends IUser {
    role_ids: number[];
    school_ids: number[];
}