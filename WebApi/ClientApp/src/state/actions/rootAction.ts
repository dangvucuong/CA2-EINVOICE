import { forgetPWAction } from './account/forgetPWAction';
import { hangHoaAction } from './category/hangHoaAction';
import { khachHangAction } from './category/khachHangAction';
import { companySizeAction } from './contact/companySizeAction';
import { contactAction } from './contact/contactAction';
import { contactStatusAction } from './contact/contactStatusAction';
import { dashBoardAction } from './dashboard/dashBoardAction';
import { hoaDonAction } from './hoa-don/hoaDonAction';
import { hoaDonDangKyPhatHanhAction } from './hoa-don/hoaDonDangKyPhatHanhAction';
import { loaiHoaDonAction } from './hoa-don/loaiHoaDonAction';
import { loaiHoaDonCTAction } from './hoa-don/loaiHoaDonCTAction';
import { logAction } from './user/logAction';
import { roleSubSystemAction } from './user/roleSubSystemAction';

import { accountAction } from './account/accountAction';
import { appConfigAction } from './commons/appConfigAction';
import { localizedResourceAction } from './commons/localizedResourceAction';
import { mainLayoutAction } from './commons/mainLayoutAction';
import { loaiHoaDonCTTemplateAction } from './hoa-don/loaiHoaDonCTTemplateAction';
import { mauHoaDonAction } from './hoa-don/mauHoaDonAction';
import { notifyAction } from './notify/notifyAction';
import { toKhaiAction } from './to-khai/toKhaiAction';
import { apiAction } from './user/apiAction';
import { menuAction } from './user/menuAction';
import { roleAction } from './user/roleAction';
import { roleApiAction } from './user/roleApiAction';
import { subSystemAction } from './user/subSystemAction';
import { userAction } from './user/userAction';
import { watermarkTemplateAction } from './category/watermarkTemplateAction';
import { hoaDonSelectBoxAction } from './hoa-don/hoaDonSelectBoxAction';
import { donViActionType } from './category/donViActionType';
import { daiLyAction } from './category/daiLyAction';
export const rootAction = {
    accountAction,
    forgetPWAction,
    category: {
        daiLyAction,
        donViActionType,
        khachHangAction,
        hangHoaAction,
        watermarkTemplateAction
    },
    contact: {
        contactAction,
        companySizeAction,
        contactStatusAction
    },
    common: {
        mainLayoutAction,
        appConfigAction,
        localizedResourceAction
    },
    hoaDon: {
        loaiHoaDonAction,
        loaiHoaDonCTAction,
        loaiHoaDonCTTemplateAction,
        mauHoaDonAction,
        hoaDonDangKyPhatHanhAction,
        hoaDonAction,
        hoaDonSelectBoxAction
    },
    notify: {
        notifyAction
    },
    toKhai: {
        toKhaiAction
    },
    user: {
        userAction,
        roleAction,
        subSystemAction,
        menuAction,
        apiAction,
        roleApiAction,
        roleSubSystemAction,
        logAction
    },
    dashBoard:dashBoardAction
}