import { IToKhai } from "../../responses/to-khai/IToKhai";
import { IToKhaiCTS } from "../../responses/to-khai/IToKhaiCTS";

export interface IToKhaiAddOrEditModel extends IToKhai {
    list_cts: IToKhaiCTS[];
}