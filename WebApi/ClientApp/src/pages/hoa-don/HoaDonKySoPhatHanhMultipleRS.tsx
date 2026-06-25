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
import { hoaDonKyLoApi } from "../../api/hoa-don/hoaDonKyLoApi";
interface IHoaDonKySoPhatHanhMultipleRSProps {
  ids: number[];
  onClose: () => void;
  isKhacNgay?: boolean;
  title?: string;
  isHoaDonCungNgay?: boolean;
}
const HoaDonKySoPhatHanhMultipleRS = (
  props: IHoaDonKySoPhatHanhMultipleRSProps
) => {
  const { title = "Ký số và gửi cấp mã", isHoaDonCungNgay = true } = props;
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

  const confirm = useConfirm();

  const handleCreateXmlKySoAsync = async () => {
    if (isHoaDonCungNgay === false) {
      NotifyHelper.Error("Chỉ được ký hóa đơn trong cùng một ngày!");
      return;
    }

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
    const res = await hoaDonKyLoApi.createXmlKySos({
      ids: props.ids,
      progress_id: "",
    });
    // setIsCreatingXml(false)
    if (res.is_success) {
      _refHoaDon.current = res.data.map((x: any) => {
        return {
          ...x,
          status_id: 1,
          code: createUUID().replace(/-/g, ""),
        };
      });
      setReRenderkey(createUUID());
      // SendToToolKySo()
    } else {
      NotifyHelper.Error(res.message ?? "Ký số phát hành thất bại");
    }
  };

  const progressSource = useMemo(() => {
    console.log({
      xxxx: _refHoaDon.current,
    });

    const result = [
      {
        id: 2,
        name: "Đã ký số",
        color: "#8dc6fc",
        count: _refHoaDon.current.filter((x) => x.status_id === 2).length,
        per: 0,
      },
      {
        id: 3,
        name: "Đã gửi CQT",
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
                let status_id = 0;
                if (
                  message.hoa_don_trang_thai_id ===
                  eHoaDonTrangThai.CHUA_GUI_CQT
                )
                  status_id = 2;
                if (
                  message.hoa_don_trang_thai_id ===
                  eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU
                )
                  status_id = 3;
                if (
                  message.hoa_don_trang_thai_id ===
                  eHoaDonTrangThai.DA_PHAT_HANH
                )
                  status_id = 4;
                if (
                  message.hoa_don_trang_thai_id ===
                  eHoaDonTrangThai.KHONG_HOP_LE
                )
                  status_id = 5;
                if (
                  message.hoa_don_trang_thai_id ===
                  eHoaDonTrangThai.LOI_THONG_DIEP
                )
                  status_id = 5;
                return {
                  ...x,
                  status_id: status_id,
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
  return (
    <Box>
      <Button
        text={title}
        leadingVisual={IssueClosedIcon}
        variant="primary"
        isLoading={isCreatingXml}
        onClick={async () => {
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
                    display: "grid",
                    gap: 2,
                    // justifyContent: "space-between"
                  }}
                >
                  {progressSource
                    .sort((a, b) => a.id - b.id)
                    .map((x) => {
                      return (
                        <Box sx={{ display: "flex" }} key={x.id}>
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
            {!isCreatingXml && (
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

export default HoaDonKySoPhatHanhMultipleRS;