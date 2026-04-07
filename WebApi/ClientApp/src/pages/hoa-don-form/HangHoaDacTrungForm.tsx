import { Box } from '@primer/react';
import SelectBoxHangHoaDacTrung from '../../component-data/selectbox-hang-hoa-dac-trung';
import { IHoaDonHangHoa } from '../../models/responses/hoa-don/IHoaDonHangHoa';
import { IHoaDonHangHoaDacTrung } from '../../models/responses/hoa-don/IHoaDonHangHoaDacTrung';
import TextInput from '../../component-ui/text-input';
interface IHangHoaDacTrungFormProps {
    hangHoa: IHoaDonHangHoa,
    onValueChanged: (hangHoa: IHoaDonHangHoa) => void
}
const HangHoaDacTrungForm = (props: IHangHoaDacTrungFormProps) => {
    const { hangHoa, onValueChanged } = props;
    const data: IHoaDonHangHoaDacTrung | undefined = hangHoa?.hang_hoa_dac_trung_json ? JSON.parse(hangHoa?.hang_hoa_dac_trung_json) : undefined;
    return (
        <Box sx={{ display: "grid", gap: 2 }}>
            <Box>
                <SelectBoxHangHoaDacTrung
                    value={data?.LHHDTrung ?? 0}
                    onValueChanged={(id) => {
                        onValueChanged({
                            ...hangHoa,
                            hang_hoa_dac_trung_json: JSON.stringify({
                                ...data,
                                LHHDTrung: id
                            })
                        })
                    }}
                />
            </Box>
            <Box>
                {/* ô tô, xe máy */}
                {data?.LHHDTrung === 1 &&
                    <Box sx={{
                        display: "grid",
                        gap: 4,
                        gridTemplateColumns: "1fr 1fr"
                    }}>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Số khung</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.SKhung}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            SKhung: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Số máy</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.SMay}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            SMay: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>
                    </Box>
                }
                {/* dịch vụ */}
                {data?.LHHDTrung === 2 &&
                    <Box sx={{
                        display: "grid",
                        gap: 2,
                        gridTemplateColumns: "1fr"
                    }}>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Biển kiểm soát</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.BKSPTVChuyen}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            BKSPTVChuyen: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>

                    </Box>
                }
                {/* nền tảng số */}
                {data?.LHHDTrung === 3 &&
                    <Box sx={{
                        display: "grid",
                        gap: 2,
                        gridTemplateColumns: "1fr 1fr"
                    }}>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Tên người gửi hàng</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.TNGHang}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            TNGHang: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Địa chỉ người gửi hàng</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.DCNGHang}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            DCNGHang: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Mã số thuế người gửi hàng</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.MSTNGHang}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            MSTNGHang: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>
                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                            <Box sx={{ fontWeight: "600" }}>Mã định danh người gửi hàng</Box>
                            <TextInput sx={{
                                flex: 1
                            }}
                                defaultValue={data?.MDDNGHang}
                                onBlur={(e) => {
                                    onValueChanged({
                                        ...hangHoa,
                                        hang_hoa_dac_trung_json: JSON.stringify({
                                            ...data,
                                            MDDNGHang: e.target.value
                                        })
                                    })
                                }}
                            />
                        </Box>

                    </Box>
                }
            </Box>
        </Box>
    );
};

export default HangHoaDacTrungForm;