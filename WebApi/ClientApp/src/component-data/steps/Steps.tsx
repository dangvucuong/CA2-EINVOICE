import { Box } from '@primer/react';
import React from 'react';
import styles from "./Steps.module.css"
import clsx from 'clsx';
const data = [
    {
        id: 1,
        name: "Tải dữ liệu",
        active: true
    },
    {
        id: 2,
        name: "Kiểm tra dữ liệu",
        active: false
    },
    {
        id: 3,
        name: "Upload dữ liệu",
        active: false
    }
]
export interface IStepData {
    id: number,
    name: string,
    is_active: boolean
}
interface IStepsProps {
    steps: IStepData[],
    isNotShowHoanThanhStep?: boolean
}
const Steps = (props: IStepsProps) => {
    return (
        <Box className={styles.steps}>
            {props.steps.map((f, fIdx) => {
                return (
                    <Box key={fIdx} className={clsx(styles.step, f.is_active ? styles.is_active : "")}>
                        <Box className={styles.step_count}>{f.id}</Box>
                        <Box className={styles.step_text}>{f.name}</Box>
                        {fIdx !== data.length && <Box className={styles.step_arrow}></Box>}
                    </Box>
                );
            })}
            {props.isNotShowHoanThanhStep !== true &&
                <Box className={clsx(styles.step_end)}>
                    <Box className={styles.step_count}>

                    </Box>
                    <Box className={styles.step_text}>Hoàn thành</Box>
                </Box>
            }
        </Box>
    );
};

export default Steps;