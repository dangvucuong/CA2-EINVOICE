import { Box } from '@primer/react';
import TextInputGroup from '../text-input-group';
import { useEffect, useMemo, useState } from 'react';
interface IMaSoThueInputProps {
    value: string,
    onValueChanged: (value: string) => void
}
const MaSoThueInput = (props: IMaSoThueInputProps) => {
    const [maSoThueMain, setMaSoThueMain] = useState("");
    const [maSoThueExt, setMaSoThueExt] = useState("");
    const maSoThue = useMemo(() => {
        return maSoThueMain + "-" + maSoThueExt
    }, [maSoThueMain, maSoThueExt])
    useEffect(() => {
        const values = props.value.split("-")
        if (values.length >= 1) {
            setMaSoThueMain(values[0])
        }
        if (values.length >= 2) {
            setMaSoThueMain(values[1])
        }
    }, [props.value])
    useEffect(() => {
        if (maSoThue !== props.value) {
            props.onValueChanged(maSoThue)
        }
    }, [maSoThue, props.value])

    return (
        <Box sx={{ display: "flex" }}>
            <TextInputGroup length={10} value={maSoThueMain} onValueChanged={(value) => {
                setMaSoThueMain(value)

            }} />
            <TextInputGroup length={3} sx={{ ml: 3 }} value={maSoThueExt} onValueChanged={(value) => {
                setMaSoThueExt(value)

            }} />

        </Box>
    );
};

export default MaSoThueInput;