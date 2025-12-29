import { TriangleDownIcon } from '@primer/octicons-react';
import { Button, SelectPanel } from '@primer/react';
import { useEffect, useMemo, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useAppSelector } from '../../hooks/useAppSelector';
import { rootAction } from '../../state/actions/rootAction';
import { RootState } from '../../state/reducers/rootReducer';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
interface ISelectBoxRoleProps {
    onValueChanged: (id: number[]) => void,
    value: number[],
    maxWidth?: any
}

const SelectBoxRole = (props: ISelectBoxRoleProps) => {
    const [open, setOpen] = useState(false)

    const { roles, status } = useAppSelector((x: RootState) => x.user.roleReducer)
    const [filter, setFilter] = useState('')
    const dispatch = useDispatch();
    const dataSource = useMemo(() => {
        return roles.map(x => ({ id: x.id, text: x.name }))
    }, [roles, filter])
    const filterdData = useMemo(() => {
        return dataSource.filter(item =>
            item.text.toLowerCase().includes(filter.toLowerCase())
        )
    }, [dataSource, filter])
    const _selectedData = useMemo(() => {
        return dataSource.filter(item =>
            props.value.includes(item.id)
        )
    }, [props.value, dataSource])
    useEffect(() => {
        if (status == eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.user.roleAction.loadStart())
        }
    }, [status])
    return (
        <>
            <SelectPanel
                renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                    <Button sx={{
                        maxWidth: 300
                    }} trailingAction={TriangleDownIcon} aria-labelledby={` ${ariaLabelledBy}`} {...anchorProps}>
                        <p style={{ maxWidth: props.maxWidth, overflow: "hidden", textOverflow: "ellipsis" }}>
                            {children || 'Chọn vai trò'}
                        </p>
                    </Button>
                )}
                title={`Đã chọn: ${_selectedData.length}`}
                placeholderText="Search"
                open={open}
                onOpenChange={setOpen}
                items={filterdData}
                selected={_selectedData}
                onSelectedChange={(data: any) => {
                    props.onValueChanged(data.map((x: any) => x.id))
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'small', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxRole;