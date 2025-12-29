import { Box, Checkbox, CheckboxGroup, FormControl, SegmentedControl } from '@primer/react';
import { useEffect, useMemo, useState } from 'react';
import { useDispatch } from 'react-redux';
import { ROLE_API_ENDPOINT, ROLE_API_VIEWALL_ENDPOINT } from '../../api/user/roleApi';
import Heading from '../../component-ui/heading';
import Text from '../../component-ui/text';
import { useCommonContext } from '../../contexts/common';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eSize } from '../../models/commons/eSize';
import { IMenu } from '../../models/responses/user/IMenu';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { useAuth } from '../../hooks/useAuth';
import { EyeIcon, PencilIcon } from '@primer/octicons-react'
interface IRootMenuProps {
    menu: IMenu,
    isReadOnly: boolean
}
interface ISubMenuProps {
    menuIdParent: number,
    isReadOnly: boolean

}

const RootMenu = (props: IRootMenuProps) => {
    const { menu } = props;
    return (
        <Box>
            <Box sx={{ pb: 2, mt: 3 }}>
                <Text text={menu.name}
                    sx={{
                        fontSize: "1.5rem",
                        fontWeight: 600
                    }}
                ></Text>
                <br />
                <Text text={menu.description}
                    sx={{
                        color: "fg.muted",
                        fontSize: 12
                    }}
                ></Text>
            </Box>
            <Box
                sx={{
                    // p: 3,
                    borderRadius: 2,
                    borderStyle: "solid",
                    borderWidth: 1,
                    borderColor: "border.default",
                    // mb: 2
                }}
            >
                <SubMenu menuIdParent={menu.id} isReadOnly={props.isReadOnly} />
            </Box>
        </Box>
    );
}
const SubMenu = (props: ISubMenuProps) => {
    const { menus } = useAppSelector(x => x.user.menuReducer);
    const subMenus = useMemo(() => {
        return menus.filter(x => x.menu_id_parent == props.menuIdParent)
    }, [menus, props.menuIdParent])
    const { apis } = useAppSelector(x => x.user.apiReducer);
    const { roleEditing } = useAppSelector(x => x.user.roleReducer);
    const { subSystems, subSystemSelectedId } = useAppSelector(x => x.user.subSystemReducer);
    const { roleApis } = useAppSelector(x => x.user.roleApiReducer);
    const { checkAccesiableTo } = useCommonContext();

    const dispatch = useDispatch();
    const roleEditingId = useMemo(() => {
        return roleEditing?.id ?? 0;
    }, [roleEditing])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT, "PUT")
    }, [])
    return (
        <>
            {subMenus.length > 0 &&
                <>
                    {subMenus.map((subMenu, index) => {
                        const apisInMenu = apis.filter(x => x.menu_id === subMenu.id)
                        return (
                            <Box
                                key={subMenu.id}
                                sx={{
                                    p: 2,
                                    pl: 3,
                                    borderBottomStyle: "solid",
                                    borderWidth: index === subMenus.length - 1 ? 0 : 1,
                                    borderColor: "border.default",
                                }}
                            >
                                <Heading size={eSize.smalll} text={subMenu.name}></Heading>
                                <Text text={subMenu.description}
                                    sx={{
                                        color: "fg.muted",
                                        fontSize: 12
                                    }}
                                ></Text>
                                <CheckboxGroup sx={{
                                    mt: 2
                                }}>

                                    {apisInMenu.map(api => {
                                        const obj = roleApis.find(x => x.api_id === api.id);
                                        return (
                                            <FormControl key={api.id} disabled={isCanNotEdit || props.isReadOnly}>
                                                <Checkbox value={api.id.toString()}
                                                    checked={obj != undefined}
                                                    onChange={(e) => {
                                                        // console.log({e: e.target});

                                                        if (e.target.checked) {
                                                            dispatch(rootAction.user.roleApiAction.addStart({
                                                                id: 0,
                                                                api_id: api.id,
                                                                role_id: roleEditingId
                                                            }))
                                                        } else {
                                                            dispatch(rootAction.user.roleApiAction.removeStart({
                                                                id: obj?.id ?? 0,
                                                                api_id: api.id,
                                                                role_id: roleEditingId
                                                            }))
                                                        }
                                                    }}
                                                />
                                                <FormControl.Label sx={{
                                                    fontSize: 13,
                                                    fontWeight: 400
                                                }}>{api.description}</FormControl.Label>
                                            </FormControl>
                                        )
                                    })}
                                </CheckboxGroup>
                                <SubMenu menuIdParent={subMenu.id} isReadOnly={props.isReadOnly} />
                            </Box>
                        )
                    })
                    }

                </>
            }
        </>
    );
}

const RoleSubSystemMenu = () => {
    const dispatch = useAppDispatch();
    const { user } = useAuth();
    const { checkAccesiableTo } = useCommonContext();


    const { roleEditing } = useAppSelector(x => x.user.roleReducer);
    const { subSystems, subSystemSelectedId } = useAppSelector(x => x.user.subSystemReducer);
    const { menus } = useAppSelector(x => x.user.menuReducer);
    const { status, roleApis } = useAppSelector(x => x.user.roleApiReducer);
    const [mode, setMode] = useState<"view" | "edit">("view");

    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(ROLE_API_ENDPOINT, "PUT")
    }, [])
    const roleEditingId = useMemo(() => {
        return roleEditing?.id ?? 0;
    }, [roleEditing])
    const rootMenus = useMemo(() => {
        return menus.filter(x => x.menu_id_parent === 0);
    }, [menus])

    const isAllowEdit = useMemo(() => {
        if (!roleEditing || !user) return false;
        if (roleEditing.donvi_ma_dv === user.donvi_ma_dv) return true;
        if (roleEditing.is_public && checkAccesiableTo(ROLE_API_VIEWALL_ENDPOINT, "GET")) return true;
        return false;
    }, [user, roleEditing])
    useEffect(() => {
        dispatch(rootAction.user.menuAction.loadStart(subSystemSelectedId))
        dispatch(rootAction.user.apiAction.loadStart(subSystemSelectedId))
    }, [])

    useEffect(() => {
        dispatch(rootAction.user.roleApiAction.loadStart({
            role_id: roleEditingId,
            sub_system_id: subSystemSelectedId
        }))
    }, [roleEditingId, subSystemSelectedId])
    useEffect(() => {
        if (status == eReducerStatusBase.is_saved ||
            status == eReducerStatusBase.is_deleted
        ) {
            dispatch(rootAction.user.roleApiAction.loadStart({
                role_id: roleEditingId,
                sub_system_id: subSystemSelectedId
            }))
        }
    }, [status])

    return (
        <Box>
            <Box sx={{
                p: 2,
                pb: 3,
                borderBottom: 1,
                borderBottomColor: "border.default",
                borderBottomStyle: "solid"
            }}>
                <Heading size={eSize.large} text={roleEditing?.name ?? ""}></Heading>
                <Text text={`Được gán: ${roleApis.length} chức năng`}
                    sx={{
                        color: "fg.muted",
                        fontSize: 12
                    }}
                ></Text>
            </Box>
            <Box sx={{
                p: 3
            }}>

                <SegmentedControl
                    aria-label="File view"
                    onChange={(index) => {
                        if (index === 0) {
                            setMode("view")
                        }
                        if (index === 1) {
                            setMode("edit")
                        }

                    }}
                    size={"small"}
                >
                    <SegmentedControl.Button defaultSelected aria-label={'Preview'} leadingIcon={EyeIcon}>
                        Xem
                    </SegmentedControl.Button>
                    <SegmentedControl.Button aria-label={'Raw'} leadingIcon={PencilIcon}>
                        Chỉnh sửa
                    </SegmentedControl.Button>

                </SegmentedControl>
                {rootMenus.map(rootMenu => {
                    return (
                        <RootMenu key={rootMenu.id} menu={rootMenu} isReadOnly={!isAllowEdit || mode !== "edit"} />
                    )
                })}


                {/* </TreeView> */}
            </Box>
        </Box>
    );
};

export default RoleSubSystemMenu;