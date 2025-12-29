import { TriangleDownIcon } from '@primer/octicons-react';
import { Box, Button, SelectPanel } from '@primer/react';
import { useMemo, useState } from 'react';
interface ISelectBoxContactStatusProps {
    onValueChanged: (id: number) => void,
    value: number,
    maxWidth?: any
}
function getColorCircle(color: string) {
    return function () {
        return (
            <Box
                bg={color}
                borderColor={color}
                width={8}
                height={8}
                borderRadius={10}
                margin="auto"
                borderWidth="1px"
                borderStyle="solid"
            />
        )
    }
}
const SelectBoxContactStatus = (props: ISelectBoxContactStatusProps) => {
    const [open, setOpen] = useState(false)
    const dataSource = useMemo(() => {
        return [
            { id: 0, text: "Tất cả", leadingVisual: getColorCircle(''), color: "" },
            { id: 1, text: "Tạo mới", leadingVisual: getColorCircle('#24292f'), color: "#24292f" },
            { id: 2, text: "Đã tiếp nhận", leadingVisual: getColorCircle('#0969da'), color: "#0969da" },
            { id: 3, text: "Đã tạo tài khoản", leadingVisual: getColorCircle('#2da44e'), color: "#2da44e" },
            { id: 4, text: "Không xử lý", leadingVisual: getColorCircle('#cf222e'), color: "#cf222e" },
        ]
    }, [])
    const _selectedData = useMemo(() => {
        return dataSource.find(item =>
            props.value === item.id
        )
    }, [props.value, dataSource])

    return (
        <>
            <SelectPanel
                renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                    <Button sx={{
                        maxWidth: 300
                    }} trailingAction={TriangleDownIcon} aria-labelledby={` ${ariaLabelledBy}`} {...anchorProps}
                        leadingVisual={getColorCircle(_selectedData?.color ?? "")}
                    >
                        <p style={{ maxWidth: props.maxWidth, overflow: "hidden", textOverflow: "ellipsis" }}>
                            {children || 'Chọn trạng thái'}
                        </p>
                    </Button>
                )}
                title={`Chọn trạng thái`}
                open={open}
                onOpenChange={setOpen}
                items={dataSource}
                selected={_selectedData}
                onSelectedChange={(data: any) => {

                    props.onValueChanged(data.id)
                }}

                onFilterChange={() => {
                    // props.onValueChanged(data.map((x: any) => x.id))
                }}
                showItemDividers={true}
                overlayProps={{ width: 'small', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxContactStatus;