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
import ChungTuKySoPhatHanhMultipleTool from "./ChungTuKySoPhatHanhMultipleTool";
// import HoaDonKySoPhatHanhMultipleTool from "./HoaDonKySoPhatHanhMultipleTool";
// import HoaDonKySoPhatHanhMultipleRS from "./HoaDonKySoPhatHanhMultipleRS";
interface IChungTuKySoPhatHanhMultipleProps {
  ids: number[];
  onClose: () => void;
}
interface IHoaDonCreateXmlKySoRespone {
  id: number;
  xml_base64?: string;
  status_id?: number;
  code?: string;
  signedtext?: string;
}
const ChungTuKySoPhatHanhMultiple = (
  props: IChungTuKySoPhatHanhMultipleProps,
) => {
  const [isShowProgess, setIsShowProgess] = useState(false);
  const { _signalrConnection, _signalrConnected, createUUID } =
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
      // const res = await hoaDonKyLoApi.createXmlKySos({
      //   ids: props.ids,
      //   progress_id,
      // });
      // if (res.is_success) {
      //   const data = res.data.map((x: any) => {
      //     const item: IHoaDonCreateXmlKySoRespone = {
      //       id: x.id,
      //       xml_base64: x.xml_base64,
      //       code: createUUID().replace(/-/g, ""),
      //     };
      //     return item;
      //   });
      //   _refHoaDon.current = data;
      //   // SendToToolKySo();
      // }
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
                    },
                  );
                  setReRenderkey(createUUID());
                }
              } else {
                NotifyHelper.Error("Có lỗi");
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
                    },
                  );
                  setReRenderkey(createUUID());
                }
              }
            }
          }
        },
      );
    }
  }, [_signalrConnected, _signalrConnection]);

  // if (user && user.is_remote_signing) {
  //   return (
  //     <HoaDonKySoPhatHanhMultipleRS ids={props.ids} onClose={props.onClose} />
  //   );
  // }
  if (user && !user.is_hsm_signing) {
    return (
      <ChungTuKySoPhatHanhMultipleTool
        ids={props.ids}
        onClose={props.onClose}
      />
    );
  }

  return (
    <Box>
      <Button
        text="Ký số và gửi cấp mã"
        leadingVisual={IssueClosedIcon}
        variant="primary"
        // isLoading={isCreatingXml}
        onClick={handleCreateXmlKySoAsync}
        sx={{
          height: 32,
        }}
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

export default ChungTuKySoPhatHanhMultiple;
