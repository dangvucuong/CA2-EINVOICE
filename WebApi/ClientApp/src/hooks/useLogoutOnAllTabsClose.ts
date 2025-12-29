// hooks/useLogoutOnAllTabsClose.ts
import { useEffect } from "react";

const TAB_KEY = "app_open_tabs";
const TOKEN_KEY = "access_token";

export function useLogoutOnAllTabsClose(onLogout?: () => void) {
  useEffect(() => {
    const tabId = Math.random().toString(36).substring(2, 10);
    const now = Date.now();

    // Hàm đọc danh sách tab đang mở
    const getTabs = () => {
      try {
        return JSON.parse(localStorage.getItem(TAB_KEY) || "[]") as {
          id: string;
          lastActive: number;
        }[];
      } catch {
        return [];
      }
    };

    // Cập nhật hoạt động tab hiện tại
    const updateTab = () => {
      const tabs = getTabs().filter((t) => Date.now() - t.lastActive < 5000);
      const exists = tabs.find((t) => t.id === tabId);
      const updated = exists
        ? tabs.map((t) =>
            t.id === tabId ? { ...t, lastActive: Date.now() } : t
          )
        : [...tabs, { id: tabId, lastActive: Date.now() }];
      localStorage.setItem(TAB_KEY, JSON.stringify(updated));
    };

    // Dọn dẹp tab không còn hoạt động
    const cleanupTabs = () => {
      const tabs = getTabs().filter((t) => Date.now() - t.lastActive < 5000);
      localStorage.setItem(TAB_KEY, JSON.stringify(tabs));

      if (tabs.length === 0) {
        // Logout khi không còn tab nào
        // localStorage.removeItem(TOKEN_KEY);
        if (onLogout) onLogout();
      }
    };

    // Cập nhật hoạt động định kỳ (đánh dấu tab còn sống)
    const interval = setInterval(updateTab, 1000);

    // Dọn dẹp định kỳ (phát hiện tab đã đóng)
    const cleanupInterval = setInterval(cleanupTabs, 2000);

    // Khi tab unload (reload hoặc đóng)
    window.addEventListener("beforeunload", cleanupTabs);

    // Khi storage thay đổi ở tab khác
    window.addEventListener("storage", cleanupTabs);

    // Đăng ký tab ngay khi mở
    updateTab();

    return () => {
      clearInterval(interval);
      clearInterval(cleanupInterval);
      window.removeEventListener("beforeunload", cleanupTabs);
      window.removeEventListener("storage", cleanupTabs);
      // cleanupTabs();
    };
  }, [onLogout]);
}
