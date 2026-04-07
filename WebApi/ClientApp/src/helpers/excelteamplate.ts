import ExcelJS from "exceljs";
import { saveAs } from "file-saver";
import moment from "moment";

export const thongKeHDThayTheTemplate = async ({
  data,
  fileName,
  startDate,
  endDate,
}: {
  data: any[];
  fileName: string;
  startDate?: string;
  endDate?: string;
}) => {
  if (!data || data?.length === 0) return;

  const excelData = data.map((item, index) => ({
    STT: (index + 1).toString(),
    "Mẫu số hd gốc": item.hoa_don_dang_ky_phat_hanh_mau_so_goc,
    "Số(No) hd gốc": item.ma_so_hoa_don_goc,
    "Ngày hóa đơn gốc": moment(item.ngay_hoa_don_goc).format("DD/MM/YYYY"),
    "Số tiền hd gốc": item.tong_tien_thanh_toan,
    "Mẫu số hd thay thế": item.hoa_don_dang_ky_phat_hanh_mau_so,
    "Số(No) hd thay thế": item.ma_so_hoa_don || "",
    "Ngày hóa đơn thay thế": moment(item.ngay_hoa_don).format("DD/MM/YYYY"),
    "Số tiền hd thay thế": item.tong_tien_thanh_toan || 0,
    "Tên khách hàng": item?.nguoi_mua_ten || "",
    "Địa chỉ khách hàng": item?.nguoi_mua_dia_chi || "",
    "Mã số thuế khách hàng": item?.nguoi_mua_mst?.toString() || "",
    "Người thực hiện": item?.nguoi_tao || "",
  }));

  const response = await fetch(
    `${process.env.PUBLIC_URL}/teamplate/ThongKeHDThayThe.xlsx`
  ); // Tải file từ thư mục public
  const arrayBuffer = await response.arrayBuffer(); // Chuyển đổi response thành ArrayBuffer

  const workbook = new ExcelJS.Workbook();
  await workbook.xlsx.load(arrayBuffer);

  const worksheet = workbook.worksheets[0];

  worksheet.getCell(`A3`).value = `Từ ngày: ${moment(startDate).format(
    "DD/MM/YYYY"
  )} Đến ngày: ${moment(endDate).format("DD/MM/YYYY")}`;

  worksheet.getCell(`A4`).value = `Tổng số hóa đơn: ${data?.length} hóa đơn.`;

  // Cập nhật dữ liệu vào template, bắt đầu từ hàng 8
  excelData.forEach((item: any, index: number) => {
    const rowIndex = index + 8; // Bắt đầu từ hàng 8
    worksheet.getCell(`A${rowIndex}`).value = item["STT"];
    worksheet.getCell(`B${rowIndex}`).value = item["Mẫu số hd gốc"];
    worksheet.getCell(`C${rowIndex}`).value = item["Số(No) hd gốc"];
    worksheet.getCell(`D${rowIndex}`).value = item["Ngày hóa đơn gốc"];
    worksheet.getCell(`E${rowIndex}`).value = item["Số tiền hd gốc"];
    worksheet.getCell(`F${rowIndex}`).value = item["Mẫu số hd thay thế"];
    worksheet.getCell(`G${rowIndex}`).value = item["Số(No) hd thay thế"];
    worksheet.getCell(`H${rowIndex}`).value = item["Ngày hóa đơn thay thế"];
    worksheet.getCell(`I${rowIndex}`).value = item["Số tiền hd thay thế"];
    worksheet.getCell(`J${rowIndex}`).value = item["Tên khách hàng"];
    worksheet.getCell(`K${rowIndex}`).value = item["Địa chỉ khách hàng"];
    worksheet.getCell(`L${rowIndex}`).value = item["Mã số thuế khách hàng"];
    worksheet.getCell(`M${rowIndex}`).value = item["Người thực hiện"];

    worksheet.getCell(`A${rowIndex}`).alignment = {
      wrapText: true,
      vertical: "middle",
      horizontal: "center",
    };
  });

  // Tạo file mới và lưu
  const updatedBuffer = await workbook.xlsx.writeBuffer();

  saveAs(new Blob([updatedBuffer]), `${fileName}.xlsx`);
};

export const thongKeHDDieuChinhTemplate = async ({
  data,
  fileName,
  startDate,
  endDate,
}: {
  data: any[];
  fileName: string;
  startDate?: string;
  endDate?: string;
}) => {
  if (!data || data?.length === 0) return;

  const excelData = data.map((item, index) => ({
    STT: (index + 1).toString(),
    "Mẫu số hd gốc": item.hoa_don_dang_ky_phat_hanh_mau_so_goc,
    "Số(No) hd gốc": item.ma_so_hoa_don_goc,
    "Ngày hóa đơn gốc": moment(item.ngay_hoa_don_goc).format("DD/MM/YYYY"),
    "Số tiền hd gốc": item.tong_tien_thanh_toan,
    "Mẫu số hd điều chỉnh": item.hoa_don_dang_ky_phat_hanh_mau_so,
    "Số(No) hd điều chỉnh": item.ma_so_hoa_don || "",
    "Ngày hóa đơn điều chỉnh": moment(item.ngay_hoa_don).format("DD/MM/YYYY"),
    "Số tiền hd điều chỉnh": item.tong_tien_thanh_toan || 0,
    "Tên khách hàng": item?.nguoi_mua_ten || "",
    "Địa chỉ khách hàng": item?.nguoi_mua_dia_chi || "",
    "Mã số thuế khách hàng": item?.nguoi_mua_mst?.toString() || "",
    "Người thực hiện": item?.nguoi_tao || "",
  }));

  const response = await fetch(
    `${process.env.PUBLIC_URL}/teamplate/ThongKeHDDieuChinh.xlsx`
  ); // Tải file từ thư mục public
  const arrayBuffer = await response.arrayBuffer(); // Chuyển đổi response thành ArrayBuffer

  const workbook = new ExcelJS.Workbook();
  await workbook.xlsx.load(arrayBuffer);

  const worksheet = workbook.worksheets[0];

  worksheet.getCell(`A3`).value = `Từ ngày: ${moment(startDate).format(
    "DD/MM/YYYY"
  )} Đến ngày: ${moment(endDate).format("DD/MM/YYYY")}`;

  worksheet.getCell(`A4`).value = `Tổng số hóa đơn: ${data?.length} hóa đơn.`;

  // Cập nhật dữ liệu vào template, bắt đầu từ hàng 8
  excelData.forEach((item: any, index: number) => {
    const rowIndex = index + 8; // Bắt đầu từ hàng 8
    worksheet.getCell(`A${rowIndex}`).value = item["STT"];
    worksheet.getCell(`B${rowIndex}`).value = item["Mẫu số hd gốc"];
    worksheet.getCell(`C${rowIndex}`).value = item["Số(No) hd gốc"];
    worksheet.getCell(`D${rowIndex}`).value = item["Ngày hóa đơn gốc"];
    worksheet.getCell(`E${rowIndex}`).value = item["Số tiền hd gốc"];
    worksheet.getCell(`F${rowIndex}`).value = item["Mẫu số hd điều chỉnh"];
    worksheet.getCell(`G${rowIndex}`).value = item["Số(No) hd điều chỉnh"];
    worksheet.getCell(`H${rowIndex}`).value = item["Ngày hóa đơn điều chỉnh"];
    worksheet.getCell(`I${rowIndex}`).value = item["Số tiền hd điều chỉnh"];
    worksheet.getCell(`J${rowIndex}`).value = item["Tên khách hàng"];
    worksheet.getCell(`K${rowIndex}`).value = item["Địa chỉ khách hàng"];
    worksheet.getCell(`L${rowIndex}`).value = item["Mã số thuế khách hàng"];
    worksheet.getCell(`M${rowIndex}`).value = item["Người thực hiện"];

    worksheet.getCell(`A${rowIndex}`).alignment = {
      wrapText: true,
      vertical: "middle",
      horizontal: "center",
    };
  });

  // Tạo file mới và lưu
  const updatedBuffer = await workbook.xlsx.writeBuffer();

  saveAs(new Blob([updatedBuffer]), `${fileName}.xlsx`);
};

export const thongKeHDDHuyTemplate = async ({
  data,
  fileName,
  startDate,
  endDate,
}: {
  data: any[];
  fileName: string;
  startDate?: string;
  endDate?: string;
}) => {
  if (!data || data?.length === 0) return;

  const excelData = data.map((item, index) => ({
    STT: (index + 1).toString(),
    "Mẫu số": item.hoa_don_dang_ky_phat_hanh_mau_so,
    "Số hóa đơn": item.ma_so_hoa_don,
    "Ngày hóa đơn": moment(item.ngay_hoa_don).format("DD/MM/YYYY"),
    "Ngày hủy": "",
    "Số tiền": item.tong_tien_thanh_toan,
    "Người phát hành": item.nguoi_tao || "",
    "Người hủy": "",
    "Tên khách hàng": item?.nguoi_mua_ten || "",
    "Địa chỉ": item?.nguoi_mua_dia_chi || "",
    "Mã số thuế": item?.nguoi_mua_mst?.toString() || "",
  }));

  const response = await fetch(
    `${process.env.PUBLIC_URL}/teamplate/ThongKeHDHuy.xlsx`
  ); // Tải file từ thư mục public
  const arrayBuffer = await response.arrayBuffer(); // Chuyển đổi response thành ArrayBuffer

  const workbook = new ExcelJS.Workbook();
  await workbook.xlsx.load(arrayBuffer);

  const worksheet = workbook.worksheets[0];

  worksheet.getCell(`A3`).value = `Từ ngày: ${moment(startDate).format(
    "DD/MM/YYYY"
  )} Đến ngày: ${moment(endDate).format("DD/MM/YYYY")}`;

  worksheet.getCell(`A4`).value = `Tổng số hóa đơn: ${data?.length} hóa đơn.`;

  // Cập nhật dữ liệu vào template, bắt đầu từ hàng 6
  excelData.forEach((item: any, index: number) => {
    const rowIndex = index + 6; // Bắt đầu từ hàng 6
    worksheet.getCell(`A${rowIndex}`).value = item["STT"];
    worksheet.getCell(`B${rowIndex}`).value = item["Mẫu số"];
    worksheet.getCell(`C${rowIndex}`).value = item["Số hóa đơn"];
    worksheet.getCell(`D${rowIndex}`).value = item["Ngày hóa đơn"];
    worksheet.getCell(`E${rowIndex}`).value = item["Ngày hủy"];
    worksheet.getCell(`F${rowIndex}`).value = item["Số tiền"];
    worksheet.getCell(`G${rowIndex}`).value = item["Người phát hành"];
    worksheet.getCell(`H${rowIndex}`).value = item["Người hủy"];
    worksheet.getCell(`I${rowIndex}`).value = item["Tên khách hàng"];
    worksheet.getCell(`J${rowIndex}`).value = item["Địa chỉ"];
    worksheet.getCell(`K${rowIndex}`).value = item["Mã số thuế"];

    worksheet.getCell(`A${rowIndex}`).alignment = {
      wrapText: true,
      vertical: "middle",
      horizontal: "center",
    };
  });

  // Tạo file mới và lưu
  const updatedBuffer = await workbook.xlsx.writeBuffer();

  saveAs(new Blob([updatedBuffer]), `${fileName}.xlsx`);
};
