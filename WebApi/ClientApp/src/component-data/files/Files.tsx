import { TrashIcon } from "@primer/octicons-react";
import { ActionList, Box, IconButton, Link } from "@primer/react";
import Text from "../../component-ui/text";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import FileType, { IsImgFile } from "./FileType";
import LinkS3 from "../link-s3";
interface IFilesProps {
  files: IUploadRespone[];
  isReadOnly?: boolean;
  onFileRemove?: (url: string) => void;
  isPreviewImg?: boolean;
}
const Files = (props: IFilesProps) => {
  const { files } = props;
  return (
    <Box
      sx={{
        width: "100%",
      }}
    >
      {files.length === 0 && <Text text="Files.Empty" />}
      {files.length > 0 && (
        <Box>
          <ActionList showDividers>
            {files.map((file, idx) => {
              return (
                <ActionList.LinkItem
                  key={idx}
                  sx={{
                    display: "flex",
                  }}
                >
                  {(props.isPreviewImg === undefined ||
                    props.isPreviewImg === false ||
                    (props.isPreviewImg && !IsImgFile(file.url))) && (
                    <LinkS3
                      url={file.url}
                      target="_blank"
                      sx={{
                        display: "flex",
                      }}
                    >
                      <ActionList.LeadingVisual>
                        <FileType file_name={file.file_name} />
                      </ActionList.LeadingVisual>
                      {file.file_name}
                    </LinkS3>
                  )}
                  {props.isPreviewImg && IsImgFile(file.url) && (
                    <img
                      src={file.url}
                      alt={file.url}
                      style={{
                        width: "100px",
                        height: "auto",
                      }}
                    />
                  )}
                  {!props.isReadOnly && (
                    <ActionList.TrailingVisual>
                      <IconButton
                        sx={{
                          mt: "-5px",
                        }}
                        aria-label={`Delete: ${idx}`}
                        title={`Delete: ${idx}`}
                        icon={TrashIcon}
                        variant="invisible"
                        onClick={() => {
                          if (props.onFileRemove) {
                            props.onFileRemove(file.url);
                          }
                        }}
                      />
                    </ActionList.TrailingVisual>
                  )}
                </ActionList.LinkItem>
              );
            })}
            {/* <ActionList.Item>Copy link</ActionList.Item>
                        <ActionList.Item>Quote reply</ActionList.Item>
                        <ActionList.Item>Edit comment</ActionList.Item> */}
          </ActionList>
        </Box>
      )}
    </Box>
  );
};

export default Files;
