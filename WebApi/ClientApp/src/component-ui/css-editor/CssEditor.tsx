import { BoldIcon, ItalicIcon } from "@primer/octicons-react";
import { Box, ButtonGroup, FormControl, IconButton } from '@primer/react';

import TextInput from '../text-input';
import ColorPicker from "../color-picker";

export interface ICssEditorValue {
    fontSize: number,
    color: string;
    isBold: boolean,
    isItalic: boolean,
    align: "left" | "center" | "right"

}
interface ICssEditorProps {
    value: ICssEditorValue,
    onValueChanged: (value: ICssEditorValue) => void
}
const AlignLeftIcon = () => {
    return (
        <img src='../../images/align-left.png' alt='Left'
            style={{
                width: "15px"
            }}
        />
    );
}
const AlignCenterIcon = () => {
    return (
        <img src='../../images/align-center.png' alt='Left'
            style={{
                width: "15px"
            }}
        />
    );
}
const AlignRightIcon = () => {
    return (
        <img src='../../images/align-right.png' alt='Left'
            style={{
                width: "15px"
            }}
        />
    );
}
const CssEditor = (props: ICssEditorProps) => {
    const {
        fontSize,
        color,
        isBold,
        isItalic,
        align
    } = props.value
    console.log({
        xxxx: props.value
    });

    return (
        <Box sx={{
        }}>
            <FormControl>
                <Box sx={{
                    display: "flex"
                }}>
                    <TextInput
                        value={fontSize}
                        sx={{
                            width: "80px"
                        }}
                        type='number'
                        min={8}
                        max={30}
                        trailingVisual="px"
                        onChange={(e) => {
                            props.onValueChanged({
                                ...props.value,
                                fontSize: parseInt(e.target.value)
                            })
                        }}
                    />
                    <Box sx={{ ml: 2 }}>
                        <ColorPicker
                            color={color}
                            onValueChanged={(color) => {
                                // console.log({
                                //     color
                                // });

                                props.onValueChanged({
                                    ...props.value,
                                    color: color
                                })
                            }}
                        />
                    </Box>


                </Box>
                <Box sx={{
                    display: "flex"
                }}>
                    <Box sx={{ mr: 2 }}>
                        <ButtonGroup>
                            <IconButton icon={BoldIcon} aria-label="Bold"
                                className="noHoverCss"
                                sx={{
                                    backgroundColor: isBold ? "accent.fg" : ""
                                }}
                                onClick={() => {
                                    props.onValueChanged({ ...props.value, isBold: !isBold })
                                }}
                            />
                            <IconButton icon={ItalicIcon} aria-label="Italic"
                                className="noHoverCss"
                                sx={{
                                    backgroundColor: isItalic ? "accent.fg" : ""
                                }}
                                onClick={() => {
                                    props.onValueChanged({ ...props.value, isItalic: !isItalic })
                                }}
                            />
                        </ButtonGroup>
                    </Box>
                    <Box sx={{ mr: 2 }}>
                        <ButtonGroup>
                            <IconButton icon={AlignLeftIcon} aria-label="Left"
                                className="noHoverCss"
                                sx={{
                                    backgroundColor: align === "left" ? "accent.fg" : ""
                                }}
                                onClick={() => {
                                    props.onValueChanged({ ...props.value, align: "left" })
                                }}
                            />
                            <IconButton icon={AlignCenterIcon} aria-label="Italic"
                                className="noHoverCss"
                                sx={{
                                    backgroundColor: align === "center" ? "accent.fg" : ""
                                }}
                                onClick={() => {
                                    props.onValueChanged({ ...props.value, align: "center" })
                                }}
                            />
                            <IconButton icon={AlignRightIcon} aria-label="Italic"
                                className="noHoverCss"
                                sx={{
                                    backgroundColor: align === "right" ? "accent.fg" : ""
                                }}
                                onClick={() => {
                                    props.onValueChanged({ ...props.value, align: "right" })
                                }}
                            />
                        </ButtonGroup>
                    </Box>
                </Box>
            </FormControl>
        </Box>
    );
};

export default CssEditor;