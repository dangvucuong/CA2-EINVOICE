/*
    Stored procedure thu hồi gói dịch vụ chữ ký số (API ThuHoiGoiDv).

    Chạy script này trên DB trước khi deploy WebApi backend mới.
    Endpoint: POST api/don-vi/chu-ky-so/thu-hoi-goi-dv

    Lưu ý: chạy trong SSMS (có hỗ trợ GO). Không dùng CREATE OR ALTER
    để tương thích SQL Server cũ hơn 2016 SP1.
*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'[dbo].[donvi_mua_chukyso_thu_hoi]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[donvi_mua_chukyso_thu_hoi]
GO

CREATE PROCEDURE [dbo].[donvi_mua_chukyso_thu_hoi]
    @donvi_mst NVARCHAR(200),
    @transaction_id NVARCHAR(200),
    @last_modified_user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE donvi_mua_chukyso
    SET
        is_deleted = 1,
        trang_thai_hd_id = 0,
        last_modified_times = GETDATE(),
        last_modified_user_id = @last_modified_user_id
    WHERE donvi_mst = @donvi_mst
      AND transaction_id = @transaction_id
      AND is_deleted = 0;

    RETURN @@ROWCOUNT;
END
GO
