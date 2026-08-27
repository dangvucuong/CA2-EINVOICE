/*
    Phân quyền API Thu hồi gói dịch vụ (HSDT gọi từ bên ngoài)

    Cơ chế hiện tại trên code:
    - Endpoint: POST api/don-vi/chu-ky-so/thu-hoi-goi-dv
    - Attribute: [MustAuthorized("[POST]api/don-vi/chu-ky-so")]
    => Tài khoản HSDT chỉ cần quyền POST api/don-vi/chu-ky-so (cùng quyền API thêm gói CKS).

    Chạy script này nếu:
    1) Chưa có quyền POST api/don-vi/chu-ky-so trong bảng api
    2) Muốn bổ sung quyền riêng POST api/don-vi/chu-ky-so/thu-hoi-goi-dv (tùy chọn, xem PHẦN B)
*/

/* ============================================================
   PHẦN A - Bắt buộc: đảm bảo quyền POST api/don-vi/chu-ky-so
   (HSDT dùng quyền này để gọi cả Insert và ThuHoiGoiDv)
   ============================================================ */

DECLARE @menu_id INT = (
    SELECT TOP 1 menu_id
    FROM api
    WHERE method = 'POST' AND endpoint = 'api/don-vi/chu-ky-so' AND is_deleted = 0
);

IF @menu_id IS NULL
BEGIN
    SET @menu_id = (
        SELECT TOP 1 menu_id
        FROM api
        WHERE endpoint LIKE 'api/don-vi/%' AND is_deleted = 0
        ORDER BY id
    );
END

IF NOT EXISTS (
    SELECT 1 FROM api
    WHERE method = 'POST' AND endpoint = 'api/don-vi/chu-ky-so' AND is_deleted = 0
)
BEGIN
    INSERT INTO api (
        menu_id, method, endpoint, description,
        is_allow_anonymous, is_check_login, is_check_authorization,
        is_active, sort_idx, is_privte,
        is_deleted, created_time, created_user_id, last_modified_times, last_modified_user_id
    )
    VALUES (
        ISNULL(@menu_id, 0),
        'POST',
        'api/don-vi/chu-ky-so',
        N'Thêm gói chữ ký số (HSDT tích hợp)',
        0, 1, 1,
        1, '999', 0,
        0, GETDATE(), 0, GETDATE(), 0
    );
    PRINT N'Đã thêm API: POST api/don-vi/chu-ky-so';
END
ELSE
BEGIN
    PRINT N'API POST api/don-vi/chu-ky-so đã tồn tại';
END

/* Gán quyền POST api/don-vi/chu-ky-so cho role HSDT (sửa @role_id theo môi trường) */
DECLARE @role_id INT = NULL; -- VD: SET @role_id = 5;
DECLARE @api_id INT = (
    SELECT TOP 1 id FROM api
    WHERE method = 'POST' AND endpoint = 'api/don-vi/chu-ky-so' AND is_deleted = 0
);

IF @role_id IS NOT NULL AND @api_id IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM role_api WHERE role_id = @role_id AND api_id = @api_id AND is_deleted = 0)
BEGIN
    INSERT INTO role_api (
        role_id, api_id,
        is_deleted, created_time, created_user_id, last_modified_times, last_modified_user_id
    )
    VALUES (@role_id, @api_id, 0, GETDATE(), 0, GETDATE(), 0);
    PRINT N'Đã gán quyền POST api/don-vi/chu-ky-so cho role_id=' + CAST(@role_id AS NVARCHAR(20));
END

/* ============================================================
   PHẦN B - Tùy chọn: quyền riêng cho thu-hoi-goi-dv
   Chỉ cần nếu đổi code sang [MustAuthorized] (không dùng fallback)
   ============================================================ */

/*
IF NOT EXISTS (
    SELECT 1 FROM api
    WHERE method = 'POST' AND endpoint = 'api/don-vi/chu-ky-so/thu-hoi-goi-dv' AND is_deleted = 0
)
BEGIN
    INSERT INTO api (
        menu_id, method, endpoint, description,
        is_allow_anonymous, is_check_login, is_check_authorization,
        is_active, sort_idx, is_privte,
        is_deleted, created_time, created_user_id, last_modified_times, last_modified_user_id
    )
    VALUES (
        ISNULL(@menu_id, 0),
        'POST',
        'api/don-vi/chu-ky-so/thu-hoi-goi-dv',
        N'Thu hồi gói dịch vụ chữ ký số (HSDT)',
        0, 1, 1,
        1, '999', 0,
        0, GETDATE(), 0, GETDATE(), 0
    );
END
*/
