import { Box, Flash } from "@primer/react";
import Button from "../../component-ui/button";
import { useState } from "react";
import { donViApi } from "../../api/category/donViApi";
import { IDonVi } from "../../models/responses/category/IDonVi";
import { NotifyHelper } from "../../helpers/toast";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
interface IButtonGipInfoProps {
    mst: string,
    onApply: (donVi: IDonVi) => void
}

const ButtonGipInfo = (props: IButtonGipInfoProps) => {
    const [isShowModal, setIsShowModal] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [thongInDonVi, setThongInDonVi] = useState<IDonVi>();


    const handleGetDataAsync = async () => {
        setIsLoading(true)
        const res = await donViApi.getGipInfo(props.mst);
        setIsLoading(false)
        if (res.is_success) {
            setThongInDonVi(res.data)
            setIsShowModal(true)
        } else {
            NotifyHelper.Error(res.message ?? "Không lấy được thông tin")
        }
    }
    return (
        <Box>
            <Button text="Kiểm tra thông tin mã số thuế" size="medium" onClick={handleGetDataAsync} isLoading={isLoading} />
            {isShowModal &&
                <Modal
                    title="Thông tin"
                    isOpen={true}
                    onClose={() => {
                        setIsShowModal(false)
                    }}
                >

                    {thongInDonVi &&
                        <>
                            <Box>
                                <Box>Mã số thuế: <b>{thongInDonVi.mst}</b></Box>
                                <Box>Tên đơn vị: <b>{thongInDonVi.ten_dv}</b></Box>
                                <Box>Địa chỉ: <b>{thongInDonVi.dia_chi}</b></Box>
                            </Box>
                            <ModalActions>
                                <Button text="Áp dụng" variant="primary" size="medium" onClick={()=>{
                                    setIsShowModal(false)
                                    props.onApply(thongInDonVi)
                                }} />
                            </ModalActions>
                        </>
                    }
                    {!thongInDonVi &&
                        <Flash variant="warning">
                            Không có thông tin
                        </Flash>
                    }


                </Modal>
            }
        </Box>
    );
};

export default ButtonGipInfo;