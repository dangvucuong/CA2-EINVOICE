import { Select, SelectProps } from '@primer/react';
import { useMemo } from 'react';
interface ISelectBoxTinhChatHangHoaProps extends SelectProps {
    onValueChanged: (id: number) => void,
    value: number,
    maxWidth?: any,
    isNoBorder?: boolean
}


const SelectBoxTinhChatHangHoa = (props: ISelectBoxTinhChatHangHoaProps) => {
    const dataSource = useMemo(() => {
        return [
            { id: 0, text: "-- Chọn --" },
            { id: 1, text: "Hàng hóa, dịch vụ" },
            { id: 2, text: "Khuyến mại" },
            { id: 3, text: "Chiết khấu" },
            { id: 4, text: "Ghi chú, diễn giải" },
            { id: 5, text: "Hàng hóa đặc trưng" },

        ]
    }, [])
    return (
        <>
            <Select
                {...props}
                onChange={(e) => {
                    const id: number = e.currentTarget.value ? parseInt(e.currentTarget.value) : 0
                    props.onValueChanged(id)
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

export default SelectBoxTinhChatHangHoa;