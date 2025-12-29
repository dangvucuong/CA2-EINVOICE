import { Box } from '@primer/react';
import React from 'react';
import { eLoaiToKhai } from '../../models/commons/eLoaiToKhai';
interface ILoaiToKhaiProps {
    id: number
}
const LoaiToKhai = (props: ILoaiToKhaiProps) => {
    return (
        <Box>
            {props.id === eLoaiToKhai.DANG_KY_MOI && <>Đăng ký mới</>}
            {props.id === eLoaiToKhai.THAY_DOI_THONG_TIN && <>Thay đổi thông tin</>}
        </Box>
    );
};

export default LoaiToKhai;