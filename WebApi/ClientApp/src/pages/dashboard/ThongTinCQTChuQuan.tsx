import { Box } from '@primer/react';
import { useMemo } from 'react';
import Heading from '../../component-ui/heading';
import { useAuth } from '../../hooks/useAuth';
import styles from "./ThongTinDonvi.module.css";
const ThongTinCQTChuQuan = () => {
    const { user } = useAuth();
    const donVi = useMemo(() => {
        if (user) {
            return user.donvi;
        }
    }, [user])

    return (
        <Box sx={{
            // width: "400px"
        }}>
            <Box sx={{
                display: "flex",
                flexDirection: "column"

            }}>
                <Box sx={{ flex: 1, display: "flex", flexDirection: "column", mb:2 }}>
                    <Heading text='Cơ quan thuế chủ quản' />
                </Box>

                <Box>
                    <table>

                        <tr className={styles.tr}>
                            {/* <td className={styles.labelTd}>Ngân hàng</td> */}
                            <td>
                                <b>{donVi?.donvi_chuquan}</b>
                            </td>
                        </tr>
                    </table>
                </Box>

            </Box>

        </Box>
    );
}

export default ThongTinCQTChuQuan;