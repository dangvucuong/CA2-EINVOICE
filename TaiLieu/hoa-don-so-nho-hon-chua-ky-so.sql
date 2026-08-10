-- Hóa đơn chặn ký số theo thứ tự: cùng đơn vị/mẫu số/ký hiệu, số nhỏ hơn số đang ký VÀ đủ 3 điều kiện:
--   ma_so_hoa_don > 0, hoa_don_trang_thai_id = 1 (nháp), is_ky_so_succes = 0 (hoặc NULL).
-- HĐ đã có số nhưng trang_thai_id > 1 (đã ký/phát hành/lỗi CQT...) thì không tính là "chưa ký" để chặn.
IF OBJECT_ID('hoa_don_select_so_nho_hon_chua_ky_so', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_so_nho_hon_chua_ky_so;
GO
CREATE PROCEDURE hoa_don_select_so_nho_hon_chua_ky_so
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT,
    @ma_so_hoa_don_hien_tai INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        hd.id,
        hd.ma_so_hoa_don
    FROM hoa_don hd
    WHERE hd.donvi_ma_dv = @donvi_ma_dv
      AND hd.hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
      AND hd.hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
      AND hd.is_deleted = 0
      AND hd.hoa_don_hinh_thuc_id <> 5
      AND hd.ma_so_hoa_don > 0
      AND hd.hoa_don_trang_thai_id = 1
      AND ISNULL(hd.is_ky_so_succes, 0) = 0
      AND hd.ma_so_hoa_don < @ma_so_hoa_don_hien_tai
      AND hd.id <> @hoa_don_id
    ORDER BY hd.ma_so_hoa_don ASC;
END
GO
