import { Box } from '@primer/react';
import Button from '../button';
import Modal from '../modal';
import ModalActions from '../modal/ModalActions';
import Text from '../text';
import { DialogHeight, DialogWidth } from '@primer/react/lib-esm/drafts';
interface ConfirmModalProps {
    onConfirm: () => void,
    onCancel: () => void,
    type?: 'default' | 'primary' | 'danger',
    iconComponent?: React.ReactNode,
    text?: string,
    closeButtonText?: string,
    confirmButtonText?: string,
    children?: React.ReactNode,
    title?: string,
    subtitle?: string,
    width?: DialogWidth,
    height?: DialogHeight,
    isSaving?:boolean

}
const ConfirmModal = (props: ConfirmModalProps) => {
    return (
        <Modal
            isOpen={true}
            onClose={props.onCancel}
            title={props.title}
            subtitle={props.subtitle}
            width={props.width ?? "small"}
            height={props.height ?? "auto"}
        >
            <Box sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                flexDirection: "column"
            }}>
                {props.iconComponent && <Box>
                    {props.iconComponent}
                </Box>
                }
                {props.text && <Box>
                    <Text text={props.text} sx={{
                        fontSize: 18,
                        fontWeight: "500"
                    }} />
                </Box>}

            </Box>
            {props.children}
            <ModalActions>
                <Button text='Đóng'
                    onClick={props.onCancel}
                />
                <Button text='Xác nhận' variant={props.type ?? "danger"}
                    onClick={props.onConfirm}
                    isLoading={props.isSaving}
                />
            </ModalActions>
        </Modal>
    );
};

export default ConfirmModal;