import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton } from "@primer/react";
import { useEffect, useMemo } from "react";
import { Helmet } from "react-helmet";
import { HOA_DONG_DANG_KY_PHAT_HANH } from "../../api/hoa-don/hoaDonDangKyPhatHanhApi";
import Button from "../../component-ui/button";
import ConfirmModal from "../../component-ui/confirm-modal";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { IHoaDonDangKyPhatHanh } from "../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import HoaDonPhatHanhEditFormModal from "./HoaDonPhatHanhEditFormModal";
import moment from "moment";
const hoaPhatPhatHanhAction = rootAction.hoaDon.hoaDonDangKyPhatHanhAction;
const HoaDonPhatHanhPage = () => {
  const {
    status,
    hoaDonDangKyPhatHanhs,
    isShowDeleteConfirm,
    hoaDonDangKyPhatHanhEditing,
    isShowEditModal,
  } = useAppSelector((x) => x.hoaDon.hoaDonDangKyPhatHanhReducer);
  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const isCanNotView = useMemo(() => {
    return !checkAccesiableTo(HOA_DONG_DANG_KY_PHAT_HANH, "GET");
  }, []);
  const isCanNotEdit = useMemo(() => {
    return !checkAccesiableTo(HOA_DONG_DANG_KY_PHAT_HANH, "PUT");
  }, []);
  const isCanNotDelete = useMemo(() => {
    return !checkAccesiableTo(HOA_DONG_DANG_KY_PHAT_HANH + "/{id}", "DELETE");
  }, []);
  useEffect(() => {
    if (
      status === eReducerStatusBase.is_not_initialization ||
      status === eReducerStatusBase.is_need_reload ||
      status === eReducerStatusBase.is_saved ||
      status === eReducerStatusBase.is_deleted
    ) {
      dispatch(hoaPhatPhatHanhAction.loadStart());
    }
  }, [status]);

  return (
    <Box>
      <Helmet>
        <title>Phát hành hóa đơn</title>
      </Helmet>
      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <DataTable
          titleComponent={<Heading text="Phát hành hóa đơn" />}
          subTitle={`Tổng số: ${hoaDonDangKyPhatHanhs.length.toLocaleString()}`}
          data={hoaDonDangKyPhatHanhs}
          height={window.innerHeight - 100}
          isLoading={status === eReducerStatusBase.is_loading}
          exportEnable
          searchEnable
          actionComponent={
            <>
              <Button
                text="Thêm mới"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={HOA_DONG_DANG_KY_PHAT_HANH}
                onClick={() => {
                  dispatch(hoaPhatPhatHanhAction.showEditModal());
                }}
              />
            </>
          }
          columns={[
            {
              header: "Tên hóa đơn",
              field: "ten_hoa_don",
              rowHeader: true,
              minWidth: "200px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Mẫu số",
              field: "mau_so",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số lượng",
              field: "so_luong",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ký hiệu",
              field: "ky_hieu",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số bắt đầu",
              field: "so_bat_dau",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số kết thúc",
              field: "so_ket_thuc",
              rowHeader: false,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày sử dụng",
              field: "ngay_su_dung",
              rowHeader: false,
              width: "150px",
              renderCell: (data: IHoaDonDangKyPhatHanh) => {
                return (
                  <Box>{moment(data.ngay_su_dung).format("DD/MM/YYYY")}</Box>
                );
              },
            },

            {
              id: "actions",
              header: "",
              width: "100px",
              renderCell: (row: IHoaDonDangKyPhatHanh) => {
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
                          aria-label={`Edit: ${row.id}`}
                          title={`Edit: ${row.id}`}
                          icon={PencilIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(hoaPhatPhatHanhAction.showEditModal(row));
                          }}
                        />
                      )}
                      {!isCanNotDelete && (
                        <IconButton
                          aria-label={`Edit: ${row.id}`}
                          title={`Edit: ${row.id}`}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(
                              hoaPhatPhatHanhAction.showDeleteConfirm(row)
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
      {isShowEditModal && (
        <HoaDonPhatHanhEditFormModal
          hoaDonDangKyPhatHanhs={hoaDonDangKyPhatHanhs}
        />
      )}
      {isShowDeleteConfirm && hoaDonDangKyPhatHanhEditing && (
        <ConfirmModal
          onCancel={() => {
            dispatch(hoaPhatPhatHanhAction.closeDeleteConfirm());
          }}
          type="danger"
          title="Xóa khách hàng"
          text="Bạn có chắc chắn muốn xóa phát hành hóa đơn này?"
          isSaving={status === eReducerStatusBase.is_deleting}
          onConfirm={() => {
            dispatch(
              hoaPhatPhatHanhAction.deleteStart(
                hoaDonDangKyPhatHanhEditing?.id ?? 0
              )
            );
          }}
        />
      )}
    </Box>
  );
};

export default HoaDonPhatHanhPage;
