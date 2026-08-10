export const getApiErrorMessage = (
    error: any,
    fallback = "Có lỗi không xác định"
): string => {
    const data = error?.response?.data;
    if (typeof data === "string" && data.trim()) {
        const trimmed = data.trim();
        if (trimmed.startsWith("{") || trimmed.startsWith("[")) {
            try {
                const parsed = JSON.parse(trimmed);
                const parsedMessage =
                    parsed?.message ?? parsed?.Message ?? parsed?.title;
                if (typeof parsedMessage === "string" && parsedMessage.trim()) {
                    return parsedMessage.trim();
                }
            } catch {
                // not JSON
            }
        }
        return trimmed;
    }
    const message = data?.message ?? data?.Message ?? data?.title;
    if (typeof message === "string" && message.trim()) {
        return message.trim();
    }
    if (error?.response?.status) {
        return `Lỗi HTTP ${error.response.status}`;
    }
    if (error?.message) {
        return error.message;
    }
    return fallback;
};

export const getApiStatusCode = (error: any): number => {
    return (
        error?.response?.data?.status_code ??
        error?.response?.status ??
        500
    );
};

export const getSignalRErrorMessage = (
    error: any,
    fallback = "Có lỗi khi gọi công cụ ký số. Vui lòng kiểm tra công cụ ký số đã được cài đặt và đang chạy."
): string => {
    if (typeof error === "string" && error.trim()) {
        return error.trim();
    }
    const message = error?.message ?? error?.source;
    if (typeof message === "string" && message.trim()) {
        return message.trim();
    }
    return fallback;
};
