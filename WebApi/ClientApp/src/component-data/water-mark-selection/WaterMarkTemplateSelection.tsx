import { Box } from '@primer/react';
import Modal from '../../component-ui/modal';
import { IWatermarkTemplate } from '../../models/responses/category/IWatermarkTemplate';
import { useState } from 'react';
import Button from '../../component-ui/button';
import ModalActions from '../../component-ui/modal/ModalActions';
import WaterMarkTemplateListSelection from './WaterMarkTemplateListSelection';
interface IWaterMarkTemplateSelectionProps {
    watermark_template_type_id: number,
    value?: string,
    onSelectionChanged: (id: number, data?: IWatermarkTemplate) => void
}
const WaterMarkTemplateSelection = (props: IWaterMarkTemplateSelectionProps) => {
    const [isShowModel, setIsShowModel] = useState(false);
    const [watermarkTemplateSelected, setWatermarkTemplateSelected] = useState<IWatermarkTemplate>();

    return (
        <Box>
            <Button text='Chọn từ mẫu có sẵn' onClick={() => {
                setIsShowModel(true)
            }} />
            {isShowModel &&
                <Modal
                    onClose={() => {
                        setIsShowModel(false)
                    }}
                    isOpen={true}
                    width={"90%"}
                    title="Chọn ảnh từ thư viện"
                >
                    <Box sx={{
                        height: window.innerHeight - 200,
                        overflow: "auto"
                    }}>
                        <WaterMarkTemplateListSelection
                            watermark_template_type_id={props.watermark_template_type_id}
                            selected={watermarkTemplateSelected}
                            onSelecedChanged={(data) => {
                                setWatermarkTemplateSelected(data)
                            }}
                        />
                    </Box>
                    <ModalActions>
                        <Button text='Đóng' onClick={() => { setIsShowModel(false) }} />
                        <Button text='Áp dụng'
                            variant='primary'
                            onClick={() => {
                                setIsShowModel(false)
                                if (watermarkTemplateSelected) {
                                    props.onSelectionChanged(watermarkTemplateSelected?.id, watermarkTemplateSelected)
                                }

                            }} disabled={!watermarkTemplateSelected}
                        />
                    </ModalActions>
                </Modal>
            }
        </Box>
    );
};

export default WaterMarkTemplateSelection;