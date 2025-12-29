import { Box, Flash, TextInput } from "@primer/react";
import { SearchIcon } from "@primer/octicons-react";
import { useAuth } from "../../hooks/useAuth";
import { Helmet } from "react-helmet";
import styles from "./TraCuuPage.module.css"
import Button from "../../component-ui/button";
import { Link } from "react-router-dom";
import { useState } from "react";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import { NotifyHelper } from "../../helpers/toast";
import HoaDonView from "../hoa-don-form/HoaDonView";
const TraCuuPage = () => {
    const { user } = useAuth();
    const [maTraCuu, setMaTraCuu] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [hoaDonId, setHoaDonId] = useState(-1);

    const handleSearch = async () => {
        setIsLoading(true)
        const res = await hoaDonApi.searchByMaTraCuu(maTraCuu);
        setIsLoading(false)

        if (res.is_success) {
            setHoaDonId(res.data)
        } else {
            setHoaDonId(0)
            NotifyHelper.Error(res.message ?? "Có lỗi")
        }
    }

    return (
        <Box sx={{
            height:window.innerHeight,
            overflowY:"auto"
        }}>
            <Helmet>
                <title>Tra cứu</title>
            </Helmet>
            {!user &&
                <Box id="header" className={styles.navsub}
                    sx={{
                        height: "72px",
                        display: "flex",
                        alignItems: "center"
                    }}
                >
                    <Box sx={{ ml: 3 }}>
                        <img alt='logo' src='../../images/logo-white.svg' height={"40px"} />
                    </Box>
                    <Box sx={{ flex: 1 }}>

                    </Box>
                    <Box sx={{
                        mr: 3,
                        display: "flex"
                    }}>
                        <Link to={"../../login"}>
                            <Button text="Đăng nhập" size="medium" sx={{ mr: 1 }} />
                        </Link>
                        <Link to={"../../register"}>
                            <Button text="Đăng ký" size="medium" />
                        </Link>

                    </Box>
                </Box>
            }
            <Box sx={{ p: 3 }}>
                <Box sx={{
                    display: "flex",
                    justifyContent: "center",
                    width: "100%"
                }}>
                    <Box sx={{ display: "flex" }}>
                        <TextInput leadingVisual={SearchIcon}
                            placeholder='Tìm kiếm theo mã tra cứu'
                            value={maTraCuu}
                            width={300}
                            onChange={(e) => {
                                setMaTraCuu(e.target.value);
                            }}
                        >
                        </TextInput>
                        <Button text="Tìm kiếm" variant="primary" size="medium" sx={{ ml: 1 }}
                            isLoading={isLoading}
                            onClick={handleSearch}
                        />
                    </Box>
                </Box>
                <Box sx={{
                    display: "flex",
                    justifyContent: "center",
                    width: "100%",
                    mt: 3
                }}>
                    {hoaDonId > 0 &&
                        <HoaDonView id={hoaDonId} />
                    }
                    {hoaDonId == 0 &&
                        <Flash variant="default">
                            Không tìm thấy Hóa đơn có mã tra cứu trùng khớp
                        </Flash>
                    }
                </Box>
            </Box>
        </Box>
    );
};

export default TraCuuPage;