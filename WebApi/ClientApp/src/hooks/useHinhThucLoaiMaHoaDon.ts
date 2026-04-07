
export const useHinhThucLoaiMaHoaDons = () => {
    // const { loaiHoaDonCTs, status } = useAppSelector(x => x.hoaDon.loaiHoaDonCTReducer)
    // const dispatch = useAppDispatch();

    // useEffect(() => {
    //     if (status === eReducerStatusBase.is_not_initialization) {
    //         dispatch(rootAction.hoaDon.loaiHoaDonCTAction.loadStart())
    //     }
    // }, [status])
    const data = [
        { id: "C", name: "Hóa đơn có mã của CQT" },
        { id: "K", name: "Hóa đơn không có mã của CQT" },
        { id: "M", name: "Hóa đơn có mã khởi tạo từ máy tính tiền" },
    ]
    return {
        hinhThucLoaiMaHoaDons: data
    };
}
