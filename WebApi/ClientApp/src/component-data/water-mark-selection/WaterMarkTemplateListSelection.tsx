import { Box } from '@primer/react';
import { useEffect } from 'react';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { IWatermarkTemplate } from '../../models/responses/category/IWatermarkTemplate';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
interface IWaterMarkTemplateListSelection {
    watermark_template_type_id: number
    selected?: IWatermarkTemplate,
    onSelecedChanged: (data: IWatermarkTemplate) => void
}
const WaterMarkTemplateListSelection = (props: IWaterMarkTemplateListSelection) => {


    const { watermarkTemplates, status } = useAppSelector(x => x.category.watermarkTemplateReducer)
    const dispatch = useAppDispatch();
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.category.watermarkTemplateAction.loadStart())
        }
    }, [status])
    return (
        <Box sx={{
            display: "flex"
        }}>
            <Box id='list' sx={{
                display: "flex",
                flexWrap: "wrap",
                flex: 1
            }}>
                {watermarkTemplates.filter(x=>x.watermark_template_type_id ===props.watermark_template_type_id).map(x => {
                    return (
                        <Box sx={{
                            mr: 2, mb: 3,
                            cursor: "pointer",
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                            width: "150px",
                            borderWidth: "1px",
                            borderStyle: props.selected?.id == x.id ? "solid" : "none",
                            borderRadius: 2,
                            pt: 2,
                            pb: 2,
                            borderColor: props.selected?.id == x.id ? "fg.muted" : ""
                        }} onClick={() => {
                            // setWatermarkTemplateSelected(x)
                            props.onSelecedChanged(x)
                        }}


                        >
                            <img src={x.small_size_url} alt={x.name}
                                style={{
                                    width: "100px"
                                }}
                            />
                            <Box>
                                <b className='limit1Line'> {x.name}</b>
                            </Box>
                        </Box>
                    );
                })}
            </Box>
            <Box id="preview" sx={{ width: "700px" }}>
                {props.selected &&
                    <img src={props.selected.url} alt={props.selected.name}
                        style={{
                            width: "675px"
                        }}
                    />
                }
            </Box>
        </Box>
    );
};

export default WaterMarkTemplateListSelection;