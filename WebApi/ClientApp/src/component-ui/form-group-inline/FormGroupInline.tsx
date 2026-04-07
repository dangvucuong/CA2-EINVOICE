import { Box } from '@primer/react';
import React from 'react';
import Text from '../text';
interface IFormGroupInlineProps {
    label: string,
    children: React.ReactNode
}
const FormGroupInline = (props: IFormGroupInlineProps) => {
    return (
        <Box sx={{
            display: "flex",
            alignItems: "center"
        }}>
            <Box sx={{
                // flex: 1,
                pr: 2
            }}>
                <Text text={props.label} sx={{
                    fontWeight: "bold",
                    color: "fg.muted",
                    whiteSpace: "break-spaces"
                }} />
            </Box>
            <Box sx={{ flex: 1 }}>
                {props.children}
            </Box>
        </Box>
    );
};

export default FormGroupInline;