import { SearchIcon } from '@primer/octicons-react';
import { Box } from '@primer/react';
import { BarElement, CategoryScale, Chart as ChartJS, Legend, LinearScale, Title, Tooltip } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import moment from 'moment';
import { useEffect, useMemo } from 'react';
import { Bar } from 'react-chartjs-2';
import { useDebounce } from 'use-debounce';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import PlaceHolder from '../../component-ui/place-holder';
import TuNgayDenNgayInput from '../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { useHoaDonTrangThaisHook } from '../../hooks/useHoaDonTrangThai';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, ChartDataLabels);
const ChartThongKeTrangThai = () => {
    const { hoaDonTrangThais } = useHoaDonTrangThaisHook();
    const { status, filter, data } = useAppSelector(x => x.dashBoard.trangThaiReport)
    const dispatch = useAppDispatch();
    const handleSelectReport = () => {
        dispatch(rootAction.dashBoard.trangThaiReportLoadStart(filter))
    }
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            handleSelectReport();
        }
    }, [status])
    const isLoading = status === eReducerStatusBase.is_loading;
    const [isShowLoading] = useDebounce(isLoading, 300)
    const hoaDonTrangThaisHasValue = useMemo(() => {
        const hoaDonTrangThaiIds = data.map(x => x.hoa_don_trang_thai_id)
        return hoaDonTrangThais.filter(x => hoaDonTrangThaiIds.includes(x.id));
    }, [hoaDonTrangThais, data])
    const chartDataSource = useMemo(() => {
        const hoaDonTrangThaiIds = data.map(x => x.hoa_don_trang_thai_id)
        const hoaDonTrangThaisHasValue = hoaDonTrangThais.filter(x => hoaDonTrangThaiIds.includes(x.id))
        return {
            labels: [...hoaDonTrangThaisHasValue.map(x => x.name)],
            datasets: [
                {
                    // label: '# of Votes',
                    data: [...hoaDonTrangThaisHasValue.map(x => {
                        const hoaDonData = data.find(y => x.id === y.hoa_don_trang_thai_id)
                        return hoaDonData?.total ?? 0
                    })],
                    backgroundColor: [
                        ...hoaDonTrangThaisHasValue.map(x => x.color)
                    ],
                    borderColor: [
                        ...hoaDonTrangThaisHasValue.map(x => x.color)
                    ],
                    borderWidth: 1,
                }


            ],
        };

    }, [hoaDonTrangThais, data])

    const options: any = {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
                display: false,
            },
            datalabels: {
                color: '#ffffff', // Set label color to white
                // anchor: 'end',
                // align: 'start',
                // offset: -10,

            },
            title: {
                display: false,
                text: 'Chart.js Bar Chart',
            },
        },
    };

    return (
        <Box>
            <Box sx={{ height: 100 }}>
                <Box className='row'>
                    <Box className='col-md-6'>
                        <Box sx={{ flex: 1, display: "flex", flexDirection: "column" }}>
                            <Heading text='Thống kê' />
                            {/* <Text text='Tổng số lượng hóa đơn: 28' /> */}
                        </Box>
                    </Box>
                </Box>
                <Box className='row' sx={{ mt: 1 }}>
                    <Box className='col-md-6'>
                        <Box sx={{
                            display: "flex",
                            alignItems: "center"
                        }}>
                            <TuNgayDenNgayInput
                                tu_ngay={filter.from_date}
                                den_ngay={filter.to_date}
                                onValueChanged={(tu_ngay, den_ngay) => {
                                    dispatch(rootAction.dashBoard.trangThaiReportChangeFilter({
                                        ...filter,
                                        from_date: moment(tu_ngay).format("YYYY-MM-DD"),
                                        to_date: moment(den_ngay).format("YYYY-MM-DD"),
                                    }))
                                }}
                            />
                            <Button text='' leadingVisual={SearchIcon} onClick={handleSelectReport}
                                size='medium'
                                variant='primary'
                                sx={{ ml: 1 }}
                            />

                            {/* <Box sx={{ ml: 3 }}>
                            <FormControl>
                                <FormControl.Label>&nbsp;</FormControl.Label>
                                <Button text='' leadingVisual={SearchIcon} onClick={handleSelectReport}
                                    size='medium'
                                    variant='primary'
                                    sx={{ pr: 1 }}
                                />

                            </FormControl>
                        </Box> */}
                        </Box>
                    </Box>
                </Box>
            </Box>
            <Box sx={{
                // height: 300
            }}>
                {isShowLoading && <PlaceHolder line_number={3} />}
                {!isShowLoading &&
                    <Bar data={chartDataSource} options={options} />
                }
            </Box>
            <Box sx={{
                display: "grid",
                gridTemplateColumns:"1fr 1fr",
                gap: 2,
                mt: 3
            }}>
                {hoaDonTrangThaisHasValue.map(x => {
                    const hoaDonData = data.find(y => x.id === y.hoa_don_trang_thai_id)
                    const total = hoaDonData?.total ?? 0
                    return (
                        <Box sx={{ display: "grid", gridTemplateColumns: "20px 1fr", gap: 2 }}>
                            <Box sx={{
                                backgroundColor: x.color,
                                height: '20px',
                                borderRadius: 2
                            }}>&nbsp;</Box>
                            <Box>{x.name}: {total.toLocaleString()}</Box>
                        </Box>


                    );
                })}
            </Box>

        </Box>
    );
};

export default ChartThongKeTrangThai;