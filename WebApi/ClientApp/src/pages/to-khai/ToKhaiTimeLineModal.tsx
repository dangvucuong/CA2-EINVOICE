import { Box, Link, Octicon, RelativeTime, Timeline } from '@primer/react';
import moment from 'moment';
import { useEffect, useState } from 'react';
import { toKhaiApi } from "../../api/to-khai/toKhaiApi";
import Button from '../../component-ui/button';
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import PlaceHolder from "../../component-ui/place-holder";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from '../../hooks/useAuth';
import { useToKhaiLogTypesHook } from "../../hooks/useToKhaiLogTypeHook";
import { IToKhaiLog } from '../../models/responses/to-khai/IToKhaiLog';
import { appInfo } from '../../AppInfo';
interface IToKhaiTimeLineModalProps {
    toKhaiId: number,
    onClose: () => void
}

export const ToKhaiTimeLineModal = (props: IToKhaiTimeLineModalProps) => {
    const { user } = useAuth();
    const [isLoading, setIsLoading] = useState(false);
    const [logs, setLogs] = useState<IToKhaiLog[]>([]);
    const { toKhaiLogTypes } = useToKhaiLogTypesHook();
    useEffect(() => {
        handleLoadData();
    }, [props.toKhaiId])
    const handleLoadData = async () => {
        setIsLoading(true)
        const res = await toKhaiApi.getLogs(props.toKhaiId)
        if (res.is_success) {
            setLogs(res.data)
        } else {
            NotifyHelper.Error("Có lỗi")
        }
        setIsLoading(false)
    }
    return (
        <Modal title={"Lịch sử"}
            onClose={() => {
                props.onClose();
            }}
            isOpen={true}
            width='large'
            height={"auto"}
        // key={khachHangEditing?.id ?? 0}

        >
            <Box
                display={"grid"}
                sx={{
                    gap: 2
                }}
            >
                {isLoading && <PlaceHolder line_number={5} />}
                {!isLoading &&
                    <Timeline>
                        {logs.map(log => {
                            const logType = toKhaiLogTypes.find(x => x.id === log.to_khai_log_type_id);
                            return (
                                <Timeline.Item>
                                    <Timeline.Badge sx={{ backgroundColor: logType?.color, color: "#fff" }}>
                                        {logType && logType.icon && <Octicon icon={logType?.icon} />}

                                    </Timeline.Badge>
                                    <Timeline.Body>
                                        <Box sx={{
                                            display: "flex"
                                        }}>
                                            <Box sx={{
                                                flex: 1
                                            }}>
                                                <Box>
                                                    <b>{log.nguoi_thuc_hien}</b> {log.noi_dung_thuc_hien}
                                                </Box>
                                                <Box>
                                                    <RelativeTime date={moment(log.ngay_thuc_hien).toDate()} />
                                                </Box>
                                            </Box>
                                            {log.file_thong_diep_url !== "" &&
                                                <Box>
                                                    <Link href={`${appInfo.baseApiURL.replace("/api", "")}/${log.file_thong_diep_url}`} target="_blank">
                                                        <Button variant='danger'>
                                                            Xem thông điệp XML

                                                        </Button>
                                                    </Link>
                                                </Box>
                                            }
                                        </Box>
                                    </Timeline.Body>
                                </Timeline.Item>
                            );
                        })}
                    </Timeline>

                }
                {/* <Timeline.Item>
                        <Timeline.Badge>
                            <Octicon icon={PlusIcon} />
                        </Timeline.Badge>
                        <Timeline.Body>
                            <Box>
                                <b>{user?.full_name}</b> Tạo tờ khai
                            </Box>
                            <Box>
                                <RelativeTime date={moment().add(-3, "days").toDate()} />
                            </Box>
                        </Timeline.Body>

                    </Timeline.Item>
                    <Timeline.Item>
                        <Timeline.Badge sx={{ backgroundColor: "accent.emphasis", color: "#fff" }}>
                            <Octicon icon={GitCommitIcon} />
                        </Timeline.Badge>
                        <Timeline.Body>
                            <Box>
                                <b>Cơ quan thuế</b> đã tiếp nhận
                            </Box>
                            <Box>
                                <RelativeTime date={moment().add(-1, "days").toDate()} />
                            </Box>
                        </Timeline.Body>
                    </Timeline.Item> */}
                {/* <Timeline.Item>
                        <Timeline.Badge sx={{ backgroundColor: "success.emphasis", color: "#fff" }}>
                            <Octicon icon={ShieldCheckIcon} />
                        </Timeline.Badge>
                        <Timeline.Body>
                            <Box sx={{
                                display: "flex"
                            }}>
                                <Box sx={{
                                    flex: 1
                                }}>
                                    <Box>
                                        <b>Cơ quan thuế</b> đã chấp nhận
                                    </Box>
                                    <Box>
                                        <RelativeTime date={moment().add(-1, "hours").toDate()} />
                                    </Box>
                                </Box>
                                <Box>
                                    <Button variant='danger'>
                                        Xem thông điệp XML
                                    </Button>
                                </Box>
                            </Box>
                        </Timeline.Body>
                    </Timeline.Item> */}


                <ModalActions>
                    <Button onClick={() => {
                        props.onClose();
                    }} text='Đóng' />

                </ModalActions>
            </Box>
        </Modal>
    );
};
