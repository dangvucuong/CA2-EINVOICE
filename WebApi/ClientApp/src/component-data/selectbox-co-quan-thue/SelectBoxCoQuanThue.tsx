import { TriangleDownIcon, XCircleFillIcon } from '@primer/octicons-react';

import { Box, Button, SelectPanel } from '@primer/react';
import { useEffect, useState } from 'react';
import { useDebounce } from 'use-debounce';
import { coQuanThueApi } from '../../api/category/coQuanThueApi';
import { eSortMode } from '../../models/commons/eSortMode';
import { ICoQuanThue } from '../../models/responses/category/ICoQuanThue';
interface ISelectBoxCoQuanThueProps {
    onValueChanged: (id: number, data?: ICoQuanThue) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean
}

const SelectBoxCoQuanThue = (props: ISelectBoxCoQuanThueProps) => {
    const [open, setOpen] = useState(false)
    const [isLoading, setIsLoading] = useState(false);
    const [filter, setFilter] = useState('')
    const [items, setItems] = useState<any[]>([]);
    const [selectedCQT, setSelectedCQT] = useState<ICoQuanThue>();


    const [searchKeyDelayed] = useDebounce(filter, 1000);


    useEffect(() => {
        handleSearchAsync();
    }, [searchKeyDelayed])
    useEffect(() => {
        handleSearchSelectedAsync();
    }, [props.value])
    const handleSearchSelectedAsync = async () => {
        if (props.value > 0) {
            setIsLoading(true);
            const res = await coQuanThueApi.selectById(props.value)
            setIsLoading(false);
            if (res.is_success) {
                setSelectedCQT(res.data)
            }
        } else {
            setSelectedCQT(undefined)

        }
    }
    const handleSearchAsync = async () => {
        setIsLoading(true);
        const res = await coQuanThueApi.selectPaging({
            page_index: 0,
            page_size: 10,
            search_key: searchKeyDelayed,
            sort_mode: eSortMode.DESC,
        })
        setIsLoading(false)
        if (res.is_success) {
            setItems(res.data.data.map((x: ICoQuanThue) => {
                return {
                    id: x.id,
                    text: `[${x.ma_cqt}] - ${x.dia_chi}`,
                    data: x
                }
            }))
        }
    }
    const _selectedData: any = items.find((x: any) => props.value === x.id)


    return (
        <>
            <SelectPanel
                renderAnchor={({ children, 'aria-labelledby': ariaLabelledBy, ...anchorProps }) => (
                    <Button sx={{
                        maxWidth: 300
                    }} trailingAction={TriangleDownIcon} aria-labelledby={` ${ariaLabelledBy}`} {...anchorProps}>
                        <p style={{ maxWidth: props.maxWidth ??"100%", overflow: "hidden", textOverflow: "ellipsis" }}>
                            {children || 'Chọn cơ quan thuế'}
                        </p>
                        {/* {!selectedCQT &&
                            <Box>Chọn cơ quan thuế</Box>
                        }
                        {selectedCQT &&
                            <Box>{`[${selectedCQT.ma_cqt}] - ${selectedCQT.ten}`}</Box>
                        } */}

                    </Button>
                )}
                title={<>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Box sx={{ flex: 1 }}>
                            Chọn cơ quan thuế
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
                loading={isLoading}
                onOpenChange={setOpen}
                items={items}
                selected={_selectedData}
                onSelectedChange={(data: any) => {
                    props.onValueChanged(data.id, data.data)
                }}
                onFilterChange={setFilter}
                showItemDividers={true}
                overlayProps={{ width: 'xlarge', height: 'medium' }}
            />
        </>
    );
};

export default SelectBoxCoQuanThue;