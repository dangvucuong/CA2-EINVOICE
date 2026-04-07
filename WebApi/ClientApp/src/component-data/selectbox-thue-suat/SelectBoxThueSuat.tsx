import { Select, SelectProps } from '@primer/react';
import { useMemo } from 'react';
interface ISelectBoxThueSuatProps extends SelectProps {
    onValueChanged: (id: string) => void,
    value: string,
    maxWidth?: any,
    isNoBorder?: boolean,
    isReadOnly?: boolean
}


const SelectBoxThueSuat = (props: ISelectBoxThueSuatProps) => {
    const dataSource = useMemo(() => {
        return [
            { id: "", text: "-- Chọn --" },
            { id: "0%", text: "0%" },
            { id: "5%", text: "5%" },
            { id: "8%", text: "8%" },
            { id: "10%", text: "10%" },
            { id: "KCT", text: "KCT" },
            { id: "KKKNT", text: "KKKNT" },

        ]
    }, [])
    return (
        <>
            <Select
                {...props}
                disabled={props.isReadOnly}
                onChange={(e) => {
                    
                    props.onValueChanged(e.currentTarget.value??"")
                }}
            >
                {dataSource.map(x => {
                    return (
                        <Select.Option value={x.id.toString()} selected={x.id === props.value}
                        // onSelect={() => {
                        //     props.onValueChanged(x.id)
                        // }}
                        >{x.text}</Select.Option>
                    )
                })}

            </Select>
        </>
    );
};

export default SelectBoxThueSuat;