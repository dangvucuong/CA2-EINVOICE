import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton } from "@primer/react";
import { useEffect, useMemo } from "react";
import { Helmet } from "react-helmet";
import { DAI_LY_API_ENDPOIT, daiLyApi } from "../../api/category/daiLyApi";
import ExportToExcelBtn from "../../component-data/export-excel-btn/ExportToExcelBtn";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import DataTableRemotePaging from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eSortMode } from "../../models/commons/eSortMode";
import { IDaiLy } from "../../models/responses/category/IDaiLy";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import DaiLyEditFormModal from "./DaiLyEditFormModal";
import DaiLyImportButton from "./DaiLyImportButton";

const DaiLyPage = () => {
  const {
    status,
    daiLys,
    filter,
    paging_res,
    isShowDeleteConfirm,
    daiLyEditing,
    isShowEditModal,
  } = useAppSelector((x) => x.category.daiLyReducer);
  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(DAI_LY_API_ENDPOIT, "GET");
  }, []);
  const isCanNotEdit = useMemo(() => {
    return !checkAccesiableTo(DAI_LY_API_ENDPOIT, "PUT");
  }, []);
  const isCanNotDelete = useMemo(() => {
    return !checkAccesiableTo(DAI_LY_API_ENDPOIT + "/{id}", "DELETE");
  }, []);
  useEffect(() => {
    dispatch(
      rootAction.category.daiLyAction.loadStart({
        ...filter,
      })
    );
  }, [filter]);
  useEffect(() => {
    if (
      status === eReducerStatusBase.is_saved ||
      status === eReducerStatusBase.is_deleted
    ) {
      dispatch(
        rootAction.category.daiLyAction.loadStart({
          ...filter,
        })
      );
    }
  }, [status, filter]);
  return (
    <Box>
      <Helmet>
        <title>Đại lý</title>
      </Helmet>
      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <DataTableRemotePaging
          titleComponent={<Heading text="Danh sách đại lý" />}
          subTitle={`Tổng số: ${(
            paging_res?.total_count ?? 0
          ).toLocaleString()}`}
          data={daiLys}
          height={window.innerHeight - 100}
          isLoading={status === eReducerStatusBase.is_loading}
          exportEnable
          actionComponent={
            <>
              <Button
                text="Thêm đại lý"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={DAI_LY_API_ENDPOIT}
                onClick={() => {
                  dispatch(rootAction.category.daiLyAction.showEditModal());
                }}
              />
              <DaiLyImportButton
                onSuccess={() => {
                  dispatch(
                    rootAction.category.daiLyAction.loadStart({
                      ...filter,
                    })
                  );
                }}
              />
              <ExportToExcelBtn
                fileName="dai-ly"
                formatDataFunction={(data) => {
                  return data.map((x: IDaiLy) => {
                    return {
                      "Mã đại lý": x.ma_dai_ly,
                      "Tên đại lý": x.ten_dai_ly,
                      Email: x.email,
                      "Số tài khoản": x.so_tai_khoan,
                    };
                  });
                }}
                fetchDataPromise={() => {
                  return new Promise((resolve, reject) => {
                    return daiLyApi
                      .getByDonViPaging({
                        ...filter,
                        page_index: 0,
                        page_size: paging_res?.total_count,
                      })
                      .then((res) => {
                        if (res.is_success) {
                          resolve(res.data.data);
                        } else {
                          NotifyHelper.Error(res.message ?? "Error");
                          resolve(undefined);
                        }
                      });
                  });
                }}
              />
            </>
          }
          searchConfig={{
            enable: true,
            onValueChanged: (key: string) => {
              dispatch(
                rootAction.category.daiLyAction.changeFilter({
                  ...filter,
                  page_index: 0,
                  search_key: key,
                })
              );
            },
          }}
          sortConfig={{
            enable: true,
            field: filter.sort_by,
            mode: filter.sort_mode ?? eSortMode.ASC,
            onValueChanged: (key: string, sort_mode: eSortMode) => {
              dispatch(
                rootAction.category.daiLyAction.changeFilter({
                  ...filter,
                  sort_by: key,
                  sort_mode: sort_mode,
                })
              );
            },
          }}
          paging={{
            onPageIndexChanged: (pageIndex) => {
              dispatch(
                rootAction.category.daiLyAction.changeFilter({
                  ...filter,
                  page_index: pageIndex,
                })
              );
            },
            pageCount: paging_res?.page_count ?? 1,
            pageIndex: paging_res?.page_number ?? 1,
            pageSize: paging_res?.page_size ?? 1,
            totalCount: paging_res?.total_count ?? 1,
          }}
          columns={[
            {
              header: "Mã đại lý",
              field: "ma_dai_ly",
              rowHeader: false,
              width: "200px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Tên đại lý",
              field: "ten_dai_ly",
              rowHeader: true,
              // sortBy: "alphanumeric"
            },

            {
              header: "Email",
              field: "email",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số tài khoản",
              field: "so_tai_khoan",
              rowHeader: false,
              // width: "150px",
              // sortBy: "alphanumeric"
            },

            {
              id: "actions",
              header: "",
              width: "100px",
              renderCell: (row: any) => {
                return (
                  <>
                    <Box
                      sx={{
                        mt: -2,
                        mb: -2,
                      }}
                    >
                      {!isCanNotEdit && (
                        <IconButton
                          aria-label={`Edit: ${row.name}`}
                          title={`Edit: ${row.name}`}
                          icon={PencilIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(
                              rootAction.category.daiLyAction.showEditModal(row)
                            );
                          }}
                        />
                      )}
                      {!isCanNotDelete && (
                        <IconButton
                          aria-label={`Edit: ${row.name}`}
                          title={`Edit: ${row.name}`}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(
                              rootAction.category.daiLyAction.showDeleteConfirm(
                                row
                              )
                            );
                          }}
                        />
                      )}
                    </Box>
                  </>
                );
              },
            },
          ]}
        />
      )}
      {isShowEditModal && <DaiLyEditFormModal />}
      {isShowDeleteConfirm && daiLyEditing && (
        <ConfirmModal
          onCancel={() => {
            dispatch(rootAction.category.daiLyAction.closeDeleteConfirm());
          }}
          type="danger"
          title="Xóa đại lý"
          text="Bạn có chắc chắn muốn xóa đại lý?"
          isSaving={status == eReducerStatusBase.is_deleting}
          onConfirm={() => {
            dispatch(
              rootAction.category.daiLyAction.deleteStart(daiLyEditing?.id ?? 0)
            );
          }}
        />
      )}
    </Box>
  );
};

export default DaiLyPage;
