import {
  DownloadIcon,
  SearchIcon,
  SortAscIcon,
  SortDescIcon,
  SyncIcon,
} from "@primer/octicons-react";
import {
  ActionList,
  ActionMenu,
  Box,
  Checkbox,
  IconButton,
  TextInput,
} from "@primer/react";
import { DataTable as DataTablePrimer, Table } from "@primer/react/drafts";
import clsx from "clsx";
import React, { useMemo, useState } from "react";
import { useDebounce } from "use-debounce";
import { useCommonContext } from "../../contexts/common";
import { eSortMode } from "../../models/commons/eSortMode";
import styles from "./DataTableRemotePaging.module.css";
import Button from "../button";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";

interface ISelectionProps {
  keyExpr?: string;
  mode: "multiple" | "single";
  onSelectionChanged: (keys: number[]) => void;
  selectedRowKeys?: number[];
}
interface ISortConfig {
  enable: boolean;
  field?: string;
  mode: eSortMode;
  onValueChanged?: (field: string, mode: eSortMode) => void;
}
interface IPaging {
  pageSize: number;
  pageIndex: number;
  pageCount: number;
  onPageIndexChanged: (pageIndex: number) => void;
}
export interface IColumn {
  id?: string;
  header: string | (() => React.ReactNode);
  field?: string;
  rowHeader?: boolean;
  renderCell?: any;
  width?: string;
  maxWidth?: string;
  minWidth?: string;
  sortBy?: "alphanumeric" | "datetime" | any;
}
interface IDataTableProps {
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
  searchEnable?: boolean;
  sortConfig?: ISortConfig;
  exportEnable?: boolean;
  selection?: ISelectionProps;
  showRefreshButton?: boolean;
  handleRefresh?: () => void;
}
function removeAccents(str: string) {
  if (!str) return str;
  return str
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/đ/g, "d")
    .replace(/Đ/g, "D");
}
function dynamicSort(property: string, mode: eSortMode) {
  var sortOrder = 1;
  // if (property[0] === "-") {
  //     sortOrder = -1;
  //     property = property.substr(1);
  // }
  if (mode === eSortMode.ASC) {
    sortOrder = 1;
  } else {
    sortOrder = -1;
  }
  return function (a: any, b: any) {
    /* next line works with strings and numbers,
     * and you may want to customize it to your needs
     */
    var result =
      a[property] < b[property] ? -1 : a[property] > b[property] ? 1 : 0;
    return result * sortOrder;
  };
}
const DataTable = (props: IDataTableProps) => {
  const [search_key, setSearch_key] = useState("");
  const [searchKeyDelayed] = useDebounce(search_key, 1000);
  const { translate } = useCommonContext();
  const dispatch = useAppDispatch();

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
    searchEnable,
    exportEnable,
    showRefreshButton = false,
    handleRefresh = () => {},
  } = props;
  const titleId = "repositories";
  const subTitileId: string = "repositories-subtitle";
  const [sortConfig, setSortConfig] = useState(props.sortConfig);

  const sort_by = useMemo(() => {
    return sortConfig?.field ?? "";
  }, [sortConfig]);
  const sort_by_name = useMemo(() => {
    return columns
      .filter((x) => x.field == sort_by)
      .map((x) => x.header)
      .join(",");
  }, [columns, sort_by]);
  const filterdData = useMemo(() => {
    const fieledColumns = columns.filter((col) => col.field);
    const temp = data
      .map((x) => {
        let isIncluded: boolean = false;
        fieledColumns.forEach((col) => {
          if (!isIncluded) {
            // console.log({
            //     data: x[`${col.field}`] && x[`${col.field}`].toString(),
            //     searchKeyDelayed: removeAccents(searchKeyDelayed.toLowerCase()),
            //     isIncluded: removeAccents(x[`${col.field}`] && x[`${col.field}`].toString().toLowerCase()),

            // });
            if (x[`${col.field}`])
              if (
                removeAccents(
                  x[`${col.field}`].toString().toLowerCase()
                ).includes(removeAccents(searchKeyDelayed.toLowerCase()))
              ) {
                isIncluded = true;
              }
          }
        });
        if (isIncluded) {
          return x;
        }
        return undefined;
      })
      .filter((x) => x !== undefined);
    const tempSort = temp.sort(
      dynamicSort(sort_by, sortConfig?.mode ?? eSortMode.DESC)
    );

    return tempSort;
  }, [searchKeyDelayed, data, columns, sort_by, sortConfig?.mode]);
  const getColumns = () => {
    let result: any[] = [];
    if (props.selection) {
      const selectedRowKeys = props.selection?.selectedRowKeys ?? [];
      const keyExpr = props.selection?.keyExpr ?? "id";

      result.push({
        header: () => {
          return (
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
          );
        },
        id: "selection",
        width: "50px",
        renderCell: (rowData: any) => {
          return (
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
          );
        },
      });
    }
    result = [...result, ...props.columns];
    return result;
  };

  return (
    <div className={clsx(styles.table, isLoading ? styles.isLoading : "")}>
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
            {translate(subTitle ?? "")}
          </Table.Subtitle>
        )}
        {props.subTitleComponent && (
          <Table.Subtitle as="p" id={subTitileId}>
            {subTitleComponent}
          </Table.Subtitle>
        )}

        <Table.Actions>
          {actionComponent}
          {searchEnable && (
            <TextInput
              leadingVisual={SearchIcon}
              placeholder="Tìm kiếm"
              value={search_key}
              onChange={(e) => {
                setSearch_key(e.target.value);
              }}
            ></TextInput>
          )}

          {showRefreshButton && (
            <Button
              text="Refresh"
              leadingVisual={SyncIcon}
              onClick={() => {
                setSortConfig({
                  field: undefined,
                  mode: eSortMode.ASC,
                  enable: true,
                });
                handleRefresh();
              }}
            />
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
                Lọc theo <b>{sort_by_name}</b>
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
                            setSortConfig({
                              ...sortConfig,
                              field: x.field ?? "",
                            });
                          }}
                        >
                          {typeof x.header === "string" ? x.header : x.header()}
                        </ActionList.Item>
                      );
                    })}

                  <ActionList.Divider></ActionList.Divider>
                  <ActionList.Item
                    role="menuitemcheckbox"
                    selected={sortConfig.mode === eSortMode.ASC}
                    onSelect={() => {
                      setSortConfig({
                        ...sortConfig,
                        mode: eSortMode.ASC,
                      });
                    }}
                  >
                    <ActionList.LeadingVisual>
                      <SortAscIcon />
                    </ActionList.LeadingVisual>
                    Thời gian xa nhất
                  </ActionList.Item>
                  <ActionList.Item
                    role="menuitemcheckbox"
                    selected={sortConfig.mode === eSortMode.DESC}
                    onSelect={() => {
                      setSortConfig({
                        ...sortConfig,
                        mode: eSortMode.DESC,
                      });
                    }}
                  >
                    <ActionList.LeadingVisual>
                      <SortDescIcon />
                    </ActionList.LeadingVisual>
                    Thời gian gần nhất
                  </ActionList.Item>
                </ActionList>
              </ActionMenu.Overlay>
            </ActionMenu>
          )}
          {exportEnable && (
            <IconButton
              icon={DownloadIcon}
              variant="default"
              aria-label="Xuất excel"
            />
          )}
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
          <DataTablePrimer
            aria-labelledby={titleId}
            aria-describedby={subTitileId}
            data={filterdData}
            columns={getColumns()}
            // columns={columns.map(x => {
            //     let item: any = { ...x };
            //     return item;
            // })}
            // initialSortColumn={2}
            // initialSortDirection={"DESC"}
          />
        )}
        {paging && (
          <Table.Pagination
            // key={searchKeyDelayed}
            aria-label="Pagination"
            pageSize={paging.pageSize}
            totalCount={paging.pageCount}
            onChange={({ pageIndex }) => {
              paging.onPageIndexChanged(pageIndex);
            }}
            defaultPageIndex={paging.pageIndex}
          ></Table.Pagination>
        )}
      </Table.Container>
    </div>
  );
};

export default DataTable;
