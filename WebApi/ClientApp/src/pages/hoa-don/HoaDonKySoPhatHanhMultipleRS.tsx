import { IssueClosedIcon } from "@primer/octicons-react";
import { Box, Label, ProgressBar, Spinner, useConfirm } from "@primer/react";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
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

interface IHoaDonKySoTrackItem {
  id: number;
  status_id: number;
  code?: string;
  xml_base64?: string;
}

const mapTrangThaiToStatusId = (trangThaiId: number): number => {
  if (trangThaiId === eHoaDonTrangThai.CHUA_GUI_CQT) return 2;
  if (
    trangThaiId ===
    eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU
  )
    return 3;
  if (trangThaiId === eHoaDonTrangThai.DA_PHAT_HANH) return 4;
  if (
    trangThaiId === eHoaDonTrangThai.KHONG_HOP_LE ||
    trangThaiId === eHoaDonTrangThai.LOI_THONG_DIEP
  )
    return 5;
  return 0;
};

const HoaDonKySoPhatHanhMultipleRS = (
  props: IHoaDonKySoPhatHanhMultipleRSProps
) => {
  const { title = "Ký số và gửi cấp mã", isHoaDonCungNgay = true } = props;
  const [isCreatingXml, setIsCreatingXml] = useState(false);
  const [isShowProgess, setIsShowProgess] = useState(false);
  const { createUUID, signalRConnectionServer } = useCommonContext();
  const [reRenderKey, setReRenderkey] = useState("");

  const _refHoaDon = useRef<IHoaDonKySoTrackItem[]>([]);
  const _signedBatchNotifiedRef = useRef(false);
  const _finalBatchNotifiedRef = useRef(false);

  const initTrackingList = useCallback(() => {
    _refHoaDon.current = props.ids.map((x) => ({
      id: x,
      status_id: 0,
    }));
    _signedBatchNotifiedRef.current = false;
    _finalBatchNotifiedRef.current = false;
    setReRenderkey(createUUID());
  }, [props.ids, createUUID]);

  useEffect(() => {
    initTrackingList();
  }, [initTrackingList]);

  const notifyBatchProgress = useCallback(() => {
    const items = _refHoaDon.current;
    const total = items.length;
    if (total === 0) return;

    const signedCount = items.filter((x) => x.status_id >= 2).length;
    const publishedCount = items.filter((x) => x.status_id === 4).length;
    const errorCount = items.filter((x) => x.status_id === 5).length;
    const allSigned = items.every((x) => x.status_id >= 2);
    const allTerminal = items.every(
      (x) => x.status_id === 4 || x.status_id === 5
    );

    if (allSigned && !_signedBatchNotifiedRef.current) {
      _signedBatchNotifiedRef.current = true;
      NotifyHelper.Success(`Đã ký số ${signedCount}/${total} hóa đơn.`);
    }

    if (allTerminal && !_finalBatchNotifiedRef.current) {
      _finalBatchNotifiedRef.current = true;
      if (errorCount > 0) {
        NotifyHelper.Error(
          `Hoàn tất: ${publishedCount} phát hành thành công, ${errorCount} hóa đơn lỗi.`
        );
      } else {
        NotifyHelper.Success(
          `Phát hành thành công ${publishedCount}/${total} hóa đơn.`
        );
      }
    }
  }, []);

  const confirm = useConfirm();

  const handleCreateXmlKySoAsync = async () => {
    if (isHoaDonCungNgay === false) {
      NotifyHelper.Error("Chỉ được ký hóa đơn trong cùng một ngày!");
      return;
    }

    setIsShowProgess(true);
    setIsCreatingXml(true);
    try {
      const res = await hoaDonKyLoApi.createXmlKySos({
        ids: props.ids,
        progress_id: "",
      });

      if (res.is_success) {
        const errRes = (res.data ?? []).filter(
          (x: any) => x.is_success === false
        );
        if (errRes.length > 0) {
          NotifyHelper.Error(
            errRes[0]?.message ?? "Có hóa đơn không tạo được XML ký số"
          );
        }

        const successItems = (res.data ?? []).filter(
          (x: any) => x.is_success !== false && x.xml_base64
        );

        if (successItems.length > 0) {
          _refHoaDon.current = successItems.map((x: any) => ({
            ...x,
            status_id: 1,
            code: createUUID().replace(/-/g, ""),
          }));
        } else {
          // RS nền / MTT bảng kê: API trả data rỗng — giữ danh sách theo dõi
          _refHoaDon.current = props.ids.map((id) => ({
            id,
            status_id: 1,
          }));
          NotifyHelper.Success(
            "Đã gửi yêu cầu ký số. Vui lòng xác nhận trên ứng dụng Remote Signing."
          );
        }
        setReRenderkey(createUUID());
      } else {
        NotifyHelper.Error(res.message ?? "Ký số phát hành thất bại");
      }
    } finally {
      setIsCreatingXml(false);
    }
  };

  const progressSource = useMemo(() => {
    const total = _refHoaDon.current.length || 1;
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
      .map((x) => ({
        ...x,
        per: (x.count * 100) / total,
      }));
  }, [_refHoaDon.current, reRenderKey]);

  useEffect(() => {
    if (!signalRConnectionServer) return;

    const handler = (message: IHoaDonPhatHanhPushNotifyModel) => {
      if (!_refHoaDon.current.find((y) => y.id === message.id)) return;

      const status_id = mapTrangThaiToStatusId(message.hoa_don_trang_thai_id);
      _refHoaDon.current = _refHoaDon.current.map((x) => {
        if (x.id !== message.id) return x;
        return { ...x, status_id };
      });

      if (status_id === 5 && message.ket_qua_phat_hanh) {
        NotifyHelper.Error(
          message.ket_qua_phat_hanh || "Phát hành hóa đơn thất bại"
        );
      }

      setReRenderkey(createUUID());
      notifyBatchProgress();
    };

    signalRConnectionServer.on("THONG_DIEP_HAS_RESULT", handler);
    return () => {
      signalRConnectionServer.off("THONG_DIEP_HAS_RESULT", handler);
    };
  }, [signalRConnectionServer, notifyBatchProgress]);

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
