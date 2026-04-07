import { Box, Label } from '@primer/react';
import { useThongBaoSaiSotTinhChatHook } from '../../hooks/useThongBaoSaiSotTinhChatHook';
interface IThongBaoSaiSotTinhChatProps {
    id: number
}

const ThongBaoSaiSotTinhChat = (props: IThongBaoSaiSotTinhChatProps) => {
    const { thongBaoSaiSotTinhChat } = useThongBaoSaiSotTinhChatHook(props.id)

    return (
        <Box sx={{
            display: "flex",
            alignItems: "center"
        }}>
            <Label sx={{
                // color: thongBaoSaiSotTinhChat?.color
            }}>
                <Box sx={{
                    mr: 1,
                }}
                    color={thongBaoSaiSotTinhChat?.color}
                >
                    <Box
                        bg={thongBaoSaiSotTinhChat?.color}
                        borderColor={thongBaoSaiSotTinhChat?.color}
                        width={12}
                        height={12}
                        borderRadius={10}
                        margin="auto"
                        borderWidth="1px"
                        borderStyle="solid"
                    />
                </Box>
                <Box sx={{}}>
                    {thongBaoSaiSotTinhChat?.name}
                </Box>
            </Label>
        </Box>
    );
};

export default ThongBaoSaiSotTinhChat;