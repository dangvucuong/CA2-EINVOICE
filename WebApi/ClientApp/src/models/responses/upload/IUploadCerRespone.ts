import { IUploadRespone } from "./IUploadRespone";

export interface IUploadCerRespone extends IUploadRespone {
    file_name: string;
    url: string;
    cer_info:ICerInfo
}
export interface ICerInfo {
    subject: string;
    issuer: string;
    // thumbprint: string;
    not_before: string;
    not_after: string;
    serial_number: string;
    // public_key: string;
    signature_algorithm: string;
    version: string;
    // extensions: string;
}