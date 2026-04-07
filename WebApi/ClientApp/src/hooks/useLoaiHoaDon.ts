import { useEffect, useMemo } from 'react';
import { useAppSelector } from './useAppSelector';
import { eReducerStatusBase } from '../state/reducer-models/eReducerStatusBase';
import { useAppDispatch } from './useAppDispatch';
import { rootAction } from '../state/actions/rootAction';

export const useLoaiHoaDons = () => {
    const { loaiHoaDons, status } = useAppSelector(x => x.hoaDon.loaiHoaDonReducer)
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.hoaDon.loaiHoaDonAction.loadStart())
        }
    }, [status])
    return {
        loaiHoaDons
    };
}

export const useLoaiHoaDon = (id: number) => {
    const { loaiHoaDons } = useLoaiHoaDons();
    const loaiHoaDon = useMemo(() => {
        return loaiHoaDons.find(x => x.id === id);
    }, [loaiHoaDons, id])
    return {
        loaiHoaDon
    };
}