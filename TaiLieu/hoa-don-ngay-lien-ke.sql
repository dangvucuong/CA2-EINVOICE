-- Ngày hóa đơn liền kề trước/sau theo NGÀY (cùng đơn vị, mẫu số, ký hiệu).
-- Loại trừ @hoa_don_id khi > 0. So sánh theo @ngay_hoa_don đang lưu/ký.
-- Khi sửa HĐ cuối (không có ngày liền kề sau ở ngày hiện tại trên DB), app kiểm tra thêm MAX(ngay) HĐ đã phát hành.
IF OBJECT_ID('hoa_don_select_ngay_lien_ke', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_ngay_lien_ke;
GO
CREATE PROCEDURE hoa_don_select_ngay_lien_ke
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT,
    @ngay_hoa_don DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ngay_truoc DATE = NULL;
    DECLARE @ngay_sau DATE = NULL;

    SELECT @ngay_truoc = MAX(ngay_hoa_don)
    FROM hoa_don
    WHERE donvi_ma_dv = @donvi_ma_dv
      AND hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
      AND hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
      AND is_deleted = 0
      AND hoa_don_trang_thai_id <> 3
      AND (
          hoa_don_trang_thai_id = 1
          OR ma_so_hoa_don > 0
          OR hoa_don_trang_thai_id = 9
      )
      AND (@hoa_don_id <= 0 OR id <> @hoa_don_id)
      AND ngay_hoa_don < @ngay_hoa_don;

    SELECT @ngay_sau = MIN(ngay_hoa_don)
    FROM hoa_don
    WHERE donvi_ma_dv = @donvi_ma_dv
      AND hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
      AND hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
      AND is_deleted = 0
      AND hoa_don_trang_thai_id <> 3
      AND (
          hoa_don_trang_thai_id = 1
          OR ma_so_hoa_don > 0
          OR hoa_don_trang_thai_id = 9
      )
      AND (@hoa_don_id <= 0 OR id <> @hoa_don_id)
      AND ngay_hoa_don > @ngay_hoa_don;

    SELECT @ngay_truoc AS ngay_truoc, @ngay_sau AS ngay_sau;
END
GO
