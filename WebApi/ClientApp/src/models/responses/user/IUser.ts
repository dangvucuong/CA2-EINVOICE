export interface IUser {
    id: number;
    donvi_ma_dv: string;
    serial_number: string;
    username: string;
    full_name: string;
    email: string;
    title: string;
    is_active: boolean;
    serial_remote_signing_numner?: string;
    is_serial_remote_signing_verified?: boolean;
}