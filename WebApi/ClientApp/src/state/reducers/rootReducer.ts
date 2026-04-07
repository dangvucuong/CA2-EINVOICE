import { combineReducers } from "redux";
import { accountReducer } from './account/accountReducer';
import { forgetPWReducer } from './account/forgetPWReducer';
import { hangHoaReducer } from './category/hangHoaReducer';
import { khachHangReducer } from './category/khachHangReducer';
import { watermarkTemplateReducer } from './category/watermarkTemplateReducer';
import { appConfigReducer } from './commons/appConfigReducer';
import { localizedResourceReducer } from './commons/localizedResourceReducer';
import { mainLayoutReducer } from './commons/mainLayoutReducer';
import { companySizeReducer } from './contact/companySizeReducer';
import { contactReducer } from './contact/contactReducer';
import { contactStatusReducer } from './contact/contactStatusReducer';
import { dashBoardReducer } from './dashboard/dashBoardReducer';
import { hoaDonDangKyPhatHanhReducer } from './hoa-don/hoaDonDangKyPhatHanhReducer';
import { hoaDonReducer } from './hoa-don/hoaDonReducer';
import { hoaDonSelectBoxReducer } from './hoa-don/hoaDonSelectBoxReducer';
import { loaiHoaDonCTReducer } from './hoa-don/loaiHoaDonCTReducer';
import { loaiHoaDonCTTemplateReducer } from './hoa-don/loaiHoaDonCTTemplateReducer';
import { loaiHoaDonReducer } from './hoa-don/loaiHoaDonReducer';
import { mauHoaDonReducer } from './hoa-don/mauHoaDonReducer';
import { notifyReducer } from './notify/notifyReducer';
import { toKhaiReducer } from './to-khai/toKhaiReducer';
import { apiReducer } from './user/apiReducer';
import { logReducer } from './user/logReducer';
import { menuReducer } from './user/menuReducer';
import { roleApiReducer } from './user/roleApiReducer';
import { roleReducer } from './user/roleReducer';
import { roleSubSystemReducer } from './user/roleSubSytemReducer';
import { subSystemReducer } from './user/subSystemReducer';
import { userReducer } from './user/userReducer';
import { donViReducer } from "./category/donViReducer";
import { daiLyReducer } from "./category/daiLyReducer";

const rootReducer = combineReducers({
    accountReducer,
    forgetPWReducer,
    category: combineReducers({
        daiLyReducer,
        donViReducer,
        khachHangReducer,
        hangHoaReducer,
        watermarkTemplateReducer
    }),
    common: combineReducers({
        mainLayoutReducer,
        appConfigReducer,
        localizedResourceReducer
    }),
    contact: combineReducers({
        contactReducer,
        contactStatusReducer,
        companySizeReducer
    }),
    hoaDon: combineReducers({
        loaiHoaDonCTReducer,
        loaiHoaDonReducer,
        loaiHoaDonCTTemplateReducer,
        mauHoaDonReducer,
        hoaDonDangKyPhatHanhReducer,
        hoaDonReducer,
        hoaDonSelectBoxReducer
    }),
  
    notify: combineReducers({
        notifyReducer
    }),
    toKhai: combineReducers({
        toKhaiReducer
    }),
    user: combineReducers({
        userReducer,
        roleReducer,
        subSystemReducer,
        menuReducer,
        apiReducer,
        roleSubSystemReducer,
        roleApiReducer,
        logReducer
    }),
    dashBoard: dashBoardReducer
})

export default rootReducer

export type RootState = ReturnType<typeof rootReducer>
