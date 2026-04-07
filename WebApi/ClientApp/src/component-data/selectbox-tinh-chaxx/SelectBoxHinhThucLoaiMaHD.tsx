import { Select, SelectProps } from '@primer/react';
import { useHinhThucLoaiMaHoaDons } from '../../hooks/useHinhThucLoaiMaHoaDon';
interface ISelectBoxHinhThucLoaiMaHDProps extends SelectProps {
    onValueChanged: (id: string) => void,
    value: string,
    maxWidth?: any,
    isNoBorder?: boolean
}


const SelectBoxHinhThucLoaiMaHD = (props: ISelectBoxHinhThucLoaiMaHDProps) => {
    const { hinhThucLoaiMaHoaDons } = useHinhThucLoaiMaHoaDons();
    return (
        <>
            <Select
                {...props}
                onChange={(e) => {
                    props.onValueChanged(e.target.value)
                }}
            >
                <Select.Option value={""} selected={props.value === ""}
                >-- Chọn --</Select.Option>
                {hinhThucLoaiMaHoaDons.map(x => {
                    return (
                        <Select.Option value={x.id.toString()} selected={x.id === props.value}
                        >{x.name}</Select.Option>
                    )
                })}

            </Select>
        </>
    );
};

export default SelectBoxHinhThucLoaiMaHD;