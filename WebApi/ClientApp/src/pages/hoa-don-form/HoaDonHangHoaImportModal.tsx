import React, { useEffect, useMemo, useState } from 'react';
import Modal from '../../component-ui/modal';
import { Box, Checkbox, FormControl, Link } from '@primer/react';
import { DownloadIcon } from "@primer/octicons-react"
import ModalActions from '../../component-ui/modal/ModalActions';
import Button from '../../component-ui/button';
import Steps from '../../component-data/steps';
import { IStepData } from '../../component-data/steps/Steps';
import Upload from '../../component-data/upload';
import Text from '../../component-ui/text';
import { IUploadRespone } from '../../models/responses/upload/IUploadRespone';
import Files from '../../component-data/files/Files';
import Heading from '../../component-ui/heading';
import { hoaDonHangHoaApi } from '../../api/hoa-don/hoaDonHangHoaApi';
import { NotifyHelper } from '../../helpers/toast';
import DataTable from '../../component-ui/data-table';
import { eSize } from '../../models/commons/eSize';
import { IHoaDonHangHoa } from '../../models/responses/hoa-don/IHoaDonHangHoa';
import { appInfo } from '../../AppInfo';
interface IHoaDonHangHoaImportModalProps {
    onClose: () => void,
    onSuccess: (data: IHoaDonHangHoa[]) => void
}

interface IValidateDataResultProps {
    file: IUploadRespone,
    onValidDone: (isValid: "" | "success" | "error", dataSource: any[]) => void
}
const _steps: IStepData[] = [
    {
        id: 1,
        name: "Upload file",
        is_active: true
    },
    {
        id: 2,
        name: "Kiểm tra dữ liệu",
        is_active: false
    },
    {
        id: 3,
        name: "Import dữ liệu",
        is_active: false
    }
]
const HoaDonHangHoaImportModal = (props: IHoaDonHangHoaImportModalProps) => {
    const [stepId, setStepId] = useState(1);
    const [uploadedFile, setUploadedFile] = useState<IUploadRespone>();
    const [isDataVilid, setIsDataVilid] = useState<"" | "success" | "error">("");
    const [dataSource, setDataSource] = useState<any[]>([]);

    const handleSubmit = () => {
        if (isDataVilid !== "success") {
            NotifyHelper.Warning("Vui lòng đảm bảo dữ liệu hợp lệ")
        } else {
            props.onSuccess(dataSource);
        }
    }
    const width = useMemo(() => {
        if (stepId === 1) {
            return "xlarge";
        }
        if (stepId === 3) {
            return "xlarge";
        }
        if (stepId === 4) {
            return "xlarge";
        }
        return "80%"
    }, [stepId])

    return (
        <>
            <Modal title={"Import"}
                onClose={() => {

                }}
                isOpen={true}
                width={width}
                height={"auto"}

            >
                <form>
                    <Box
                        display={"grid"}
                        sx={{
                            gap: 2
                        }}
                    >

                        <Box>
                            <Steps steps={_steps.map(x => ({
                                ...x,
                                is_active: x.id === stepId
                            }))} />
                        </Box>
                        <Box sx={{ mt: 3 }}>
                            {stepId === 1 &&
                                <Box>
                                    <Box>
                                        {uploadedFile &&
                                            <Box>
                                                <Heading text='File đã upload' />
                                                <Files files={[uploadedFile]}
                                                    isPreviewImg={false}
                                                    onFileRemove={() => {
                                                        setUploadedFile(undefined)
                                                    }}
                                                />
                                            </Box>
                                        }
                                        <Upload onUploadSuccess={(data) => {
                                            setUploadedFile(data)
                                            setStepId(2)
                                        }} />
                                    </Box>
                                    <Box sx={{
                                        mt: 2,
                                        display: "flex",
                                        flexDirection: "column",
                                        justifyContent: "center",
                                        alignItems: "center",
                                    }}>
                                        <Link href={`${appInfo.baseApiURL.replace("/api", "")}/Template/Template-import-hang-hoa.xlsx`} target="_blank">
                                            <Button text='Tải file mẫu'
                                                size='medium'
                                                variant='invisible'
                                                leadingVisual={DownloadIcon}
                                            />
                                        </Link>
                                        <Box>
                                            <Text text='Vui lòng format dữ liệu theo file mẫu để import dữ liệu chính xác'
                                                sx={{
                                                    color: "fg.muted"
                                                }}
                                            />
                                        </Box>
                                    </Box>
                                </Box>
                            }
                            {stepId === 2 && uploadedFile &&
                                <Box>
                                    <ValidateDataResult
                                        file={uploadedFile}
                                        onValidDone={(isValid, data) => {
                                            setIsDataVilid(isValid)
                                            setDataSource(data)
                                        }}
                                    />
                                </Box>
                            }
                        </Box>

                        <ModalActions>
                            <Button onClick={() => {
                                props.onClose()
                            }} text='Đóng' />
                            <Button variant='primary'
                                type='button'
                                text='Import'
                                onClick={handleSubmit}
                            />
                        </ModalActions>
                    </Box>
                </form>
            </Modal>
        </>
    );
};

const ValidateDataResult = (props: IValidateDataResultProps) => {
    const [isLoading, setIsLoading] = useState(false);
    const [dataSource, setDataSource] = useState<any[]>([]);
    const [isOnlyShowErrData, setIsOnlyShowErrData] = useState(false);
    // console.log({
    //     dataSource
    // });

    const errData = useMemo(() => {
        return dataSource.filter(x => x.ma_loi !== undefined && x.ma_loi !== "")
    }, [dataSource])
    useEffect(() => {
        if (errData.length > 0 || dataSource.length <= 0) {
            props.onValidDone("error", [])
        } else {
            props.onValidDone("success", dataSource.map(x => ({ ...x, thue_vat: x.thue_suat })))
        }
    }, [errData, dataSource])
    useEffect(() => {
        handleRead();
    }, [props.file.url])
    const handleRead = async () => {
        setIsLoading(true);
        const res = await hoaDonHangHoaApi.readFromExcel(props.file);
        setIsLoading(false);
        if (res.is_success) {
            setDataSource(res.data)
        } else {
            NotifyHelper.Error("Error")
        }
    }
    return (
        <Box sx={{
            height: window.innerHeight - 300,
            overflow: "scroll"
        }}>
            <DataTable
                // titleComponent={<Heading text='Danh sách hàng hóa' />}
                titleComponent={
                    <>
                        {errData.length > 0 &&
                            <FormControl>
                                <Checkbox checked={isOnlyShowErrData} onChange={(e) => {
                                    setIsOnlyShowErrData(e.target.checked)
                                }} />
                                <FormControl.Label>Chỉ hiển thị các dòng không hợp lệ</FormControl.Label>

                                <FormControl.Caption>
                                    <FormControl.Validation variant='error'>
                                        Có {errData.length} dòng không hợp lệ
                                    </FormControl.Validation>
                                </FormControl.Caption>
                            </FormControl>
                        }
                    </>

                }
                data={isOnlyShowErrData ? errData : dataSource}
                height={window.innerHeight - 300}
                isLoading={isLoading}
                actionComponent={
                    <>
                        <Heading text={`Tổng số: ${dataSource.length} bản ghi`} size={eSize.medium} />
                    </>
                }
                columns={[
                    {
                        header: 'Kết quả',
                        field: 'ma_loi',
                        rowHeader: false,
                        minWidth: "200px",
                        renderCell: (data: any) => {
                            return (
                                <>
                                    {data.ma_loi &&
                                        <FormControl.Validation variant='error'>
                                            <Box sx={{ whiteSpace: "break-spaces" }} className='limit2Line'>
                                                {data.ma_loi}
                                            </Box>
                                        </FormControl.Validation>
                                    }
                                    {!data.ma_loi &&
                                        <FormControl.Validation variant='success'>
                                            {/* <Box sx={{ whiteSpace: "break-spaces" }} className='limit2Line'>
                                                {data.ma_loi}
                                            </Box> */}
                                        </FormControl.Validation>
                                    }
                                </>
                            );
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'STT',
                        field: 'stt',
                        rowHeader: false,
                        width: "100px"
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Mã hàng',
                        field: 'ma_hang',
                        rowHeader: false,
                        width: "100px"
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Tên hàng',
                        field: 'ten_hang',
                        rowHeader: true,
                        // sortBy: "alphanumeric"
                    },

                    {
                        header: 'ĐVT',
                        field: 'dvt',
                        rowHeader: false,
                        width: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Số lượng',
                        field: 'so_luong',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: any) => {
                            return <Box>{data.so_luong.toLocaleString()}</Box>
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Đơn giá',
                        field: 'don_gia',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: any) => {
                            return <Box>{data.don_gia.toLocaleString()}</Box>
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Tỷ lệ chiết khấu (%)',
                        field: 'ty_le_chiet_khau',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: any) => {
                            return <Box>{data.ty_le_chiet_khau.toLocaleString()}</Box>
                        }
                    },
                    {
                        header: 'Thuế suất',
                        field: 'thue_suat',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: any) => {
                            return <Box>{data.thue_suat}</Box>
                        }
                    },
                    {
                        header: 'Thành tiền',
                        field: 'thanh_tien',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: any) => {
                            return <Box>{data.thanh_tien.toLocaleString()}</Box>
                        }
                    },
                ]}
            />
        </Box>
    );
}

export default HoaDonHangHoaImportModal;