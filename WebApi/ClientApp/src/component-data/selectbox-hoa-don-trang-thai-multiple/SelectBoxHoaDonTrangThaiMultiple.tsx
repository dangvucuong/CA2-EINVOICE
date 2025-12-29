import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';
import { Box, Button, SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
import { useHoaDonTrangThaisHook } from '../../hooks/useHoaDonTrangThai';

interface ISelectBoxHoaDonTrangThaiMultipleProps {
    onValueChanged: (ids: number[]) => void,
    value: number[],
    maxWidth?: any,
    isShowClearBtn?: boolean

}

const SelectBoxHoaDonTrangThaiMultiple = (props: ISelectBoxHoaDonTrangThaiMultipleProps) => {
    const [open, setOpen] = useState(false)
    const { hoaDonTrangThais } = useHoaDonTrangThaisHook();
    const [filter, setFilter] = useState('')
    const dataSource = useMemo(() => {
        return hoaDonTrangThais.map(x => {
            return {
                id: x.id,
                text: x.name
            }
        })
    }, [hoaDonTrangThais])
    const filterdData = useMemo(() => {
        return dataSource.filter(item =>
            item.text.toLowerCase().includes(filter.toLowerCase())
        )
    }, [dataSource, filter])
    const _selectedDatas = useMemo(() => {
        return dataSource.filter(item => props.value.includes(item.id))
    }, [props.value, dataSource])

    return (
        <>
            <SelectPanel
                renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                    <Button sx={{
                        maxWidth: 300
                    }} trailingAction={TriangleDownIcon} aria-labelledby={` ${ariaLabelledBy}`} {...anchorProps}>
                        <p style={{ maxWidth: props.maxWidth, overflow: "hidden", textOverflow: "ellipsis" }}>
                            {children || 'Chọn trạng thái'}
                        </p>
                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            Chọn
                        </Box>
                        {props.isShowClearBtn && props.value.length !== 0 &&
                            <Button
                                trailingVisual={XCircleFillIcon}
                                variant='invisible'
                                sx={{
                                    color: "danger.emphasis"
                                }}
                                onClick={() => {
                                    props.onValueChanged([])
                                }}
                            >
                                Bỏ chọn
                            </Button>
                        }
                    </Box>
                </>}
                placeholderText="Search"
                open={open}
                onOpenChange={setOpen}
                items={filterdData}
                selected={_selectedDatas}
                onSelectedChange={(data: any[]) => {
                    props.onValueChanged(data.map(x => x.id))
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'medium', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxHoaDonTrangThaiMultiple;