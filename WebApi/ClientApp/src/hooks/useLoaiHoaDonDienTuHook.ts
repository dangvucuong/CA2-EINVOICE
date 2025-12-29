const loaiHoaDonTienTus = [
  {
    id: 1,
    name: "Hóa đơn điện tử theo Nghị định 123/2020/NĐ-CP, Nghị định 70/2025/NĐ-CP",
  },
  {
    id: 2,
    name: "Hóa đơn điện tử có mã xác thực của CQT theo Quyết định số 1209/QĐ-BTC",
  },
  {
    id: 3,
    name: "Các loại hóa đơn theo Nghị định số 51/2010/NĐ-CP",
  },
  {
    id: 4,
    name: "Hóa đơn đặt in theo Nghị định 123/2020/NĐ-CP",
  },
];

export const useLoaiHoaDonDienTusHook = () => {
  return {
    loaiHoaDonTienTus: loaiHoaDonTienTus,
  };
};
