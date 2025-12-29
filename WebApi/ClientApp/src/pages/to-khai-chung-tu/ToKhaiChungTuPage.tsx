import {
  ChecklistIcon,
  EyeIcon,
  HistoryIcon,
  PencilIcon,
  PlusIcon,
  SyncIcon,
} from "@primer/octicons-react";

import { Box, IconButton } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import { Helmet } from "react-helmet";
import { useHistory } from "react-router-dom";
import { TO_KHAI_API } from "../../api/to-khai/toKhaiApi";
import LoaiToKhai from "../../component-data/loai-to-khai";
import Button from "../../component-ui/button";
import DataTable from "../../component-ui/data-table/DataTable";
import Heading from "../../component-ui/heading";
import { eSortMode } from "../../models/commons/eSortMode";
import { ToKhaiTimeLineModal } from "./ToKhaiTimeLineModal";
import { axiosClient } from "../../api/axiosClient";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import ToKhaiChungTuStatus from "../../component-data/to-khai-chung-tu-status";
import { parseSoapResponse } from "../../helpers/common";
import XemToKhaiChungTu from "./XemToKhaiChungTu";

const ToKhaiChungTuPage = () => {
  const history = useHistory();
  const [danhsachtokhai, setDanhsachtokhai] = useState<any[]>([]);
  const { user } = useAuth();
  const [openHistoryModal, setOpenHistoryModal] = useState(false);
  const [openModalXemToKhai, setOpenModalXemToKhai] = useState(false);
  const [type, setType] = useState(5); // 5 xem tờ khai, 6 xem kết quả
  const [toKhaiEditing, setToKhaiEditing] = useState<any>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    // Giả sử mã đơn vị là "DV001"
    LayDanhSachToKhai(user?.donvi_ma_dv);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const LayDanhSachToKhai = async (madonvi: string | undefined) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Laydanhsachtokhaict xmlns="http://tempuri.org/">
      <madonvi>${madonvi}</madonvi>
    </Laydanhsachtokhaict>
  </soap12:Body>
</soap12:Envelope>`;
    setLoading(true);

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

    setLoading(false);

    if (parseRes.status === "success") {
      setDanhsachtokhai(parseRes.data);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Tờ khai</title>
      </Helmet>
      <Box>
        <DataTable
          titleComponent={<Heading text="Danh sách tờ khai chứng từ" />}
          subTitle={`Tổng số: ${danhsachtokhai?.length.toLocaleString()}`}
          data={danhsachtokhai}
          height={window.innerHeight - 100}
          isLoading={loading}
          // exportEnable
          searchEnable
          sortConfig={{
            enable: true,
            mode: eSortMode.ASC,
          }}
          actionComponent={
            <>
              <Button
                text="Refresh"
                leadingVisual={SyncIcon}
                onClick={() => {
                  LayDanhSachToKhai(user?.donvi_ma_dv);
                }}
              />
              <Button
                text="Thêm tờ khai"
                variant="primary"
                leadingVisual={PlusIcon}
                apiAuthorizedMethod="POST"
                apiAuthorized={TO_KHAI_API}
                onClick={() => {
                  history.push("../../to-khai-chung-tu/0");
                }}
              />
            </>
          }
          columns={[
            {
              header: "Mã tờ khai",
              field: "MatokhaiCT",
              rowHeader: true,
              //   width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày lập",
              field: "NLap",
              rowHeader: false,
              //   width: "150px",
              renderCell: (data: any) => {
                return <Box>{moment(data.NLap).format("DD/MM/YYYY")}</Box>;
              },
            },
            {
              header: "Hình thức tờ khai",
              field: "HThuc",
              rowHeader: false,
              //   width: "200px",
              renderCell: (data: any) => {
                return <LoaiToKhai id={data.HThuc} />;
              },
            },

            {
              header: "Trạng thái",
              field: "Trangthai",
              rowHeader: false,
              //   maxWidth: "200px",
              renderCell: (data: any) => {
                return <ToKhaiChungTuStatus id={data.Trangthai} />;
              },
            },
            {
              header: "Kết quả phản hồi",
              field: "ketquaphanhoi",
              rowHeader: true,
              //   width: "100px",
              // sortBy: "alphanumeric"
            },
            {
              id: "actions",
              header: "",
              width: "120px",
              renderCell: (row: any) => {
                return (
                  <>
                    <Box
                      sx={{
                        mt: -2,
                        mb: -2,
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        width: "100%",
                      }}
                    >
                      {row?.Trangthai === 1 && (
                        <IconButton
                          aria-label={`Edit`}
                          title={`Edit`}
                          icon={PencilIcon}
                          variant="invisible"
                          onClick={() => {
                            history.push(
                              `../../to-khai-chung-tu/${row.MatokhaiCT}`
                            );
                          }}
                        />
                      )}

                      <IconButton
                        aria-label={`Xem tờ khai`}
                        title={`Xem tờ khai`}
                        icon={EyeIcon}
                        variant="invisible"
                        onClick={() => {
                          setType(5);
                          setToKhaiEditing(row);
                          setOpenModalXemToKhai(true);
                        }}
                      />

                      {row?.Trangthai === 3 && (
                        <>
                          <IconButton
                            aria-label={`Xem kết quả`}
                            title={`Xem kết quả`}
                            icon={ChecklistIcon}
                            variant="invisible"
                            onClick={() => {
                              setType(6);
                              setToKhaiEditing(row);
                              setOpenModalXemToKhai(true);
                            }}
                          />

                          <IconButton
                            aria-label={`Xem lịch sử`}
                            title={`Xem lịch sử`}
                            icon={HistoryIcon}
                            variant="invisible"
                            onClick={() => {
                              setToKhaiEditing(row);
                              setOpenHistoryModal(true);
                            }}
                          />
                        </>
                      )}
                    </Box>
                  </>
                );
              },
            },
          ]}
        />

        {openHistoryModal && toKhaiEditing && (
          <ToKhaiTimeLineModal
            MatokhaiCT={toKhaiEditing?.MatokhaiCT}
            onClose={() => {
              setOpenHistoryModal(false);
            }}
          />
        )}

        {openModalXemToKhai && toKhaiEditing && (
          <XemToKhaiChungTu
            isOpen={openModalXemToKhai}
            onClose={() => setOpenModalXemToKhai(false)}
            matokhaiCT={toKhaiEditing?.MatokhaiCT}
            user={user}
            type={type}
          />
        )}
      </Box>
    </Box>
  );
};

export default ToKhaiChungTuPage;
