import {
  ChangePWPage,
  ContactPage,
  HangHoaPage,
  HoaDonForm,
  HomePage,
  KhachHangPage,
  LogPage,
  MauHoDonPage,
  MauHoaDonListPage,
  RolePage,
  UserPage,
} from "../../pages";
import BangTongHopForm from "../../pages/bang-tong-hop/BangTongHopForm";
import BangTongHopPage from "../../pages/bang-tong-hop/BangTongHopPage";
import QuanlychungtuPage from "../../pages/chung-tu";
import ChungTuForm from "../../pages/chung-tu-form";
import ChungTuPhatHanhPage from "../../pages/chung-tu-phat-hanh";
import ChungTuThayThePage from "../../pages/chung-tu-thay-the";
import DaiLyPage from "../../pages/dai-ly";
import DashBoardPage from "../../pages/dashboard";
import DonViPage from "../../pages/don-vi";
import HoaDonPage from "../../pages/hoa-don";
import HoaDonDieuChinhPage from "../../pages/hoa-don-dieu-chinh";
import HoaDonMayTinhTienPage from "../../pages/hoa-don-may-tien-tien";
import HoaDonThayThePage from "../../pages/hoa-don-thay-the";
import HoaDonViewPage from "../../pages/hoa-don-view";
import KySoConfigPage from "../../pages/ky-so-config";
import MauChungTuPage from "../../pages/mau-chung-tu";
import TaiNguyenPage from "../../pages/tai-nguyen";
import ThongBaoSaiSotPage from "../../pages/tbss";
import ThongBaoSaiSotCTPage from "../../pages/tbss-ct";
import ThongBaoSaiSotCTForm from "../../pages/tbss-ct/ThongBaoSaiSotCTForm";
import ThongBaoSaiSotForm from "../../pages/tbss/ThongBaoSaiSotForm";
import ThongKePage from "../../pages/thong-ke";
import ToKhaiPage from "../../pages/to-khai";
import ToKhaiChungTuPage from "../../pages/to-khai-chung-tu";
import { ToKhaiCTForm } from "../../pages/to-khai-chung-tu/ToKhaiCTForm";
import { ToKhaiForm } from "../../pages/to-khai/ToKhaiForm";
import { HoaDonPhatHanhPage } from "./../../pages/index";

const appRouters = [
  {
    path: "/home",
    component: HomePage,
  },
  {
    path: "/change-pw",
    component: ChangePWPage,
  },
  {
    path: "/user",
    component: UserPage,
  },
  {
    path: "/role",
    component: RolePage,
  },
  {
    path: "/khach-hang",
    component: KhachHangPage,
  },
  {
    path: "/don-vi",
    component: DonViPage,
  },
  {
    path: "/hang-hoa",
    component: HangHoaPage,
  },
  {
    path: "/dai-ly",
    component: DaiLyPage,
  },
  {
    path: "/contact",
    component: ContactPage,
  },
  {
    path: "/log",
    component: LogPage,
  },
  {
    path: "/tai-nguyen",
    component: TaiNguyenPage,
  },
  {
    path: "/to-khai",
    component: ToKhaiPage,
  },
  {
    path: "/to-khai/:id",
    component: ToKhaiForm,
  },
  {
    path: "/mau-hoa-don-form/:id",
    component: MauHoDonPage,
  },
  {
    path: "/mau-hoa-don",
    component: MauHoaDonListPage,
  },
  {
    path: "/hoa-don/form/:id",
    component: HoaDonForm,
  },
  {
    path: "/hoa-don/view/:id",
    component: HoaDonViewPage,
  },
  {
    path: "/hoa-don/:tab?",
    component: HoaDonPage,
  },
  {
    path: "/hoa-don-mtt/form/:id",
    component: HoaDonForm,
  },
  {
    path: "/hoa-don-dieu-chinh/form/:id",
    component: HoaDonForm,
  },
  {
    path: "/hoa-don-thay-the/form/:id",
    component: HoaDonForm,
  },
  {
    path: "/hoa-don-mtt/:tab?",
    component: HoaDonMayTinhTienPage,
  },

  {
    path: "/dang-ky-phat-hanh-hoa-don",
    component: HoaDonPhatHanhPage,
  },
  {
    path: "/thong-ke/:tab?/:mode?",
    component: ThongKePage,
  },
  {
    path: "/hoa-don-dieu-chinh",
    component: HoaDonDieuChinhPage,
  },
  {
    path: "/hoa-don-thay-the",
    component: HoaDonThayThePage,
  },
  {
    path: "/ky-so-config",
    component: KySoConfigPage,
  },
  {
    path: "/dashboard",
    component: DashBoardPage,
  },
  {
    path: "/tbss",
    component: ThongBaoSaiSotPage,
  },

  {
    path: "/tbss/:id",
    component: ThongBaoSaiSotForm,
  },
  {
    path: "/bang-tong-hop",
    component: BangTongHopPage,
  },
  {
    path: "/bang-tong-hop/:id",
    component: BangTongHopForm,
  },

  //Chứng từ
  //Chứng từ
  {
    path: "/chung-tu/:tab?",
    component: QuanlychungtuPage,
  },
  {
    path: "/chung-tu/form/:id",
    component: ChungTuForm,
  },
  {
    path: "/to-khai-chung-tu",
    component: ToKhaiChungTuPage,
  },
  {
    path: "/to-khai-chung-tu/:id",
    component: ToKhaiCTForm,
  },
  {
    path: "/dang-ky-phat-hanh-chung-tu",
    component: ChungTuPhatHanhPage,
  },
  {
    path: "/tbss-ct",
    component: ThongBaoSaiSotCTPage,
  },
  {
    path: "/tbss-ct/:id",
    component: ThongBaoSaiSotCTForm,
  },

  {
    path: "/chung-tu-thay-the",
    component: ChungTuThayThePage,
  },
  {
    path: "/mau-chung-tu",
    component: MauChungTuPage,
  },
];
export default appRouters.map((route: any) => {
  return {
    ...route,
    //   component: withNavigationWatcher(route.component)
  };
});
