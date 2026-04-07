import { Box } from '@primer/react';
import { useMemo } from 'react';
import Heading from '../../component-ui/heading';
import { useAuth } from '../../hooks/useAuth';
import styles from "./ThongTinDonvi.module.css";
const ThongTinThanhToan = () => {
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
                    <Heading text='Thông tin thanh toán' />
                </Box>

                <Box>
                    <table>

                        <tr className={styles.tr}>
                            <td className={styles.labelTd}>Số tài khoản</td>
                            <td>
                                <b>{donVi?.stk}</b>
                            </td>
                        </tr>
                        <tr className={styles.tr}>
                            <td className={styles.labelTd}>Ngân hàng</td>
                            <td>
                                <b>{donVi?.ngan_hang}</b>
                            </td>
                        </tr>
                    </table>
                </Box>

            </Box>

        </Box>
    );
}

export default ThongTinThanhToan;