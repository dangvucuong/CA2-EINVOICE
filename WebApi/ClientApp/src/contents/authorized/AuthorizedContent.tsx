import React, { useEffect } from 'react';
import MainLayout from '../../layouts';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { rootAction } from '../../state/actions/rootAction';
import { useLogoutOnAllTabsClose } from '../../hooks/useLogoutOnAllTabsClose';
import { clearAccessToken, clearRefreshToken } from '../../api/apiClient';

const AuthorizedContent = () => {
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(rootAction.notify.notifyAction.loadSummaryStart())
    }, [])
   useLogoutOnAllTabsClose(() => {
    //    clearAccessToken();
    //    clearRefreshToken();
     });
    return (

        <MainLayout>

        </MainLayout>

    );
};

export default AuthorizedContent;