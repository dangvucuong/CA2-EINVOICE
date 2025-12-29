import { SearchIcon } from '@primer/octicons-react';
import { Box, FormControl } from '@primer/react';
import { CategoryScale, Chart as ChartJS, Legend, LineElement, LinearScale, PointElement, Title, Tooltip } from 'chart.js';
import moment from 'moment';
import { useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import { useDebounce } from 'use-debounce';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import TextInput from '../../component-ui/text-input';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);
const ChartLichSuPhatHanh = () => {
    const { status, data, filter } = useAppSelector(x => x.dashBoard.lichSuTheoNgayReport)
    const dispatch = useAppDispatch();
    const isLoading = status === eReducerStatusBase.is_loading;
    const [isShowLoading] = useDebounce(isLoading, 300)
    const handleSelectReport = () => {
        dispatch(rootAction.dashBoard.lichSuTheoNgayReportLoadStart(filter))
    }
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            handleSelectReport();
        }
    }, [status])
    const chartDataSource = {
        labels: [...data.map(x => moment(x.date).format("DD/MM"))],
        // labels: ["a", "b", "c"],
        datasets: [
            {
                label: 'My First Dataset',
                // data: [15, 30, 20],
                data: [...data.map(x => x.total_count)],
                borderColor: '#C53104',
                backgroundColor: 'rgba(197, 49, 4, 0.2)', // Màu nền
                fill: false,
                tension: 0.2,
                // borderWidth:
            }
        ],
    };

    const options: any = {
        scales: {
            y: {
                beginAtZero: true,
            },
        },
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
                display: false,
            },
            datalabels: {
                color: '#C53104', // Set label color to white
                anchor: 'end',
                align: 'start',
                offset: -30,
                font: {
                    size: 15 // Set font size for legend labels
                }

            },
            title: {
                display: false,
                text: 'Chart.js Line Chart',
            },
        },
    };
    return (
        <Box sx={{
        }}>
            <Box sx={{
                display: "flex",
            }}>
                <Box sx={{ flex: 1, display: "flex", flexDirection: "column" }}>
                    <Heading text='Lịch sử phát hành' />
                </Box>
                <Box sx={{
                    display: "flex"
                }}>
                    <FormControl>
                        <FormControl.Label>Từ ngày</FormControl.Label>
                        <TextInput type="date"
                            width={130}
                            value={filter.from_date}
                            onChange={(e) => {
                                dispatch(rootAction.dashBoard.lichSuTheoNgayReportChangeFilter({
                                    ...filter,
                                    from_date: moment(e.target.value).format("YYYY-MM-DD")
                                }))
                            }}
                        />

                    </FormControl>

                    <Box sx={{ ml: 3 }}>
                        <FormControl>
                            <FormControl.Label>Đến ngày</FormControl.Label>
                            <TextInput type="date"
                                width={130}
                                value={filter.to_date}
                                onChange={(e) => {
                                    // setDenNgay(e.target.value)
                                    dispatch(rootAction.dashBoard.lichSuTheoNgayReportChangeFilter({
                                        ...filter,
                                        to_date: moment(e.target.value).format("YYYY-MM-DD")
                                    }))
                                }}
                            />

                        </FormControl>
                    </Box>
                    <Box sx={{ ml: 3 }}>
                        <FormControl>
                            <FormControl.Label>&nbsp;</FormControl.Label>
                            <Button text='' leadingVisual={SearchIcon} onClick={handleSelectReport}
                                size='medium'
                                variant='primary'
                                sx={{ pr: 1 }}
                            />

                        </FormControl>
                    </Box>
                </Box>
            </Box>
            <Box sx={{
                display: "flex",
                justifyContent: "center",
                mt: 2
            }}>
                <Line data={chartDataSource} options={options}
                />
            </Box>
        </Box>
    );
}

export default ChartLichSuPhatHanh;