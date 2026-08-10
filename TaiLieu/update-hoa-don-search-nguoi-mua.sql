/*
    Bổ sung tìm kiếm theo tên người mua (nguoi_mua_ten, nguoi_mua_ten_donvi)
    cho ô tìm kiếm @search_key trên các trang danh sách hóa đơn.
*/
SET NOCOUNT ON;

DECLARE @procedures TABLE (name SYSNAME);
INSERT INTO @procedures (name)
VALUES
    (N'hoa_don_select_bydonvi_paging'),
    (N'hoa_don_select_bydonvi_paging_thaythe'),
    (N'hoa_don_select_bydonvi_paging_dieuchinh'),
    (N'hoa_don_select_bydonvi_paging_thongke_page'),
    (N'hoa_don_select_cho_phan_hoi_cqt'),
    (N'hoa_don_select_chua_gui_cqt');

DECLARE @proc SYSNAME;
DECLARE @def NVARCHAR(MAX);
DECLARE @old NVARCHAR(200) = N'or a.nguoi_mua_mst LIKE ''%'' + @search_key +''%''';
DECLARE @new NVARCHAR(500) = N'or a.nguoi_mua_mst LIKE ''%'' + @search_key +''%''
        OR a.nguoi_mua_ten LIKE N''%'' + @search_key + N''%''
        OR a.nguoi_mua_ten_donvi LIKE N''%'' + @search_key + N''%''';
DECLARE @oldUpper NVARCHAR(200) = N'OR a.nguoi_mua_mst LIKE ''%'' + @search_key + ''%''';
DECLARE @newUpper NVARCHAR(500) = N'OR a.nguoi_mua_mst LIKE ''%'' + @search_key + ''%''
                OR a.nguoi_mua_ten LIKE N''%'' + @search_key + N''%''
                OR a.nguoi_mua_ten_donvi LIKE N''%'' + @search_key + N''%''';

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
SELECT name FROM @procedures;

OPEN cur;
FETCH NEXT FROM cur INTO @proc;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @def = OBJECT_DEFINITION(OBJECT_ID(N'dbo.' + @proc));

    IF @def IS NULL
    BEGIN
        PRINT N'SKIP (missing): ' + @proc;
    END
    ELSE IF CHARINDEX(N'nguoi_mua_ten LIKE', @def) > 0
    BEGIN
        PRINT N'SKIP (already patched): ' + @proc;
    END
    ELSE IF CHARINDEX(@old, @def) > 0
    BEGIN
        SET @def = REPLACE(@def, @old, @new);
        SET @def = REPLACE(@def, N'CREATE PROCEDURE', N'ALTER PROCEDURE');
        EXEC sys.sp_executesql @def;
        PRINT N'PATCHED: ' + @proc;
    END
    ELSE IF CHARINDEX(@oldUpper, @def) > 0
    BEGIN
        SET @def = REPLACE(@def, @oldUpper, @newUpper);
        SET @def = REPLACE(@def, N'CREATE PROCEDURE', N'ALTER PROCEDURE');
        EXEC sys.sp_executesql @def;
        PRINT N'PATCHED (upper OR): ' + @proc;
    END
    ELSE
    BEGIN
        PRINT N'SKIP (pattern not found): ' + @proc;
    END

    FETCH NEXT FROM cur INTO @proc;
END

CLOSE cur;
DEALLOCATE cur;
