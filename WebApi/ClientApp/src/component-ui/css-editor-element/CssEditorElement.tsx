import { Box, Checkbox, Octicon } from '@primer/react';
import React, { useState } from 'react';
import { ChevronDownIcon, ChevronUpIcon } from "@primer/octicons-react"
import CssEditor, { ICssEditorValue } from '../css-editor/CssEditor';
export interface ICssEditorElementData {
    elementId: string,
    elementText: string,
    isDisplay: boolean,
    type: string,
    cssValue?: ICssEditorValue,
}
export interface ICssEditorElementProps {
    data: ICssEditorElementData
    onValueChanged: (value?: ICssEditorElementData) => void
}
const CssEditorElement = (props: ICssEditorElementProps) => {
    const [isShowDetail, setIsShowDetail] = useState(false);

    return (
        <Box
            sx={{
                borderWidth: "1px",
                borderStyle: "solid",
                borderColor: "border.default",
                borderRadius: 2,
                p: 2,
                mb: 2
            }}
        >
            <Box sx={{
                display: "flex"
            }}>
                <Box id='display'>
                    <Checkbox checked={props.data.isDisplay}
                        onChange={(e) => {
                            props.onValueChanged({
                                ...props.data,
                                isDisplay: e.target.checked
                            })
                        }}
                    />
                </Box>
                <Box sx={{
                    flex: 1,
                    ml: 2,
                    mr: 2,
                    fontWeight: "600",
                    fontSize: "14px"
                }}>
                    {props.data.elementText}
                </Box>
                <Box sx={{
                    cursor: "pointer"
                }}
                    onClick={() => {
                        setIsShowDetail(!isShowDetail)
                    }}
                >
                    <Octicon icon={isShowDetail ? ChevronUpIcon : ChevronDownIcon} />
                </Box>
            </Box>
            {isShowDetail &&
                <Box sx={{
                    mt: 2,
                    borderTopWidth: "1px",
                    borderTopStyle: "dashed",
                    borderTopColor: "border.default",
                    pt: 2
                }}>
                    <CssEditor
                        value={
                            props.data.cssValue ??
                            {
                                color: "#1E1E1E",
                                fontSize: 14,
                                align: "left",
                                isBold: false,
                                isItalic: false
                            }
                        }
                        onValueChanged={(cssValue) => {
                            console.log({
                                cssValue: cssValue
                            });
                            props.onValueChanged({
                                ...props.data,
                                cssValue: cssValue
                            })

                        }}
                    />
                </Box>
            }
        </Box>
    );
};

export default CssEditorElement;