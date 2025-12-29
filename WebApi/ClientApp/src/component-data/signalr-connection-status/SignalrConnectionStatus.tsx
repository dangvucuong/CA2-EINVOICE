import { Box, useConfirm } from '@primer/react';
import { StopIcon } from '@primer/octicons-react';
import React from 'react';
import { useCommonContext } from '../../contexts/common';
import Button from '../../component-ui/button';
import { useAuth } from '../../hooks/useAuth';

const ConnectingIcon = () => {
    return (
        <Box sx={{
            width: "16px"
        }}>
            <svg aria-label="currently running" width="100%" height="100%" fill="none" viewBox="0 0 16 16" className="anim-rotate" xmlns="http://www.w3.org/2000/svg">
                <path fill="none" stroke="#DBAB0A" stroke-width="2" d="M3.05 3.05a7 7 0 1 1 9.9 9.9 7 7 0 0 1-9.9-9.9Z" opacity=".5"></path>
                <path fill="#DBAB0A" fill-rule="evenodd" d="M8 4a4 4 0 1 0 0 8 4 4 0 0 0 0-8Z" clip-rule="evenodd"></path>
                <path fill="#DBAB0A" d="M14 8a6 6 0 0 0-6-6V0a8 8 0 0 1 8 8h-2Z"></path>
            </svg>
        </Box>
    )
}

const ConnectedIcon = () => {
    return (
        <Box sx={{
            width: "16px"
        }}>
            <svg aria-label="currently running" width="100%" height="100%" fill="none" viewBox="0 0 16 16" className="anim-zoom-in-zoom-out" xmlns="http://www.w3.org/2000/svg">
                <path fill="none" stroke="green" stroke-width="2" d="M3.05 3.05a7 7 0 1 1 9.9 9.9 7 7 0 0 1-9.9-9.9Z" opacity="1"></path>
                <path fill="green" fill-rule="evenodd" d="M8 4a4 4 0 1 0 0 8 4 4 0 0 0 0-8Z" clip-rule="evenodd"></path>
                <path fill="green" d="M14 8a6 6 0 0 0-6-6V0a8 8 0 0 1 8 8h-2Z"></path>
            </svg>
        </Box>
    )
}


const SignalrConnectionStatus = () => {
    const confirm = useConfirm();

    const { _signalrConnected, _signalrReConnectedCount, _signalrStopped, handleReconnect, handleDisconnect, _signalrReConnectMaxCount } = useCommonContext();
    // console.log({
    //     _signalrStopped,
    //     _signalrReConnectedCount,
    //     _signalrConnected,
    //     _signalrReConnectMaxCount

    // });
    const { user } = useAuth();
    const handleReconnectClick = async () => {
        if (await confirm({
            title: "Kết nối đến tool ký số",
            content: "Vui lòng đảm bảo đã cài đặt tool ký số và đang cắm USB token",
            cancelButtonContent: "Đóng",
            confirmButtonContent: "Kết nối",
            confirmButtonType: "primary"
        })) {
            handleReconnect();
        }
    }
    const handleDisconnnectClick = async () => {
        if (await confirm({
            title: "Ngắt nối đến tool ký số",
            content: "Bạn sẽ không thực hiện được các thao tác ký số và phát hành nếu chưa kết nối đến tool ký số",
            cancelButtonContent: "Đóng",
            confirmButtonContent: "Ngắt kết nối",
            confirmButtonType: "danger"
        })) {
            handleDisconnect();
        }
    }

    return (
        <Box>
            {user && !user.is_hsm_signing && !user.is_remote_signing && <>
                {!_signalrConnected &&
                    <>
                        {_signalrStopped &&
                            <Box>
                                <Button text={`Chưa kết nối`} size='medium' tooltip='Chưa kết nối đến tool ký số' tooltipdDirection='s'
                                    onClick={handleReconnectClick}
                                    leadingVisual={StopIcon}
                                    variant='danger'
                                />
                            </Box>
                        }
                        {!_signalrStopped &&
                            <Box>
                                <Button text={`Đang kết nối ${_signalrReConnectedCount > 0 ? `(${_signalrReConnectMaxCount - _signalrReConnectedCount})` : ""}`} size='medium' leadingVisual={ConnectingIcon} tooltip='Đang kết nối đang tool Chữ ký số' tooltipdDirection='s' />
                            </Box>
                        }
                    </>
                }
                {_signalrConnected &&
                    <Box>
                        <Button text={"Đã kết nối"} size='medium' leadingVisual={ConnectedIcon} tooltip='Đã kết nối đang tool Chữ ký số' tooltipdDirection='s'
                            onClick={handleDisconnnectClick}
                        />
                    </Box>}
            </>
            }



        </Box>
    );
};

export default SignalrConnectionStatus;