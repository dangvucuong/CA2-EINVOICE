import {
    ListUnorderedIcon,
    ProjectRoadmapIcon,
    SearchIcon
} from "@primer/octicons-react";
import { Box, SegmentedControl } from '@primer/react';
import moment from "moment";
import { useEffect, useState } from "react";
import { Helmet } from 'react-helmet';
import { useHistory, useParams } from "react-router-dom";
import DaiLySearch from "../../component-data/daily-search";
import KhachHangSearch from "../../component-data/khachhang-search";
import Button from '../../component-ui/button';
import TuNgayDenNgayInput from "../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput";
import { useCommonContext } from "../../contexts/common";
import { IDaiLy } from "../../models/responses/category/IDaiLy";
import { IKhachHang } from "../../models/responses/category/IKhachHang";
import HangHoaList from "./hoa-don/HangHoaList";
import HoaDonList from "./hoa-don/HoaDonList";

interface IThongKePageFilter {
    tu_ngay?: string
    den_ngay?: string
    nguoi_mua_mst?: string
    ma_dai_ly?: string
    render_key?: string
    hoa_don_trang_thai_ids?: number[]
    hoa_don_hinh_thuc_id?: number
}
const formatDate = (date: any) => date.toISOString().split("T")[0];

const ThongKePage = () => {
    const { tab, mode }: any = useParams();
    const { createUUID } = useCommonContext();
    const history = useHistory();
    const now = new Date();
    const firstDay = new Date(now.getFullYear(), now.getMonth(), 1); // Ngày đầu tiên
    const lastDay = new Date(now.getFullYear(), now.getMonth() + 1, 0); // Ngày cuối cùng
    const [tuNgay, setTuNgay] = useState<string | undefined>(moment(firstDay).format("YYYY-MM-DD"));
    const [denNgay, setDenNgay] = useState<string | undefined>(moment(lastDay).format("YYYY-MM-DD"));
    const [khachHang, setKhachHang] = useState<IKhachHang>();
    const [daiLy, setDaiLy] = useState<IDaiLy>();
    const [filter, setFilter] = useState<IThongKePageFilter>({
        tu_ngay: moment(firstDay).format("YYYY-MM-DD"),
        den_ngay: moment(lastDay).format("YYYY-MM-DD")
    });
    useEffect(() => {
        if (!mode) {
            history.push('../../thong-ke/hoa-don/raw')
        }
    }, [mode])


    return (
        <Box>
            <Helmet>
                <title>Thống kê</title>
            </Helmet>

            <Box id="toolbar" sx={{
                display: "flex",
                mt: 3,
                alignItems: "center",
                justifyContent: "center",
                flexWrap: "wrap"
            }}>
                <Box id="left" sx={{
                    flex: 1,
                    display: "flex",

                }}>
                    <TuNgayDenNgayInput
                        tu_ngay={tuNgay}
                        den_ngay={denNgay}
                        onValueChanged={(tuNgay, denNgay) => {
                            console.log({
                                tuNgay
                            });

                            setTuNgay(tuNgay)
                            setDenNgay(denNgay)
                        }}
                    />
                    <Box sx={{ ml: 1 }}>
                        <KhachHangSearch
                            isShowClearBtn={true}
                            maxWidth={"300px"}
                            value={khachHang?.id ?? 0}
                            onValueChanged={(data) => {
                                setKhachHang(data)
                            }}
                        />
                    </Box>
                    <Box sx={{ ml: 1 }}>
                        <DaiLySearch
                            isShowClearBtn={true}
                            maxWidth={"300px"}
                            value={daiLy?.id ?? 0}
                            onValueChanged={(data) => {
                                setDaiLy(data)
                            }}
                        />
                    </Box>
                    <Button text='Tổng hợp'
                        variant='primary'
                        size="medium"
                        leadingVisual={SearchIcon}
                        sx={{
                            ml: 3
                        }}
                        onClick={() => {
                            setFilter({
                                ma_dai_ly: daiLy?.ma_dai_ly ?? "",
                                nguoi_mua_mst: khachHang?.mst ?? "",
                                tu_ngay: tuNgay,
                                den_ngay: denNgay,
                                render_key: createUUID()
                            })
                        }}
                    />


                </Box>
                <Box id="right">

                    <SegmentedControl
                        aria-label="File view"

                        onChange={(index) => {
                            if (index === 0) {
                                history.push('../../thong-ke/hoa-don/raw')
                            }
                            if (index === 1) {
                                history.push('../../thong-ke/hang-hoa/raw')
                            }
                            // if (index === 2) {
                            //     history.push('../../thong-ke/hoa-don/top-gia-tri')
                            // }
                        }}
                        size={"small"}
                    >
                        <SegmentedControl.Button selected={tab === "hoa-don" && mode === "raw"} aria-label={'Preview'} leadingIcon={ListUnorderedIcon}>
                            Hóa đơn
                        </SegmentedControl.Button>
                        <SegmentedControl.Button selected={tab === "hang-hoa" && mode === "raw"} aria-label={'Raw'} leadingIcon={ProjectRoadmapIcon}>
                            Hàng hóa
                        </SegmentedControl.Button>
                        {/* <SegmentedControl.Button aria-label={'Raw'} leadingIcon={GraphIcon}>
                                Xếp hạng theo số lượng
                            </SegmentedControl.Button>
                            <SegmentedControl.Button aria-label={'Blame'} leadingIcon={GraphIcon}>
                                Xếp hạng theo giá trị
                            </SegmentedControl.Button> */}
                    </SegmentedControl>

                    {/* </FormControl> */}

                </Box>
            </Box>
            <Box sx={{
                mt: 3
            }}>
                {tab === "hoa-don" && mode === "raw" &&
                    <HoaDonList
                        tu_ngay={filter?.tu_ngay}
                        den_ngay={filter?.den_ngay}
                        nguoi_mua_mst={filter?.nguoi_mua_mst}
                        ma_dai_ly={filter?.ma_dai_ly}
                        render_key={filter?.render_key}

                        hoa_don_trang_thai_ids={filter?.hoa_don_trang_thai_ids}
                        hoa_don_hinh_thuc_id={filter?.hoa_don_hinh_thuc_id}

                        onFilterChanged={(data) => {
                            setFilter(x => ({
                                ...x,
                                ...data,
                                render_key: createUUID()
                            }))
                        }}
                    />
                }
                {tab === "hang-hoa" && mode === "raw" &&
                    <HangHoaList
                        tu_ngay={filter?.tu_ngay}
                        den_ngay={filter?.den_ngay}
                        nguoi_mua_mst={filter?.nguoi_mua_mst}
                        ma_dai_ly={filter?.ma_dai_ly}
                        render_key={filter?.render_key}

                        hoa_don_trang_thai_ids={filter?.hoa_don_trang_thai_ids}
                        hoa_don_hinh_thuc_id={filter?.hoa_don_hinh_thuc_id}
                    />
                }
                {/* {tab === "hoa-don" && mode === "top-gia-tri" &&
                    <TopGiaTri
                        tu_ngay={filter?.tu_ngay}
                        den_ngay={filter?.den_ngay}
                    />
                } */}
            </Box>
        </Box>
    );
};

export default ThongKePage;