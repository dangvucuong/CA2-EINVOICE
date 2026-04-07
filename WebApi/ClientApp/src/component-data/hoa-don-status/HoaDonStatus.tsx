import { Box, Label } from '@primer/react';
import { useHoaDonTrangThaiHook } from '../../hooks/useHoaDonTrangThai';
interface IHoaDonStatusProps {
    id: number
}

const HoaDonStatus = (props: IHoaDonStatusProps) => {
    const {hoaDonTrangThai}= useHoaDonTrangThaiHook(props.id)
    
    return (
        <Box sx={{
            display: "flex",
            alignItems: "center"
        }}>
            <Label sx={{
                color: hoaDonTrangThai?.color
            }}>
                <Box sx={{
                    mr: 1,
                }}
                    color={hoaDonTrangThai?.color}
                >
                    <Box
                        bg={hoaDonTrangThai?.color}
                        borderColor={hoaDonTrangThai?.color}
                        width={12}
                        height={12}
                        borderRadius={10}
                        margin="auto"
                        borderWidth="1px"
                        borderStyle="solid"
                    />
                </Box>
                <Box sx={{}}>
                    {hoaDonTrangThai?.name}
                </Box>
            </Label>
        </Box>
    );
};

export default HoaDonStatus;