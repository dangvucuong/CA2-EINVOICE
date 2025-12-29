import { SearchIcon, SortAscIcon, SortDescIcon } from "@primer/octicons-react";
import {
  ActionList,
  ActionMenu,
  Box,
  Checkbox,
  Radio,
  TextInput,
} from "@primer/react";
import { DataTable, Table } from "@primer/react/drafts";
import clsx from "clsx";
import React, { useEffect, useMemo, useState } from "react";
import { useDebounce } from "use-debounce";
import { useCommonContext } from "../../contexts/common";
import { useWindowSize } from "../../hooks/useWindowSize";
import { eSortMode } from "../../models/commons/eSortMode";
import Button from "../button";
import styles from "./DataTableRemotePaging.module.css";
interface ISelectionProps {
  keyExpr?: string;
  mode: "multiple" | "single";
  onSelectionChanged: (keys: number[]) => void;
  selectedRowKeys?: number[];
}
interface ISearchConfig {
  enable: boolean;
  search_key?: string;
  onValueChanged: (key: string) => void;
}
interface ISortConfig {
  enable: boolean;
  field?: string;
  mode: eSortMode;
  onValueChanged: (field: string, mode: eSortMode) => void;
}
interface IPaging {
  pageSize: number;
  pageIndex: number;
  pageCount: number;
  totalCount: number;
  pageSizeItems?: number[];
  onPageIndexChanged: (pageIndex: number) => void;
  onPageSizeChanged?: (size: number) => void;
}
interface IColumn {
  id?: string;
  header: string | React.ReactNode;
  field?: string;
  rowHeader?: boolean;
  renderCell?: any;
  sortBy?: "alphanumeric" | "datetime" | any;
  width?: string;
  maxWidth?: string;
  minWidth?: string;
  colums?: IColumn[];
}
interface IDataTableRemotePagingProps {
  title?: string;
  titleComponent?: React.ReactNode;
  subTitle?: string;
  subTitleComponent?: React.ReactNode;
  data: any[];
  columns: IColumn[];
  actionComponent?: React.ReactNode;
  isLoading?: boolean;
  paging?: IPaging;
  height?: any;
  searchConfig?: ISearchConfig;
  sortConfig?: ISortConfig;
  exportEnable?: boolean;
  selection?: ISelectionProps;
}
const DataTableRemotePaging = (props: IDataTableRemotePagingProps) => {
  const [search_key, setSearch_key] = useState(props.searchConfig?.search_key);
  const { isMobile } = useWindowSize();

  const [searchKeyDelayed] = useDebounce(search_key, 1000);
  const { createUUID } = useCommonContext();
  const {
    data,
    columns,
    title,
    titleComponent,
    subTitle,
    subTitleComponent,
    actionComponent,
    isLoading,
    paging,
    height,
    sortConfig,
    searchConfig,
    exportEnable,
  } = props;
  const titleId = "repositories";
  const subTitileId: string = "repositories-subtitle";
  // console.log({
  //     search_key: props.searchConfig?.search_key
  // });

  useEffect(() => {
    if (
      searchKeyDelayed !== undefined &&
      searchKeyDelayed !== searchConfig?.search_key
    ) {
      props.searchConfig?.onValueChanged(searchKeyDelayed ?? "");
    }
  }, [searchKeyDelayed, props.searchConfig?.search_key]);
  const sort_by = useMemo(() => {
    return sortConfig?.field ?? "";
  }, [sortConfig]);
  const sort_by_name = useMemo(() => {
    return columns
      .filter((x) => x.field === sort_by)
      .map((x) => x.header)
      .join(",");
  }, [columns, sort_by]);
  const getColumns = () => {
    let result: any[] = [];
    if (props.selection) {
      const selectedRowKeys = props.selection?.selectedRowKeys ?? [];
      const keyExpr = props.selection?.keyExpr ?? "id";

      result.push({
        header: () => {
          return (
            <>
              {props.selection?.mode === "multiple" && (
                <Checkbox
                  checked={
                    data.find((x) => !selectedRowKeys.includes(x[keyExpr])) ===
                    undefined
                  }
                  onChange={(e) => {
                    if (e.target.checked) {
                      props.selection?.onSelectionChanged(
                        data.map((x) => x[keyExpr])
                      );
                    } else {
                      props.selection?.onSelectionChanged([]);
                    }
                  }}
                />
              )}
            </>
          );
        },
        id: "selection",
        width: "50px",
        renderCell: (rowData: any) => {
          return (
            <>
              {props.selection?.mode === "single" && (
                <Radio
                  value={rowData[keyExpr]}
                  name="default-radio-name"
                  checked={selectedRowKeys.includes(rowData[keyExpr])}
                  onChange={(e) => {
                    if (e.target.checked) {
                      console.log({
                        value: rowData[keyExpr],
                      });
                      props.selection?.onSelectionChanged([rowData[keyExpr]]);
                    }
                  }}
                />
              )}
              {props.selection?.mode === "multiple" && (
                <Checkbox
                  checked={selectedRowKeys.includes(rowData[keyExpr])}
                  onChange={(e) => {
                    if (e.target.checked) {
                      props.selection?.onSelectionChanged([
                        ...selectedRowKeys,
                        rowData[keyExpr],
                      ]);
                    } else {
                      props.selection?.onSelectionChanged(
                        selectedRowKeys.filter((x) => x !== rowData[keyExpr])
                      );
                    }
                  }}
                />
              )}
            </>
          );
        },
      });
    }
    result = [...result, ...props.columns];
    return result;
  };
  return (
    <Box className={clsx(styles.table, isLoading ? styles.isLoading : "")}>
      <Table.Container
        sx={
          {
            // height: height,
            // minHeight: height,
            // maxHeight: height
          }
        }
      >
        {props.title && (
          <Table.Title as="h2" id={titleId}>
            {title}
          </Table.Title>
        )}
        {props.titleComponent && (
          <Table.Title as="h2" id={titleId}>
            {titleComponent}
          </Table.Title>
        )}
        {props.subTitle && (
          <Table.Subtitle as="p" id={subTitileId}>
            {subTitle}
          </Table.Subtitle>
        )}
        {props.subTitleComponent && (
          <Table.Subtitle as="p" id={subTitileId}>
            {subTitleComponent}
          </Table.Subtitle>
        )}

        <Table.Actions>
          {actionComponent}
          {searchConfig && searchConfig.enable && (
            <TextInput
              leadingVisual={SearchIcon}
              placeholder="Tìm kiếm"
              value={search_key}
              onChange={(e) => {
                setSearch_key(e.target.value);
                // console.log({
                //     e
                // });
              }}
            ></TextInput>
          )}
          {sortConfig && sortConfig.enable && (
            <ActionMenu>
              <ActionMenu.Button
                leadingVisual={
                  sortConfig?.mode === eSortMode.DESC
                    ? SortDescIcon
                    : SortAscIcon
                }
              >
                Sort by <b>{sort_by_name}</b>
              </ActionMenu.Button>
              <ActionMenu.Overlay>
                <ActionList selectionVariant="single" role="menu" aria-label="">
                  {columns
                    .filter((x) => x.field)
                    .map((x) => {
                      return (
                        <ActionList.Item
                          key={x.id}
                          role="menuitemcheckbox"
                          selected={x.field === sortConfig.field}
                          onSelect={() => {
                            props.sortConfig?.onValueChanged(
                              x.field ?? "",
                              sortConfig.mode
                            );
                          }}
                        >
                          {x.header}
                        </ActionList.Item>
                      );
                    })}

                  <ActionList.Divider></ActionList.Divider>
                  <ActionList.Item
                    role="menuitemcheckbox"
                    selected={sortConfig.mode === eSortMode.ASC}
                    onSelect={() => {
                      props.sortConfig?.onValueChanged(
                        sortConfig.field ?? "",
                        eSortMode.ASC
                      );
                    }}
                  >
                    <ActionList.LeadingVisual>
                      <SortAscIcon />
                    </ActionList.LeadingVisual>
                    Ascending
                  </ActionList.Item>
                  <ActionList.Item
                    role="menuitemcheckbox"
                    selected={sortConfig.mode === eSortMode.DESC}
                    onSelect={() => {
                      props.sortConfig?.onValueChanged(
                        sortConfig.field ?? "",
                        eSortMode.DESC
                      );
                    }}
                  >
                    <ActionList.LeadingVisual>
                      <SortDescIcon />
                    </ActionList.LeadingVisual>
                    Descending
                  </ActionList.Item>
                </ActionList>
              </ActionMenu.Overlay>
            </ActionMenu>
          )}
          {/* {exportEnable &&
                        <IconButton icon={DownloadIcon} variant="default" aria-label="Download" />
                    } */}
        </Table.Actions>

        {isLoading && (
          <Table.Skeleton
            aria-labelledby={titleId}
            aria-describedby={subTitileId}
            columns={columns.map((x) => {
              let item: any = { ...x };
              return item;
            })}
            rows={100}
          />
        )}
        {!isLoading && (
          // <Box
          //   sx={{
          //     display: "grid",
          //     gridTemplateColumns: "1fr auto",
          //     //thẻ div con trực tiếp đầu tiên có height bằng độ cao màn hình -300px
          //     "> div:first-of-type": {
          //       height: "calc(100vh - 300px)",
          //       overflowY: "auto", // cho phép bảng cuộn nếu tràn
          //     },
          //   }}
          // >
          //   <DataTable
          //     aria-labelledby={titleId}
          //     aria-describedby={subTitileId}
          //     data={data}
          //     columns={getColumns()}
          //     // columns={columns.map(x => {
          //     //     let item: any = { ...x };
          //     //     return item;
          //     // })}
          //   />
          // </Box>
          <DataTable
            aria-labelledby={titleId}
            aria-describedby={subTitileId}
            data={data}
            columns={getColumns()}
            // columns={columns.map(x => {
            //     let item: any = { ...x };
            //     return item;
            // })}
          />
        )}
        {paging && (
          <Table.Pagination
            key={createUUID()}
            aria-label="Pagination1"
            pageSize={paging.pageSize}
            totalCount={paging.totalCount ?? 1}
            defaultPageIndex={paging.pageIndex}
            showPages={!isMobile}
            onChange={({ pageIndex }) => {
              paging.onPageIndexChanged(pageIndex);
            }}
          ></Table.Pagination>
        )}
      </Table.Container>
      {props.paging?.pageSizeItems && !isMobile && (
        <Box
          sx={{
            position: "absolute",
            bottom: "8px",
            left: "100px",
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
            }}
          >
            <Box
              sx={{
                fontSize: "11px",
                color: "fg.muted",
              }}
            >
              Page size&nbsp;&nbsp;
            </Box>
            {props.paging?.pageSizeItems.map((x) => {
              return (
                <Button
                  key={x}
                  className={clsx(
                    styles.pageSize,
                    x === props.paging?.pageSize ? styles.selected : ""
                  )}
                  variant="invisible"
                  text={x.toString()}
                  onClick={() => {
                    if (
                      props.paging?.onPageSizeChanged &&
                      x !== props.paging?.pageSize
                    ) {
                      props.paging?.onPageSizeChanged(x);
                    }
                  }}
                />
              );
            })}
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default DataTableRemotePaging;
