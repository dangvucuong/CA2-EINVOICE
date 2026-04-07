import { Box, Label } from '@primer/react';
import { useThongBaoSaiSotStatusHook } from '../../hooks/useThongBaoSaiSotStatusHook';
interface ThongBaoSaiSotStatusProps {
    id: number
}

const ThongBaoSaiSotStatus = (props: ThongBaoSaiSotStatusProps) => {
    const { thongBaoSaiSotTrangThai } = useThongBaoSaiSotStatusHook(props.id)

    return (
        <Box sx={{
            display: "flex",
            alignItems: "center"
        }}>
            <Label sx={{
                // color: thongBaoSaiSotTrangThai?.color
            }}>
                <Box sx={{
                    mr: 1,
                }}
                    color={thongBaoSaiSotTrangThai?.color}
                >
                    <Box
                        bg={thongBaoSaiSotTrangThai?.color}
                        borderColor={thongBaoSaiSotTrangThai?.color}
                        width={12}
                        height={12}
                        borderRadius={10}
                        margin="auto"
                        borderWidth="1px"
                        borderStyle="solid"
                    />
                </Box>
                <Box sx={{}}>
                    {thongBaoSaiSotTrangThai?.name}
                </Box>
            </Label>
        </Box>
    );
};

export default ThongBaoSaiSotStatus;