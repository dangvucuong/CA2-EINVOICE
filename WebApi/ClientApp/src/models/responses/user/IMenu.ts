
export interface IMenu {
    id: number;
    sub_system_id: number;
    menu_id_parent: number;
    name: string;
    name_en: string;
    description: string;
    icon: string;
    path: string;
    is_active: boolean;
    sort_idx: string;
}

export interface IMenuViewModel {
    id: number;
    sub_system_id: number;
    menu_id_parent: number;
    name: string;
    name_en: string;
    description: string;
    icon: string;
    path: string;
    is_active: boolean;
    sort_idx: string;
    items: IMenuViewModel[];
    
}
