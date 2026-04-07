import {
  EyeIcon,
  HistoryIcon,
  PlusIcon,
  SyncIcon,
  TrashIcon,
} from "@primer/octicons-react";

import { Box, IconButton, useConfirm } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import { Helmet } from "react-helmet";
import { useHistory } from "react-router-dom";
import { TO_KHAI_API } from "../../api/to-khai/toKhaiApi";
import LoaiToKhai from "../../component-data/loai-to-khai";
import ToKhaiStatus from "../../component-data/to-khai-status";
import Button from "../../component-ui/button";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eSortMode } from "../../models/commons/eSortMode";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { ToKhaiTimeLineModal } from "./ToKhaiTimeLineModal";

interface ISortConfig {
  enable: boolean;
  field?: string;
  mode: eSortMode;
  onValueChanged?: (field: string, mode: eSortMode) => void;
}

const ToKhaiPage = () => {
  const history = useHistory();
  const dispatch = useAppDispatch();
  const { status, toKhais, toKhaiEditing, isShowLogModal } = useAppSelector(
    (x) => x.toKhai.toKhaiReducer
  );
  const [sortConfig, setSortConfig] = useState<ISortConfig>({
    field: undefined,
    mode: eSortMode.ASC,
    enable: true,
  });

  useEffect(() => {
    if (
      status === eReducerStatusBase.is_not_initialization ||
      status === eReducerStatusBase.is_need_reload
    ) {
      dispatch(rootAction.toKhai.toKhaiAction.loadStart());
    }
  }, [status]);
  const confirm = useConfirm();
  const handleAsync = async (id: number) => {
    if (
      await confirm({
        content: "Bạn có chắc chắn muốn xóa tờ khai này không?",
        title: "Lưu ý",
        cancelButtonContent: "Không xóa",
        confirmButtonContent: "Xóa tờ khai",
        confirmButtonType: "danger",
      })
    ) {
      dispatch(rootAction.toKhai.toKhaiAction.deleteStart(id));
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Tờ khai</title>
      </Helmet>
      <Box>
        <DataTable
          titleComponent={<Heading text="Danh sách tờ khai" />}
          subTitle={`Tổng số: ${(0).toLocaleString()}`}
          data={toKhais}
          height={window.innerHeight - 100}
          isLoading={status === eReducerStatusBase.is_loading}
          // exportEnable
          searchEnable
          sortConfig={sortConfig}
          showRefreshButton={true}
          handleRefresh={() => {
            dispatch(rootAction.toKhai.toKhaiAction.loadStart());
          }}
          actionComponent={
            <>
              {/* <Button
                text="Refresh"
                leadingVisual={SyncIcon}
                onClick={() => {
                  setSortConfig({
                    field: undefined,
                    mode: eSortMode.ASC,
                    enable: true,
                  });
                  dispatch(rootAction.toKhai.toKhaiAction.loadStart());
                }}
              /> */}
              <Button
                text="Thêm tờ khai"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={TO_KHAI_API}
                onClick={() => {
                  history.push("../../to-khai/0");
                  // dispatch(rootAction.category.khachHangAction.showEditModal())
                }}
              />
            </>
          }
          columns={[
            {
              header: "Số",
              field: "ma_to_khai",
              rowHeader: true,
              width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày lập",
              field: "ngay_lap",
              rowHeader: false,
              width: "150px",
              renderCell: (data: any) => {
                return <Box>{moment(data.ngay_lap).format("DD/MM/YYYY")}</Box>;
              },
            },
            {
              header: "Hình thức tờ khai",
              field: "loai_to_khai_id",
              rowHeader: false,
              width: "200px",
              renderCell: (data: IToKhai) => {
                return <LoaiToKhai id={data.loai_to_khai_id} />;
              },
            },
            {
              header: "Hình thức hóa đơn",
              field: "hinh_thuc_hoa_don",
              rowHeader: false,
              minWidth: "350px",
              renderCell: (data: IToKhai) => {
                return (
                  <Box>
                    {data.is_hoadon_khong_co_ma_cqt &&
                      "Không có mã của cơ quan thuế;"}
                    {data.is_hoadon_co_ma_cqt && "Có mã của cơ quan thuế;"}
                    {data.is_hoadon_co_ma_cqt_mtt &&
                      "Có mã của cơ quan thuế (Khởi tạo từ máy tính tiền);"}
                  </Box>
                );
              },
            },

            {
              header: "Trạng thái",
              field: "to_khai_status_id",
              rowHeader: false,
              maxWidth: "250px",
              renderCell: (data: any) => {
                return (
                  <Box>
                    <ToKhaiStatus id={data.to_khai_status_id} />
                    {data.to_khai_status_id === eToKhaiStatus.CQT_TU_CHOI && (
                      <Box sx={{ mt: 1 }}>{data.ly_do_tu_choi}</Box>
                    )}
                  </Box>
                );
              },
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
                      <IconButton
                        aria-label={`Xem`}
                        title={`Xem`}
                        icon={EyeIcon}
                        variant="invisible"
                        onClick={() => {
                          history.push(`../../to-khai/${row.id}`);
                        }}
                      />
                      <IconButton
                        aria-label={`Lịch sử`}
                        title={`Lịch sử`}
                        icon={HistoryIcon}
                        variant="invisible"
                        onClick={() => {
                          dispatch(
                            rootAction.toKhai.toKhaiAction.showLogModal(row)
                          );
                        }}
                      />
                    </Box>
                  </>
                );
              },
            },
            {
              id: "cmd",
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
                      {row.to_khai_status_id === eToKhaiStatus.TAO_MOI && (
                        <IconButton
                          aria-label={`Xóa`}
                          title={`Xóa`}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            handleAsync(row.id);
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
        {isShowLogModal && toKhaiEditing && (
          <ToKhaiTimeLineModal
            toKhaiId={toKhaiEditing?.id}
            onClose={() => {
              dispatch(rootAction.toKhai.toKhaiAction.closeLogModal());
            }}
          />
        )}
      </Box>
    </Box>
  );
};

export default ToKhaiPage;
