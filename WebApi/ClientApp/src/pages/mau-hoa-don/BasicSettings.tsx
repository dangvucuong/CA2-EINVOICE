import { ImageIcon } from '@primer/octicons-react';
import { Box, Checkbox, FormControl, Octicon, Radio, RadioGroup, SubNav } from '@primer/react';
import Files from '../../component-data/files/Files';
import Upload from '../../component-data/upload';
import RangeInput from '../../component-ui/range-input';
import Text from '../../component-ui/text';
import TextInput from '../../component-ui/text-input';
import { IUploadRespone } from '../../models/responses/upload/IUploadRespone';
import WaterMarkTemplateSelection from '../../component-data/water-mark-selection';
export interface IIBasicSettingsData {
    isShowLogoOrWatermark: "logo" | "watermark" | "vien",
    logoFile?: IUploadRespone,
    waterMarkFile?: IUploadRespone,
    vienFile?: IUploadRespone,
    isShowWatterMarkInnerTable: boolean,
    opacity: number,
    logoPosition: "left" | "right"
}
interface IBasicSettingsProps {
    data: IIBasicSettingsData,
    onValueChanged: (data: IIBasicSettingsData) => void,
    register: any,
    errors: any,

}
const BasicSettings = (props: IBasicSettingsProps) => {
    const {
        isShowLogoOrWatermark,
        logoFile,
        waterMarkFile,
        vienFile,
        isShowWatterMarkInnerTable,
        opacity,

    } = props.data;
    const { register, errors } = props;
    return (
        <Box>

            <FormControl>
                <FormControl.Label>
                    <Text text='Tên mẫu' />
                </FormControl.Label>
                <TextInput
                    register={register}
                    name='name'
                    errors={errors}
                    required
                    validateMessage='Vui lòng điền Tên mẫu'
                />
            </FormControl>

            <FormControl sx={{
                mt: 2,
                mb: 2
            }}>
                <SubNav aria-label="Main">
                    <SubNav.Links>
                        <SubNav.Link selected={isShowLogoOrWatermark === "logo"} sx={{
                            cursor: "pointer"
                        }}
                            onClick={() => {
                                // setIsShowLogoOrWatermark("logo")
                                props.onValueChanged({
                                    ...props.data,
                                    isShowLogoOrWatermark: "logo"
                                })
                            }}
                        >
                            Logo
                        </SubNav.Link>
                        <SubNav.Link selected={isShowLogoOrWatermark === "watermark"}
                            sx={{
                                cursor: "pointer"
                            }}
                            onClick={() => {
                                props.onValueChanged({
                                    ...props.data,
                                    isShowLogoOrWatermark: "watermark"
                                })
                            }}
                        >Hình nền chìm</SubNav.Link>
                        <SubNav.Link selected={isShowLogoOrWatermark === "vien"}
                            sx={{
                                cursor: "pointer"
                            }}
                            onClick={() => {
                                props.onValueChanged({
                                    ...props.data,
                                    isShowLogoOrWatermark: "vien"
                                })
                            }}
                        >Viền</SubNav.Link>
                    </SubNav.Links>
                </SubNav>
            </FormControl>
            {isShowLogoOrWatermark === "logo" &&
                <>
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Logo' />
                        </FormControl.Label>
                        {!logoFile &&
                            <Upload
                                accept={"image/*"}
                                icon={<Octicon icon={ImageIcon} size={"medium"} />}
                                onUploadSuccess={(data) => {
                                    props.onValueChanged({
                                        ...props.data,
                                        logoFile: data
                                    })
                                }}
                            />
                        }
                        {logoFile &&
                            <>
                                <Files files={[logoFile]} isPreviewImg
                                    onFileRemove={() => {
                                        props.onValueChanged({
                                            ...props.data,
                                            logoFile: undefined
                                        })
                                    }}
                                />
                                <FormControl>
                                    <Box className='radio-horizontal'>
                                        <RadioGroup name="viTriLogo">
                                            <RadioGroup.Label>Vị trí logo</RadioGroup.Label>
                                            <FormControl>
                                                <Radio value="left" checked={props.data.logoPosition === "left"} onChange={(e) => {
                                                    if (e.target.checked) {
                                                        props.onValueChanged({
                                                            ...props.data,
                                                            logoPosition: "left"
                                                        })
                                                    }
                                                }} />
                                                <FormControl.Label>Bên trái</FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Radio value="right" checked={props.data.logoPosition === "right"} onChange={(e) => {
                                                    if (e.target.checked) {
                                                        props.onValueChanged({
                                                            ...props.data,
                                                            logoPosition: "right"
                                                        })
                                                    }
                                                }} />
                                                <FormControl.Label>Bên phải</FormControl.Label>
                                            </FormControl>

                                        </RadioGroup>
                                    </Box>
                                </FormControl>
                            </>
                        }
                    </FormControl>
                </>
            }
            {isShowLogoOrWatermark === "watermark" &&
                <>
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Watermark (Hình nền chìm)' />
                        </FormControl.Label>
                        {!waterMarkFile &&
                            <Box>
                                <Box sx={{ mb: 2 }}>
                                    <WaterMarkTemplateSelection
                                        watermark_template_type_id={2}
                                        onSelectionChanged={(id, data) => {
                                            if (data) {
                                                props.onValueChanged({
                                                    ...props.data,
                                                    waterMarkFile: {
                                                        file_name: data?.name,
                                                        url: data?.url
                                                    }
                                                })
                                            }

                                        }} />
                                </Box>
                                <Upload
                                    icon={<Octicon icon={ImageIcon} size={"medium"} />}
                                    onUploadSuccess={(data) => {
                                        props.onValueChanged({
                                            ...props.data,
                                            waterMarkFile: data
                                        })
                                    }}

                                />
                            </Box>
                        }
                        {waterMarkFile &&
                            <>
                                <Files files={[waterMarkFile]} isPreviewImg
                                    onFileRemove={() => {
                                        props.onValueChanged({
                                            ...props.data,
                                            waterMarkFile: undefined
                                        })
                                    }}
                                />
                                <FormControl>
                                    <FormControl.Label>Hiển thị trong phần bảng biểu</FormControl.Label>
                                    <Checkbox checked={isShowWatterMarkInnerTable} onChange={(e) => {
                                        props.onValueChanged({
                                            ...props.data,
                                            isShowWatterMarkInnerTable: e.target.checked
                                        })
                                    }} />
                                </FormControl>
                                <FormControl sx={{
                                    width: "100%",
                                }}
                                >
                                    <Box sx={{
                                        display: "flex",
                                        width: "100%",
                                        fontWeight: "600",
                                        fontSize: "14px"
                                    }}>
                                        <Box>
                                            Độ nét
                                        </Box>
                                        <Box sx={{
                                            flex: 1
                                        }}>&nbsp;</Box>
                                        <Box>{opacity}%</Box>
                                    </Box>
                                    <Box style={{
                                        width: "100%"
                                    }}>
                                        <RangeInput value={opacity}
                                            onValueChanged={(value) => {
                                                props.onValueChanged({
                                                    ...props.data,
                                                    opacity: value
                                                })
                                            }}
                                        />
                                    </Box>
                                </FormControl>
                            </>
                        }

                    </FormControl>
                </>
            }
            {isShowLogoOrWatermark === "vien" &&
                <>
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Viền' />
                        </FormControl.Label>
                        {!vienFile &&
                            <Box>
                                <Box sx={{ mb: 2 }}>
                                    <WaterMarkTemplateSelection
                                        watermark_template_type_id={1}
                                        onSelectionChanged={(id, data) => {
                                            if (data) {
                                                props.onValueChanged({
                                                    ...props.data,
                                                    vienFile: {
                                                        file_name: data?.name,
                                                        url: data?.url
                                                    }
                                                })
                                            }

                                        }} />
                                </Box>
                                <Upload
                                    icon={<Octicon icon={ImageIcon} size={"medium"} />}
                                    onUploadSuccess={(data) => {
                                        props.onValueChanged({
                                            ...props.data,
                                            vienFile: data
                                        })
                                    }}

                                />
                            </Box>
                        }
                        {vienFile &&
                            <>
                                <Files files={[vienFile]} isPreviewImg
                                    onFileRemove={() => {
                                        props.onValueChanged({
                                            ...props.data,
                                            vienFile: undefined
                                        })
                                    }}
                                />

                            </>
                        }

                    </FormControl>
                </>
            }


        </Box>
    );
};

export default BasicSettings;