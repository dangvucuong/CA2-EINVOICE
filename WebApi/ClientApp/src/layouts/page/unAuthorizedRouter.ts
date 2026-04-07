import ForgetPWPage from "../../pages/forget-pw";
import DeletePasskey from "../../pages/login/DeletePasskey";
import LoginPage from "../../pages/login/LoginPage";
import RegisterPasskey from "../../pages/login/RegisterPasskey";
import RegisterPage from "../../pages/register";

const unAuthorizedRouter = [
    {
        path: '/login',
        component: LoginPage
    },
    {
        path: '/forget-pw',
        component: ForgetPWPage
    },
    {
        path: '/register',
        component: RegisterPage
    },
    {
        path: '/dang-ky-passkey',
        component: RegisterPasskey
    },
    {
        path: '/xoa-passkey',
        component: DeletePasskey
    },
    // {
    //     path: '/tra-cuu',
    //     component: TraCuuPage
    // },


]
export default unAuthorizedRouter.map((route: any) => {
    return {
        ...route
        //   component: withNavigationWatcher(route.component)
    };
});
