-- SELECT *
-- from hoa_don a
-- where a.donvi_ma_dv='0103930279-999'
-- and a.is_deleted=0

-- select *
-- from dm_hanghoa

ALTER TABLE dm_hanghoa add don_gia DECIMAL

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[dm_hanghoa_insert]
	@donvi_ma_dv nvarchar (200),
	@ma_hang_hoa nvarchar (200),
	@ten_hang_hoa nvarchar (2000),
	@dvt nvarchar (200),
	@ma_loai_hoang_hoa nvarchar (200),
	@is_deleted bit,
	@created_time datetime,
	@created_user_id int,
	@last_modified_times datetime,
	@last_modified_user_id int,
    @don_gia DECIMAL(18,2)=0
as
begin
	insert into dm_hanghoa
		(donvi_ma_dv,
		ma_hang_hoa,
		ten_hang_hoa,
		dvt,
		ma_loai_hoang_hoa,
		is_deleted,
		created_time,
		created_user_id,
		last_modified_times,
		last_modified_user_id,
        don_gia)
	values
		(@donvi_ma_dv,
		@ma_hang_hoa,
		@ten_hang_hoa,
		@dvt,
		@ma_loai_hoang_hoa,
		@is_deleted,
		@created_time,
		@created_user_id,
		@last_modified_times,
		@last_modified_user_id,
        @don_gia)
	return @@identity
end
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[dm_hanghoa_update]
	@id int,
	@donvi_ma_dv nvarchar (200),
	@ma_hang_hoa nvarchar (200),
	@ten_hang_hoa nvarchar (2000),
	@dvt nvarchar (200),
	@ma_loai_hoang_hoa nvarchar (200),
	@last_modified_times datetime,
	@last_modified_user_id int,
    @don_gia DECIMAL(18,2)=0
as
begin
	update dm_hanghoa
	set
		donvi_ma_dv=@donvi_ma_dv,
		ma_hang_hoa=@ma_hang_hoa,
		ten_hang_hoa=@ten_hang_hoa,
		dvt=@dvt,
		ma_loai_hoang_hoa=@ma_loai_hoang_hoa,
		last_modified_times=@last_modified_times,
		last_modified_user_id=@last_modified_user_id,
        don_gia=@don_gia
	where id=@id
end
GO


ALTER TABLE khachhang  ADD ma_dv_ngan_sach NVARCHAR(100)

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[khachhang_insert]
	@donvi_ma_dv nvarchar (2000),
	@ten_khach_hang nvarchar (2000),
	@ten_don_vi nvarchar (2000),
	@dia_chi nvarchar (2000),
	@stk nvarchar (2000),
	@mst nvarchar (2000),
	@email nvarchar (2000),
	@is_deleted bit,
	@created_time datetime,
	@created_user_id int,
	@last_modified_times datetime,
	@last_modified_user_id int,
    @ma_dv_ngan_sach NVARCHAR(1000)=''
as
begin
	insert into khachhang
		(donvi_ma_dv,
		ten_khach_hang,
		ten_don_vi,
		dia_chi,
		stk,
		mst,
		email,
		is_deleted,
		created_time,
		created_user_id,
		last_modified_times,
		last_modified_user_id,
        ma_dv_ngan_sach)
	values
		(@donvi_ma_dv,
		@ten_khach_hang,
		@ten_don_vi,
		@dia_chi,
		@stk,
		@mst,
		@email,
		@is_deleted,
		@created_time,
		@created_user_id,
		@last_modified_times,
		@last_modified_user_id,
        @ma_dv_ngan_sach)
	return @@identity
end
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[khachhang_update]
	@id int,
	@donvi_ma_dv nvarchar (2000),
	@ten_khach_hang nvarchar (2000),
	@ten_don_vi nvarchar (2000),
	@dia_chi nvarchar (2000),
	@stk nvarchar (2000),
	@mst nvarchar (2000),
	@email nvarchar (2000),
	@last_modified_times datetime,
	@last_modified_user_id int,
    @ma_dv_ngan_sach NVARCHAR(1000)=''
as
begin
	update khachhang
	set
		donvi_ma_dv=@donvi_ma_dv,
		ten_khach_hang=@ten_khach_hang,
		ten_don_vi=@ten_don_vi,
		dia_chi=@dia_chi,
		stk=@stk,
		mst=@mst,
		email=@email,
		last_modified_times=@last_modified_times,
		last_modified_user_id=@last_modified_user_id,
        ma_dv_ngan_sach=@ma_dv_ngan_sach
	where id=@id
end
GO
