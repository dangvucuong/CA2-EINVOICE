import { useEffect, useMemo } from 'react';
import { useAppSelector } from './useAppSelector';
import { eReducerStatusBase } from '../state/reducer-models/eReducerStatusBase';
import { useAppDispatch } from './useAppDispatch';
import { rootAction } from '../state/actions/rootAction';

export const useLoaiHoaDonCTs = () => {
    const { loaiHoaDonCTs, status } = useAppSelector(x => x.hoaDon.loaiHoaDonCTReducer)
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.hoaDon.loaiHoaDonCTAction.loadStart())
        }
    }, [status])
    return {
        loaiHoaDonCTs
    };
}

export const useLoaiHoaDonCT = (id: number) => {
    const { loaiHoaDonCTs } = useLoaiHoaDonCTs();
    const loaiHoaDon = useMemo(() => {
        return loaiHoaDonCTs.find(x => x.id === id);
    }, [loaiHoaDonCTs, id])
    return {
        loaiHoaDonCT: loaiHoaDon
    };
}