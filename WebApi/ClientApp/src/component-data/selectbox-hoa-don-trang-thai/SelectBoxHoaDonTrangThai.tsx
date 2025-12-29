import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';
import { Box, Button, SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
import { useHoaDonTrangThaisHook } from '../../hooks/useHoaDonTrangThai';

interface ISelectBoxHoaDonTrangThaiProps {
    onValueChanged: (id: number) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean

}

const SelectBoxHoaDonTrangThai = (props: ISelectBoxHoaDonTrangThaiProps) => {
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
    const _selectedData = useMemo(() => {
        return dataSource.find(item => item.id === props.value)
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
                        {props.isShowClearBtn && props.value !==0 &&
                            <Button
                                trailingVisual={XCircleFillIcon}
                                variant='invisible'
                                sx={{
                                    color: "danger.emphasis"
                                }}
                                onClick={() => {
                                    props.onValueChanged(0)
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
                selected={_selectedData}
                onSelectedChange={(data: any) => {
                    props.onValueChanged(data.id)
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'medium', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxHoaDonTrangThai;