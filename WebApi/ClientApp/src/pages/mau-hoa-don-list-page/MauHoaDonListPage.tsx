import {
  LockIcon,
  PencilIcon,
  PlusIcon,
  TrashIcon,
} from "@primer/octicons-react";
import { Box, IconButton, Label, Octicon, useConfirm } from "@primer/react";
import { useEffect, useMemo } from "react";
import { Helmet } from "react-helmet";
import { useHistory } from "react-router-dom";
import { HANG_HOA_API_ENDPOINT } from "../../api/category/hangHoaApi";
import { mauHoaDonApi } from "../../api/hoa-don/mauHoaDonApi";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eSortMode } from "../../models/commons/eSortMode";
import { IMauHoaDon } from "../../models/responses/hoa-don/IMauHoaDon";
import { IMauHoaDonVM } from "../../models/responses/hoa-don/IMauHoaDonVM";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import ExportToExcelBtn from "../../component-data/export-excel-btn/ExportToExcelBtn";

const MauHoaDonListPage = () => {
  const {
    status,
    mauHoaDons,
    isShowDeleteConfirm,
    mauHoaDonEditing,
    isShowEditModal,
  } = useAppSelector((x) => x.hoaDon.mauHoaDonReducer);
  const dispatch = useAppDispatch();
  const history = useHistory();
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
    if (
      status === eReducerStatusBase.is_not_initialization ||
      status === eReducerStatusBase.is_need_reload
    ) {
      dispatch(rootAction.hoaDon.mauHoaDonAction.loadStart());
    }
  }, [status]);
  useEffect(() => {
    if (
      status === eReducerStatusBase.is_saved ||
      status === eReducerStatusBase.is_deleted
    ) {
      dispatch(rootAction.hoaDon.mauHoaDonAction.loadStart());
    }
  }, [status]);
  const confirm = useConfirm();
  const handleActiveAsync = async (data: IMauHoaDon, is_active: boolean) => {
    if (
      await confirm({
        content: is_active
          ? "Bạn có chắc chắn muốn Sử dụng mẫu hóa đơn này? Trong mỗi loại hóa đơn chỉ được phép sử dụng 1 mẫu hóa đơn tại một thời điểm"
          : "Bạn có chắc chắn muốn Ngừng sử dụng mẫu hóa đơn này",
        title: is_active ? "Sử dụng mẫu hóa đơn" : "Ngừng sử dụng mẫu hóa đơn",
        cancelButtonContent: "Đóng",
        confirmButtonContent: is_active ? "Sử dụng" : "Ngưng sử dụng",
        confirmButtonType: is_active ? "primary" : "danger",
      })
    ) {
      const res = await mauHoaDonApi.updateActive({
        id: data.id,
        is_active: is_active,
      });
      if (res.is_success) {
        NotifyHelper.Success("Success");
        dispatch(rootAction.hoaDon.mauHoaDonAction.loadStart());
      } else {
        NotifyHelper.Error(res.message ?? "Error");
      }
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Mẫu hóa đơn</title>
      </Helmet>
      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <DataTable
          titleComponent={<Heading text="Danh sách Mẫu hóa đơn" />}
          subTitle={`Tổng số: ${mauHoaDons.length.toLocaleString()}`}
          data={mauHoaDons?.sort((a, b) => {
            return a?.loai_hoa_don_ct_id - b?.loai_hoa_don_ct_id;
          })}
          height={window.innerHeight - 100}
          isLoading={status === eReducerStatusBase.is_loading}
          // exportEnable
          searchEnable
          sortConfig={{
            enable: true,
            mode: eSortMode.ASC,
          }}
          actionComponent={
            <>
              <Button
                text="Thêm Mẫu hóa đơn"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={HANG_HOA_API_ENDPOINT}
                onClick={() => {
                  history.push("../../mau-hoa-don-form/0");
                }}
              />

              <ExportToExcelBtn
                fileName="Mẫu_hóa_đơn"
                formatDataFunction={(data) => {
                  return data.map(
                    (x: IMauHoaDon & { loai_hoa_don_ct_name?: string }) => {
                      return {
                        "Loại hóa đơn": x?.loai_hoa_don_ct_name,
                        "Tên mẫu": x.name,
                        "Trạng thái": x.is_active
                          ? "Đang sử dụng"
                          : "Chưa sử dụng",
                        "Đã khóa": x.is_locked ? "Đã khóa" : "Chưa khóa",
                      };
                    }
                  );
                }}
                fetchDataPromise={() => {
                  return new Promise((resolve, reject) => {
                    return mauHoaDonApi.getByDonVi().then((res) => {
                      if (res.is_success) {
                        resolve(res.data);
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
          columns={[
            {
              header: "Loại hóa đơn",
              field: "loai_hoa_don_ct_name",
              rowHeader: false,
              width: "250px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Tên mẫu",
              field: "name",
              rowHeader: true,
              // sortBy: "alphanumeric"
            },
            {
              header: "Active",
              field: "is_active",
              rowHeader: false,
              width: "200px",
              renderCell: (data: IMauHoaDonVM) => {
                return (
                  <>
                    <Box
                      sx={{
                        cursor: "pointer",
                      }}
                      onClick={() => {
                        handleActiveAsync(data, !data.is_active);
                      }}
                    >
                      {data.is_active && (
                        <Label variant="success" size="small">
                          Đang sử dụng
                        </Label>
                      )}
                      {!data.is_active && (
                        <Label variant="secondary" size="small">
                          Chưa sử dụng
                        </Label>
                      )}
                    </Box>
                  </>
                );
              },
            },
            {
              header: "Đã khóa",
              field: "is_lock",
              rowHeader: false,
              renderCell: (data: IMauHoaDon) => {
                return <>{data.is_locked && <Octicon icon={LockIcon} />}</>;
              },
              // sortBy: "alphanumeric"
            },

            {
              id: "actions",
              header: "",
              width: "100px",
              renderCell: (row: IMauHoaDonVM) => {
                return (
                  <>
                    {row.is_locked !== true && (
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
                                // dispatch(rootAction.hoaDon.mauHoaDonAction.showEditModal(row))
                                history.push(
                                  `../../mau-hoa-don-form/${row.id}`
                                );
                              }}
                            />
                          )}
                          {!isCanNotDelete && !row.is_active && (
                            <IconButton
                              aria-label={`Edit: ${row.name}`}
                              title={`Edit: ${row.name}`}
                              icon={TrashIcon}
                              variant="invisible"
                              onClick={() => {
                                dispatch(
                                  rootAction.hoaDon.mauHoaDonAction.showDeleteConfirm(
                                    row
                                  )
                                );
                              }}
                            />
                          )}
                        </Box>
                      </>
                    )}
                  </>
                );
              },
            },
          ]}
        />
      )}

      {isShowDeleteConfirm && mauHoaDonEditing && (
        <ConfirmModal
          onCancel={() => {
            dispatch(rootAction.hoaDon.mauHoaDonAction.closeDeleteConfirm());
          }}
          type="danger"
          title="Xóa hóa đơn"
          text="Bạn có chắc chắn muốn xóa hóa đơn này?"
          isSaving={status == eReducerStatusBase.is_deleting}
          onConfirm={() => {
            dispatch(
              rootAction.hoaDon.mauHoaDonAction.deleteStart(
                mauHoaDonEditing?.id ?? 0
              )
            );
          }}
        />
      )}
    </Box>
  );
};

export default MauHoaDonListPage;
