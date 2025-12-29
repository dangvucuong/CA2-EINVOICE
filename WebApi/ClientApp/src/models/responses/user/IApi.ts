
export interface IApi{
    id: number;
    menu_id: number;
    method: string;
    endpoint: string;
    description: string;
    is_allow_anonymous: boolean;
    is_check_login: boolean;
    is_check_authorization: boolean;
    is_active: boolean;
    sort_idx: string;
}