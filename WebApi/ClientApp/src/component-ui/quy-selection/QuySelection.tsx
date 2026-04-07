import { SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';

const thangs = [1, 2, 3, 4];
interface IQuySelectionProps {
    onValueChanged: (id: number, data?: any) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean
}
const QuySelection = (props:IQuySelectionProps) => {
    const [open, setOpen] = useState(false)
    const [filter, setFilter] = useState('');


    const dataSource = useMemo(() => {
        return thangs
            .map(x => ({ id: x, text: `Quý ${x}` }))
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

export default QuySelection;