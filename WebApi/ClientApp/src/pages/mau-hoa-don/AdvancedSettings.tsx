import { PlusIcon } from '@primer/octicons-react';
import { Box } from '@primer/react';
import Button from '../../component-ui/button';
import CssEditorElement from '../../component-ui/css-editor-element';
import { ICssEditorElementData } from '../../component-ui/css-editor-element/CssEditorElement';
import Heading from '../../component-ui/heading';
import { eSize } from '../../models/commons/eSize';
export interface IAdvancedSettingsProps {
    cssElements: ICssEditorElementData[],
    onValueChanged: (data: ICssEditorElementData[]) => void
}

const AdvancedSettings = (props: IAdvancedSettingsProps) => {
    const { cssElements } = props;
    const handleChanged = (elementId: string, newData: ICssEditorElementData) => {
        const newValues = cssElements.map(x => {
            if (x.elementId === elementId) {
                const obj: ICssEditorElementData = {
                    ...x,
                    ...newData
                }
                return obj;
            }
            return {
                ...x
            }
        })
        props.onValueChanged(newValues);
    }
    return (
        <Box>
            <Heading text='Người bán' size={eSize.medium} sx={{ mb: 1 }} />
            {cssElements.filter(x => x.type === "nguoi_ban").map(x => {
                return (
                    <Box key={x.elementId}>
                        <CssEditorElement
                            data={{
                                elementId: x.elementId,
                                elementText: x.elementText,
                                type: "nguoi_ban",
                                cssValue: cssElements.find(e => e.elementId === x.elementId)?.cssValue,
                                isDisplay: cssElements.find(e => e.elementId === x.elementId)?.isDisplay ?? true
                            }}
                            onValueChanged={(data) => {
                                if (data) {
                                    handleChanged(x.elementId, data);
                                }
                            }}
                        />
                    </Box>
                )
            })}
            <Heading text='Người mua' size={eSize.medium} sx={{ mb: 1 }} />
            {cssElements.filter(x => x.type === "nguoi_mua").map(x => {
                return (
                    <Box key={x.elementId}>
                        <CssEditorElement
                            data={{
                                elementId: x.elementId,
                                elementText: x.elementText,
                                type: "nguoi_mua",
                                cssValue: cssElements.find(e => e.elementId === x.elementId)?.cssValue,
                                isDisplay: cssElements.find(e => e.elementId === x.elementId)?.isDisplay ?? true
                            }}
                            onValueChanged={(data) => {
                                if (data) {
                                    handleChanged(x.elementId, data);
                                }
                            }}
                        />
                    </Box>
                )
            })}
            {/* <CssEditorElement
                elementId='dia_chi'
                elementText='Địa chỉ'
                onValueChanged={(value) => {

                }}
            />
            <CssEditorElement
                elementId='mst'
                elementText='Mã số thuế'
                onValueChanged={(value) => {

                }}
            />
            <CssEditorElement
                elementId='dien_thoai'
                elementText='Điện thoại'
                onValueChanged={(value) => {

                }}
            />
            <CssEditorElement
                elementId='fax'
                elementText='Fax'
                onValueChanged={(value) => {

                }}
            />
            <CssEditorElement
                elementId='website'
                elementText='Website'
                onValueChanged={(value) => {

                }}
            />
            <CssEditorElement
                elementId='email'
                elementText='Email'
                onValueChanged={(value) => {

                }}
            /> */}
            {/* <Button text='Thêm nội dung' leadingVisual={PlusIcon} size='medium'
                // variant='invisible'
                block
            /> */}
        </Box>
    );
};

export default AdvancedSettings;