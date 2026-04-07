import { Box, Link, LinkButton } from '@primer/react';
import { DownloadIcon } from '@primer/octicons-react';
import React, { useState } from 'react';
import Steps from '../steps';
import { IStepData } from '../steps/Steps';
import Upload from '../upload';
import Text from '../../component-ui/text';
import Button from '../../component-ui/button';
interface IImportFormProps {
    stepId: number
}
const _steps: IStepData[] = [
    {
        id: 1,
        name: "Upload file",
        is_active: true
    },
    {
        id: 2,
        name: "Kiểm tra dữ liệu",
        is_active: false
    },
    {
        id: 3,
        name: "Import dữ liệu",
        is_active: false
    }
]
const ImportForm = (props: IImportFormProps) => {
    const [steps, setSteps] = useState(_steps.map(x => ({
        ...x,
        is_active: x.id === props.stepId
    })));


    return (
        <Box>
            <Box>
                <Steps steps={_steps} />
            </Box>
            <Box sx={{ mt: 3 }}>
                {props.stepId === 1 &&
                    <Box>
                        <Upload onUploadSuccess={() => {

                        }} />
                        <Box sx={{
                            mt: 2,
                            display: "flex",
                            flexDirection: "column",
                            justifyContent: "center",
                            alignItems: "center",
                        }}>
                            <Button text='Tải file mẫu'
                                size='medium'
                                variant='invisible'
                                leadingVisual={DownloadIcon}
                            />
                            <Box>
                                <Text text='Vui lòng format dữ liệu theo file mẫu để import dữ liệu chính xác'
                                    sx={{
                                        color: "fg.muted"
                                    }}
                                />
                            </Box>
                        </Box>
                    </Box>
                }
            </Box>
        </Box>
    );
};

export default ImportForm;