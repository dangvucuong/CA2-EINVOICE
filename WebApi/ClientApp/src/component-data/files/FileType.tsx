import { Box } from '@primer/react';
import React, { useMemo } from 'react';
import styles from "./FileType.module.css"
interface IFileTypeProps {
    file_name?: string,
    file_type?: string
}
const imgFiles = ["png", "svg", "jpg", "gif", "jpg", "jpeg"]
const getFileType = (url: string) => {
    if (url) {
        return url.split('.').pop()?.toLowerCase() ?? "";
    }
    return "";
}
export const IsImgFile = (url: string) => {
    const fileType = getFileType(url);
    return imgFiles.includes(fileType);
}
const FileType = (props: IFileTypeProps) => {
    const { file_name, file_type } = props;
    const fileType = useMemo(() => {
        if (!file_type && file_name) {
            return file_name.split('.').pop()?.toLowerCase();
        }
        return (file_type ?? "").toLowerCase();
    }, [file_name, file_type])
    

    return (
        <Box>
            {fileType === "pdf" && <img src='../../../images/pdf.png' alt='pdf' className={styles.icon} />}
            {fileType === "msg" && <img src='../../../images/msg.png' alt='pdf' className={styles.icon} />}
            {(
                fileType === "png"
                || fileType === "svg"
                || fileType === "jpg"
                || fileType === "gif"
                || fileType === "jpg"
                || fileType === "jpeg"
            )
                && <img src='../../../images/image.png' alt='png' className={styles.icon} />}
            {(fileType === "doc" || fileType === "docx") && <img src='../../../images/word.png' alt='png' className={styles.icon} />}
            {(fileType === "xls" || fileType === "xlsx") && <img src='../../../images/excel.png' alt='png' className={styles.icon} />}
        </Box>
    );
};

export default FileType;