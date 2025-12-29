import { NoteIcon } from '@primer/octicons-react';
import { Box } from '@primer/react';
import { useCommonContext } from '../../contexts/common';
import { useAppSelector } from '../../hooks/useAppSelector';
import RoleSubSystemMenu from './RoleSubSystemMenu';

const RoleDetail = () => {
    const { roleEditing } = useAppSelector(x => x.user.roleReducer)
    const { checkAccesiableTo } = useCommonContext();
    
    return (
        <Box>
            {!roleEditing &&
                <Box sx={{
                    p: 3
                }}>
                    <Box sx={{
                        height: window.innerHeight - 100,
                        backgroundColor: "canvas.subtle",
                        borderRadius: 2,
                        display: "flex",
                        flexDirection: "column",
                        flex: 1,
                        // alignItems:"center",
                        justifyContent: "center"
                    }}>
                        <Box>
                            {/* <Blankslate.Visual>
                                <NoteIcon size="medium" />
                            </Blankslate.Visual>
                            <Blankslate.Heading>Chọn một vai trò từ danh sách để cấu hình</Blankslate.Heading>
                            <Blankslate.Description>
                                Để cấu hình vai trò đang chọn được truy cập các phần mềm nào, tick chọn tại phần mềm tương ứng
                            </Blankslate.Description>
                            <Blankslate.Description>
                                Để cấu hình các chức năng mà vai trò được truy cập, tick chọn tại các chức năng tương ứng
                            </Blankslate.Description> */}

                        </Box>
                    </Box>
                </Box>
            }
            {roleEditing &&
                <Box sx={{
                    display: "flex",
                    p: 0
                }}>

                    <Box sx={{
                        flex: 1,
                        height: window.innerHeight - 75,
                        overflowY: "scroll"
                    }}>
                        <RoleSubSystemMenu/>
                    </Box>
                </Box>
            }
        </Box>
    );
};

export default RoleDetail;