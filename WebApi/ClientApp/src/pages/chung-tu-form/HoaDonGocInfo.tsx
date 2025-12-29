import { Box, FormControl, TabNav } from "@primer/react";
import moment from "moment";
import { useMemo, useState } from "react";
import HoaDonSelect from "../../component-data/hoa-don-select";
import SelectBoxLoaiHoaDonNghiDinh from "../../component-data/selectbox-loai-hoadon-clq";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import TextInput from "../../component-ui/text-input";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";
import { useHistory } from "react-router-dom";
export interface IHoaDonGocInfoValue {
    hoaDonGocId: number;
    hoa_don_dang_ky_phat_hanh_mau_so_goc: string;
    hoa_don_dang_ky_phat_hanh_ky_hieu_goc: string;
    ma_so_hoa_don_goc: string;
    ngay_hoa_don_goc: string;
    hoa_don_nghi_dinh_id_goc: number;
}
interface IHoaDonGocInfoModalProps {
    value?: IHoaDonGocInfoValue,
    onSubmit: (data: IHoaDonGocInfoValue, hoaDon?: IHoaDon) => void

}
const HoaDonGocInfoModal = (props: IHoaDonGocInfoModalProps) => {
    const [hoaDonGoc, setHoaDonGoc] = useState<IHoaDon>();
    const [selectedTab, setselectedTab] = useState<"ca2" | "other">("ca2");
    const history = useHistory();

    const [hoaDonGocId, setHoaDonGocId] = useState(props.value?.hoaDonGocId ?? 0);
    const [hoa_don_dang_ky_phat_hanh_mau_so_goc, setHoa_don_dang_ky_phat_hanh_mau_so_goc] = useState(props.value?.hoa_don_dang_ky_phat_hanh_mau_so_goc ?? "");
    const [hoa_don_dang_ky_phat_hanh_ky_hieu_goc, setHoa_don_dang_ky_phat_hanh_ky_hieu_goc] = useState(props.value?.hoa_don_dang_ky_phat_hanh_ky_hieu_goc ?? "");
    const [ma_so_hoa_don_goc, setMa_so_hoa_don_goc] = useState(props.value?.ma_so_hoa_don_goc ?? "");
    const [ngay_hoa_don_goc, setNgay_hoa_don_goc] = useState(props.value?.ngay_hoa_don_goc ? moment(props.value?.ngay_hoa_don_goc).format("YYYY-MM-DD") : "");
    const [hoa_don_nghi_dinh_id_goc, setHoa_don_nghi_dinh_id_goc] = useState(props.value?.hoa_don_nghi_dinh_id_goc ?? 0);

    const isValid = useMemo(() => {
        if (!hoa_don_dang_ky_phat_hanh_mau_so_goc) return false;
        if (!hoa_don_dang_ky_phat_hanh_ky_hieu_goc) return false;
        if (!ma_so_hoa_don_goc) return false;
        if (!ngay_hoa_don_goc) return false;
        if (hoa_don_nghi_dinh_id_goc === 0) return false;
        return true;
    }, [hoa_don_dang_ky_phat_hanh_mau_so_goc, hoa_don_dang_ky_phat_hanh_ky_hieu_goc, ma_so_hoa_don_goc, ngay_hoa_don_goc, hoa_don_nghi_dinh_id_goc])
    const onSubmit = () => {
        props.onSubmit({
            hoaDonGocId,
            hoa_don_dang_ky_phat_hanh_mau_so_goc,
            hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
            ma_so_hoa_don_goc,
            ngay_hoa_don_goc,
            hoa_don_nghi_dinh_id_goc
        }, hoaDonGoc)
    }
    return (
        <Modal
            onClose={() => {
                if (isValid && props.value) {
                    onSubmit();
                } else {
                    history.goBack();
                }
            }}
            title="Điền thông tin hóa đơn gốc"
            isOpen={true}
            width={selectedTab === "ca2" ? "100%" : "large"}
        >
            <Box>
                <Box>Đối với hóa đơn <b>Đã có</b> trên hệ thống <b>Ca2 E-invoice</b>, Anh/ Chị có thể tìm kiếm từ Hóa đơn đã tạo.</Box>
                <Box>Anh/ Chị cũng có thể điền thủ công các thông tin của hóa đơn gốc.</Box>

                <Box sx={{
                    mt: 3
                }}>
                    <SelectBoxLoaiHoaDonNghiDinh
                        onValueChanged={(id) => {
                            setHoa_don_nghi_dinh_id_goc(id)
                        }}
                        value={hoa_don_nghi_dinh_id_goc}

                    />
                </Box>
                <Box sx={{
                    mt: 3
                }}>
                    <TabNav aria-label="Main">
                        <TabNav.Link selected={selectedTab === "ca2" ? true : undefined}
                            sx={{
                                cursor: "pointer"
                            }}
                            onClick={() => {
                                setselectedTab("ca2")
                            }}
                        >Hóa đơn trên Ca2 E-invoice</TabNav.Link>
                        <TabNav.Link selected={selectedTab === "other" ? true : undefined}
                            sx={{
                                cursor: "pointer"
                            }}
                            onClick={() => {
                                setselectedTab("other")
                            }}
                        >
                            Hóa đơn trên Hệ thống khác
                        </TabNav.Link>

                    </TabNav>
                </Box>
                {selectedTab === "ca2" &&
                    <Box sx={{
                        height: window.innerHeight - 400,
                        overflow: "auto"
                    }}>

                        <HoaDonSelect
                            isSingleMode={true}
                            onSelected={(ids, hoaDons) => {
                                // setHoaDonIds(ids)
                                // setHoaDons(hoaDons)
                                if (hoaDons.length === 1 && ids.length === 1) {
                                    const id = ids[0];
                                    const hoaDon = hoaDons[0];
                                    setHoaDonGoc(hoaDon)
                                    setHoaDonGocId(id)
                                    setHoa_don_dang_ky_phat_hanh_ky_hieu_goc(hoaDon?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? "")
                                    setHoa_don_dang_ky_phat_hanh_mau_so_goc(hoaDon?.hoa_don_dang_ky_phat_hanh_mau_so ?? "")
                                    setMa_so_hoa_don_goc(hoaDon?.ma_so_hoa_don ?? "")
                                    setNgay_hoa_don_goc(hoaDon?.ngay_hoa_don ? moment(hoaDon?.ngay_hoa_don).format("YYYY-MM-DD") : "")
                                }
                            }} />
                        {/* <SelectBoxHoaDon
                                    placeHolder='Chọn từ hóa đơn đã tạo'
                                    value={hoaDonGocId}
                                    onValueChanged={(ids, hoaDons) => {
                                        if (hoaDons.length === 1 && ids.length === 1) {
                                            const id = ids[0];
                                            const hoaDon = hoaDons[0];
                                            setHoaDonGoc(hoaDon)
                                            setHoaDonGocId(id)
                                            setHoa_don_dang_ky_phat_hanh_ky_hieu_goc(hoaDon?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? "")
                                            setHoa_don_dang_ky_phat_hanh_mau_so_goc(hoaDon?.hoa_don_dang_ky_phat_hanh_mau_so ?? "")
                                            setMa_so_hoa_don_goc(hoaDon?.ma_so_hoa_don ?? "")
                                            setNgay_hoa_don_goc(hoaDon?.ngay_hoa_don ? moment(hoaDon?.ngay_hoa_don).format("YYYY-MM-DD") : "")
                                        }
                                    }} /> */}

                    </Box>
                }
                {selectedTab === "other" &&
                    <>
                        <Box sx={{
                            display: "grid",
                            gap: 2,
                            mt: 3,
                            ml: 3
                        }}>
                            <FormControl>
                                <FormControl.Label>
                                    Ký hiệu mẫu số
                                </FormControl.Label>
                                <TextInput width={100} value={hoa_don_dang_ky_phat_hanh_mau_so_goc}
                                    onChange={(e) => {
                                        setHoa_don_dang_ky_phat_hanh_mau_so_goc(e.target.value)
                                    }}
                                />

                            </FormControl>
                            <FormControl>
                                <FormControl.Label>
                                    Ký hiệu hóa đơn
                                </FormControl.Label>
                                <TextInput value={hoa_don_dang_ky_phat_hanh_ky_hieu_goc}
                                    onChange={(e) => {
                                        setHoa_don_dang_ky_phat_hanh_ky_hieu_goc(e.target.value)
                                    }}
                                />

                            </FormControl>
                            <FormControl>
                                <FormControl.Label>
                                    Số hóa đơn
                                </FormControl.Label>
                                <TextInput width={100} value={ma_so_hoa_don_goc}
                                    onChange={(e) => {
                                        setMa_so_hoa_don_goc(e.target.value)
                                    }}
                                />

                            </FormControl>
                            <FormControl>
                                <FormControl.Label>
                                    Ngày hóa đơn
                                </FormControl.Label>
                                <TextInput type="date" value={ngay_hoa_don_goc}
                                    onChange={(e) => {
                                        setNgay_hoa_don_goc(moment(e.target.value).format("YYYY-MM-DD"))
                                    }}
                                />

                            </FormControl>
                        </Box>
                    </>}

            </Box>
            <ModalActions>
                <Button variant="primary" text="Xác nhận" size="medium"
                    disabled={!isValid}
                    onClick={onSubmit}
                />
            </ModalActions>
        </Modal>
    );
};

export default HoaDonGocInfoModal;