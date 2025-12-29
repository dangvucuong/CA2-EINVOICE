import { ActionList, Box, Button, SelectPanel } from '@primer/react';
import React, { useEffect, useState } from 'react';
import { useDebounce } from 'use-debounce';
import { khachHangApi } from '../../api/category/khachHangApi';
import { IMyTextInputProps } from '../../component-ui/text-input/TextInput';
import { eSortMode } from '../../models/commons/eSortMode';
import { IKhachHang } from '../../models/responses/category/IKhachHang';
import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';
import Text from '../../component-ui/text';

interface IKhachHangSearchProps extends IMyTextInputProps {
    onValueChanged: (data?: IKhachHang) => void,
    isShowClearBtn?: boolean,
    maxWidth?: any,
}

const KhachHangSearch = (props: IKhachHangSearchProps) => {
    const [filter, setFilter] = React.useState("")
    const [isLoading, setIsLoading] = useState(false);

    const [open, setOpen] = useState(false)
    const [khachHangs, setKhachHangs] = useState<IKhachHang[]>([]);
    const [delayFilter] = useDebounce(filter, 500);
    const dataSource = khachHangs.map(x => ({
        id: x.id,
        text: `${x.ten_khach_hang} - ${x.ten_don_vi}`,
        khachHang: x

    }))
   
    useEffect(() => {
        handleLoadKhachHangAsync();
    }, [delayFilter])

    const handleLoadKhachHangAsync = async () => {
        setIsLoading(true)
        const res = await khachHangApi.getByDonViPaging({
            search_key: delayFilter,
            page_index: 0,
            page_size: 10,
            sort_mode: eSortMode.DESC,
            sort_by: ""
        });
        setIsLoading(false)

        if (res.is_success) {
            setKhachHangs(res.data.data)
        }
    }
    const _selectedData = dataSource.find((x: any) => props.value === x.id)


    return (
        <Box>
            <SelectPanel
                renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                    <Button sx={{
                        maxWidth: 300
                    }} trailingAction={TriangleDownIcon} aria-labelledby={` ${ariaLabelledBy}`} {...anchorProps}>
                        <p style={{ maxWidth: props.maxWidth ?? "100%", overflow: "hidden", textOverflow: "ellipsis" }}>
                            {children || (props.placeholder ?? "Chọn khách hàng")}
                        </p>

                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            {props.placeholder ?? "Chọn khách hàng"}
                        </Box>
                        {props.isShowClearBtn &&
                            <Button
                                trailingVisual={XCircleFillIcon}
                                variant='invisible'
                                sx={{
                                    color: "danger.emphasis"
                                }}
                                onClick={() => {
                                    props.onValueChanged(undefined)
                                }}
                            >
                                Bỏ chọn
                            </Button>
                        }
                    </Box>
                </>}
                renderItem={(data: any) => {
                    return (
                        <Box sx={{ ml: 1, mr: 1 }}>
                            <ActionList.Item onSelect={() => {
                                setOpen(false)
                                props.onValueChanged(
                                    data.khachHang
                                )
                            }}
                            >
                                <Box sx={{ ml: 3, mr: 3 }}>
                                    <Box><b>{data.khachHang.mst}</b> - {data.khachHang.ten_don_vi}</Box>
                                    <Box><b>{data.khachHang.ten_khach_hang}</b></Box>
                                    <Box><Text text={data.khachHang.dia_chi} sx={{
                                        color: 'fg.muted',
                                        fontSize: "12px"
                                    }} /></Box>
                                </Box>
                            </ActionList.Item>
                        </Box>
                    )
                }}
                placeholderText="Search"
                open={open}
                loading={isLoading}
                onOpenChange={setOpen}
                items={dataSource}
                selected={_selectedData}
                onSelectedChange={(data: any) => {
                    props.onValueChanged(khachHangs.find(x => x.id === data.id))
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'xlarge', height: 'medium' }}
            />
        </Box>
    );
};

export default KhachHangSearch;