import { IUploadRespone } from "../../responses/upload/IUploadRespone";

export interface IHoaDonImportRequest extends IUploadRespone {
  loai_hoa_don_ct_id: number;
  hoa_don_dang_ky_phat_hanh_mau_so: string;
  hoa_don_dang_ky_phat_hanh_ky_hieu: string;
  ten_hoa_don: string;
  template: "" | "hoc_phi" | "nuoc";
  importType?: string;
}
