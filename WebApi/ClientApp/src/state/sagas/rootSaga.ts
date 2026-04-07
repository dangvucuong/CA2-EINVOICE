
import { all } from "redux-saga/effects"
import { accountSaga } from "./account/accountSaga"
import { appConfigSaga } from "./common/appConfigApiSaga"
import { localizedResourceSaga } from "./common/localizedResourceSaga"
import { apiSaga } from "./user/apiSaga"
import { menuSaga } from "./user/menuSaga"
import { roleApiSaga } from "./user/roleApiSaga"
import { roleSaga } from "./user/roleSaga"
import { roleSubSystemSaga } from "./user/roleSubSystemSaga"
import { subSystemSaga } from "./user/subSystemSaga"
import { userSaga } from "./user/userSaga"
import { khachHangSaga } from "./category/khachHangSaga"
import { hangHoaSaga } from "./category/hangHoaSaga"
import { forgetPWSaga } from "./account/forgetPWSaga"
import { contactSaga } from "./contact/contactSaga"
import { contactStatusSaga } from "./contact/contactStatusSaga"
import { companySizeSaga } from "./contact/companySizeSaga"
import { notifySaga } from "./notify/notifySaga"
import { logSaga } from "./user/logSaga"
import { toKhaiSaga } from "./to-khai/toKhaiSaga"
import { loaiHoaDonSaga } from "./hoa-don/loaiHoaDonSaga"
import { loaiHoaDonCTSaga } from "./hoa-don/loaiHoaDonCTSaga"
import { loaiHoaDonCTTemplateSaga } from "./hoa-don/loaiHoaDonCTTemplateSaga"
import { mauHoaDonSaga } from "./hoa-don/mauHoaDonSaga"
import { hoaDonDangKyPhatHanhSaga } from "./hoa-don/hoaDonDangKyPhatHanhSaga"
import { hoaDonSaga } from "./hoa-don/hoaDonSaga"
import { dashBoardSaga } from "./dashboard/dashBoardSaga"
import { watermarkTemplateSaga } from "./category/watermarkTemplateSaga"
import { hoaDonSelectBoxSaga } from "./hoa-don/hoaDonSelectBoxSaga"
import { donViSaga } from "./category/donViSaga"
import { daiLySaga } from "./category/daiLySaga"
export default function* rootSaga() {
    yield all([
        accountSaga(),
        appConfigSaga(),
        apiSaga(),
        //
        contactSaga(),
        contactStatusSaga(),
        companySizeSaga(),
        //
        daiLySaga(),
        dashBoardSaga(),
        donViSaga(),
        //
        forgetPWSaga(),
        //
        hangHoaSaga(),
        hoaDonDangKyPhatHanhSaga(),
        hoaDonSaga(),
        hoaDonSelectBoxSaga(),
        //
        khachHangSaga(),
        //
        localizedResourceSaga(),
        logSaga(),
        loaiHoaDonSaga(),
        loaiHoaDonCTSaga(),
        loaiHoaDonCTTemplateSaga(),

        mauHoaDonSaga(),
        menuSaga(),

        notifySaga(),

        roleSaga(),
        roleApiSaga(),
        roleSubSystemSaga(),

        subSystemSaga(),

        toKhaiSaga(),

        userSaga(),
        //
        watermarkTemplateSaga()
    ])


}
