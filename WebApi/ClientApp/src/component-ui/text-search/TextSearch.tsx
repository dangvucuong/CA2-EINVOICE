import { TextInput } from '@primer/react';
import { SearchIcon } from "@primer/octicons-react"
import React from 'react';
import { eSize } from '../../models/commons/eSize';
import { useLocalized } from '../../hooks/useLocalized';
import { useCommonContext } from '../../contexts/common';
interface ITextSearchProps {
    placeholder?: string,
    width?: any,
    size?: eSize
}
const TextSearch = (props: ITextSearchProps) => {
    const { translate } = useCommonContext();
    return (
        <TextInput leadingVisual={SearchIcon}
            placeholder={props.placeholder ? translate(props.placeholder) : translate('Base.Label.TextSearch.PlaceHolder')}
            sx={{
                width: props.width ?? "100%"
            }}
            // size={props.size?.toString() ?? "small"}
            onChange={(e) => {
                // setSearch_key(e.target.value);
                // console.log({
                //     e
                // });

            }}
        >
        </TextInput>
    );
};

export default TextSearch;