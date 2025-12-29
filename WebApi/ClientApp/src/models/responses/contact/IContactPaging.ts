import { IPagingRespone } from "../IBasePagingRespone";
import { IContact } from "./IContact";

export interface IContactPaging extends IPagingRespone<IContact[]> { }