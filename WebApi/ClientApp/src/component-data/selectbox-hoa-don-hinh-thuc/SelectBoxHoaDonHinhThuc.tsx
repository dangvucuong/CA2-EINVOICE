import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';
import { Box, Button, SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
import { useHoaDonHinhThucs } from '../../hooks/useHoaDonHinhThuc';
import { LeadingVisual } from '@primer/react/lib/ActionList/Visuals';

interface ISelectBoxHoaDonHinhThucProps {
    onValueChanged: (id: number) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean

}
const getLeadingVisual = (data: any): any => {
    return function () {
        const color = data.color ?? "";
        return <Box
            sx={{
                backgroundColor: color,
                borderColor: color,
                width: 14,
                height: 14,
                borderRadius: 10,
                margin: 'auto',
                borderWidth: '1px',
                borderStyle: 'solid',
            }}
        />
    }
}

const SelectBoxHoaDonHinhThuc = (props: ISelectBoxHoaDonHinhThucProps) => {
    const [open, setOpen] = useState(false)
    const { hoaDonHinhThucs } = useHoaDonHinhThucs();
    const [filter, setFilter] = useState('')
    const dataSource = useMemo(() => {
        return hoaDonHinhThucs.map(x => {
            return {
                id: x.id,
                text: x.name,
                leadingVisual: getLeadingVisual(x),
            }
        })
    }, [hoaDonHinhThucs])
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
                            {children || 'Chọn hình thức'}
                        </p>
                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            Chọn
                        </Box>
                        {props.isShowClearBtn && props.value !== 0 &&
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

export default SelectBoxHoaDonHinhThuc;