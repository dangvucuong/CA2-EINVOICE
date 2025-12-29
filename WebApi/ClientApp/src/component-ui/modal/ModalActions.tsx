import { Box } from '@primer/react';
import React from 'react';
import styles from "./ModalActions.module.css";
interface IModalActionsProps {
    children?: React.ReactNode
}
const ModalActions = (props: IModalActionsProps) => {
    return (
        <Box
            className={styles.actions}
            sx={{
                p: 3,
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                borderTop: "1px",
                borderTopStyle: "solid",
                borderTopColor: "border.default",
                mb: -3,
                ml: -3,
                mr: -3,
                mt: 3
            }}>
            {props.children}
        </Box>
    );
};

export default ModalActions;