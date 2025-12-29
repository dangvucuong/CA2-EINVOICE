import { PlusIcon, TrashIcon, UploadIcon } from '@primer/octicons-react';

import { Box, IconButton, TextInput } from '@primer/react';
import moment from 'moment';
import { useState } from 'react';
import SelectBoxHoaDon from '../../component-data/selectbox-hoa-don';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import { eSize } from '../../models/commons/eSize';
import { IThongBaoSaiSotChiTiet } from '../../models/responses/tbss/IThongBaoSaiSotChiTiet';
import ThongBaoSaiSotChiTietImportModal from './ThongBaoSaiSotChiTietImportModal';
interface IThongBaoSaiSotFormHoaDonProps {
    data: IThongBaoSaiSotChiTiet[],
    onValueChanged: (data: IThongBaoSaiSotChiTiet[]) => void,
    allowSelect?: boolean
}
const PlusIconAccent = () => {
    return <Box sx={{ color: "accent.fg" }}>
        <PlusIcon />
    </Box>
}
const ThongBaoSaiSotFormHoaDon = (props: IThongBaoSaiSotFormHoaDonProps) => {
    const [isShowImportModal, setIsShowImportModal] = useState(false);
    return (
        <Box>

            <Box sx={{ mb: 3, display: "flex", alignItems: "center" }}>
                <Box sx={{ mr: 2, flex: 1 }}>
                    <Heading text='Danh sách hóa đơn cần thông báo' size={eSize.medium} />
                </Box>
                {props.allowSelect &&
                    <>
                        <Box sx={{
                            display: "flex"
                        }}>
                            <SelectBoxHoaDon
                                placeHolder='Thêm hóa đơn từ danh sách'
                                value={0}
                                variant={"primary"}
                                leadingVisual={PlusIcon}
                                onValueChanged={(ids, hoa_dons) => {

                                    if (hoa_dons) {

                                        props.onValueChanged([
                                            ...props.data,
                                            ...hoa_dons.map(hoa_don => {
                                                return {
                                                    id: 0,
                                                    hoa_don_dang_ky_phat_hanh_ky_hieu: hoa_don?.hoa_don_dang_ky_phat_hanh_ky_hieu,
                                                    hoa_don_dang_ky_phat_hanh_mau_so: hoa_don?.hoa_don_dang_ky_phat_hanh_mau_so,
                                                    hoa_don_id: hoa_don?.id,
                                                    ma_so_hoa_don: hoa_don?.ma_so_hoa_don?.toString(),
                                                    thong_bao_sai_sot_id: 0,
                                                    ngay_hoa_don: hoa_don?.ngay_hoa_don ? moment(hoa_don?.ngay_hoa_don).format("YYYY-MM-DD") : hoa_don?.ngay_hoa_don,
                                                    ma_cqt_cap: (hoa_don?.phat_hanh_ma_ketqua_cqt ?? "") !== ""
                                                        ? hoa_don?.phat_hanh_ma_ketqua_cqt :
                                                        (hoa_don?.ma_so_hoa_don_mtt ?? ""),


                                                }
                                            })

                                        ])
                                    }
                                }}
                            />
                            {/* <Button text='Import từ excel' leadingVisual={UploadIcon}
                        variant='invisible'
                        size="medium"
                        onClick={() => {
                            // setIsShowImportModal(true)
                        }}
                    /> */}
                        </Box>
                    </>
                }
                <Button
                    text="Import từ excel"
                    leadingVisual={UploadIcon}
                    variant="invisible"
                    size="medium"
                    onClick={() => {
                        setIsShowImportModal(true);
                        // console.log(hangHoas, "hang hóa log");
                    }}
                />

            </Box>
            <Box>

                {/* <TextInputNumber /> */}
                <table className='myTable'>
                    <thead>
                        <tr>
                            <td style={{ textAlign: "center", width: "50px" }}>STT</td>
                            <td>Ký hiệu mẫu số</td>
                            <td>Ký hiệu hóa đơn</td>
                            <td>Số hóa đơn</td>
                            <td>Ngày hóa đơn</td>
                            <td>Mã CQT</td>
                            <td style={{ textAlign: "center", width: "50px" }}></td>
                        </tr>
                    </thead>
                    <tbody>
                        {props.data.map((item, idx) => {
                            return (
                                <tr className="tr-no-padding">
                                    <td style={{ textAlign: "center", width: "50px" }}>{idx + 1}</td>
                                    <td>
                                        {/* {item.hoa_don_dang_ky_phat_hanh_mau_so ?? ""} */}
                                        <TextInput
                                            className="noborder"
                                            block
                                            defaultValue={item.hoa_don_dang_ky_phat_hanh_mau_so}
                                            onChange={(e) => {
                                                props.onValueChanged(props.data.map((x, i) => {
                                                    if (i === idx) {
                                                        return {
                                                            ...x,
                                                            hoa_don_dang_ky_phat_hanh_mau_so: e.target.value
                                                        }
                                                    }
                                                    return {
                                                        ...x
                                                    }
                                                }))

                                            }}
                                        />
                                    </td>
                                    <td>
                                        <TextInput
                                            className="noborder"
                                            block
                                            defaultValue={item.hoa_don_dang_ky_phat_hanh_ky_hieu}
                                            onChange={(e) => {
                                                props.onValueChanged(props.data.map((x, i) => {
                                                    if (i === idx) {
                                                        return {
                                                            ...x,
                                                            hoa_don_dang_ky_phat_hanh_ky_hieu: e.target.value
                                                        }
                                                    }
                                                    return {
                                                        ...x
                                                    }
                                                }))

                                            }}
                                        />
                                    </td>
                                    <td>
                                        <TextInput
                                            className="noborder"
                                            block
                                            defaultValue={item.ma_so_hoa_don}
                                            onChange={(e) => {
                                                props.onValueChanged(props.data.map((x, i) => {
                                                    if (i === idx) {
                                                        return {
                                                            ...x,
                                                            ma_so_hoa_don: e.target.value
                                                        }
                                                    }
                                                    return {
                                                        ...x
                                                    }
                                                }))

                                            }}
                                        />
                                    </td>
                                    <td>
                                        {/* {item.ngay_hoa_don} */}
                                        {/* {item.ngay_hoa_don ? moment(item.ngay_hoa_don).format("DD/MM/YYYY") : ""} */}
                                        <TextInput
                                            className="noborder"
                                            block
                                            type='date'
                                            value={item.ngay_hoa_don ? moment(item.ngay_hoa_don).format("YYYY-MM-DD") : undefined}
                                            onChange={(e) => {
                                                props.onValueChanged(props.data.map((x, i) => {
                                                    if (i === idx) {
                                                        return {
                                                            ...x,
                                                            ngay_hoa_don: e.target.value
                                                        }
                                                    }
                                                    return {
                                                        ...x
                                                    }
                                                }))

                                            }}
                                        />
                                    </td>
                                    <td>
                                        {/* {item.ma_cqt_cap} */}
                                        <TextInput
                                            className="noborder"
                                            block
                                            // type='date'
                                            defaultValue={item.ma_cqt_cap}
                                            onChange={(e) => {
                                                props.onValueChanged(props.data.map((x, i) => {
                                                    if (i === idx) {
                                                        return {
                                                            ...x,
                                                            ma_cqt_cap: e.target.value
                                                        }
                                                    }
                                                    return {
                                                        ...x
                                                    }
                                                }))

                                            }}
                                        />
                                    </td>
                                    <td style={{ textAlign: "center" }}>
                                        <Box sx={{
                                            m: -2
                                        }}>
                                            <IconButton
                                                aria-label={`Remove: ${idx}`}
                                                title={`Remove: ${idx}`}
                                                icon={TrashIcon}
                                                variant="invisible"
                                                onClick={() => {
                                                    let arr = [...props.data]
                                                    arr.splice(idx, 1)
                                                    props.onValueChanged(arr)
                                                }}
                                            />
                                        </Box>
                                    </td>
                                </tr>
                            );
                        })}
                        <tr>
                            <td colSpan={7}>
                                <Box sx={{
                                    width: '100%',
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center"
                                }}>
                                    <Button leadingVisual={PlusIconAccent} text='Thêm thủ công'
                                        variant="invisible"
                                        size="medium"

                                        sx={{
                                            color: "accent.fg"
                                        }}
                                        onClick={() => {
                                            const fake: any = {}
                                            props.onValueChanged([
                                                ...props.data,
                                                fake
                                            ])
                                        }}
                                    />
                                </Box>
                            </td>
                        </tr>

                    </tbody>


                </table>
            </Box>
            {isShowImportModal && (
                <ThongBaoSaiSotChiTietImportModal
                    onClose={() => {
                        setIsShowImportModal(false);
                    }}
                    onSuccess={(data) => {
                       props.onValueChanged(data)
                        setIsShowImportModal(false);
                    }}
                />
            )}
        </Box>
    );
};

export default ThongBaoSaiSotFormHoaDon;