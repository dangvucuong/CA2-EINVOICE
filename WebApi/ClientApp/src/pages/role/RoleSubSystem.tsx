import { ActionList, Box, Checkbox, FormControl } from '@primer/react';
import { useEffect, useMemo } from 'react';
import Heading from '../../component-ui/heading';
import Text from '../../component-ui/text';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eSize } from '../../models/commons/eSize';
import { ISubSystem } from '../../models/responses/user/ISubSystem';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { useCommonContext } from '../../contexts/common';
import { ROLE_API_ENDPOINT } from '../../api/user/roleApi';
const subSystemAction = rootAction.user.subSystemAction;
const RoleSubSystem = () => {
    const { checkAccesiableTo } = useCommonContext();
    const dispatch = useAppDispatch();
    const { status: subSystemStatus, subSystems, subSystemSelectedId } = useAppSelector(x => x.user.subSystemReducer);
    const { status, roleSubSystems } = useAppSelector(x => x.user.roleSubSystemReducer);
    const { roleEditing } = useAppSelector(x => x.user.roleReducer);
    const roleEditingId = useMemo(() => {
        return roleEditing?.id ?? 0;
    }, [roleEditing])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT, "PUT")
    }, [])
    useEffect(() => {
        if (subSystemStatus == eReducerStatusBase.is_not_initialization) {
            dispatch(subSystemAction.loadStart())
        }
    }, [subSystemStatus])
    useEffect(() => {
        dispatch(rootAction.user.roleSubSystemAction.loadStart(roleEditingId))
    }, [roleEditingId])
    useEffect(() => {
        if (status == eReducerStatusBase.is_deleted || status == eReducerStatusBase.is_saved) {
            dispatch(rootAction.user.roleSubSystemAction.loadStart(roleEditingId))
        }
    }, [status, roleEditingId])
    const toggleSelectedChanged = (subSystem: ISubSystem) => {
        const obj = roleSubSystems.find(x => x.sub_system_id == subSystem.id && x.role_id == roleEditingId);
        if (obj) {
            dispatch(rootAction.user.roleSubSystemAction.removeStart(obj))
        } else {
            dispatch(rootAction.user.roleSubSystemAction.addStart({
                id: 0,
                role_id: roleEditingId,
                sub_system_id: subSystem.id
            }))

        }
    }
    return (
        <Box sx={{

        }}>
            <Box sx={{
                p: 2,
                pb: 3,
                borderBottom: 1,
                borderBottomColor: "border.default",
                borderBottomStyle: "solid"
            }}>
                <Heading size={eSize.medium} text='RoleSubSystem.Heading'></Heading>
                <Text text='RoleSubSystem.SubHeading'
                    sx={{
                        color: "fg.muted",
                        fontSize: 12
                    }}
                ></Text>
            </Box>

            <ActionList
                selectionVariant="single"
                showDividers
                role="menu"
                aria-label="RoleSubSystem"


            >
                {subSystems.map((subSystem, index) => {
                    const obj = roleSubSystems.find(x => x.sub_system_id === subSystem.id && x.role_id === roleEditingId);

                    return (
                        <ActionList.Item
                            key={index}
                            role="menuitemcheckbox"
                            selected={subSystem.id === subSystemSelectedId}
                            className={subSystem.id === subSystemSelectedId ? "listItemSelected" : ""}
                            aria-checked={subSystemSelectedId === subSystem.id}
                            onSelect={() => {
                                dispatch(rootAction.user.subSystemAction.changeEditing(subSystem.id))
                            }}

                        >

                            <Box sx={{
                                display: "flex"
                            }}>
                                <FormControl disabled={isCanNotEdit}>
                                    <Checkbox
                                        checked={obj ? true : false}
                                        onChange={() => {
                                            if (!isCanNotEdit) {
                                                toggleSelectedChanged(subSystem)
                                            }
                                        }} />

                                </FormControl>
                                <FormControl.Label sx={{
                                    fontSize: 13,
                                    fontWeight: 600
                                }}>{subSystem.short_name}</FormControl.Label>
                            </Box>

                            <ActionList.Description variant="block">{subSystem.name}</ActionList.Description>
                        </ActionList.Item>
                    )
                }
                )
                }
            </ActionList>


        </Box>
    );
};

export default RoleSubSystem;