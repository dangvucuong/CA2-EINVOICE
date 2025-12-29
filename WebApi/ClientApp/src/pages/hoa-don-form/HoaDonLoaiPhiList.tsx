import { AlertIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton, Octicon } from '@primer/react';
import { useEffect } from 'react';
import Button from '../../component-ui/button';
import Heading from "../../component-ui/heading";
import TextInputNumber from "../../component-ui/text-input-number/TextInputNumber";
import TextInput from '../../component-ui/text-input/TextInput';
import { eSize } from "../../models/commons/eSize";
import { IHoaDonLoaiPhi, IsHoaDonLoaiPhiValid } from "../../models/responses/hoa-don/IHoaDonLoaiPhi";

interface IHoaDonLoaiPhiListProps {
    loaiPhis: IHoaDonLoaiPhi[],
    onValueChanged: (loaiPhis: IHoaDonLoaiPhi[]) => void
}
function isNumber(value: any) {
    if (isNaN(value)) return false;
    return typeof value === 'number';
}

const PlusIconAccent = () => {
    return <Box sx={{ color: "accent.fg" }}>
        <PlusIcon />
    </Box>
}
const HoaDonLoaiPhiList
    = (props: IHoaDonLoaiPhiListProps) => {
        const { loaiPhis } = props;
        const setLoaiPhis = (loaiPhis: IHoaDonLoaiPhi[]) => {
            props.onValueChanged(loaiPhis)
        }
        // const [loaiPhis, setloaiPhis] = useState<IHoaDonLoaiPhi[]>(props.loaiPhis);
        useEffect(() => {
            props.onValueChanged(loaiPhis)
        }, [loaiPhis])

        return (
            <Box>
                <Box sx={{ mb: 3, display: "flex", alignItems: "center" }}>
                    <Box sx={{ mr: 2, flex: 1 }}>
                        <Heading text='Danh sách loại phí' size={eSize.medium} />
                    </Box>
                    <Box>

                    </Box>

                </Box>
                <Box>

                    {/* <TextInputNumber /> */}
                    <table className='myTable'>
                        <thead>
                            <tr>
                                <td style={{ width: "50px" }}></td>
                                <td style={{ textAlign: "center", width: "50px" }}>STT</td>
                                <td style={{ minWidth: "200px" }}>Tên loại phí</td>
                                <td style={{ width: "120px", textAlign: "right" }}>Thành tiền</td>
                            </tr>
                        </thead>
                        <tbody>
                            {loaiPhis.map((loaiPhi, idx) => {
                                return (
                                    <tr key={idx} className="tr-no-padding">
                                        <td style={{ width: 50 }}>
                                            <IconButton icon={TrashIcon}
                                                aria-label={`Delete:`}
                                                title={`Delete:`}
                                                variant="invisible"
                                                onClick={() => {
                                                    let arr = [...loaiPhis]
                                                    arr.splice(idx, 1)
                                                    setLoaiPhis(arr)
                                                }}
                                            />
                                        </td>
                                        <td style={{ textAlign: "center", width: "50px" }}>

                                            {!IsHoaDonLoaiPhiValid(loaiPhi) ? <Box>
                                                <Octicon icon={AlertIcon} />
                                            </Box>
                                                : <>{idx + 1}</>}
                                        </td>

                                        <td>
                                            <TextInput block
                                                value={loaiPhi.ten_le_phi}
                                                className="noborder"
                                                onChange={(e) => {
                                                    setLoaiPhis(loaiPhis.map((x, i) => {
                                                        if (i === idx) {
                                                            return {
                                                                ...x,
                                                                ten_le_phi: e.target.value
                                                            }
                                                        }
                                                        return ({
                                                            ...x
                                                        })
                                                    }))
                                                }}
                                            />
                                        </td>

                                        <td>
                                            <TextInputNumber
                                                // type='number'
                                                className="noborder"
                                                defaultValue={loaiPhi.so_tien}
                                                onValueChanged={(value) => {
                                                    setLoaiPhis(loaiPhis.map((x, i) => {
                                                        if (i === idx) {
                                                            return {
                                                                ...x,
                                                                so_tien: value,
                                                            }
                                                        }
                                                        return ({
                                                            ...x
                                                        })
                                                    }))
                                                }}

                                            />

                                        </td>

                                    </tr>

                                )
                            })}

                            <tr>
                                <td colSpan={10}>
                                    <Box sx={{
                                        width: '100%',
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center"
                                    }}>
                                        <Button leadingVisual={PlusIconAccent} text='Thêm loại phí'
                                            variant="invisible"
                                            size="medium"

                                            sx={{
                                                color: "accent.fg"
                                            }}
                                            onClick={() => {
                                                const newLoaiPhi: any = {};
                                                setLoaiPhis([...loaiPhis, newLoaiPhi])
                                            }}
                                        />
                                    </Box>
                                </td>
                            </tr>
                        </tbody>

                    </table>
                </Box>
              
            </Box>
        );
    };

export default HoaDonLoaiPhiList
    ;