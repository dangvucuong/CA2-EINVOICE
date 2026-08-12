import { IssueClosedIcon } from "@primer/octicons-react";
import { Box, useConfirm } from "@primer/react";
import { useEffect, useRef, useState } from "react";
import { hoaDonKyLoApi } from "../../api/hoa-don/hoaDonKyLoApi";
import ProcessModal from "../../component-data/process-modal";
import Button from "../../component-ui/button";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import {
  IProcessChangedModel,
  IProcessStepDataBase,
} from "../../models/responses/hub/IProcessChangedModel";
import HoaDonKySoPhatHanhMultipleTool from "./HoaDonKySoPhatHanhMultipleTool";
import HoaDonKySoPhatHanhMultipleRS from "./HoaDonKySoPhatHanhMultipleRS";
import { getSignalRErrorMessage } from "../../api/apiErrorHelper";
interface IHoaDonKySoPhatHanhMultipleProps {
  ids: number[];
  onClose: () => void;
  isKhacNgay?: boolean;
  title?: string;
  isHoaDonCungNgay?: boolean;
}
interface IHoaDonCreateXmlKySoRespone {
  id: number;
  xml_base64?: string;
  status_id?: number;
  code?: string;
  signedtext?: string;
}
const HoaDonKySoPhatHanhMultiple = (
  props: IHoaDonKySoPhatHanhMultipleProps
) => {
  const { title = "Ký số và gửi cấp mã", isHoaDonCungNgay = true } = props;
  const [isShowProgess, setIsShowProgess] = useState(false);
  const { _signalrHubProxy, _signalrConnected, createUUID } =
    useCommonContext();
  const { user } = useAuth();

  const [progress_id, setProgress_id] = useState("");
  const _refHoaDon = useRef<IHoaDonCreateXmlKySoRespone[]>([]);
  const _refProgress = useRef<IProcessChangedModel<IProcessStepDataBase>>();
  const [reRenderKey, setReRenderkey] = useState("");
  const [isChanged, setIsChanged] = useState(false);
  const confirm = useConfirm();
  useEffect(() => {
    _refHoaDon.current = props.ids.map((x) => {
      return {
        id: x,
        status_id: 0,
      };
    });
  }, [props.ids]);

  const handleCreateXmlKySoAsync = async () => {
    if (isHoaDonCungNgay === false) {
      NotifyHelper.Error("Chỉ được ký hóa đơn trong cùng một ngày!");
      return;
    }

    // ;
    if (props.isKhacNgay) {
      if (
        !(await confirm({
          content: (
            <div>
              <p>
                Điểm c, khoản 7, Điều 1, Nghị định 70/2025/NĐ-CP (sửa đổi, bổ
                sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19 tháng
                10 năm 2020 của Chính phủ quy định về hóa đơn, chứng từ), quy
                định: “Trường hợp hóa đơn điện tử đã lập có thời điểm ký số trên
                hóa đơn khác thời điểm lập hóa đơn thì thời điểm ký số và thời
                điểm gửi cơ quan thuế cấp mã đối với hóa đơn có mã của cơ quan
                thuế hoặc thời điểm chuyển dữ liệu hóa đơn điện tử đến cơ quan
                thuế đối với hóa đơn điện tử không có mã của cơ quan thuế chậm
                nhất là ngày làm việc tiếp theo kể từ thời điểm lập hóa đơn.”
              </p>
              <p>
                Hóa đơn của bạn đang có ngày ký khác ngày lập, bạn muốn tiếp tục
                ký gửi Thuế vui lòng click “Xác nhận và tiếp tục
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
    if (
      await confirm({
        content: `Bạn có chắc chắn muốn ký và phát hành ${props.ids.length} hóa đơn đã chọn`,
        title: `Lưu ý`,
        cancelButtonContent: "Đóng",
        confirmButtonContent: "Tiếp tục",
        confirmButtonType: "primary",
      })
    ) {
      setIsShowProgess(true);
      setIsChanged(true);
      const progress_id = createUUID();
      setProgress_id(progress_id);
      const res = await hoaDonKyLoApi.createXmlKySos({
        ids: props.ids,
        progress_id,
      });
      if (res.is_success) {
        const errRes = (res.data ?? []).filter((x: any) => x.is_success === false);
        if (errRes.length > 0) {
          NotifyHelper.Error(errRes[0]?.message ?? "Có hóa đơn không tạo được XML ký số");
        }
        const data = (res.data ?? [])
          .filter((x: any) => x.is_success !== false && x.xml_base64)
          .map((x: any) => {
            const item: IHoaDonCreateXmlKySoRespone = {
              id: x.id,
              xml_base64: x.xml_base64,
              code: createUUID().replace(/-/g, ""),
            };
            return item;
          });
        _refHoaDon.current = data;
        // SendToToolKySo();
      }
    }
  };

  const SendToToolKySo = () => {
    try {
      if (_refProgress.current) {
        _refProgress.current.processStatus.steps.forEach((step, idx) => {
          if (step.id === 2) {
            //bước ký số
            if (_refProgress.current != null) {
              _refProgress.current.is_finished = true;
              _refProgress.current.processStatus.steps[idx] = {
                ...step,
                data: {
                  ...step.data,
                  is_done: true,
                },
              };
            }
          }
        });
      }
      setReRenderkey(createUUID());
      if (!_signalrConnected) {
        NotifyHelper.Error("Chưa kết nối đến tool ký số");
        if (_refProgress.current) {
          _refProgress.current.processStatus.steps.forEach((step, idx) => {
            if (step.id === 2) {
              //bước ký số
              if (_refProgress.current != null)
                _refProgress.current.processStatus.steps[idx] = {
                  ...step,
                  data: {
                    ...step.data,
                    error: (step.data.error = step.data.total),
                  },
                };
            }
          });
        }
        setReRenderkey(createUUID());
        return;
      }
      _refHoaDon.current.forEach((hd: IHoaDonCreateXmlKySoRespone) => {
        var code = hd.code;
        hd.signedtext = "";
        var content =
          code + "|" + user?.serial_number + "|" + hd.xml_base64 + "|XML";
        _signalrHubProxy
          .invoke("send", content)
          .done(function () {})
          .fail(function (error: any) {
            NotifyHelper.Error(getSignalRErrorMessage(error));
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
            // setReRenderkey(createUUID())
            if (returnCode === "1") {
              _refHoaDon.current.forEach((x, index) => {
                if (x.code === code) {
                  _refHoaDon.current[index] = {
                    ...x,
                    signedtext,
                    status_id: 2,
                  };
                }
              });
              if (_refProgress.current) {
                _refProgress.current.processStatus.steps.forEach(
                  (step, idx) => {
                    if (step.id === 2) {
                      //bước ký số
                      if (_refProgress.current != null)
                        _refProgress.current.processStatus.steps[idx] = {
                          ...step,
                          data: {
                            ...step.data,
                            success: (step.data.success += 1),
                          },
                        };
                    }
                  }
                );
                setReRenderkey(createUUID());
              }
            } else {
              NotifyHelper.Error(signedtext?.trim() || "Ký số thất bại");
              if (_refProgress.current) {
                _refProgress.current.processStatus.steps.forEach(
                  (step, idx) => {
                    if (step.id === 2) {
                      //bước ký số
                      if (_refProgress.current != null)
                        _refProgress.current.processStatus.steps[idx] = {
                          ...step,
                          data: {
                            ...step.data,
                            error: (step.data.error += 1),
                          },
                        };
                    }
                  }
                );
                setReRenderkey(createUUID());
              }
            }
          }
        }
      });
    }
  }, [_signalrConnected, _signalrHubProxy]);
  //   ;

  if (user && user.is_remote_signing) {
    return (
      <HoaDonKySoPhatHanhMultipleRS
        ids={props.ids}
        onClose={props.onClose}
        isKhacNgay={props.isKhacNgay}
        isHoaDonCungNgay={isHoaDonCungNgay}
        title={title}
      />
    );
  }
  if (user && !user.is_hsm_signing) {
    return (
      <HoaDonKySoPhatHanhMultipleTool
        ids={props.ids}
        onClose={props.onClose}
        isKhacNgay={props.isKhacNgay}
        isHoaDonCungNgay={isHoaDonCungNgay}
        title={title}
      />
    );
  }

  return (
    <Box>
      <Button
        text={title}
        leadingVisual={IssueClosedIcon}
        variant="primary"
        // isLoading={isCreatingXml}
        onClick={handleCreateXmlKySoAsync}
      />
      {isShowProgess && (
        <ProcessModal
          process_id={progress_id}
          onClose={() => {
            setIsShowProgess(false);
            if (isChanged) {
              props.onClose();
            }
          }}
          // dataSourceUpdated={_refProgress.current}
          onDatasourceChanged={(data) => {
            _refProgress.current = data;
          }}
          descriptionText={`Anh/chị có thể đóng cửa sổ này. Kết quả từ CQT trả về sẽ được hệ thống cập nhật tự động`}
        />
      )}
    </Box>
  );
};

export default HoaDonKySoPhatHanhMultiple;
