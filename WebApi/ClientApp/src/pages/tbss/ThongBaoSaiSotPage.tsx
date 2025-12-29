import {
  PencilIcon,
  PlusIcon,
  HistoryIcon,
  TrashIcon,
} from "@primer/octicons-react";
import { useEffect, useMemo, useState } from "react";
import { IThongBaoSaiSot } from "../../models/responses/tbss/IThongBaoSaiSot";

import { Box, IconButton } from "@primer/react";
import { Helmet } from "react-helmet";
import { useHistory } from "react-router-dom";
import {
  THONG_BAO_SAI_SOT_API,
  thongBaoSaiSotApi,
} from "../../api/tbss/thongBaoSaiSotApi";
import ThongBaoSaiSotTinhChat from "../../component-data/tbss-tinh-chat";
import Button from "../../component-ui/button";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import ThongBaoSaiSotStatus from "../../component-data/tbss-status";
import { ThongBaoSaiSotTimelineModal } from "./ThongBaoSaiSotTimelineModal";
import { IThongBaoSaiSotChiTiet } from "../../models/responses/tbss/IThongBaoSaiSotChiTiet";
import moment from "moment";
import TBSSViewBtn from "./TBSSViewBtn";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
import ConfirmModal from "../../component-ui/confirm-modal";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { NotifyHelper } from "../../helpers/toast";

const ThongBaoSaiSotPage = () => {
  const [thongBaoSaiSots, setThongBaoSaiSots] = useState<IThongBaoSaiSot[]>([]);
  const [thongBaoSaiSotChiTiets, setThongBaoSaiSotChiTiets] = useState<
    IThongBaoSaiSotChiTiet[]
  >([]);
  const [isShowLogModal, setIsShowLogModal] = useState(false);
  const [editingData, setEditingData] = useState<IThongBaoSaiSot>();
  const [isShowDeleteConfirm, setIsShowDeleteConfirm] = useState(false);

  const history = useHistory();
  useEffect(() => {
    handleReloadAsync();
  }, []);
  const dataSource = useMemo(() => {
    return thongBaoSaiSots.map((tbss) => {
      const hoa_don_items = thongBaoSaiSotChiTiets.filter(
        (x) => x.thong_bao_sai_sot_id === tbss.id
      );
      return {
        ...tbss,
        hoa_don_items: hoa_don_items,
        hoa_don_items_text_search: hoa_don_items
          .map(
            (x) =>
              `${x.hoa_don_dang_ky_phat_hanh_mau_so}${
                x.hoa_don_dang_ky_phat_hanh_ky_hieu
              }_${x.ma_so_hoa_don} ${moment(x.ngay_hoa_don).format(
                "DD/MM/YYYY"
              )}`
          )
          .join(";"),
      };
    });
  }, [thongBaoSaiSots, thongBaoSaiSotChiTiets]);

  const handleReloadAsync = async () => {
    const res = await thongBaoSaiSotApi.getByDonVi();
    if (res.is_success) {
      setThongBaoSaiSots(res.data.list);
      setThongBaoSaiSotChiTiets(res.data.listChiTiet);
    }
  };

  const handleDelete = async (id: number) => {
    const res = await thongBaoSaiSotApi.delete(id);
    if (res.is_success) {
      NotifyHelper.Success("Xóa thông báo sai sót thành công");
      setIsShowDeleteConfirm(false);
      setEditingData(null as any);
      await handleReloadAsync();
    } else {
      NotifyHelper.Error(res.message || "Xóa thông báo sai sót thất bại");
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Thông báo sai sót</title>
      </Helmet>

      <DataTable
        titleComponent={<Heading text="Danh sách thông báo sai sót" />}
        subTitle={`Tổng số: ${thongBaoSaiSots.length.toLocaleString()}`}
        data={dataSource}
        height={window.innerHeight - 100}
        // isLoading={status === eReducerStatusBase.is_loading}
        exportEnable
        actionComponent={
          <>
            <Button
              text="Thêm mới"
              variant="primary"
              leadingVisual={PlusIcon}
              apiAuthorizedMethod="POST"
              apiAuthorized={THONG_BAO_SAI_SOT_API}
              onClick={() => {
                history.push("../../tbss/0");
                // dispatch(rootAction.category.khachHangAction.showEditModal())
              }}
            />
          </>
        }
        searchEnable={true}
        columns={[
          {
            header: "Id",
            field: "id",
            rowHeader: false,
            width: "50px",
          },
          {
            id: "actions",
            header: "",
            width: "180px",
            renderCell: (data: IThongBaoSaiSot) => {
              return (
                <>
                  <Box
                    sx={{
                      mt: -2,
                      mb: -2,
                      display: "flex",
                      justifyContent: "center",
                      gap: 1,
                    }}
                  >
                    <TBSSViewBtn id={data.id} />
                    {data.thong_bao_sai_sot_trang_thai_id === 1 && (
                      <>
                        <IconButton
                          aria-label={`Sửa: ${data.id}`}
                          title={`Sửa: ${data.id}`}
                          icon={PencilIcon}
                          variant="invisible"
                          onClick={() => {
                            history.push(`../../tbss/${data.id}`);
                          }}
                        />

                        <IconButton
                          aria-label={`Xóa: ${data.id}`}
                          title={`Xóa: ${data.id}`}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            setEditingData(data);
                            setIsShowDeleteConfirm(true);
                          }}
                        />
                      </>
                    )}

                    <IconButton
                      aria-label={`Lịch sử: ${data.id}`}
                      title={`Lịch sử: ${data.id}`}
                      icon={HistoryIcon}
                      variant="invisible"
                      onClick={() => {
                        setIsShowLogModal(true);
                        setEditingData(data);
                      }}
                    />
                  </Box>
                </>
              );
            },
          },
          {
            header: "Mã CQT",
            field: "ma_cqt",
            rowHeader: false,
            width: "100px",
          },
          {
            header: "Tên CQT",
            field: "ten_cqt",
            rowHeader: true,
            width: "250px",
          },
          {
            header: "Trạng thái",
            field: "thong_bao_sai_sot_trang_thai_id",
            width: "200px",
            // maxWidth: "200px",
            rowHeader: false,
            renderCell: (data: IThongBaoSaiSot) => {
              return (
                <ThongBaoSaiSotStatus
                  id={data.thong_bao_sai_sot_trang_thai_id}
                />
              );
            },
          },
          {
            header: "Lý do",
            field: "ly_do",
            rowHeader: false,
            width: "200px",
          },
          {
            header: "Tính chất",
            field: "thong_bao_sai_sot_tinh_chat_id",
            width: "150px",
            rowHeader: false,
            renderCell: (data: IThongBaoSaiSot) => {
              return (
                <ThongBaoSaiSotTinhChat
                  id={data.thong_bao_sai_sot_tinh_chat_id}
                />
              );
            },
          },
          {
            header: "Hóa đơn",
            field: "hoa_don_items_text_search",
            minWidth: "230px",
            rowHeader: false,
            renderCell: (data: any) => {
              return (
                <Box>
                  <ul
                    style={{
                      margin: 0,
                      padding: 0,
                    }}
                  >
                    {data.hoa_don_items.map(
                      (x: IThongBaoSaiSotChiTiet, idx: number) => {
                        return (
                          <li
                            key={idx}
                            style={{
                              display: "flex",
                              flexWrap: "wrap",
                            }}
                          >
                            <b>
                              {x.hoa_don_dang_ky_phat_hanh_mau_so}
                              {x.hoa_don_dang_ky_phat_hanh_ky_hieu}_
                              {x.ma_so_hoa_don}
                            </b>
                            <Box
                              sx={{
                                color: "fg.muted",
                                ml: 1,
                              }}
                            >
                              {" - ngày "}
                              {moment(x.ngay_hoa_don).format("DD/MM/YYYY")}
                            </Box>
                          </li>
                        );
                      }
                    )}
                  </ul>
                </Box>
              );
            },
          },
          {
            header: "Kết quả phản hồi",
            field: "ket_qua_phan_hoi",
            minWidth: "200px",
            rowHeader: false,
          },
        ]}
      />
      {isShowLogModal && editingData && (
        <ThongBaoSaiSotTimelineModal
          id={editingData.id}
          onClose={() => {
            setIsShowLogModal(false);
          }}
        />
      )}

      {isShowDeleteConfirm && editingData && (
        <ConfirmModal
          onCancel={() => {
            setEditingData(null as any);
            setIsShowDeleteConfirm(false);
          }}
          type="danger"
          title="Xóa thông báo sai sót"
          text="Bạn có chắc chắn muốn xóa thông báo này?"
          //   isSaving={status === eReducerStatusBase.is_deleting}
          onConfirm={() => {
            // dispatch(
            //   rootAction.hoaDon.hoaDonAction.deleteStart(editingData?.id ?? 0)
            // );

            handleDelete(editingData.id);
          }}
        />
      )}
    </Box>
  );
};

export default ThongBaoSaiSotPage;
