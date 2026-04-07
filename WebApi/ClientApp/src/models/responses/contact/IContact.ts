export interface IContact {
    id: number;
    name: string;
    address: string;
    email: string;
    phone: string;
    tax_code: string;
    info: string | null;
    serial: string | null;
    register_at: string;
    contact_status_id: number;
    company_size_id: number;
    note: string | null;
}