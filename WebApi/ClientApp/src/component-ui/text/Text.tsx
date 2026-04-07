import React from 'react';
import { Text } from "@primer/react"
import { TextProps } from '@primer/react/lib-esm';
import { useLocalized } from '../../hooks/useLocalized';
interface IMyTextProps extends TextProps {
    text: string
}
const MyText = (props: IMyTextProps) => {
    return (
        <Text sx={{
            ...props.sx,
            // fontSize: props.fontSize ? props.fontSize.toString() : 13
        }}>
            {useLocalized(props.text)}
        </Text>
    );
};

export default MyText;