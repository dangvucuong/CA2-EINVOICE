import React from 'react';
import "./RangeInput.css"
interface IRangeInputProps {
    value: number,
    onValueChanged: (value: number) => void
}
const RangeInput = (props: IRangeInputProps) => {

    return (
        <input type='range'
            value={props.value}
            style={{
                width: "100%"
            }}
            onChange={(e) => {
                console.log({
                    range: e.target.value
                });
                props.onValueChanged(parseInt(e.target.value))

            }} />
    );
};

export default RangeInput;