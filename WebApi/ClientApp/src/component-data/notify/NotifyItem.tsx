import { Box, RelativeTime } from '@primer/react';
import React from 'react';
// import UserAvatar from '../../component-ui/user-avatar';
// import { INotifyUserItemRespone } from '../../models/responses/notify/INotifyUserItemRespone';
export interface INotifyUser {
    id: number;
    app_notify_id: number;
    object_data_json: string;
    title: string;
    title_en: string;
    content: string;
    content_en: string;
    app_account_id: number;
    created_at: string;
    sent_at: string | null;
    seem_time: string | null;
    seem_device_id: string;
    user_id: number;
}
export interface INotifyUserItemRespone extends INotifyUser {
    app_notify_type_id: number;
    app_notify_type_key: string;
    app_notify_type_icon: string;
    data: any;
}
interface INotifyItemProps {
    notify: INotifyUserItemRespone
}
const NotifyItem = ({ notify }: INotifyItemProps) => {
    return (
        <Box sx={{
            display: "flex",
            // alignItems: "center",
            pb: 2,
            pt: 2,
        }}>
            <Box id="avatar">
                {/* <UserAvatar
                    fullName='Vũ Thiên Hải'
                    url=''
                    size='large'
                /> */}
            </Box>
            <Box sx={{
                flex: 1,
                ml: 3,
                mr: 3
            }}>
                <Box className='limit2Line'>
                    <Box dangerouslySetInnerHTML={{ __html: notify.content }}></Box>
                </Box>
                <Box sx={{
                    fontSize: "12px",
                    color: "fg.muted"
                }}>
                    <RelativeTime datetime={notify.created_at} />
                </Box>
            </Box>
            {!notify.seem_time &&
                <Box sx={{
                    width: "10px",
                    height: "10px",
                    backgroundColor: "#0d6efd",
                    borderRadius: "50%",
                    mt: 1
                }}>
                    &nbsp;
                </Box>
            }
        </Box>
    );
};

export default NotifyItem;