-- Hóa đơn chặn ký số đơn lẻ: HĐ nháp đã cấp số, chưa ký, đứng TRƯỚC theo (ngay_hoa_don, ma_so_hoa_don).
IF OBJECT_ID('hoa_don_select_so_nho_hon_chua_ky_so', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_so_nho_hon_chua_ky_so;
GO
CREATE PROCEDURE hoa_don_select_so_nho_hon_chua_ky_so
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT,
    @ma_so_hoa_don_hien_tai INT,
    @ngay_hoa_don_hien_tai DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        hd.id,
        hd.ma_so_hoa_don,
        hd.ngay_hoa_don
    FROM hoa_don hd
    WHERE hd.donvi_ma_dv = @donvi_ma_dv
      AND hd.hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
      AND hd.hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
      AND hd.is_deleted = 0
      AND hd.hoa_don_trang_thai_id <> 3
      AND hd.hoa_don_hinh_thuc_id <> 5
      AND hd.ma_so_hoa_don > 0
      AND hd.hoa_don_trang_thai_id = 1
      AND ISNULL(hd.is_ky_so_succes, 0) = 0
      AND hd.id <> @hoa_don_id
      AND (
          hd.ngay_hoa_don < @ngay_hoa_don_hien_tai
          OR (hd.ngay_hoa_don = @ngay_hoa_don_hien_tai AND hd.ma_so_hoa_don < @ma_so_hoa_don_hien_tai)
      )
    ORDER BY hd.ngay_hoa_don ASC, hd.ma_so_hoa_don ASC;
END
GO
