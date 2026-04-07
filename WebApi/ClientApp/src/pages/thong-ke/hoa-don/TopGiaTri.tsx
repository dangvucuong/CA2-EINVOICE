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

interface ITopGiaTriProps {
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
            text: 'Top 10 khách hàng có tổng giá trị hóa đơn lớn nhất',
        },
    },
};


const TopGiaTri = (props: ITopGiaTriProps) => {
    const [dataSource, setDataSource] = useState<any[]>([]);
    console.log({
        dataSource
    });
    const getChartDataSource = () => {
        return {
            labels: dataSource.map(x => x.donvi_ten_dv),
            datasets: [
                {
                    label: 'Tổng tiền hóa đơn',
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
        const res = await thongKeApi.selectTopGiaTri({
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

export default TopGiaTri;