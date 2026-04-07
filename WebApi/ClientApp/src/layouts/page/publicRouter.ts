import HoaDonViewPage from "../../pages/hoa-don-view";
import TraCuuPage from "../../pages/tra-cuu";

const publicRouter = [
    {
        path: '/tra-cuu',
        component: TraCuuPage
    },
    {
        path: '/hoa-don/view/:id',
        component: HoaDonViewPage
    },

]
export default publicRouter.map((route: any) => {
    return {
        ...route
        //   component: withNavigationWatcher(route.component)
    };
});
