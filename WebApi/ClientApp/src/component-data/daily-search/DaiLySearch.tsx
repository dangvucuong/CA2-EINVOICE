import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';
import { Box, Button, SelectPanel } from '@primer/react';
import React, { useEffect, useState } from 'react';
import { useDebounce } from 'use-debounce';
import { daiLyApi } from '../../api/category/daiLyApi';
import { IMyTextInputProps } from '../../component-ui/text-input/TextInput';
import { eSortMode } from '../../models/commons/eSortMode';
import { IDaiLy } from '../../models/responses/category/IDaiLy';

interface IDaiLySearchProps extends IMyTextInputProps {
    onValueChanged: (data?: IDaiLy) => void,
    isShowClearBtn?: boolean,
    maxWidth?: any,
}

const DaiLySearch = (props: IDaiLySearchProps) => {
    const [filter, setFilter] = React.useState("")
    const [isLoading, setIsLoading] = useState(false);

    const [open, setOpen] = useState(false)
    const [daiLys, setDaiLys] = useState<IDaiLy[]>([]);
    const [delayFilter] = useDebounce(filter, 500);
    const dataSource = daiLys.map(x => ({
        id: x.id,
        text: `${x.ma_dai_ly} - ${x.ten_dai_ly}`,
        daiLy: x

    }))

    useEffect(() => {
        handleLoadKhachHangAsync();
    }, [delayFilter])

    const handleLoadKhachHangAsync = async () => {
        setIsLoading(true)
        const res = await daiLyApi.getByDonViPaging({
            search_key: delayFilter,
            page_index: 0,
            page_size: 10,
            sort_mode: eSortMode.DESC,
            sort_by: ""
        });
        setIsLoading(false)

        if (res.is_success) {
            setDaiLys(res.data.data)
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
                            {children || (props.placeholder ?? "Chọn đại lý")}
                        </p>

                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            {props.placeholder ?? "Chọn đại lý"}
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
               
                placeholderText="Search"
                open={open}
                loading={isLoading}
                onOpenChange={setOpen}
                items={dataSource}
                selected={_selectedData}
                onSelectedChange={(data: any) => {
                    props.onValueChanged(daiLys.find(x => x.id === data.id))
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'large', height: 'medium' }}
            />
        </Box>
    );
};

export default DaiLySearch;