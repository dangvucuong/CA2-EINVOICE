import moment from 'moment';
import { useMemo } from 'react';

export const useNgayThangNam = (date?: Date) => {
    const data = useMemo(() => {
        if (date) return date;
        return new Date();
    }, [date])

    const momentData = moment(data);

    return {
        date: data,
        dd: momentData.format("DD"),
        mm: momentData.format("MM"),
        yyyy: momentData.format("YYYY"),
        text:`Ngày ${momentData.format("DD")} tháng ${momentData.format("MM")} năm ${momentData.format("YYYY")}`
    }
}