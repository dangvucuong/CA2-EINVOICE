ALTER procedure [dbo].[hoa_don_select_report_trangthai]
    @donvi_ma_dv NVARCHAR(100),
    @from_date DATE,
    @to_date DATE
as
BEGIN
    select a.donvi_ma_dv,
        a.hoa_don_trang_thai_id,
        COUNT(a.id) as total
    from hoa_don a WITH (NOLOCK)
    where a.is_deleted=0
        and a.donvi_ma_dv= @donvi_ma_dv
        and (@from_date IS NULL OR  a.ngay_tao>= @from_date)
        and (@to_date IS NULL OR a.ngay_tao < DATEADD(DAY,1,@to_date))
    GROUP by a.donvi_ma_dv, a.hoa_don_trang_thai_id
END