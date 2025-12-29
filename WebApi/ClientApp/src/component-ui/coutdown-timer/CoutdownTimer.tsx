import { Box } from '@primer/react';
import { useEffect, useState } from 'react';
import { CircularProgressbar, buildStyles } from 'react-circular-progressbar';
import 'react-circular-progressbar/dist/styles.css'; // 
import styles from "./CoutdownTimer.module.css"
interface ICoutdownTimerProps {
    seconds: number;
    onTimeout: () => void
}
const CoutdownTimer = (props: ICoutdownTimerProps) => {
    const [seconds, setSeconds] = useState(props.seconds);
    const [totalSeconds, setTotalSeconds] = useState(props.seconds);

    useEffect(() => {
        if (seconds > 0) {
            const interval = setInterval(() => {
                setSeconds(prevSeconds => prevSeconds - 1);
            }, 1000);

            // Xóa interval khi component bị unmount
            return () => clearInterval(interval);
        } else {
            props.onTimeout()
            // Bộ đếm đã về 0, thực hiện hành động tại đây
            // alert('Bộ đếm đã về 0!');
        }
    }, [seconds]);

    // Hàm để định dạng thời gian còn lại thành phút và giây
    const formatTime = (time: number) => {
        const minutes = Math.floor(time / 60);
        const seconds = time % 60;
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    };
    // Tính toán tỷ lệ phần trăm còn lại cho vòng tròn tiến trình
    const percentage = (seconds / totalSeconds) * 100;
    return (
        <Box sx={{
            height: "40px",
            width: "40px",
            textAlign: "center"
        }}
            className={styles.container}
        >
            {/* {formatTime(seconds)} */}
            <Box
            //  sx={{ mt: "2px" }}
            >
                <CircularProgressbar
                    value={100 - percentage}
                    text={formatTime(seconds)}
                    styles={buildStyles({
                        textColor: '#000',
                        pathColor: '#fff',
                        trailColor: '#cf222e',

                    })}
                />
            </Box>
        </Box>
    );
};

export default CoutdownTimer;