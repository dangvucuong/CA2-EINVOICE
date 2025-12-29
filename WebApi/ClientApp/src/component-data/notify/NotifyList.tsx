import { BellIcon, BellFillIcon, EyeClosedIcon, KebabHorizontalIcon, PencilIcon } from "@primer/octicons-react";
import { ActionList, ActionMenu, Box, IconButton, UnderlineNav } from '@primer/react';
import { useEffect } from "react";
import Heading from '../../component-ui/heading';
import { useCommonContext } from "../../contexts/common";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import NotifyItem from "./NotifyItem";
const NotifyList = () => {
    const { translate } = useCommonContext();
    // const { notifies, status } = useAppSelector(x => x.notifyReducer);
    const dispatch = useAppDispatch();
    // useEffect(() => {
    //     if (status === eReducerStatusBase.is_not_initialization || status === eReducerStatusBase.is_need_reload) {
    //         dispatch(rootAction.notifyAction.loadStart({
    //             is_unread_only: false,
    //             paging: {
    //                 page_size: 20,
    //             }
    //         }));
    //     }
    // }, [status])
    return (
        <Box sx={{
            p: 3,
            flex: 1,
            display: "flex",
            flexDirection: "column"
        }}>
            <Box sx={{
                display: "flex"
            }}>
                <Box sx={{
                    flex: 1
                }}>
                    <Heading text='Thông báo' />
                </Box>
                <Box>
                    <ActionMenu>

                        <ActionMenu.Anchor>
                            <IconButton icon={KebabHorizontalIcon} aria-label="Open menu" variant="invisible" />
                        </ActionMenu.Anchor>
                        <ActionMenu.Overlay width="medium">
                            <ActionList>
                                <ActionList.Item
                                    disabled={false}
                                    onSelect={() => {
                                        // dispatch(rootAction.dg_chuan.ket_qua.boMinhChungAction.showEditModal(props.boMinhChung))

                                    }}>
                                    <ActionList.LeadingVisual>
                                        <PencilIcon />
                                    </ActionList.LeadingVisual>
                                    {translate('Đánh dấu tất cả là đã đọc')}
                                </ActionList.Item>
                                {/* <ActionList.Divider /> */}

                            </ActionList>
                        </ActionMenu.Overlay>
                    </ActionMenu>
                </Box>

            </Box>
            <Box sx={{
                ml: -3,
                mr: -3
            }}>
                <UnderlineNav aria-label="Repository with icons">
                    <UnderlineNav.Item icon={BellIcon} aria-current="page">
                        {translate('Tất cả')}
                    </UnderlineNav.Item>
                    <UnderlineNav.Item icon={EyeClosedIcon}>
                        {translate('Chưa đọc')}
                    </UnderlineNav.Item>

                </UnderlineNav>
            </Box>
            <Box sx={{
                height: window.innerHeight - 100,
                mt: 1,
                ml: -3,
                mr: -3
            }}>
                <ActionList showDividers>
                    {/* {notifies.map(notify => {
                        return (
                            <ActionList.Item key={notify.id}>
                                <NotifyItem notify={notify} />
                            </ActionList.Item>
                        );
                    })} */}



                </ActionList>
            </Box>
        </Box>
    );
};

export default NotifyList;