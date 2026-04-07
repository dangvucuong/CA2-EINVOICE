import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
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
import ChungTuPhatHanhEditFormModal from "./ChungTuPhatHanhEditFormModal";
import moment from "moment";
import { axiosClient } from "../../api/axiosClient";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { parseSoapResponse } from "../../helpers/common";
const hoaPhatPhatHanhAction = rootAction.hoaDon.hoaDonDangKyPhatHanhAction;

const ChungTuPhatHanhPage = () => {
  const { status, isShowDeleteConfirm, hoaDonDangKyPhatHanhEditing } =
    useAppSelector((x) => x.hoaDon.hoaDonDangKyPhatHanhReducer);
  const [openModal, setOpenModal] = useState(false);
  const [detailData, setDetailData] = useState<any>();

  const dispatch = useAppDispatch();
  const { checkAccesiableTo } = useCommonContext();
  const [danhsachphathanh, setDanhsachphathanh] = useState<any[]>([]);
  const [openDeleteModal, setOpenDeleteModal] = useState(false);

  const { user } = useAuth();

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
    LayDanhSachPhatHanh(user?.donvi_ma_dv, "03/TNCN");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const LayDanhSachPhatHanh = async (
    madonvi: string | undefined,
    mau_so: string | undefined
  ) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayDSHoaDonDKPH xmlns="http://tempuri.org/">
      <madonvi>${madonvi}</madonvi>
      <mau_so>${mau_so}</mau_so>
    </LayDSHoaDonDKPH>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setDanhsachphathanh(parseRes.data);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const Xoaphathanhchungtu = async (
    madonvi: string,
    idphathanh: string,
    kyhieu: string
  ) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
  <Xoaphathanhchungtu xmlns="http://tempuri.org/">
      <idphathanh>${idphathanh}</idphathanh>
      <madonvi>${madonvi}</madonvi>
      <mauso>03/TNCN</mauso>
      <kyhieu>${kyhieu}</kyhieu>
    </Xoaphathanhchungtu>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setOpenDeleteModal(false);
      setDetailData(undefined);
      NotifyHelper.Success(parseRes.message);
      LayDanhSachPhatHanh(user?.donvi_ma_dv, "03/TNCN");
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Phát hành chứng từ</title>
      </Helmet>
      {isCanNotView && <UnAuthorizedPage />}
      {!isCanNotView && (
        <DataTable
          titleComponent={<Heading text="Phát hành chứng từ" />}
          subTitle={`Tổng số: ${danhsachphathanh.length.toLocaleString()}`}
          data={danhsachphathanh}
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
                  setOpenModal(true);
                }}
              />
            </>
          }
          columns={[
            {
              header: "Mẫu số",
              field: "mau_so",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số lượng",
              field: "so_luong",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ký hiệu",
              field: "ky_hieu",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số bắt đầu",
              field: "so_bat_dau",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Số kết thúc",
              field: "so_ket_thuc",
              rowHeader: false,
              // width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày sử dụng",
              field: "ngay_su_dung",
              rowHeader: false,
              // width: "150px",
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
                      <IconButton
                        aria-label={`Sửa`}
                        title={`Sửa`}
                        icon={PencilIcon}
                        variant="invisible"
                        onClick={() => {
                          setDetailData(row);
                          setOpenModal(true);
                        }}
                      />

                      <IconButton
                        aria-label={`Xóa: ${row.id}`}
                        title={`Xóa: ${row.id}`}
                        icon={TrashIcon}
                        variant="invisible"
                        onClick={() => {
                          setDetailData(row);
                          setOpenDeleteModal(true);
                        }}
                      />

                      {/* {!isCanNotEdit && (
                        <IconButton
                          aria-label={`Sửa: ${row.id}`}
                          title={`Sửa: ${row.id}`}
                          icon={PencilIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(hoaPhatPhatHanhAction.showEditModal(row));
                          }}
                        />
                      )} */}
                      {/* {!isCanNotDelete && (
                        <IconButton
                          aria-label={`Xóa: ${row.id}`}
                          title={`Xóa: ${row.id}`}
                          icon={TrashIcon}
                          variant="invisible"
                          onClick={() => {
                            dispatch(
                              hoaPhatPhatHanhAction.showDeleteConfirm(row)
                            );
                          }}
                        />
                      )} */}
                    </Box>
                  </>
                );
              },
            },
          ]}
        />
      )}
      {openModal && (
        <ChungTuPhatHanhEditFormModal
          onClose={() => {
            setDetailData(undefined);
            setOpenModal(false);
          }}
          detailData={detailData}
          onSuccess={() => {
            LayDanhSachPhatHanh(user?.donvi_ma_dv, "03/TNCN");
          }}
        />
      )}
      {openDeleteModal && detailData && (
        <ConfirmModal
          onCancel={() => {
            setOpenDeleteModal(false);
            setDetailData(undefined);
          }}
          type="danger"
          title="Xác nhận xóa"
          text="Bạn có chắc chắn muốn xóa phát hành chứng từ này?"
          onConfirm={() => {
            Xoaphathanhchungtu(
              user?.donvi_ma_dv as string,
              detailData.id,
              detailData.ky_hieu
            );
          }}
        />
      )}
    </Box>
  );
};

export default ChungTuPhatHanhPage;
