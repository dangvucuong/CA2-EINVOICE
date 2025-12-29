import { Button, ButtonProps } from '@primer/react';
import { ArrowLeftIcon } from '@primer/octicons-react';
import React from 'react';
import { useHistory } from 'react-router-dom';
interface IBackButton extends ButtonProps {

}
const BackButton = (props: IBackButton) => {
    const history = useHistory();
    return (
        <Button leadingVisual={ArrowLeftIcon} variant='invisible'
            onClick={() => {
                history.goBack()
            }}
        >
            Quay lại
        </Button>
    );
};

export default BackButton;