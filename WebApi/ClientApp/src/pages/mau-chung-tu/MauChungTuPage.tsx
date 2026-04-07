import { EyeIcon, PlusIcon } from "@primer/octicons-react";
import { Helmet } from "react-helmet";

import { Box, IconButton } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import Button from "../../component-ui/button";
import { DataTable } from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import MauChungTuModal from "./MauChungTuModal";
import { parseSoapResponse } from "../../helpers/common";
import { useAuth } from "../../hooks/useAuth";
import { NotifyHelper } from "../../helpers/toast";
import { axiosClient } from "../../api/axiosClient";

const MauChungTuPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [openModal, setOpenModal] = useState(false);
  const { user } = useAuth();
  const [danhsachmau, setDanhsachmau] = useState<any[]>([]);
  const [dataEdit, setDataEdit] = useState<any>(null);

  useEffect(() => {
    // Giả sử mã đơn vị là "DV001"
    setIsLoading(true);
    LayDanhSachMau();
    setIsLoading(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const LayDanhSachMau = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayDanhSachMau xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi_ma_dv}</madonvi>
    </LayDanhSachMau>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      const sortedData = parseRes?.data?.sort(
        (a: any, b: any) => b.idMauHD - a.idMauHD,
      );
      setDanhsachmau(
        sortedData.map((item: any, index: number) => ({
          ...item,
          stt: index + 1,
        })),
      );
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Mẫu chứng từ</title>
      </Helmet>
      <Box
        id="header"
        sx={{
          display: "flex",
        }}
      >
        <Box
          sx={{
            flex: 1,
          }}
        >
          <Heading text="Mẫu chứng từ" />
        </Box>
        <Box sx={{ display: "flex" }}>
          <Button
            text="Thêm mới"
            leadingVisual={PlusIcon}
            variant="primary"
            size="medium"
            onClick={() => {
              setDataEdit(null);
              setOpenModal(true);
            }}
          />
        </Box>
      </Box>
      <Box sx={{ mt: 3 }}>
        <DataTable
          title={`Tổng số: ${(danhsachmau?.length).toLocaleString()}`}
          data={danhsachmau}
          height={window.innerHeight - 100}
          isLoading={isLoading}
          columns={[
            {
              header: "STT",
              field: "stt",
              rowHeader: false,
              width: "80px",
              // sortBy: "alphanumeric"
            },
            {
              header: "Tên chứng từ",
              field: "TenHD",
              rowHeader: true,
              // sortBy: "alphanumeric"
            },
            {
              header: "Ký hiệu mẫu số",
              field: "Mauso",
              rowHeader: true,

              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày tạo",
              field: "ThoigianPH",
              rowHeader: false,
              renderCell: (cell: any) => {
                return (
                  <Box>{moment(cell.ThoigianPH).format("DD/MM/YYYY")}</Box>
                );
              },
              // sortBy: "alphanumeric"
            },
            {
              header: "Số quyết định",
              field: "SoQD",
              rowHeader: false,
              // sortBy: "alphanumeric"
            },
            {
              header: "Ngày quyết định",
              field: "NgayQD",
              rowHeader: false,
              renderCell: (cell: any) => {
                return <Box>{moment(cell.NgayQD).format("DD/MM/YYYY")}</Box>;
              },
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
                      <IconButton
                        aria-label={`Edit`}
                        title={`Edit`}
                        icon={EyeIcon}
                        variant="invisible"
                        onClick={() => {
                          setDataEdit(row);
                          setOpenModal(true);
                        }}
                      />
                    </Box>
                  </>
                );
              },
            },
          ]}
        />

        {openModal && (
          <MauChungTuModal
            openModal={openModal}
            onClose={() => setOpenModal(false)}
            onRefresh={() => LayDanhSachMau()}
            dataEdit={dataEdit}
          />
        )}
      </Box>
    </Box>
  );
};

export default MauChungTuPage;
