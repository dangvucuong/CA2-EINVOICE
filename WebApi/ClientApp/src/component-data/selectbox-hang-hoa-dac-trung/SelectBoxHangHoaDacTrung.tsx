import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';

import { Box, Button, SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
interface ISelectBoxHangHoaDacTrungProps {
    onValueChanged: (id: number, data?: any) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean
}

const SelectBoxHangHoaDacTrung = (props: ISelectBoxHangHoaDacTrungProps) => {
    const [open, setOpen] = useState(false)
    const [filter, setFilter] = useState('');

    const dataSource = useMemo(() => {
        return [
            { id: 1, text: "Hàng hóa là xe mô tô, xe ô tô" },
            { id: 2, text: "Dịch vụ vận chuyển" },
            { id: 3, text: "Dịch vụ vận chuyển trên nền tảng số, TMĐT" },

        ]
    }, [])
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
                            {children || 'Chọn loại hóa đơn đặc trưng'}
                        </p>


                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            Chọn loại hóa đơn đặc trưng
                        </Box>
                        {props.isShowClearBtn && props.value > 0 &&
                            <Button
                                trailingVisual={XCircleFillIcon}
                                variant='invisible'
                                sx={{
                                    color: "danger.emphasis"
                                }}
                                onClick={() => {
                                    props.onValueChanged(0, undefined)
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
                overlayProps={{ width: 'large', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxHangHoaDacTrung;