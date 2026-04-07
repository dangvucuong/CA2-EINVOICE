import { Button, ButtonProps, Spinner, Tooltip } from '@primer/react';
import { useMemo } from 'react';
import { useCommonContext } from '../../contexts/common';
interface IMyButtonProps extends ButtonProps {
    text?: string,
    isLoading?: boolean,
    apiAuthorized?: string,
    apiAuthorizedMethod?: "GET" | "PUT" | "POST" | "DELETE",
    tooltip?: string,
    tooltipdDirection?:'n' | 'ne' | 'e' | 'se' | 's' | 'sw' | 'w' | 'nw',
}
const Loading = () => {
    return <Spinner size='small' />
}
const MyButton = (props: IMyButtonProps) => {
    const { checkAccesiableTo, translate } = useCommonContext();
    const isAuthorized = useMemo(() => {
        if (props.apiAuthorized) {
            return checkAccesiableTo(props.apiAuthorized, props.apiAuthorizedMethod ?? "GET")
        }
        return true;
    }, [props.apiAuthorized])
    return (
        <>
            {isAuthorized && <>

                {!props.tooltip &&
                    <Button
                        {...props}
                        size={props.size ?? "small"}
                        leadingVisual={(props.isLoading) ? Loading : props.leadingVisual}
                        disabled={props.isLoading || props.disabled || !isAuthorized}
                    >
                        {translate(props.text ?? "")}
                        {props.children}
                    </Button>
                }
                {props.tooltip &&
                    <Tooltip aria-label={props.tooltip} direction={props.tooltipdDirection??'n'}>

                        <Button
                            {...props}
                            size={props.size ?? "small"}
                            leadingVisual={(props.isLoading) ? Loading : props.leadingVisual}
                            disabled={props.isLoading || props.disabled || !isAuthorized}
                        >
                            {translate(props.text ?? "")}
                            {props.children}
                        </Button>
                    </Tooltip>

                }
            </>

            }
            {!isAuthorized &&
                <Tooltip aria-label="Unauthorized" direction='s'>
                    <Button
                        {...props}
                        size={props.size ?? "small"}
                        leadingVisual={(props.isLoading) ? Loading : props.leadingVisual}
                        disabled={props.isLoading || props.disabled || !isAuthorized}
                    >
                        {translate(props.text ?? "")}
                        {props.children}
                    </Button>
                </Tooltip>
            }
        </>

    );
};

export default MyButton;