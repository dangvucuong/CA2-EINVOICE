import { SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
import Button from '../button';

interface IBangTongHopLoaiHangHoaSelectionProps {
    onValueChanged: (id: number, data?: any) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean
}
const BangTongHopLoaiHangHoaSelection = (props: IBangTongHopLoaiHangHoaSelectionProps) => {
    const [open, setOpen] = useState(false)
    const [filter, setFilter] = useState('');


    const dataSource = useMemo(() => {
        return [
            { id: 1, text: "Hàng hóa, dịch vụ khác" },
            { id: 2, text: "Vận tải hàng không" },
            { id: 3, text: "Xăng dầu" },
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
                // title="Select labels"
                // renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                //     <Button
                //         // trailingAction={TriangleDownIcon}
                //         aria-labelledby={` ${ariaLabelledBy}`}
                //         {...anchorProps}
                //         aria-haspopup="dialog"
                //     >
                //         {children ?? 'Select Labels'}
                //     </Button>
                // )}
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

export default BangTongHopLoaiHangHoaSelection;