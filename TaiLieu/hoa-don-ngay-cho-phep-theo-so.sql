-- Biên ngày HĐ khi sửa HĐ nháp đã có số: giữ thứ tự (ngày, số).
-- Áp dụng cho HĐ GTGT và HĐ máy tính tiền (cùng đơn vị, mẫu số, ký hiệu đăng ký).
-- ngay_toi_thieu = MAX(ngay) của HĐ khác có số nhỏ hơn; ngay_toi_da = MIN(ngay) của HĐ khác có số lớn hơn.
IF OBJECT_ID('hoa_don_select_ngay_cho_phep_theo_so', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_ngay_cho_phep_theo_so;
GO
CREATE PROCEDURE hoa_don_select_ngay_cho_phep_theo_so
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT,
    @ma_so_hoa_don INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (
            SELECT MAX(hd.ngay_hoa_don)
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
              AND hd.ma_so_hoa_don < @ma_so_hoa_don
        ) AS ngay_toi_thieu,
        (
            SELECT MIN(hd.ngay_hoa_don)
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
              AND hd.ma_so_hoa_don > @ma_so_hoa_don
        ) AS ngay_toi_da;
END
GO
