import { IThongBaoSaiSot } from "../../responses/tbss/IThongBaoSaiSot";
import { IThongBaoSaiSotChiTiet } from "../../responses/tbss/IThongBaoSaiSotChiTiet";

export interface IThongBaoSaiSotAddOrEditRequest extends IThongBaoSaiSot {
    thong_bao_sai_sot_chi_tiets: IThongBaoSaiSotChiTiet[];
}