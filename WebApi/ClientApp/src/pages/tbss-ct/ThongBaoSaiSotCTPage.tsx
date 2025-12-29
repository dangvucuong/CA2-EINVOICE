import {
  PencilIcon,
  PlusIcon,
  HistoryIcon,
  ChecklistIcon,
} from "@primer/octicons-react";
import { useCallback, useEffect, useMemo, useState } from "react";
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
import TBSSChungTuViewBtn from "./TBSSChungTuViewBtn";
import { NotifyHelper } from "../../helpers/toast";
import { parseSoapResponse } from "../../helpers/common";
import { axiosClient } from "../../api/axiosClient";
import { useAuth } from "../../hooks/useAuth";
import XemKetQuaTBSSCT from "./XemKetQuaTBSSCT";

const ThongBaoSaiSotCTPage = () => {
  const [isShowLogModal, setIsShowLogModal] = useState(false);
  const [editingData, setEditingData] = useState<any>();
  const [dataSource, setDataSource] = useState<any[]>([]);
  const { user } = useAuth();
  const history = useHistory();
  const [openHistoryModal, setOpenHistoryModal] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isOpenResultModal, setIsOpenResultModal] = useState(false);

  useEffect(() => {
    // Giả sử mã đơn vị là "DV001"
    Laydanhsachtbsschungtu(user?.donvi_ma_dv);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const Laydanhsachtbsschungtu = async (madonvi: string | undefined) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Laydanhsachtbsschungtu  xmlns="http://tempuri.org/">
      <madonvi>${madonvi}</madonvi>
    </Laydanhsachtbsschungtu>
  </soap12:Body>
</soap12:Envelope>`;
    setIsLoading(true);

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
    setIsLoading(false);

    if (parseRes.status === "success") {
      const newData = parseRes.data?.map((item: any, index: number) => ({
        ...item,
        TrangthaiguiCQT: GetTinhtrangTBSS(item?.Trangthai),

        // Ketquadoichieu có chuỗi vKQ , hãy cắt nó đi
        kqxly: item?.kqxly ? item?.kqxly.split("vKQ")[1] : "",
      }));
      console.log(parseRes.data);

      setDataSource(newData);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const GetTinhtrangTBSS = useCallback((status: number): string => {
    switch (status) {
      case 1:
        return "Mới lập";
      case 2:
        return "Đã ký";
      case 3:
        return "Đã gửi CQT";
      default:
        return "";
    }
  }, []);

  return (
    <Box>
      <Helmet>
        <title>Thông báo sai sót</title>
      </Helmet>

      <DataTable
        titleComponent={
          <Heading text="Danh sách chứng từ đã gửi thông báo sai sót lên cơ quan thuế" />
        }
        subTitle={`Tổng số: ${dataSource?.length}`}
        data={dataSource}
        height={window.innerHeight - 100}
        isLoading={isLoading}
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
                history.push("../../tbss-ct/0");
                // dispatch(rootAction.category.khachHangAction.showEditModal())
              }}
            />
          </>
        }
        searchEnable={true}
        columns={[
          {
            header: "MaTBSSCT",
            field: "MaTBSSCT",
            rowHeader: false,
            width: "50px",
          },
          {
            id: "actions",
            header: "",
            width: "120px",
            renderCell: (data: any) => {
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
                    {/* {data?.Trangthai === 1 && (
                      <IconButton
                        aria-label={`Edit: ${data.MaTBSSCT}`}
                        title={`Edit: ${data.MaTBSSCT}`}
                        icon={PencilIcon}
                        variant="invisible"
                        onClick={() => {
                          history.push(`../../tbss-ct/${data.MaTBSSCT}`);
                        }}
                      />
                    )} */}
                    {data?.Trangthai === 3 && (
                      <IconButton
                        aria-label={`History: ${data.MaTBSSCT}`}
                        title={`History: ${data.MaTBSSCT}`}
                        icon={HistoryIcon}
                        variant="invisible"
                        onClick={() => {
                          setOpenHistoryModal(true);
                          setEditingData(data);
                        }}
                      />
                    )}

                    {data?.Trangthai === 3 && (
                      <IconButton
                        aria-label={`Xem kết quả`}
                        title={`Xem kết quả`}
                        icon={ChecklistIcon}
                        variant="invisible"
                        onClick={() => {
                          setIsOpenResultModal(true);
                          setEditingData(data);
                        }}
                      />
                    )}

                    {data?.KQKhac !== "-1" && data?.KQ301 && (
                      <TBSSChungTuViewBtn matbss_ct={data.MaTBSSCT} />
                    )}
                  </Box>
                </>
              );
            },
          },
          {
            header: "Ký hiệu",
            field: "KHCTu",
            rowHeader: true,
            width: "100px",
          },
          {
            header: "Số chứng từ",
            field: "SCTu",
            rowHeader: true,
            width: "100px",
          },
          {
            header: "Ngày lập chứng từ",
            field: "NLap",
            rowHeader: true,
            width: "160px",
            renderCell: (data: any) => {
              return moment(data?.NLap).format("DD/MM/YYYY");
            },
          },

          {
            header: "Lý do",
            field: "LDo",
            rowHeader: false,
            width: "200px",
          },
          {
            header: "Trạng thái gửi CQT",
            field: "TrangthaiguiCQT",
            width: "200px",
            // maxWidth: "200px",
            rowHeader: false,
          },
          {
            header: "Kết quả đối chiếu",
            field: "Ketquadoichieu",
            minWidth: "100px",
            rowHeader: false,
          },
          {
            header: "Kết quả đối chiếu",
            field: "kqxly",
            minWidth: "100px",
            rowHeader: false,
          },
        ]}
      />
      {openHistoryModal && editingData && (
        <ThongBaoSaiSotTimelineModal
          matbss_ct={editingData.MaTBSSCT}
          onClose={() => {
            setOpenHistoryModal(false);
          }}
        />
      )}

      {isOpenResultModal && editingData && (
        <XemKetQuaTBSSCT
          isOpen={isOpenResultModal}
          onClose={() => setIsOpenResultModal(false)}
          matbss={editingData.MaTBSSCT}
          user={user}
          type={5}
        />
      )}
    </Box>
  );
};

export default ThongBaoSaiSotCTPage;
