import { useMemo } from 'react';
import { eHoaDonTrangThai } from '../models/commons/eHoaDonTrangThai';

const hoaDonTrangThais = [
    {
        id: eHoaDonTrangThai.NHAP,
        name: "Hóa đơn nháp",
        name_en: "Hóa đơn nháp",
        color: "#8dc6fc"
    },
    {
        id: eHoaDonTrangThai.DA_PHAT_HANH,
        name: "Đã phát hành",
        name_en: "Đã phát hành",
        color: "#0cf478"
    },
    {
        id: eHoaDonTrangThai.DA_HUY,
        name: "Đã hủy",
        name_en: "Đã hủy",
        color: "#ffd78e"
    },
    {
        id: eHoaDonTrangThai.CHUA_CO_KET_QUA_PHAN_HOI,
        name: "Chưa có kết quả phản hồi",
        name_en: "Chưa có kết quả phản hồi",
        color: "#ffd78e"
    },
    {
        id: eHoaDonTrangThai.DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT,
        name: "Phản hồi kỹ thuật",
        name_en: "Phản hồi kỹ thuật",
        color: "#ffd78e"
    },
    // {
    //     id: eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
    //     name: "Phản hồi chưa kiểm tra dữ liệu",
    //     name_en: "Phản hồi chưa kiểm tra dữ liệu",
    //     color: "#ffd78e"
    // },
    {
        id: eHoaDonTrangThai.KHONG_HOP_LE,
        name: "Không hợp lệ",
        name_en: "Không hợp lệ",
        color: "#ff0000"
    },
    {
        id: eHoaDonTrangThai.LOI_THONG_DIEP,
        name: "Lỗi thông điệp",
        name_en: "Lỗi thông điệp",
        color: "#ff0000"
    },
    {
        id: eHoaDonTrangThai.CHUA_GUI_CQT,
        name: "Chưa gửi CQT",
        name_en: "Chưa gửi CQT",
        color: "#ffd78e"
    },
]

export const useHoaDonTrangThaisHook = () => {
    return {
        hoaDonTrangThais
    };
}

export const useHoaDonTrangThaiHook = (id: number) => {
    const { hoaDonTrangThais } = useHoaDonTrangThaisHook();
    const hoaDonTrangThai = useMemo(() => {
        return hoaDonTrangThais.find(x => x.id === id);
    }, [hoaDonTrangThais, id])
    return {
        hoaDonTrangThai
    };
}