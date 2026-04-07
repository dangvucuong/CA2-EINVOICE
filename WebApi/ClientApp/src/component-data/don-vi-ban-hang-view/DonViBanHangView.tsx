import React from 'react';
import { IDonVi } from '../../models/responses/category/IDonVi';
import { Box } from '@primer/react';
import Text from '../../component-ui/text';
interface IDonViBanHangViewProps {
    donvi: IDonVi
}
const DonViBanHangView = (props: IDonViBanHangViewProps) => {
    const { donvi } = props;
    return (
        <Box>
            <Box sx={{
                display: "flex",
                flexDirection: "column"
            }}>
                <Text text={donvi.ten_dv} sx={{ fontSize: 15, fontWeight: 500 }} />
                <Text text={`Mã số thuế: ${donvi.mst}`} sx={{ mt: 1 }} />
                <Text text={`Địa chỉ: ${donvi.dia_chi}`} sx={{ mt: 1 }} />
                <Text text={`${donvi.stk ? `Số tài khoản: ${donvi.stk}; ` : ''} ${donvi.ngan_hang ? `Ngân hàng: ${donvi.ngan_hang};` : ''}`} sx={{ mt: 1 }} />
                <Text text={`${donvi.dien_thoai ? `Điện thoại: ${donvi.dien_thoai}; ` : ''} ${donvi.email ? `Email: ${donvi.email}; ` : ''} ${donvi.fax ? `Fax: ${donvi.fax};` : ''}`}
                    sx={{ mt: 1 }}
                />

            </Box>
        </Box>
    );
};

export default DonViBanHangView