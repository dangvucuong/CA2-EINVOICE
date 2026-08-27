import HoaDonViewPage from "../../pages/hoa-don-view";
import TraCuuPage from "../../pages/tra-cuu";
import TraCuuChungTuPage from "../../pages/tra-cuu-chung-tu";

/** Giữ đồng bộ với PublicContent (PublicContent dùng lazy import). */
const publicRouter = [
  {
    path: "/tra-cuu",
    component: TraCuuPage,
  },
  {
    path: "/tra-cuu-chung-tu",
    component: TraCuuChungTuPage,
  },
  {
    path: "/hoa-don/view/:id",
    component: HoaDonViewPage,
  },
];
export default publicRouter.map((route: any) => {
  return {
    ...route,
  };
});
