import {
    BarElement,
    CategoryScale,
    Chart as ChartJS,
    Legend,
    LinearScale,
    Title,
    Tooltip,
} from 'chart.js';
import { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';
import { thongKeApi } from '../../../api/hoa-don/thongKeApi';

interface ITopSoLuongProps {
    tu_ngay?: string,
    den_ngay?: string
}
ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend
);
const options: any = {
    indexAxis: 'y' as const,
    elements: {
        bar: {
            borderWidth: 2,
        },
    },
    responsive: true,
    plugins: {
        legend: {
            position: 'right' as const,
        },
        title: {
            display: true,
            text: 'Top 10 khách hàng phát hành nhiều hóa đơn',
        },
    },
};


// export const data = {
//     labels: ['January', 'February', 'March', 'April', 'May'],
//     datasets: [
//         {
//             label: 'Số lượng hóa đơn',
//             backgroundColor: '#DE3F0F',
//             borderColor: 'rgba(75,192,192,1)',
//             borderWidth: 1,
//             hoverBackgroundColor: 'rgba(75,192,192,0.4)',
//             hoverBorderColor: 'rgba(75,192,192,1)',
//             data: [65, 59, 80, 81, 56],
//         },
//     ],
// };

const TopSoLuong = (props: ITopSoLuongProps) => {
    const [dataSource, setDataSource] = useState<any[]>([]);
    console.log({
        dataSource
    });
    const getChartDataSource = () => {
        return {
            labels: dataSource.map(x => x.donvi_ten_dv),
            datasets: [
                {
                    label: 'Tổng số lượng',
                    backgroundColor: 'rgba(75,192,192,1)',
                    borderColor: 'rgba(75,192,192,1)',
                    borderWidth: 1,
                    hoverBackgroundColor: '#DE3F0F',
                    hoverBorderColor: 'rgba(75,192,192,1)',
                    data: dataSource.map(x => x.so_luong_hoa_don),
                },
            ],
        }
    }
    const chartData = getChartDataSource();
    console.log({
        chartData
    });

    useEffect(() => {
        handleGetData();
    }, [props.tu_ngay, props.den_ngay])
    const handleGetData = async () => {
        // debugger
        const res = await thongKeApi.selectTopSoLuong({
            from_date: props.den_ngay,
            tu_ngay: props.tu_ngay,
            top: 10
        })
        if (res.is_success) {
            setDataSource(res.data)
        }
    }
    return (
        <div>
            <Bar options={options} data={chartData} />
        </div>
    );
};

export default TopSoLuong;