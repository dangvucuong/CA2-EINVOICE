import { SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';

const thangs = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
interface IThangSelectionProps {
    onValueChanged: (id: number, data?: any) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean
}
const ThangSelection = (props:IThangSelectionProps) => {
    const [open, setOpen] = useState(false)
    const [filter, setFilter] = useState('');


    const dataSource = useMemo(() => {
        return thangs
            .map(x => ({ id: x, text: `Tháng ${x}` }))
    }, [thangs])
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

export default ThangSelection;