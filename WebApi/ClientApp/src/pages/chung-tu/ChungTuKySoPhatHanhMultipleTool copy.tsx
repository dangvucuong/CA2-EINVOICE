import { IssueClosedIcon } from "@primer/octicons-react";
import { Box, Label, ProgressBar, Spinner } from "@primer/react";
import { useEffect, useMemo, useRef, useState } from "react";
import { hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import { eHoaDonTrangThai } from "../../models/commons/eHoaDonTrangThai";
import { IHoaDonPhatHanhPushNotifyModel } from "../../models/responses/hub/IHoaDonPhatHanhPushNotifyModel";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
interface IChungTuKySoPhatHanhMultipleToolProps {
  ids: number[];
  onClose: () => void;
}
const ChungTuKySoPhatHanhMultipleTool = (
  props: IChungTuKySoPhatHanhMultipleToolProps
) => {
  const [isCreatingXml, setIsCreatingXml] = useState(false);
  const [isShowProgess, setIsShowProgess] = useState(false);
  const {
    _signalrHubProxy,
    _signalrConnected,
    createUUID,
    signalRConnectionServer,
  } = useCommonContext();
  const { user } = useAuth();
  const [reRenderKey, setReRenderkey] = useState("");

  const _refHoaDon = useRef<any[]>([]);

  useEffect(() => {
    _refHoaDon.current = props.ids.map((x) => {
      return {
        id: x,
        status_id: 0,
      };
    });
    setReRenderkey(createUUID());
  }, [props.ids]);

  const handleCreateXmlKySoAsync = async () => {
    setIsShowProgess(true);
    setIsCreatingXml(true);

    const res = await LaySoCTMulti_update(user?.donvi_ma_dv || "", "CT/25E");

    setIsCreatingXml(false);
    if (res?.status === "success") {
      console.log({
        res: res?.results,
      });

      _refHoaDon.current = res?.results?.map((x: any) => {
        return {
          ...x,
          xml_base64: x?.data,
          status_id: 1,
          code: createUUID().replace(/-/g, ""),
        };
      });
      console.log({
        yyy: _refHoaDon.current,
      });

      setReRenderkey(createUUID());
      SendToToolKySo();
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
  };

  const SendToToolKySo = () => {
    try {
      // _codeRef.current = [];
      _refHoaDon.current.forEach((hd: any) => {
        var code = hd.code;
        // _codeRef.current.push({
        //     code: code,
        //     signedtext: ""
        // });
        hd[`signedtext`] = "";
        var content =
          code + "|" + user?.serial_number + "|" + hd.xml_base64 + "|XML";
        // 
        _signalrHubProxy
          .invoke("send", content)
          .done(function () {})
          .fail(function (error: any) {
            NotifyHelper.Error("Có lỗi");
            console.log("Invocation failed. Error: " + error);
          });
      });
    } catch (error) {
      window.location.reload();
    }
  };

  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        if (eventName === "SERVER") {
          const ketquas = data.split("|");
          const [returnCode, code, signedtext] = ketquas;
          const hoaDon = _refHoaDon.current.find((x: any) => x.code === code);
          if (_refHoaDon.current && hoaDon) {
            if (returnCode === "1") {
              _refHoaDon.current = _refHoaDon.current.map((x: any) => {
                if (x.code === code) {
                  return {
                    ...x,
                    signedtext,
                    status_id: 2,
                  };
                }
                return {
                  ...x,
                };
              });
              setReRenderkey(createUUID());

              //   handleUpdateKySoSuccss(hoaDon.id, signedtext);

              UpdateChungTuSauKy({
                xmldaky: signedtext,
                trangthai: 2,
                mst: user?.donvi_ma_dv,
                machungtu: hoaDon?.mactu,
              });
            } else {
              NotifyHelper.Error("Có lỗi");
            }
          }
        }
      });
    }
  }, [_signalrConnected, _signalrHubProxy]);

  const progressSource = useMemo(() => {
    console.log({
      xxxx: _refHoaDon.current,
    });

    const result = [
      {
        id: 0,
        name: "Khởi tạo",
        color: "lightgrey",
        count: _refHoaDon.current.filter((x) => x.status_id === 0).length,
        per: 10,
      },
      {
        id: 1,
        name: "Đã khởi tạo",
        color: "grey",
        count: _refHoaDon.current.filter((x) => x.status_id === 1).length,
        per: 0,
      },
      {
        id: 2,
        name: "Đã ký số",
        color: "#8dc6fc",
        count: _refHoaDon.current.filter((x) => x.status_id === 2).length,
        per: 0,
      },
      {
        id: 3,
        name: "Đang gửi CQT",
        color: "#ffd78e",
        count: _refHoaDon.current.filter((x) => x.status_id === 3).length,
        per: 0,
      },
      {
        id: 4,
        name: "Đã phát hành",
        color: "#0cf478",
        count: _refHoaDon.current.filter((x) => x.status_id === 4).length,
        per: 0,
      },
      {
        id: 5,
        name: "Phát hành lỗi",
        color: "#ff0000",
        count: _refHoaDon.current.filter((x) => x.status_id === 5).length,
        per: 0,
      },
    ];
    return result
      .filter((x) => x.id > 0 || x.count > 0)
      .map((x) => {
        return {
          ...x,
          per: (x.count * 100) / _refHoaDon.current.length,
        };
      });
  }, [_refHoaDon.current, reRenderKey]);

  useEffect(() => {
    if (signalRConnectionServer) {
      signalRConnectionServer.on(
        "THONG_DIEP_HAS_RESULT",
        (message: IHoaDonPhatHanhPushNotifyModel) => {
          console.log({
            THONG_DIEP_HAS_RESULT: message,
          });
          if (_refHoaDon.current.find((y: any) => y.id === message.id)) {
            // const random:number = Math.floor(Math.random() * 10) + 1;
            // setTimeout (()=>{
            _refHoaDon.current = _refHoaDon.current.map((x: any) => {
              if (x.id === message.id) {
                return {
                  ...x,
                  status_id:
                    message.hoa_don_trang_thai_id ===
                    eHoaDonTrangThai.DA_PHAT_HANH
                      ? 4
                      : 5,
                };
              }
              return {
                ...x,
              };
            });
            setReRenderkey(createUUID());
            // },random*1000)
          }
        }
      );
    }
  }, [signalRConnectionServer]);

  //   const handleUpdateKySoSuccss = async (
  //     hoaDonId: number,
  //     signedtext: string
  //   ) => {
  //     const res = await hoaDonApi.updateKySoSuccess({
  //       signed_text: signedtext,
  //       id: hoaDonId,
  //     });
  //     if (res.is_success) {
  //       handlePhatHanhAsync(hoaDonId, signedtext);
  //     } else {
  //       NotifyHelper.Error(res.message ?? "Có lỗi");
  //     }
  //   };

  const handlePhatHanhAsync = async (mactu: number, signedtext: string) => {
    const res = await GuichungtulenCQT({
      machungtu: mactu,
      madonvi: user?.donvi_ma_dv,
    });
    if (res.status === "success") {
      _refHoaDon.current = _refHoaDon.current.map((x: any) => {
        if (x.mactu === mactu) {
          return {
            ...x,
            status_id: 3,
          };
        }
        return {
          ...x,
        };
      });
      setReRenderkey(createUUID());
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
  };

  const UpdateChungTuSauKy = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
      <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
        <soap12:Body>
          <UpdateChungTuSauKy xmlns="http://tempuri.org/">
            <xmlthongdiep>${values?.xmldaky}</xmlthongdiep>
            <trangthai>${values?.trangthai}</trangthai>
            <mst>${values?.mst}</mst>
            <machungtu>${values?.machungtu}</machungtu>
          </UpdateChungTuSauKy>
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
      handlePhatHanhAsync(values?.machungtu, values?.xmldaky);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const GuichungtulenCQT = async (values: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
      <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
        <soap12:Body>
          <GuiChungTuCQT xmlns="http://tempuri.org/">
            <machungtu>${values?.machungtu}</machungtu>
            <madonvi>${values?.madonvi}</madonvi>
          </GuiChungTuCQT>
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
    return parseRes;
  };

  const LaySoCTMulti_update = async (mst: string, kyhieu: string) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
    <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
      <soap12:Body>
        <LaySoCTMulti_update xmlns="http://tempuri.org/">
          <MasothueTC>${mst}</MasothueTC>
          <KHCTu>${kyhieu}</KHCTu>
            <DanhSachMaCTu>
            ${props.ids.map((id) => `<string>${id}</string>`).join("")}
        </DanhSachMaCTu>
        </LaySoCTMulti_update>
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
    return parseRes;
  };

  return (
    <Box>
      <Button
        text="Ký số và gửi cấp mã"
        leadingVisual={IssueClosedIcon}
        variant="primary"
        isLoading={isCreatingXml}
        onClick={() => {
          setIsShowProgess(true);
        }}
      />
      {isShowProgess && (
        <Modal
          title="Ký số và gửi cấp mã"
          onClose={() => {
            setIsShowProgess(false);
            props.onClose();
          }}
          isOpen
        >
          <Box sx={{ p: 3, pb: 0 }}>
            <Box sx={{ pb: 3 }}>
              <ProgressBar aria-valuenow={100} aria-label="progress" animated>
                {progressSource
                  .sort((a, b) => b.id - a.id)
                  .map((x) => {
                    return (
                      <ProgressBar.Item
                        progress={x.per}
                        key={x.id}
                        sx={{
                          backgroundColor: x.color,
                        }}
                      />
                    );
                  })}
              </ProgressBar>
            </Box>
            <Box sx={{ mt: 3 }}>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                <Box
                  sx={{
                    flex: 1,
                    display: "flex",
                    flexWrap: "wrap",
                    // justifyContent: "space-between"
                  }}
                >
                  {progressSource
                    .sort((a, b) => a.id - b.id)
                    .map((x) => {
                      return (
                        <Box
                          sx={{ display: "flex", mb: 2, width: "30%" }}
                          key={x.id}
                        >
                          <Box
                            sx={{
                              backgroundColor: x.color,
                              mr: 2,
                              height: "20px",
                              width: "20px",
                              borderRadius: 2,
                            }}
                          ></Box>
                          <Box sx={{ mr: 2 }}>{x.name}</Box>
                          {x.count > 0 && <Label>{x.count}</Label>}
                          {x.id === 0 && isCreatingXml && (
                            <Box sx={{ mt: "2px", ml: 1 }}>
                              <Spinner size="small" />
                            </Box>
                          )}
                        </Box>
                      );
                    })}
                </Box>
              </Box>
            </Box>
          </Box>

          <ModalActions>
            <Button
              text="Đóng"
              onClick={() => {
                setIsShowProgess(false);
                props.onClose();
              }}
            />
            {progressSource.find((x) => x.id === 0) && !isCreatingXml && (
              <Button
                text="Bắt đầu"
                variant="primary"
                onClick={() => {
                  handleCreateXmlKySoAsync();
                }}
              />
            )}
          </ModalActions>
        </Modal>
      )}
    </Box>
  );
};

export default ChungTuKySoPhatHanhMultipleTool;
