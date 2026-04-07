import { useMemo } from 'react';

const lyDoDieuChinhs = [
    {
        id: 1,
        name: "Điều chỉnh tăng",
        name_en: "Điều chỉnh tăng",

    },
    {
        id: 2,
        name: "Điều chỉnh giảm",
        name_en: "Điều chỉnh giảm",

    },
    {
        id: 3,
        name: "Điều chỉnh thông tin",
        name_en: "Điều chỉnh thông tin",

    },
    {
        id: 20,
        name: "Điều chỉnh thuế",
        name_en: "Điều chỉnh thuế",

    },

]

export const useLyDoDieuChinhsHook = () => {
    return {
        lyDoDieuChinhs
    };
}

export const useLyDoDieuChinhHook = (id: number) => {
    const { lyDoDieuChinhs } = useLyDoDieuChinhsHook();
    const lyDoDieuChinh = useMemo(() => {
        return lyDoDieuChinhs.find(x => x.id === id);
    }, [lyDoDieuChinhs, id])
    return {
        lyDoDieuChinh
    };
}