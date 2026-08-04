-- Ngày hóa đơn liền kề trước/sau (cùng đơn vị, mẫu số, ký hiệu) theo thứ tự ma_so_hoa_don (đã cấp số) rồi id.
-- @hoa_don_id = 0: lập mới (coi như cuối lô — chỉ có ngày liền kề trước nếu đã có HĐ trong lô).
IF OBJECT_ID('hoa_don_select_ngay_lien_ke', 'P') IS NOT NULL
    DROP PROCEDURE hoa_don_select_ngay_lien_ke;
GO
CREATE PROCEDURE hoa_don_select_ngay_lien_ke
    @donvi_ma_dv NVARCHAR(50),
    @mau_so NVARCHAR(50),
    @ky_hieu NVARCHAR(50),
    @hoa_don_id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ngay_truoc DATE = NULL;
    DECLARE @ngay_sau DATE = NULL;

    DECLARE @cur_rn INT = NULL;

    ;WITH eligible AS (
        SELECT
            id,
            ngay_hoa_don,
            sort_key = CASE WHEN ma_so_hoa_don > 0 THEN ma_so_hoa_don ELSE 2147483647 END
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
    ),
    ranked AS (
        SELECT
            id,
            ngay_hoa_don,
            rn = ROW_NUMBER() OVER (ORDER BY sort_key, id)
        FROM eligible
    )
    SELECT @cur_rn = rn FROM ranked WHERE id = @hoa_don_id AND @hoa_don_id > 0;

    IF @hoa_don_id = 0
    BEGIN
        SELECT @ngay_truoc = ngay_hoa_don
        FROM ranked
        WHERE rn = (SELECT MAX(rn) FROM ranked);
    END
    ELSE IF @cur_rn IS NOT NULL
    BEGIN
        SELECT @ngay_truoc = ngay_hoa_don FROM ranked WHERE rn = @cur_rn - 1;
        SELECT @ngay_sau = ngay_hoa_don FROM ranked WHERE rn = @cur_rn + 1;
    END

    SELECT @ngay_truoc AS ngay_truoc, @ngay_sau AS ngay_sau;
END
GO
