import { Box } from '@primer/react';
import { useState } from 'react';
import { HexColorPicker } from "react-colorful";
import Modal from '../modal';
import TextInput from '../text-input';
interface IColorPickerProps {
    color: string,
    onValueChanged: (color: string) => void
}
const ColorPicker = (props: IColorPickerProps) => {
    // const [color, setColor] = useState("#1E1E1E");
    const { color } = props;
    const [isShowPicker, setIsShowPicker] = useState(false);

    return (
        <Box >
            <Box>
                <TextInput value={color}
                    sx={{
                        mr: 0,
                        pr: 0,
                        width: "120px"
                    }}
                    onChange={(e) => {
                        props.onValueChanged(e.target.value)
                    }}
                    trailingVisual={<>
                        <Box sx={{
                            backgroundColor: color,
                            width: "32px",
                            height: "32px",
                            borderRadius: 2,
                            cursor: "pointer",
                            border: "1px",
                            borderStyle: "solid"
                        }}
                            onClick={() => {
                                setIsShowPicker(true)
                            }}
                        >
                            &nbsp;
                        </Box>
                    </>}
                />
            </Box>
            {isShowPicker &&

                <Modal
                    title="Chọn màu"
                    isOpen={true}
                    width={"small"}
                    onClose={() => {
                        setIsShowPicker(false)
                    }}
                >
                    <Box sx={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "center",
                        alignItems: "center"
                    }}>
                        <HexColorPicker color={color} onChange={(newColor) => {
                            // setColor(color)
                            props.onValueChanged(newColor)
                        }} />

                    </Box>
                </Modal>
            }
        </Box>
    );
};

export default ColorPicker;