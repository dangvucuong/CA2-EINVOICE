import React from 'react';
import styles from "./RolePage.module.css"
import { Box } from '@primer/react';
import RoleList from './RoleList';
import RoleDetail from './RoleDetail';
import { Helmet } from 'react-helmet';
const RolePage = () => {
    return (
        <>
            <Helmet>
                <title>Roles</title>
            </Helmet>

            <Box className={styles.rolePage} sx={{
                display: "flex",
                m: -3
            }}>
                <Box className={styles.roleList} sx={{
                    width: "300px",
                    borderRightStyle: "solid",
                    borderColor: "border.default",
                    // borderRadius: 2,
                    borderWidth: 1,
                    p: 3,
                    height: window.innerHeight - 75

                }}>
                    <RoleList />
                </Box>
                <Box className={styles.roleDetail} sx={{
                    flex: 1
                }}>
                    <RoleDetail />
                </Box>
            </Box>
        </>
    );
};

export default RolePage;