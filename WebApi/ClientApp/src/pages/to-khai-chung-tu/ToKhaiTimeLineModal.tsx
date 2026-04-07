import { Box } from "@primer/react";
import moment from "moment";
import { useEffect, useState } from "react";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import PlaceHolder from "../../component-ui/place-holder";
import { NotifyHelper } from "../../helpers/toast";
import { DataTable } from "../../component-ui/data-table";
import { axiosClient } from "../../api/axiosClient";
import { formatXml } from "../../helpers/common";

interface IToKhaiTimeLineModalProps {
  MatokhaiCT: string;
  onClose: () => void;
}

export const ToKhaiTimeLineModal = (props: IToKhaiTimeLineModalProps) => {
  const { MatokhaiCT, onClose = () => {} } = props;
  const [isLoading, setIsLoading] = useState(false);
  const [danhsachtruyennhan, setDanhsachtruyennhan] = useState<any[]>([]);
  const [openXMLModal, setOpenXMLModal] = useState(false);
  const [xmlContent, setXMLContent] = useState("");

  useEffect(() => {
    handleLoadData(MatokhaiCT);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [MatokhaiCT]);

  const handleLoadData = async (matokhai: string) => {
    setIsLoading(true);
    await LayDanhSachToKhai(matokhai);
    setIsLoading(false);
  };

  const LayDanhSachToKhai = async (matokhai: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayDanhSachTruyenNhanToKhai xmlns="http://tempuri.org/">
      <matokhaict>${matokhai}</matokhaict>
    </LayDanhSachTruyenNhanToKhai>
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

    setIsLoading(true);

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      setDanhsachtruyennhan(
        Array.isArray(parseRes.data)
          ? parseRes.data.map((item: any, index: number) => ({
              ...item,
              Thoigian: moment(item.NLap).format("DD/MM/YYYY HH:mm:ss"),
              key: item.MatokhaiCT ?? item.id ?? index + 1,
              id: item.MatokhaiCT,
            }))
          : []
      );
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  function parseSoapResponse(soapXmlString: string) {
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(soapXmlString, "text/xml");

    const resultNode = Array.from(xmlDoc.getElementsByTagName("*")).find(
      (node) => node.nodeName.endsWith("Result")
    );

    if (!resultNode || !resultNode.textContent) {
      return null;
    }

    const jsonText = resultNode.textContent.trim();
    return JSON.parse(jsonText);
  }

  return (
    <Modal
      title={"Nhật ký truyền nhận"}
      onClose={() => {
        onClose();
      }}
      isOpen={true}
      width="1000px"
      height={"auto"}
      // key={khachHangEditing?.id ?? 0}
    >
      <Box
        display={"grid"}
        sx={{
          gap: 2,
        }}
      >
        {isLoading && <PlaceHolder line_number={5} />}
        {!isLoading && (
          <DataTable
            data={danhsachtruyennhan}
            height={window.innerHeight - 100}
            // isLoading={status === eReducerStatusBase.is_loading}
            // exportEnable
            searchEnable={false}
            columns={[
              {
                header: "STT",
                field: "key",
                rowHeader: true,
                width: "50px",
                // sortBy: "alphanumeric"
              },
              {
                header: "Mã người gửi",
                field: "MNGui",
                rowHeader: true,
                //   width: "100px",
                // sortBy: "alphanumeric"
              },
              {
                header: "Mã người nhận",
                field: "MNNhan",
                rowHeader: true,
                //   width: "100px",
                // sortBy: "alphanumeric"
              },
              {
                header: "Mã loại thông điệp",
                field: "MLTDiep",
                rowHeader: true,
                width: "100px",
                // sortBy: "alphanumeric"
              },

              {
                header: "Thời gian",
                field: "Thoigian",
                rowHeader: false,
                //   width: "150px",
              },

              {
                header: "Kết quả",
                field: "Trangthai",
                rowHeader: true,
                //   width: "100px",
                // sortBy: "alphanumeric"

                maxWidth: "200px",
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
                          cursor: "pointer",
                          ":hover": { textDecoration: "underline" },
                        }}
                        onClick={() => {
                          const base64Str = row?.XMLThongdiep;

                          // 1. decode Base64 -> byte array
                          const bytes = Uint8Array.from(atob(base64Str), (c) =>
                            c.charCodeAt(0)
                          );

                          // 2. decode UTF-8 từ byte array
                          const xmlDecoded = new TextDecoder("utf-8").decode(
                            bytes
                          );
                          const prettyXml = formatXml(xmlDecoded);

                          setXMLContent(prettyXml);
                          // 3. set state
                          setOpenXMLModal(true);
                        }}
                      >
                        Xem chi tiết
                      </Box>
                    </>
                  );
                },
              },
            ]}
          />
        )}

        <ModalActions>
          <Button
            onClick={() => {
              props.onClose();
            }}
            text="Đóng"
          />
        </ModalActions>
      </Box>

      {openXMLModal && (
        <Modal
          title="XML thông điệp"
          onClose={() => {
            setOpenXMLModal(false);
            // Đóng modal
          }}
          isOpen={openXMLModal}
          width="600px"
        >
          <Box
            padding={2}
            overflowY="auto"
            width={"800px"}
            maxHeight={window.innerHeight - 200}
            height={window.innerHeight - 200}
            fontFamily="monospace"
            fontSize={12}
            border="1px solid #ddd"
            borderRadius={4}
            bg="#f9f9f9"
          >
            <pre>{xmlContent}</pre>
          </Box>
        </Modal>
      )}
    </Modal>
  );
};
