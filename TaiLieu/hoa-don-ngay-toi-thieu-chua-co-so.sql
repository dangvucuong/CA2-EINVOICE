-- Ngày tối thiểu khi sửa HĐ nháp CHƯA có số: không được nhỏ hơn ngày của HĐ đã có số
-- mà lại nằm sau ngày đang chọn (tránh lệch thứ tự khi cấp số).
IF OBJECT_ID('hoa_don_select_ngay_toi_thieu_chua_co_so', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_ngay_toi_thieu_chua_co_so;
GO
CREATE PROCEDURE hoa_don_select_ngay_toi_thieu_chua_co_so
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT,
    @ngay_hoa_don DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MAX(hd.ngay_hoa_don) AS ngay_toi_thieu
    FROM hoa_don hd
    WHERE hd.donvi_ma_dv = @donvi_ma_dv
      AND hd.hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
      AND hd.hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
      AND hd.is_deleted = 0
      AND hd.hoa_don_trang_thai_id <> 3
      AND hd.hoa_don_hinh_thuc_id <> 5
      AND (
          hd.hoa_don_trang_thai_id = 1
          OR hd.ma_so_hoa_don > 0
          OR hd.hoa_don_trang_thai_id = 9
      )
      AND hd.ma_so_hoa_don > 0
      AND (@hoa_don_id <= 0 OR hd.id <> @hoa_don_id)
      AND hd.ngay_hoa_don > @ngay_hoa_don;
END
GO
