import moment from "moment";

/** ĐKPH có ngày sử dụng thuộc năm hiện tại (bỏ qua lọc nếu ngày không hợp lệ/thiếu). */
export const isDangKyPhatHanhInCurrentYear = (
    ngaySuDung?: string | null
): boolean => {
    if (!ngaySuDung) {
        return true;
    }
    const parsed = moment(ngaySuDung);
    if (!parsed.isValid()) {
        return true;
    }
    return parsed.year() === moment().year();
};
