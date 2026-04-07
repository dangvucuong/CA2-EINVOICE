import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton } from "@primer/react";
import { useEffect, useMemo } from "react";
import { Helmet } from "react-helmet";
import {
  HANG_HOA_API_ENDPOINT,
  hangHoaApi,
} from "../../api/category/hangHoaApi";
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
import { IHangHoa } from "../../models/responses/category/IHangHoa";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import HangHoaEditFormModal from "./HangHoaEditFormModal";
import HangHoaImportButton from "./HangHoaImportButton";

const HangHoaPage = () => {
  const {
    status,
    hangHoas,
    filter,
    paging_res,
    isShowDeleteConfirm,
    hangHoaEditing,
    isShowEditModal,
  } = useAppSelector((x) => x.category.hangHoaReducer);
  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT, "GET");
  }, []);
  const isCanNotEdit = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT, "PUT");
  }, []);
  const isCanNotDelete = useMemo(() => {
    return !checkAccesiableTo(HANG_HOA_API_ENDPOINT + "/{id}", "DELETE");
  }, []);
  useEffect(() => {
    dispatch(
      rootAction.category.hangHoaAction.loadStart({
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
        rootAction.category.hangHoaAction.loadStart({
          ...filter,
        })
      );
    }
  }, [status, filter]);
  return (
    <Box>
      <Helmet>
        <title>Hàng hóa</title>
      </Helmet>
      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <DataTableRemotePaging
          titleComponent={<Heading text="Danh sách hàng hóa" />}
          subTitle={`Tổng số: ${(
            paging_res?.total_count ?? 0
          ).toLocaleString()}`}
          data={hangHoas}
          height={window.innerHeight - 100}
          isLoading={status === eReducerStatusBase.is_loading}
          exportEnable
          actionComponent={
            <>
              <Button
                text="Thêm Hàng hóa"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={HANG_HOA_API_ENDPOINT}
                onClick={() => {
                  dispatch(rootAction.category.hangHoaAction.showEditModal());
                }}
              />
              <HangHoaImportButton
                onSuccess={() => {
                  dispatch(
                    rootAction.category.hangHoaAction.loadStart({
                      ...filter,
                    })
                  );
                }}
              />
              <ExportToExcelBtn
                fileName="hang-hoa"
                formatDataFunction={(data) => {
                  return data.map((x: IHangHoa) => {
                    return {
                      "Mã hàng hóa": x.ma_hang_hoa,
                      "Tên hàng hóa": x.ten_hang_hoa,
                      "Đơn vị tính": x.dvt,
                      "Mã loại hàng hóa": x.ma_loai_hoang_hoa,
                    };
                  });
                }}
                fetchDataPromise={() => {
                  return new Promise((resolve, reject) => {
                    return hangHoaApi
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
                rootAction.category.hangHoaAction.changeFilter({
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
                rootAction.category.hangHoaAction.changeFilter({
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
                rootAction.category.hangHoaAction.changeFilter({
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
              header: "Mã hàng",
              field: "ma_hang_hoa",
              rowHeader: false,
              width: "200px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Tên hàng",
              field: "ten_hang_hoa",
              rowHeader: true,
              // sortBy: "alphanumeric"
            },

            {
              header: "Đơn vị tính",
              field: "dvt",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Giá mặc định",
              field: "don_gia",
              rowHeader: false,
              width: "150px",
              // header: <Box sx={{
              //     textAlign: "right",
              //     width: "100%"
              // }}>Giá mặc định</Box>,
              renderCell: (row: any) => {
                return (
                  <Box
                    sx={{
                      textAlign: "right",
                      width: "100%",
                    }}
                  >
                    {row?.don_gia && <>{row.don_gia.toLocaleString()}</>}
                  </Box>
                );
              },
            },
            {
              header: "Mã loại hàng hóa",
              field: "ma_loai_hoang_hoa",
              rowHeader: false,
              width: "150px",
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
                              rootAction.category.hangHoaAction.showEditModal(
                                row
                              )
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
                              rootAction.category.hangHoaAction.showDeleteConfirm(
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
      {isShowEditModal && <HangHoaEditFormModal />}
      {isShowDeleteConfirm && hangHoaEditing && (
        <ConfirmModal
          onCancel={() => {
            dispatch(rootAction.category.hangHoaAction.closeDeleteConfirm());
          }}
          type="danger"
          title="Xóa khách hàng"
          text="Bạn có chắc chắn muốn xóa khách hàng này?"
          isSaving={status == eReducerStatusBase.is_deleting}
          onConfirm={() => {
            dispatch(
              rootAction.category.hangHoaAction.deleteStart(
                hangHoaEditing?.id ?? 0
              )
            );
          }}
        />
      )}
    </Box>
  );
};

export default HangHoaPage;
