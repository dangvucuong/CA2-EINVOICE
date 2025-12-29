import { HubConnectionState } from '@microsoft/signalr';
import { AlertFillIcon, CheckIcon, } from '@primer/octicons-react';
import { Box, Flash, Label, Octicon, ProgressBar, Spinner, Timeline } from '@primer/react';
import { useEffect, useState } from 'react';
import Modal from '../../component-ui/modal';
import { useHubContext } from '../../contexts/HubProvider';
import { useAuth } from '../../hooks/useAuth';
import { IProcessChangedModel, IProcessStepDataBase } from '../../models/responses/hub/IProcessChangedModel';
interface IProcessModalProps {
    process_id: string,
    onClose?: () => void,
    onDatasourceChanged?: (data: IProcessChangedModel<IProcessStepDataBase>) => void,
    dataSourceUpdated?: IProcessChangedModel<IProcessStepDataBase>,
    descriptionText?: string
}

const ProcessModal = (props: IProcessModalProps) => {
    const { user } = useAuth();
    const [isOpen, setIsOpen] = useState(true);
    const { _connectionServer } = useHubContext();
    const [dataSource, setDataSource] = useState<IProcessChangedModel<IProcessStepDataBase>>();
    const is_finished = dataSource?.is_finished ?? false;
    const processStatus = dataSource?.processStatus;
    const steps = processStatus?.steps ?? [];
    useEffect(() => {
        setDataSource(props.dataSourceUpdated)
    }, [props.dataSourceUpdated])
    useEffect(() => {
        if (_connectionServer && _connectionServer.state === HubConnectionState.Connected) {
            _connectionServer.on('PROCESS_CHANGED', OnProcessChanged);

        }
        return () => {
            _connectionServer.off('PROCESS_CHANGED', OnProcessChanged);

        };
    }, [_connectionServer])
    const OnProcessChanged = (data: IProcessChangedModel<IProcessStepDataBase>) => {
        console.log({
            data
        });

        if (data.user_id === (user?.user_id.toString() ?? "") && data.processStatus?.progress_id === props.process_id) {
            setDataSource(data);
            if (props.onDatasourceChanged) {
                props.onDatasourceChanged(data);
            }
        }
    }
    return (
        <Modal isOpen={isOpen}
            onClose={() => {

                setIsOpen(false)
                if (props.onClose) props.onClose()
            }}
            title="Trạng thái"
            width={"large"}
        >
            <Box>
                {dataSource &&
                    <Timeline>
                        {steps.map(step => {
                            const successPercent = Math.round(100 * step.data.success / (step.data.total));
                            const errorPercent = Math.round(100 * step.data.error / (step.data.total));
                            const remainingPecent = 100 - successPercent - errorPercent;
                            const is_done = remainingPecent === 0;
                            const is_processing = remainingPecent > 0;
                            return (
                                <Timeline.Item key={step.id}>
                                    <Timeline.Badge sx={{
                                        backgroundColor: is_done ? (
                                            step.data.success === step.data.total
                                                ? "success.emphasis"
                                                : "danger.emphasis"
                                        ) : (is_processing ? "#0969da" : undefined)
                                    }}>
                                        {is_processing &&
                                            <Spinner size='small' sx={{ color: "#ffff" }} />
                                        }
                                        {is_done &&
                                            <>
                                                {step.data.success !== step.data.total &&
                                                    <Octicon icon={AlertFillIcon} aria-label="Success" sx={{ color: "#ffff" }} />
                                                }
                                                {step.data.success === step.data.total &&
                                                    <Octicon icon={CheckIcon} aria-label="Success" sx={{ color: "#ffff" }} />
                                                }
                                            </>
                                        }

                                    </Timeline.Badge>
                                    <Timeline.Body>
                                        <Box sx={{ display: "flex", width: "100%", flexDirection: "column" }}>
                                            <Box sx={{ display: "flex" }}>
                                                <Box sx={{
                                                    fontWeight: 600,
                                                    flex: 1
                                                }}>
                                                    {step.name}
                                                </Box>
                                                <Label>
                                                    {successPercent + errorPercent} %
                                                </Label>
                                            </Box>
                                            {(is_processing || is_done) &&
                                                <Box>
                                                    <Box sx={{ mt: 3, display: "flex" }}>
                                                        <Box sx={{ display: "flex", flex: 1, alignItems: "center" }}>
                                                            <Box sx={{
                                                                backgroundColor: "#0969da",
                                                                mr: 2,
                                                                height: "15px",
                                                                width: "15px",
                                                                borderRadius: 2
                                                            }}>

                                                            </Box>
                                                            <Box sx={{ display: "flex", flex: 1 }}>Tổng số: {step.data.total}</Box>
                                                        </Box>
                                                        <Box sx={{ display: "flex", flex: 1, alignItems: "center" }}>
                                                            <Box sx={{
                                                                backgroundColor: "#1f883d",
                                                                mr: 2,
                                                                height: "15px",
                                                                width: "15px",
                                                                borderRadius: 2
                                                            }}>

                                                            </Box>
                                                            <Box sx={{ display: "flex", flex: 1 }}>Thành công: {step.data.success}</Box>
                                                        </Box>
                                                        {step.data.error > 0 &&
                                                            <Box sx={{ display: "flex", flex: 1, alignItems: "center" }}>
                                                                <Box sx={{
                                                                    backgroundColor: "#cf222e",
                                                                    mr: 2,
                                                                    height: "15px",
                                                                    width: "15px",
                                                                    borderRadius: 2
                                                                }}>

                                                                </Box>
                                                                <Box sx={{ display: "flex", flex: 1 }}>Lỗi: {step.data.error}</Box>
                                                            </Box>
                                                        }

                                                    </Box>
                                                    <Box sx={{ mt: 3 }}>
                                                        <ProgressBar aria-valuenow={100} aria-label="" barSize="small" >
                                                            <ProgressBar.Item progress={successPercent} sx={{ bg: '#1f883d' }} />
                                                            <ProgressBar.Item progress={errorPercent} sx={{ bg: '#cf222e' }} />
                                                            <ProgressBar.Item progress={remainingPecent} sx={{ bg: '#0969da' }} />

                                                        </ProgressBar>
                                                    </Box>
                                                </Box>
                                            }
                                        </Box>
                                    </Timeline.Body>
                                </Timeline.Item>
                            );
                        })}
                        <Timeline.Item>
                            <Timeline.Badge sx={{
                                backgroundColor: is_finished ? "success.emphasis" : undefined
                            }}>
                                {is_finished &&
                                    <Octicon icon={CheckIcon} aria-label="Success" sx={{ color: "#ffff" }} />
                                }

                            </Timeline.Badge>
                            <Timeline.Body>
                                Kết thúc
                            </Timeline.Body>
                        </Timeline.Item>

                    </Timeline>
                }
                <Box>
                    <Flash variant='warning'>
                        {props.descriptionText ?? ""}

                    </Flash>
                </Box>
            </Box>

        </Modal>
    );
};

export default ProcessModal;