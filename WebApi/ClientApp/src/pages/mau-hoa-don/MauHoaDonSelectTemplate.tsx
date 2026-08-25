import { VideoIcon } from '@primer/octicons-react';
import { Box, Link } from '@primer/react';
import { useEffect, useMemo, useRef, useState } from 'react';
import { appInfo } from '../../AppInfo';
import { mauHoaDonApi } from '../../api/hoa-don/mauHoaDonApi';
import SelectBoxLoaiHoaDonCT from '../../component-data/selectbox-loai-hoa-don-ct';
import Button from '../../component-ui/button';
import FormGroupInline from '../../component-ui/form-group-inline';
import Heading from '../../component-ui/heading';
import PlaceHolder from '../../component-ui/place-holder';
import Text from '../../component-ui/text';
import { NotifyHelper } from '../../helpers/toast';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { ILoaiHoaDonCTTemplate } from '../../models/responses/hoa-don/ILoaiHoaDonCTTemplate';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';

export interface IMauHoaDonUploadXsltResult {
    template: ILoaiHoaDonCTTemplate;
    xslt_path: string;
    file_name: string;
}

interface IMauHoaDonSelectTemplateProps {
    onSelectionChanged: (data: ILoaiHoaDonCTTemplate) => void;
    onUploadXsltContinue: (data: IMauHoaDonUploadXsltResult) => void;
}

const MauHoaDonSelectTemplate = (props: IMauHoaDonSelectTemplateProps) => {
    const [loaiHoaDonCTId, setLoaiHoaDonCTId] = useState(0);
    const [isUploadingXslt, setIsUploadingXslt] = useState(false);
    const xsltFileInputRef = useRef<HTMLInputElement>(null);
    const { loaiHoaDonCTTemplates, status } = useAppSelector(x => x.hoaDon.loaiHoaDonCTTemplateReducer)
    const dispatch = useAppDispatch();
    const loaiHoaDonCTTemplatesFiltered = useMemo(() => {
        return loaiHoaDonCTTemplates.filter(x => x.loai_hoa_don_ct_id === loaiHoaDonCTId || loaiHoaDonCTId === 0)
    }, [loaiHoaDonCTId, loaiHoaDonCTTemplates])

    const defaultTemplateForLoai = useMemo(() => {
        return loaiHoaDonCTTemplates.find(x => x.loai_hoa_don_ct_id === loaiHoaDonCTId);
    }, [loaiHoaDonCTId, loaiHoaDonCTTemplates])

    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.hoaDon.loaiHoaDonCTTemplateAction.loadStart())
        }
    }, [status])

    const handleUploadXslt = async (file?: File) => {
        if (!file) return;

        if (loaiHoaDonCTId <= 0) {
            NotifyHelper.Error("Vui lòng chọn loại hóa đơn trước khi upload XSLT");
            return;
        }

        const template = defaultTemplateForLoai;
        if (!template) {
            NotifyHelper.Error("Không tìm thấy template cho loại hóa đơn đã chọn");
            return;
        }

        setIsUploadingXslt(true);
        const res = await mauHoaDonApi.uploadXslt(file);
        setIsUploadingXslt(false);

        if (res.is_success) {
            NotifyHelper.Success("Upload file XSLT thành công");
            props.onUploadXsltContinue({
                template,
                xslt_path: res.data.xslt_path,
                file_name: res.data.file_name,
            });
        } else {
            NotifyHelper.Error(res.message ?? "Upload file XSLT thất bại");
        }
    };

    return (
        <Box>
            <Box sx={{
                display: "flex",
                mb: 3,
                pb: 3,
                borderBottomColor: "border.default",
                borderBottomWidth: "1",
                borderBottomStyle: "solid"
            }}>
                <Box sx={{
                    flex: 1
                }}>
                    <Heading text='Thiết lập mẫu hóa đơn' />
                </Box>
                <Box>
                    <Button text='Xem hướng dẫn' sx={{ mr: 2, minWidth: "100px" }} size='large' variant='primary'
                        leadingVisual={VideoIcon}
                    />
                </Box>
            </Box>
            <Box sx={{
                mb: 3,
                display: "flex",
                alignItems: "flex-end",
                gap: 3,
                flexWrap: "wrap",
            }}>
                <FormGroupInline label='Chọn loại hóa đơn phù hợp'>
                    <SelectBoxLoaiHoaDonCT
                        value={loaiHoaDonCTId}
                        onValueChanged={(id) => {
                            setLoaiHoaDonCTId(id)
                        }}
                    />
                </FormGroupInline>
                <Box>
                    <input
                        ref={xsltFileInputRef}
                        type="file"
                        accept=".xslt"
                        style={{ display: "none" }}
                        onChange={(e) => {
                            const file = e.target.files?.[0];
                            if (file) {
                                handleUploadXslt(file);
                            }
                            e.target.value = "";
                        }}
                    />
                    <Button
                        text="Upload XSLT"
                        variant="primary"
                        size="large"
                        type="button"
                        disabled={loaiHoaDonCTId <= 0}
                        isLoading={isUploadingXslt}
                        onClick={() => {
                            xsltFileInputRef.current?.click();
                        }}
                    />
                </Box>
            </Box>
            <Box>
                {status === eReducerStatusBase.is_loading && <PlaceHolder line_number={10} />}
                {status !== eReducerStatusBase.is_loading &&
                    <Box className='row'>
                        {loaiHoaDonCTTemplatesFiltered.map(x => {
                            return (
                                <Box className='col-md-3' key={x.id} sx={{
                                    m: 2,
                                    p: 0
                                }}>
                                    <Box sx={{
                                        borderStyle: "solid",
                                        borderRadius: 2,
                                        borderWidth: 1,
                                        borderColor: "border.default",
                                        cursor: "pointer"
                                    }}>
                                        <img src={appInfo.baseApiURL.replace("/api", "") + x.thumbnail} alt={x.thumbnail} style={{
                                            borderTopLeftRadius: "4px",
                                            borderTopRightRadius: "4px",
                                        }}
                                            width={"100%"}
                                            height={"auto"}
                                        />
                                        <Box sx={{
                                            backgroundColor: "canvas.subtle",
                                            height: "60px",
                                            p: 2,
                                            borderRadius: 2
                                        }}>
                                            <Box sx={{
                                                display: "flex"
                                            }}>
                                                <Text text={x.name} sx={{
                                                    fontSize: 15,
                                                    fontWeight: 500,
                                                    flex: 1
                                                }}
                                                />
                                                <Box sx={{ ml: 3, display: "flex" }}>
                                                    <Link target='_blank' href={`../../../${x.path}`}>
                                                        <Button text='Tải mẫu'
                                                            variant='invisible'
                                                            disabled={!x.path || x.path === ""}
                                                            onClick={() => {
                                                                props.onSelectionChanged(x)
                                                            }}
                                                        />
                                                    </Link>
                                                    <Button text='Sử dụng'
                                                        variant='primary'
                                                        disabled={!x.path || x.path === ""}
                                                        onClick={() => {
                                                            props.onSelectionChanged(x)
                                                        }}
                                                    />
                                                </Box>
                                            </Box>

                                        </Box>
                                    </Box>
                                </Box>
                            );
                        })}
                    </Box>
                }
            </Box>
        </Box>
    );
};

export default MauHoaDonSelectTemplate;
