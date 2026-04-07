import { KeyIcon, PaperAirplaneIcon, PencilIcon, PlusIcon, ShieldCheckIcon, ShieldSlashIcon } from "@primer/octicons-react";
import { Box, Link, Octicon, RelativeTime, Timeline } from '@primer/react';

import { useEffect, useState } from 'react';
import { appInfo } from '../../AppInfo';
import { hoaDonApi } from '../../api/hoa-don/hoaDonApi';
import Button from '../../component-ui/button/Button';
import PlaceHolder from '../../component-ui/place-holder';
import { NotifyHelper } from '../../helpers/toast';
import { useHoaDonLogTypes } from '../../hooks/useHoaDonLogTypeHook';
import { eHoaDonLogType } from '../../models/commons/eHoaDonLogType';
import { IHoaDonLog } from '../../models/responses/hoa-don/IHoaDonLog';
import { useToKhaiLogTypesHook } from "../../hooks/useToKhaiLogTypeHook";
import { thongBaoSaiSotApi } from "../../api/tbss/thongBaoSaiSotApi";
import moment from "moment";

export const hoaDonLogTypes = [
    {
        id: eHoaDonLogType.TAO_MOI,
        color: "#ffd78e",
        icon: PlusIcon
    },
    {
        id: eHoaDonLogType.CAP_NHAT,
        color: "#ffd78e",
        icon: PencilIcon
    },
    {
        id: eHoaDonLogType.KY,
        color: "#a4f287",
        icon: KeyIcon
    },
    {
        id: eHoaDonLogType.GUI_CQT,
        color: "#a2eeef",
        icon: PaperAirplaneIcon
    },
    {
        id: eHoaDonLogType.CQT_TU_CHOI,
        color: "#d73a4a",
        icon: ShieldSlashIcon
    },
    {
        id: eHoaDonLogType.CQT_DONG_Y,
        color: "#0cf478",
        icon: ShieldCheckIcon
    }
]
interface IThongBaoSaiSotTimeLineProps {
    id: number
}
const ThongBaoSaiSotTimeline = (props: IThongBaoSaiSotTimeLineProps) => {
    const [isLoading, setIsLoading] = useState(false);
    const [logs, setLogs] = useState<any[]>([]);
    const { toKhaiLogTypes } = useToKhaiLogTypesHook()
    useEffect(() => {
        handleLoadData();
    }, [props.id])
    const handleLoadData = async () => {
        setIsLoading(true)
        const res = await thongBaoSaiSotApi.getLogs(props.id)
        if (res.is_success) {
            setLogs(res.data)
        } else {
            NotifyHelper.Error("Có lỗi")
        }
        setIsLoading(false)
    }
    return (
        <Box>
            {isLoading && <PlaceHolder line_number={5} />}
            {!isLoading &&
                <Timeline>
                    {logs.map(log => {
                        const logType = hoaDonLogTypes.find(x => x.id === log.hoa_don_log_type_id);
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
        </Box>
    );
};

export default ThongBaoSaiSotTimeline;