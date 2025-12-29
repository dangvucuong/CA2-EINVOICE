import { Box, SxProp, TextInput } from '@primer/react';
import { useEffect, useRef, useState } from 'react';
import styles from "./TextInputGroup.module.css";
interface ITextInputGroupProps extends SxProp {
    length: number,
    value: string,
    onValueChanged: (value: string) => void
}
const TextInputGroup = (props: ITextInputGroupProps) => {
    const textInputRefs = useRef<any[]>([]);
    const [values, setValues] = useState<string[]>([]);


    useEffect(() => {
        let chars = props.value.split('');
        let defaultValues: string[] = [];
        for (let index = 0; index < props.length; index++) {
            if (chars.length - 1 >= index) {
                defaultValues.push(chars[index]);
            } else {
                defaultValues.push("");
            }
        }
        setValues(defaultValues)
    }
        , [props.value, props.length])

    // const isInputFull = useMemo(() => {
    //     return values.filter(x => x !== "").length === props.length
    // }, [values])
    useEffect(() => {
        props.onValueChanged(values.join(''))
        // if (isInputFull && props.value !== values.join('')) {
        //     props.onValueChanged(values.join(''))
        // } else {

        // }
    }, [props.value, values])

    // Hàm này sẽ được gọi khi có sự thay đổi trong TextInput
    const handleChange = (index: number) => (e: any) => {
        const { value } = e.target;
        setValues(values.map((x, idx) => {
            if (idx !== index) {
                return x;
            } else {
                return value;
            }
        }
        ))

        // Nếu độ dài của giá trị nhập vào đủ 1 ký tự
        if (value.length === 1) {
            // Nếu TextInput không phải là TextInput cuối cùng, chuyển focus sang TextInput tiếp theo
            if (index < values.length - 1) {
                textInputRefs.current[index + 1].focus();
            }
        } else {
            if (index > 0) {
                textInputRefs.current[index - 1].focus();
            }
        }
    };

    return (
        <Box className={styles.group} sx={{ ...props.sx }}>
            {values.map((s, index) => (
                <TextInput key={index} ref={el => textInputRefs.current[index] = el} onChange={handleChange(index)}
                />
            ))}
        </Box>
    );
};

export default TextInputGroup;