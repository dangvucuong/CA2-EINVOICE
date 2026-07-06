import { Box, FormControl, Radio, RadioGroup } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { bangTongHopApi } from "../../api/bang-tong-hop/bangTongHopApi";
import BangTongHopLoaiHangHoaSelection from "../../component-ui/bang-tong-hop-loai-hang-hoa";
import Button from "../../component-ui/button";
import { DataTable } from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import QuySelection from "../../component-ui/quy-selection";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import ThangSelection from "../../component-ui/thang-selection";
import TuNgayDenNgayInput from "../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput";
import { NotifyHelper } from "../../helpers/toast";
import { IBangTongHopAddOrEditRequest } from "../../models/responses/bang-tong-hop/IBangTongHopAddOrEditRequest";
import { IHoaDon } from "../../models/responses/hoa-don/IHoaDon";


const BangTongHopForm = () => {
    const { id: pId }: any = useParams();
    const bangTongHopId = pId ? parseInt(pId) : 0;
    const history = useHistory();

    const [tuNgay, setTuNgay] = useState<string>();
    const [denNgay, setDenNgay] = useState<string>();

    const [isSaving, setIsSaving] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);
    const [hoaDonSelectedIds, setHoaDonSelectedIds] = useState<number[]>([]);


    const [loaiKyTongHop, setloaiKyTongHop] = useState<"ngay" | "thang" | "quy">("ngay");
    const [ngay, setNgay] = useState<string>();

    const [nam, setNam] = useState(new Date().getFullYear());
    const [thang, setThang] = useState(new Date().getMonth() + 1);
    const [quy, setQuy] = useState(Math.ceil((new Date().getMonth() + 1) / 3));
    const [lanNop, setLanNop] = useState<"lan_dau" | "bo_sung" | "">("");

    const [loaiHangHoaId, setLoaiHangHoaId] = useState(1);
    const [sttBangTongHop, setSttBangTongHop] = useState(0);

    const [savedFormData, setSavedFormData] = useState<IBangTongHopAddOrEditRequest>();

    useEffect(() => {
        if (bangTongHopId > 0) {
            handletGetViewData()
        }
    }, [bangTongHopId])
    useEffect(() => {
        if (savedFormData) {
            const bang_tong_hop_du_lieu_type: any = savedFormData.bang_tong_hop_du_lieu_type
            setloaiKyTongHop(bang_tong_hop_du_lieu_type)
            setNam(savedFormData.nam)
            setNgay(savedFormData.ngay)
            setQuy(savedFormData.quy)
            setThang(savedFormData.thang)
            setLanNop(savedFormData.is_lan_dau ? "lan_dau" : "bo_sung")
            setLoaiHangHoaId(savedFormData.bang_tong_hop_du_lieu_loai_hang_hoa_id)
            setSttBangTongHop(savedFormData.so_thu_tu_lan_bo_sung)
        }
    }, [savedFormData])


    const handleSubmit = async () => {
        let isValid = true;
        if (loaiKyTongHop === "ngay" && !ngay) {
            NotifyHelper.Error("Vui lòng điền kỳ tính thuế")
            isValid = false; return;
        }
        if (!lanNop) {
            NotifyHelper.Error("Vui lòng điền lần nộp")
            isValid = false; return;
        }
        if (!loaiHangHoaId && loaiHangHoaId <= 0) {
            NotifyHelper.Error("Vui lòng chọn loại hàng hóa")
            isValid = false; return;
        }
        if (!sttBangTongHop && sttBangTongHop <= 0) {
            NotifyHelper.Error("Vui lòng điền Số thứ tự bảng tổng hợp")
            isValid = false; return;
        }
        if (hoaDonSelectedIds.length <= 0) {
            NotifyHelper.Error("Vui lòng chọn hóa đơn từ danh sách")
            isValid = false; return;
        }
        if (isValid) {
            setIsSaving(true)
            const res = await bangTongHopApi.save({
                bang_tong_hop_du_lieu_loai_hang_hoa_id: loaiHangHoaId,
                bang_tong_hop_du_lieu_trang_thai_id: 0,
                bang_tong_hop_du_lieu_type: loaiKyTongHop,
                // bo_sung_lan_thu: sttBangTongHop,
                is_lan_dau: lanNop === "lan_dau",
                donvi_ma_dv: "",
                hoa_don_ids: hoaDonSelectedIds,
                ket_qua_phat_hanh: "",
                ky_du_lieu: "",
                ngay: ngay ? moment(ngay).format("YYYY-MM-DD") : "",
                thang: thang,
                quy: quy,
                phat_hanh_uuid: "",
                so_luong_hoa_don: hoaDonSelectedIds.length > 2000 ? 2000 : hoaDonSelectedIds.length,
                id: pId,
                so_thu_tu_lan_bo_sung: sttBangTongHop,
                user_id_phathanh: "",
                nam: nam,
                hoa_dons: []
            });
            setIsSaving(false)
            if (res.is_success) {
                NotifyHelper.Success("Success")
                history.push("../../bang-tong-hop")
            } else {
                NotifyHelper.Error("Error")

            }
        }
    }
    const handleGetHoaDonAsync = async () => {
        setIsLoading(true)
        const res = await bangTongHopApi.selectHoaDonForTongHop(tuNgay, denNgay)
        if (res.is_success) {
            setHoaDons(res.data ?? [])
            setHoaDonSelectedIds([])
        } else {
            NotifyHelper.Error(res.message ?? "Không thể tổng hợp hóa đơn")
        }
        setIsLoading(false)
    }
    const handletGetViewData = async () => {
        setIsLoading(true)
        const res = await bangTongHopApi.selectViewById(bangTongHopId)
        if (res.is_success) {
            setSavedFormData(res.data)
            setHoaDons(res.data.hoa_dons)
            setHoaDonSelectedIds(res.data.hoa_don_ids)
        } else {
            NotifyHelper.Error(res.message ?? "Error")
        }
        setIsLoading(false)
    }


    return (
        <Box>
            <Box sx={{ display: "flex" }}>
                <Box sx={{
                    width: "300px",
                    minHeight: window.innerHeight - 80,
                    borderRight: "1px",
                    borderRightStyle: "solid",
                    borderRightColor: "border.default"
                }}>
                    <Box
                        display={"grid"}
                        sx={{
                            gap: 2,
                        }}
                    >
                        <RadioGroup name="" onChange={(e: any) => {
                            // console.log({
                            //     x: e
                            // });
                            setloaiKyTongHop(e)
                        }}>
                            <RadioGroup.Label>Loại kỳ tổng hợp</RadioGroup.Label>
                            <FormControl>
                                <Radio value="ngay" checked={loaiKyTongHop === "ngay"} />
                                <FormControl.Label>Theo ngày</FormControl.Label>
                            </FormControl>
                            <FormControl>
                                <Radio value="thang" checked={loaiKyTongHop === "thang"} />
                                <FormControl.Label>Theo tháng</FormControl.Label>
                            </FormControl>
                            <FormControl>
                                <Radio value="quy" checked={loaiKyTongHop === "quy"} />
                                <FormControl.Label>Theo quý</FormControl.Label>
                            </FormControl>
                        </RadioGroup>
                        <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
                            {loaiKyTongHop !== "ngay" &&
                                <FormControl>
                                    <TextInput type="number" min={0} value={nam} onChange={(e) => {
                                        setNam(parseInt(e.target.value))
                                    }}
                                        width={100}
                                    />
                                    <FormControl.Label>Năm</FormControl.Label>
                                </FormControl>
                            }
                            <FormControl>
                                {loaiKyTongHop === "ngay" &&
                                    <TextInput type="date" value={ngay}
                                        onChange={(e) => {
                                            setNgay(e.target.value)
                                        }}
                                    />
                                }
                                {loaiKyTongHop === "thang" &&
                                    <ThangSelection value={thang}
                                        onValueChanged={(id) => {
                                            setThang(id)
                                        }}
                                    />
                                }

                                {loaiKyTongHop === "quy" &&
                                    <QuySelection value={quy}
                                        onValueChanged={(id) => {
                                            setQuy(id)
                                        }}
                                    />
                                }

                                <FormControl.Label>Kỳ tính thuế</FormControl.Label>
                            </FormControl>
                        </Box>


                        <RadioGroup name="" sx={{ mt: 3 }} onChange={(value: any) => {
                            setLanNop(value)
                        }}>
                            <RadioGroup.Label>Lần nộp</RadioGroup.Label>
                            <FormControl>
                                <FormControl>
                                    <Radio value="lan_dau" checked={lanNop === "lan_dau"} />
                                    <FormControl.Label>Nộp lần đầu</FormControl.Label>
                                </FormControl>
                            </FormControl>
                            <FormControl>
                                <FormControl>
                                    <Radio value="bo_sung" checked={lanNop === "bo_sung"} />
                                    <FormControl.Label>Nộp bổ sung</FormControl.Label>
                                </FormControl>
                            </FormControl>
                        </RadioGroup>

                        <FormControl>
                            <TextInput type="number" min={1} width={80}
                                value={sttBangTongHop}
                                onChange={(e) => {
                                    setSttBangTongHop(parseInt(e.target.value))
                                }}
                            />
                            <FormControl.Label>Số thứ tự bảng tổng hợp</FormControl.Label>
                        </FormControl>


                        <FormControl sx={{ mt: 3 }}>

                            <FormControl.Label>Loại hàng hóa</FormControl.Label>
                            <BangTongHopLoaiHangHoaSelection
                                onValueChanged={(id) => {
                                    setLoaiHangHoaId(id)
                                }}
                                value={loaiHangHoaId}
                            />
                        </FormControl>
                        <Box sx={{
                            mt: 3,
                            ml: -3,
                            p: 3,
                            borderTop: 1,
                            borderTopStyle: "solid",
                            borderTopColor: "border.default"
                        }}>
                            <Button text="Cập nhật"
                                size="large"
                                variant="primary"
                                block
                                isLoading={isSaving}
                                onClick={handleSubmit}
                            />
                        </Box>

                    </Box>

                </Box>
                <Box sx={{
                    flex: 1,
                    pl: 3,
                    pr: 3
                }}>
                    <Box sx={{
                        height: window.innerHeight - 110,
                        overflowY: "auto"
                    }}>
                        <DataTable
                            titleComponent={<Heading text='Danh sách hóa đơn' />}
                            subTitleComponent={<>
                                <Box>
                                    <Text text="(Giới hạn 2 nghìn hóa đơn một lần)" />
                                </Box>
                            </>}
                            // subTitle={`Tổng số: ${(thongBaoSaiSots.length).toLocaleString()}`}
                            data={hoaDons}
                            height={window.innerHeight - 100}

                            // isLoading={status === eReducerStatusBase.is_loading}
                            // exportEnable
                            selection={{
                                mode: "multiple",
                                selectedRowKeys: hoaDonSelectedIds,
                                onSelectionChanged: (keys) => {
                                    setHoaDonSelectedIds(keys)
                                }
                            }}
                            actionComponent={<>
                                <TuNgayDenNgayInput
                                    tu_ngay={tuNgay}
                                    den_ngay={denNgay}
                                    onValueChanged={(tu_ngay, den_ngay) => {
                                        setTuNgay(tu_ngay)
                                        setDenNgay(den_ngay)
                                    }}

                                />
                                <Button text="Tổng hợp" size="medium" variant="primary" onClick={handleGetHoaDonAsync}
                                    isLoading={isLoading}
                                />
                            </>}
                            searchEnable={true}

                            columns={[
                                {
                                    header: 'Id',
                                    field: 'id',
                                    rowHeader: false,
                                    width: "100px"
                                },
                                {
                                    header: 'Ký hiệu',
                                    field: 'hoa_don_dang_ky_phat_hanh_ky_hieu',
                                    rowHeader: true,
                                    width: "100px",
                                    renderCell: (data: IHoaDon) => {
                                        return <Link to={`../../hoa-don/form/${data.id}`}>{data.hoa_don_dang_ky_phat_hanh_ky_hieu}</Link>
                                    }
                                    // sortBy: "alphanumeric"
                                },

                                {
                                    header: 'Loại HĐ',
                                    field: 'ten_hoa_don',
                                    rowHeader: false,
                                    minWidth: "200px",
                                    // sortBy: "alphanumeric"
                                },
                                {
                                    header: 'Ngày HĐ',
                                    field: 'ngay_hoa_don',
                                    rowHeader: false,
                                    width: "100px",
                                    renderCell: (cell: IHoaDon) => {
                                        return (
                                            <Box>{moment(cell.ngay_hoa_don).format("DD/MM/YYYY")}</Box>
                                        )
                                    }
                                    // sortBy: "alphanumeric"
                                },
                                {
                                    header: 'Số HĐ',
                                    field: 'ma_so_hoa_don',
                                    rowHeader: false,
                                    width: "100px",
                                    // sortBy: "alphanumeric"
                                },
                                // {
                                //     header: 'MST',
                                //     field: 'nguoi_mua_mst',
                                //     rowHeader: false,
                                //     width: "140px",
                                //     // sortBy: "alphanumeric"
                                // },
                                {
                                    header: 'Người mua',
                                    field: 'nguoi_mua_ten_donvi',
                                    rowHeader: false,
                                    minWidth: "300px",
                                    renderCell: (cell: IHoaDon) => {
                                        return (
                                            <Box sx={{
                                                display: "flex",
                                                flexDirection: "column"
                                            }}>
                                                <Box>{cell.nguoi_mua_ten_donvi}</Box>
                                                <Box sx={{
                                                    fontSize: "12px",
                                                    color: "fg.muted"
                                                }}>{cell.nguoi_mua_mst} - {cell.nguoi_mua_email}</Box>
                                            </Box>
                                        )
                                    }
                                },
                                {
                                    header: 'Tổng tiền',
                                    field: 'tong_tien_thanh_toan',
                                    rowHeader: false,
                                    width: "100px",
                                    renderCell: (cell: IHoaDon) => {
                                        return (
                                            <Box sx={{
                                                display: "flex",
                                                flexDirection: "column"
                                            }}>
                                                <Box><b>{cell.tong_tien_thanh_toan.toLocaleString()}</b></Box>

                                            </Box>
                                        )
                                    }
                                    // sortBy: "alphanumeric"
                                },


                            ]}
                        />
                    </Box>
                </Box>

            </Box>
        </Box>
    );
};

export default BangTongHopForm;