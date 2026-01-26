import { IssueClosedIcon } from "@primer/octicons-react";
import { Box, Label, ProgressBar, Spinner, useConfirm } from "@primer/react";
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
interface IHoaDonKySoPhatHanhMultipleToolProps {
  ids: number[];
  onClose: () => void;
  isKhacNgay?: boolean;
  title?: string;
  isHoaDonCungNgay?: boolean;
}
const HoaDonKySoPhatHanhMultipleTool = (
  props: IHoaDonKySoPhatHanhMultipleToolProps,
) => {
  const [isCreatingXml, setIsCreatingXml] = useState(false);
  const [isShowProgess, setIsShowProgess] = useState(false);
  const [serialNumber, setSerialNumber] = useState("");
  const { title = "Ký số và gửi cấp mã", isHoaDonCungNgay = true } = props;

  const {
    _signalrConnection,
    _signalrConnected,
    createUUID,
    signalRConnectionServer,
    _signalrSelectCert,
  } = useCommonContext();
  const { user } = useAuth();
  const [reRenderKey, setReRenderkey] = useState("");

  const _refHoaDon = useRef<any[]>([]);
  useEffect(() => {
    setSerialNumber(user?.serial_number ?? "");
  }, [user]);
  useEffect(() => {
    _refHoaDon.current = props.ids.map((x) => {
      return {
        id: x,
        status_id: 0,
      };
    });
    setReRenderkey(createUUID());
  }, [props.ids]);

  const confirm = useConfirm();

  const handleCreateXmlKySoAsync = async () => {
    // if (props.isKhacNgay) {
    //     if (!await confirm({
    //         content: "Tồn tại hóa đơn có ngày hóa đơn khác ngày ký, Anh/Chị có chắc chắn muốn ký số hóa đơn này?",
    //         title: "Lưu ý",
    //         cancelButtonContent: "Không ký",
    //         confirmButtonContent: "Tiếp tục ký số",
    //         confirmButtonType: "danger"
    //     })) {
    //         return;
    //     }
    // }
    setIsShowProgess(true);
    setIsCreatingXml(true);
    const res = await hoaDonApi.createXmlKySos({
      ids: props.ids,
    });
    setIsCreatingXml(false);

    if (res.is_success) {
      console.log({
        res: res.data,
      });

      const errRes = res?.data?.filter((x: any) => x.is_success === false);
      if (errRes.length > 0) {
        NotifyHelper.Error(errRes[0]?.message ?? "Có lỗi xảy ra");
      }

      _refHoaDon.current = res.data.map((x: any) => {
        return {
          ...x,
          status_id: x.is_success === false ? 6 : 1,
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
      _refHoaDon.current
        .filter((x) => x.is_success !== false)
        .forEach((hd: any) => {
          var code = hd.code;
          // _codeRef.current.push({
          //     code: code,
          //     signedtext: ""
          // });
          hd[`signedtext`] = "";
          var content =
            code + "|" + serialNumber + "|" + hd.xml_base64 + "|XML";
          // debugger
          _signalrConnection
            ?.invoke("send", content)
            .then(function () {})
            .catch((error: any) => {
              NotifyHelper.Error("Có lỗi");
              console.log("Invocation failed. Error: " + error);
            });
          if (hd.bien_ban_base64) {
            var contentBienBan =
              code +
              "_BB" +
              "|" +
              serialNumber +
              "|" +
              hd.bien_ban_base64 +
              "|XML|NDBBan|NBan";
            _signalrConnection
              ?.invoke("send", contentBienBan)
              .then(function () {
                console.log({
                  sendSuccessBienBan: contentBienBan,
                });
              })
              .catch((error: any) => {
                NotifyHelper.Error("Có lỗi");
                console.log("Invocation failed. Error: " + error);
              });
          }
        });
    } catch (error) {
      window.location.reload();
    }
  };
  useEffect(() => {
    if (_signalrConnected) {
      _signalrConnection?.on(
        "addMessage",
        function (eventName: any, data: any) {
          if (eventName === "SERVER") {
            const ketquas = data.split("|");
            const [returnCode, code, signedtext] = ketquas;

            if (signedtext === "CertInf") {
              const [nhaCungCap, serial, tuNgay, denNgay, subject] =
                ketquas.slice(3);
              let issuer = nhaCungCap;
              const match = nhaCungCap.match(/CN=([^,]+)/);
              if (match) {
                issuer = match[1];
              } else {
              }
              const data: any = {
                returnCode,
                code,
                signedtext,
                nhaCungCap,
                serial,
                tuNgay,
                denNgay,
                subject,
                issuer,
              };
              setSerialNumber(serial);
            }
          }
        },
      );
    }
  }, [_signalrConnected, _signalrConnection]);
  let isUpdatingMessageToolKySo = false;
  const messageQueueToolKySo: any[] = [];
  const processQueueToolKySo = () => {
    if (isUpdatingMessageToolKySo || messageQueueToolKySo.length === 0) return;
    const nextMessage = messageQueueToolKySo.shift();
    xuLyMessageToolKySo(nextMessage);
  };
  const xuLyMessageToolKySo = (ketquas: any) => {
    isUpdatingMessageToolKySo = true;
    try {
      const [returnCode, code, signedtext] = ketquas;
      let hoaDonCode = code;
      const isCodeBienBan = code.endsWith("_BB");
      hoaDonCode = code.replace("_BB", "");
      const hoaDonIdx = _refHoaDon.current.findIndex(
        (x: any) => x.code === hoaDonCode,
      );
      const hoaDon = hoaDonIdx >= 0 ? _refHoaDon.current[hoaDonIdx] : undefined;
      if (returnCode === "1" && hoaDon) {
        if (isCodeBienBan) {
          _refHoaDon.current[hoaDonIdx] = {
            ...hoaDon,
            bienBanSignedText: signedtext,
          };
        } else {
          _refHoaDon.current[hoaDonIdx] = {
            ...hoaDon,
            status_id: 2,
            signedtext,
          };
        }
        console.log({
          xx: _refHoaDon.current,
        });

        const hoaDonUpdated =
          hoaDonIdx >= 0 ? _refHoaDon.current[hoaDonIdx] : undefined;
        if (hoaDonUpdated) {
          debugger;
          if (hoaDonUpdated.status_id === 2) {
            if (hoaDonUpdated.xml_base64 && hoaDonUpdated.signedtext) {
              if (!hoaDonUpdated.bien_ban_base64) {
                debugger;
                handleUpdateKySoSuccss(hoaDon.id, hoaDonUpdated.signedtext);
              } else {
                if (hoaDonUpdated.bienBanSignedText) {
                  debugger;
                  handleUpdateKySoSuccss(
                    hoaDon.id,
                    hoaDonUpdated.signedtext,
                    hoaDonUpdated.bienBanSignedText,
                  );
                }
              }
            }
          }
        }
        setReRenderkey(createUUID());
      }
    } finally {
      isUpdatingMessageToolKySo = false;
      processQueueToolKySo();
    }
  };
  useEffect(() => {
    if (_signalrConnected) {
      _signalrConnection?.on(
        "addMessage",
        function (eventName: any, data: any) {
          // debugger
          if (eventName === "SERVER") {
            const ketquas = data.split("|");
            messageQueueToolKySo.push(ketquas);
            processQueueToolKySo();
            // const [returnCode, code, signedtext] = ketquas;
            // let hoaDonCode = code;
            // const isCodeBienBan = code.endsWith("_BB");
            // hoaDonCode = code.replace("_BB", "");
            // const hoaDonIdx = _refHoaDon.current.findIndex((x: any) => x.code === hoaDonCode);
            // const hoaDon = hoaDonIdx >= 0 ? _refHoaDon.current[hoaDonIdx] : undefined;
            // if (returnCode === "1" && hoaDon) {
            //   if (isCodeBienBan) {
            //     _refHoaDon.current[hoaDonIdx] = {
            //       ...hoaDon,
            //       bienBanSignedText: signedtext
            //     }
            //   } else {
            //     _refHoaDon.current[hoaDonIdx] = {
            //       ...hoaDon,
            //       status_id: 2,
            //       signedtext
            //     }
            //   }
            //   const hoaDonUpdated = hoaDonIdx >= 0 ? _refHoaDon.current[hoaDonIdx] : undefined;
            //   if (hoaDonUpdated) {
            //     if (hoaDonUpdated.status_id === 2) {
            //       if (hoaDonUpdated.hoa_don_base64 && hoaDonUpdated.signedtext) {
            //         if (!hoaDonUpdated.bien_ban_base64) {
            //           handleUpdateKySoSuccss(hoaDon.id, signedtext);
            //         } else {
            //           if (hoaDonUpdated.bienBanSignedText) {
            //             handleUpdateKySoSuccss(hoaDon.id, signedtext, hoaDonUpdated.bienBanSignedText);
            //           }
            //         }
            //       }
            //     }
            //   }
            //   setReRenderkey(createUUID());
            // }
            // debugger
            // if (_refHoaDon.current && hoaDon) {
            //   if (returnCode === "1") {
            //     _refHoaDon.current = _refHoaDon.current.map((x: any) => {
            //       if (x.code === code) {
            //         return {
            //           ...x,
            //           signedtext,
            //           status_id: 2,
            //         };
            //       }
            //       return {
            //         ...x,
            //       };
            //     });
            //     setReRenderkey(createUUID());

            //     handleUpdateKySoSuccss(hoaDon.id, signedtext);
            //   } else {
            //     NotifyHelper.Error("Có lỗi");
            //   }
            // }
          }
        },
      );
    }
  }, [_signalrConnected, _signalrConnection]);

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
        idx: 1,
      },
      {
        id: 1,
        name: "Đã khởi tạo",
        color: "grey",
        count: _refHoaDon.current.filter((x) => x.status_id === 1).length,
        per: 0,
        idx: 2,
      },
      {
        id: 2,
        name: "Đã ký số",
        color: "#8dc6fc",
        count: _refHoaDon.current.filter((x) => x.status_id === 2).length,
        per: 0,
        idx: 3,
      },
      {
        id: 6,
        name: "Ký số thất bại",
        color: "#ff0000",
        count: _refHoaDon.current.filter((x) => x.status_id === 6).length,
        per: 0,
        idx: 4,
      },
      {
        id: 3,
        name: "Đang gửi CQT",
        color: "#ffd78e",
        count: _refHoaDon.current.filter((x) => x.status_id === 3).length,
        per: 0,
        idx: 5,
      },
      {
        id: 4,
        name: "Đã phát hành",
        color: "#0cf478",
        count: _refHoaDon.current.filter((x) => x.status_id === 4).length,
        per: 0,
        idx: 6,
      },
      {
        id: 5,
        name: "Phát hành lỗi",
        color: "#ff0000",
        count: _refHoaDon.current.filter((x) => x.status_id === 5).length,
        per: 0,
        idx: 7,
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

  const messageQueue: any[] = [];
  let isUpdating = false;
  const xuLyMessage = (message: any) => {
    isUpdating = true;
    try {
      if (_refHoaDon.current.find((y) => y.id === message.id)) {
        _refHoaDon.current = _refHoaDon.current.map((x) => {
          if (x.id === message.id) {
            return {
              ...x,
              status_id:
                message.hoa_don_trang_thai_id === eHoaDonTrangThai.DA_PHAT_HANH
                  ? 4
                  : 5,
            };
          }
          return x;
        });
        setReRenderkey(createUUID());
      }
    } finally {
      isUpdating = false;
      processQueue();
    }
  };
  const processQueue = () => {
    if (isUpdating || messageQueue.length === 0) return;
    const nextMessage = messageQueue.shift();
    xuLyMessage(nextMessage);
  };
  console.log({
    signalRConnectionServer,
  });

  useEffect(() => {
    if (signalRConnectionServer) {
      signalRConnectionServer.on(
        "THONG_DIEP_HAS_RESULT",
        async (message: IHoaDonPhatHanhPushNotifyModel) => {
          console.log({
            THONG_DIEP_HAS_RESULT: message,
          });
          messageQueue.push(message);
          processQueue();
        },
      );
    }
  }, [signalRConnectionServer]);

  const handleUpdateKySoSuccss = async (
    hoaDonId: number,
    signedtext: string,
    bienBanSignedText?: string,
  ) => {
    const res = await hoaDonApi.updateKySoSuccess({
      signed_text: signedtext,
      id: hoaDonId,
      bienBanSignedText,
    });
    if (res.is_success) {
      handlePhatHanhAsync(hoaDonId, signedtext);
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
  };
  const handlePhatHanhAsync = async (hoaDonId: number, signedtext: string) => {
    const res = await hoaDonApi.phatHanh({
      signed_text: signedtext,
      id: hoaDonId,
    });
    if (res.is_success) {
      _refHoaDon.current = _refHoaDon.current.map((x: any) => {
        if (x.id === hoaDonId) {
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

  return (
    <Box>
      <Button
        text={title}
        leadingVisual={IssueClosedIcon}
        variant="primary"
        isLoading={isCreatingXml}
        onClick={async () => {
          if (isHoaDonCungNgay === false) {
            NotifyHelper.Error("Chỉ được ký hóa đơn trong cùng một ngày!");
            return;
          }

          if (props.isKhacNgay) {
            if (
              !(await confirm({
                content: (
                  <div>
                    <p>
                      Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi,
                      bổ sung một số điều của Nghị định số 123/2020/NĐ-CP ngày
                      19 tháng 10 năm 2020 của Chính phủ quy định về hóa đơn,
                      chứng từ), quy định: “Trường hợp hóa đơn điện tử đã lập có
                      thời điểm ký số trên hóa đơn khác thời điểm lập hóa đơn
                      thì thời điểm ký số và thời điểm gửi cơ quan thuế cấp mã
                      đối với hóa đơn có mã của cơ quan thuế hoặc thời điểm
                      chuyển dữ liệu hóa đơn điện tử đến cơ quan thuế đối với
                      hóa đơn điện tử không có mã của cơ quan thuế chậm nhất là
                      ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
                    </p>
                    <p>
                      Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn
                      tiếp tục ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
                    </p>
                  </div>
                ),
                title: "Lưu ý",
                cancelButtonContent: "Không ký",
                confirmButtonContent: "Tiếp tục ký số",
                confirmButtonType: "danger",
              }))
            ) {
              return;
            }
          }
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
            <Box
              sx={{
                mt: 3,
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              {(!serialNumber || serialNumber === "") && (
                <Box sx={{ display: "grid", gap: 2 }}>
                  <Box>Tài khoản chưa được gán serial</Box>
                  <Box sx={{ fontWeight: 600 }}>
                    Anh/Chị có thể chọn serial để tiếp tục ký số, Vui lòng đảm
                    bảo đã mở ứng dụng chữ ký số
                  </Box>
                  <Button
                    text="Chọn serial"
                    variant="primary"
                    onClick={_signalrSelectCert}
                  />
                </Box>
              )}
            </Box>
            {serialNumber && (
              <Box sx={{ pb: 3 }}>
                <ProgressBar aria-valuenow={100} aria-label="progress" animated>
                  {progressSource
                    .sort((a, b) => b.idx - a.idx)
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
            )}
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
                    .sort((a, b) => a.idx - b.idx)
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

export default HoaDonKySoPhatHanhMultipleTool;
