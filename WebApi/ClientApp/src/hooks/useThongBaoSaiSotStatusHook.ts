import { useMemo } from "react";


const thongBaoSaiSotTrangThai = [
    {
        id: 1,
        name: 'Tạo mới',
        color: "#ffd78e"
    },
    {
        id: 2,
        name: 'Chờ Cơ quan thuế',
        color: "#8dc6fc"
    },
    {
        id: 3,
        name: 'Cơ quan thuế từ chối',
        color: "#d73a4a"

    },
    {
        id: 4,
        name: 'Cơ quan thuế chấp nhận',
        color: "#0cf478"
    },
    {
        id: 5,
        name: 'Cơ quan thuế tiếp nhận',
        color: "#0cf478"
    },


]

export const useThongBaoSaiSotStatusesHook = () => {
    return {
        thongBaoSaiSotTrangThais: thongBaoSaiSotTrangThai
    };
}

export const useThongBaoSaiSotStatusHook = (id: number) => {
    const { thongBaoSaiSotTrangThais } = useThongBaoSaiSotStatusesHook();
    const thongBaoSaiSotTrangThai = useMemo(() => {
        return thongBaoSaiSotTrangThais.find(x => x.id === id);
    }, [thongBaoSaiSotTrangThais, id])
    return {
        thongBaoSaiSotTrangThai
    };
}
