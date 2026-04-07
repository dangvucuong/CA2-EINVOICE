import { useAppSelector } from './useAppSelector';

export const useAuth = () => {
    const { user, appSelected } = useAppSelector(x => x.accountReducer)
    return {
        user,
        appSelected
    };
}