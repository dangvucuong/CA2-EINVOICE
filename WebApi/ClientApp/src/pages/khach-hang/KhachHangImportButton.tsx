import { UploadIcon } from "@primer/octicons-react";
import { useState } from "react";
import Button from "../../component-ui/button/Button";
import KhachHangImportModal from "./KhachHangImportModal";
interface IKhachHangImportButtonProps {
    onSuccess: () => void
}
const KhachHangImportButton = (props: IKhachHangImportButtonProps) => {
    const [isShowImportModal, setIsShowImportModal] = useState(false);

    return (
        <>
            <Button text="Nhập khẩu" leadingVisual={UploadIcon} size="medium"
                sx={{ ml: 1 }}
                onClick={() => {
                    setIsShowImportModal(true)
                }}
            />
            {isShowImportModal &&
                <KhachHangImportModal
                    onClose={() => {
                        setIsShowImportModal(false)
                    }}
                    onSuccess={() => {
                        setIsShowImportModal(false);
                        props.onSuccess();
                    }}
                />
            }
        </>
    );
};

export default KhachHangImportButton;