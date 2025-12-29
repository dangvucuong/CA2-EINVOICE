import { Dialog, DialogProps } from '@primer/react/drafts';
import React from 'react';
import { useCommonContext } from '../../contexts/common';
interface IMyModalProps extends DialogProps {
    children?: React.ReactNode,
    titleComponent?: React.ReactNode
    onClose: () => void,
    width?: "small" | "medium" | "large" | "xlarge" | any;
    // height?: any,
    isOpen?: boolean,



}
const MyModal = (props: IMyModalProps) => {
    const { translate } = useCommonContext();
    return (
        <>
            {props.isOpen &&
                <Dialog
                    title={props.title ? translate(props.title?.toString()) : undefined}
                    subtitle={props.subtitle ? translate(props.subtitle?.toString()) : undefined}
                    {...props}
                    // isOpen={props.isOpen}
                    // width={props.width}
                    // height={props.height}
                    sx={props.sx}
                    onClose={props.onClose}

                // aria-labelledby="hello"
                >
                    {props.titleComponent &&
                        <Dialog.Header>
                            {/* {useLocalized(props.title ?? "")} */}
                            {props.titleComponent}
                        </Dialog.Header>
                    }
                    {props.titleComponent &&
                        <Dialog.Header>
                            {props.titleComponent}
                        </Dialog.Header>
                    }
                    {/* <Box sx={{
                        p: 3
                    }}> */}
                    {props.children}

                    {/* </Box> */}

                </Dialog>
            }
        </>
    );
};

export default MyModal;