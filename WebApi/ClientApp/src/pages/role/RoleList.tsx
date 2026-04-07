import { KebabHorizontalIcon, PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { ActionList, ActionMenu, Box, IconButton } from '@primer/react';

import { useEffect, useMemo, useState } from "react";
import { ROLE_API_ENDPOINT, ROLE_API_VIEWALL_ENDPOINT } from "../../api/user/roleApi";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import Heading from "../../component-ui/heading";
import Text from "../../component-ui/text";
import TextSearch from '../../component-ui/text-search';
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eSize } from "../../models/commons/eSize";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import RoleEditForm from "./RoleEditForm";
import { useAuth } from "../../hooks/useAuth";


const RoleList = () => {
    const { user } = useAuth();
    const { status, roles, roleEditing } = useAppSelector(x => x.user.roleReducer);
    const [isShowModal, setIsShowModal] = useState(false);
    const [isShowDeleteConfirm, setIsShowDeleteConfirm] = useState(false);
    const { checkAccesiableTo } = useCommonContext();
    const dispatch = useAppDispatch();
    const isCanNotView = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT, "GET") && !checkAccesiableTo(ROLE_API_ENDPOINT + "-public", "GET")
    }, [])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT, "PUT")
    }, [])
    const isCanNotDelete = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT + "/{id}", "DELETE")
    }, [])
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization || status === eReducerStatusBase.is_need_reload) {
            dispatch(rootAction.user.roleAction.loadStart());
        }
    }, [status])
    useEffect(() => {
        if (status === eReducerStatusBase.is_saved) {
            setIsShowModal(false);
            NotifyHelper.Success("Saved !");
            dispatch(rootAction.user.roleAction.loadStart());
        }
    }, [status])
    useEffect(() => {
        if (status === eReducerStatusBase.is_deleted) {
            setIsShowDeleteConfirm(false);
            NotifyHelper.Success("Deleted !");
            dispatch(rootAction.user.roleAction.loadStart());
        }
    }, [status])
    const handleAddNewClick = () => {
        setIsShowModal(true);
        // const obj: any = { id: 0 };
        dispatch(rootAction.user.roleAction.changeEditing(undefined));
    }
    return (
        <Box>
            <Box sx={{
                display: "flex"
            }}>
                <Heading text="Roles" size={eSize.medium} sx={{
                    flex: 1
                }} />
                <Button variant='primary' leadingVisual={PlusIcon} onClick={handleAddNewClick}
                    apiAuthorized={ROLE_API_ENDPOINT}
                    apiAuthorizedMethod="POST"
                >
                    <Text text="Thêm mới" />
                </Button>
            </Box>
            <Box sx={{
                mt: 2
            }}>
                <TextSearch placeholder="Tìm kiếm ..." />
            </Box>
            <Box sx={{
                mt: 2,
                ml: -2,
                mr: -2
            }}>
                {isCanNotView && <UnAuthorizedPage />}
                {!isCanNotView &&
                    <ActionList selectionVariant="single" showDividers role="menu" aria-label="">
                        {roles.map((role, index) => {

                            let isAllow = false;
                            if (role.donvi_ma_dv === user?.donvi_ma_dv) isAllow = true;
                            if (role.is_public && checkAccesiableTo(ROLE_API_VIEWALL_ENDPOINT, "GET")) isAllow = true;
                            return (
                                <ActionList.Item
                                    key={role.id}
                                    role="menuitemradio"
                                    selected={roleEditing?.id === role.id}

                                    // aria-checked={index === selectedIndex}
                                    onSelect={() => {
                                        dispatch(rootAction.user.roleAction.changeEditing(role))
                                    }}
                                    className={roleEditing?.id === role.id ? "listItemSelected" : ""}

                                >

                                    {role.name}
                                    <ActionList.Description variant="block">
                                        <Box sx={{
                                            whiteSpace: "pre-line"
                                        }}>
                                            {role.description}
                                        </Box>
                                    </ActionList.Description>
                                    {isAllow &&
                                        <ActionList.TrailingVisual>
                                            <ActionMenu>

                                                <ActionMenu.Anchor>
                                                    <IconButton icon={KebabHorizontalIcon} aria-label="Open menu" variant="invisible" />
                                                </ActionMenu.Anchor>
                                                <ActionMenu.Overlay width="auto">
                                                    <ActionList>
                                                        <ActionList.Item
                                                            disabled={isCanNotEdit}
                                                            onSelect={() => {
                                                                dispatch(rootAction.user.roleAction.changeEditing(role))
                                                                setIsShowModal(true)
                                                            }}>
                                                            <ActionList.LeadingVisual>
                                                                <PencilIcon />
                                                            </ActionList.LeadingVisual>
                                                            Sửa

                                                        </ActionList.Item>
                                                        <ActionList.Divider />
                                                        <ActionList.Item
                                                            variant="danger"
                                                            disabled={isCanNotDelete}
                                                            onSelect={() => {
                                                                dispatch(rootAction.user.roleAction.changeEditing(role))
                                                                setIsShowDeleteConfirm(true)
                                                            }}
                                                        >
                                                            <ActionList.LeadingVisual>
                                                                <TrashIcon />
                                                            </ActionList.LeadingVisual>
                                                            Xóa
                                                        </ActionList.Item>
                                                    </ActionList>
                                                </ActionMenu.Overlay>
                                            </ActionMenu>

                                        </ActionList.TrailingVisual>
                                    }

                                </ActionList.Item>
                            )
                        }

                        )}
                    </ActionList>
                }
            </Box>
            {isShowModal &&
                <RoleEditForm
                    onClose={() => {
                        setIsShowModal(false)
                    }}
                    isOpen={isShowModal}
                />
            }
            {isShowDeleteConfirm && roleEditing &&
                <ConfirmModal
                    onCancel={() => { setIsShowDeleteConfirm(false) }}
                    type="danger"
                    title="Xóa vai trò"
                    text="Bạn có chắc chắn muốn xóa vai trò này?"
                    isSaving={status == eReducerStatusBase.is_deleting}
                    onConfirm={() => {
                        dispatch(rootAction.user.roleAction.deleteStart(roleEditing?.id ?? 0))
                    }}
                />
            }
        </Box >
    );
};

export default RoleList;