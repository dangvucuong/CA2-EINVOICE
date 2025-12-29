import { ActionList, Box } from '@primer/react';
import clsx from 'clsx';
import styles from "./ContactStatus.module.css";
import { eContactStatus } from '../../models/commons/eContactStatus';
interface IContactStatusProps {
    id: number
}
const ContactStatus = (props: IContactStatusProps) => {
    return (
        <Box sx={{
            display: "flex",
            alignItems: "center"
        }}>
            <Box className={clsx(
                styles.status,
                `status_${props.id}`
            )}>
                &nbsp;
            </Box>
            <Box>
                {props.id === eContactStatus.NEW ? "new" : ""}
                {props.id === eContactStatus.PENDING ? "pending" : ""}
                {props.id === eContactStatus.APPROVAL ? "approval" : ""}
                {props.id === eContactStatus.REJECT ? "reject" : ""}
            </Box>
        </Box>
    );
};

export default ContactStatus;