import { useMemo } from "react";

const thongBaoSaiSotTinhChats = [
  {
    id: 0,
    name: "Mới",
    color: "#d73a4a",
  },
  // {
  //     id: 1,
  //     name: 'Hủy',
  //     color: "#d73a4a"
  // },
  {
    id: 2,
    name: "Điều chỉnh",
    color: "#a2eeef",
  },
  {
    id: 3,
    name: "Thay thế",
    color: "#8dc6fc",
  },
  {
    id: 4,
    name: "Giải trình",
    color: "#ffd78e",
  },
  {
    id: 5,
    name: "Sai sót do tổng hợp",
    color: "#ffd78e",
  },

  {
    id: 6,
    name: "Thông báo",
    color: "#ffd78e",
  },
];

export const useThongBaoSaiSotTinhChatsHook = () => {
  return {
    thongBaoSaiSotTinhChats: thongBaoSaiSotTinhChats,
  };
};

export const useThongBaoSaiSotTinhChatHook = (id: number) => {
  const { thongBaoSaiSotTinhChats } = useThongBaoSaiSotTinhChatsHook();
  const thongBaoSaiSotTinhChat = useMemo(() => {
    return thongBaoSaiSotTinhChats.find((x) => x.id === id);
  }, [thongBaoSaiSotTinhChats, id]);
  return {
    thongBaoSaiSotTinhChat,
  };
};
