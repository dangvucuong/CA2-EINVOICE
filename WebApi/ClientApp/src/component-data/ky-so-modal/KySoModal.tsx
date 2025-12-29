import { Box } from "@primer/react";
import { useEffect, useMemo, useRef, useState } from "react";
import Button from "../../component-ui/button/Button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import { useCommonContext } from "../../contexts/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import Text from "../../component-ui/text";
import { DownloadIcon } from "@primer/octicons-react";
import { userApi } from "../../api/user/userApi";

interface IKySoModalProps {
  base64: string;
  base64BienBan?: string;
  onSuccess: (signedtext: string, bienBanSignedText?: string) => void;
  onClose: () => void;
}
const KySoModal = (props: IKySoModalProps) => {
  const { base64, onClose, onSuccess } = props;
  const { user } = useAuth();
  const { createUUID } = useCommonContext();
  const {
    _signalrHubProxy,
    _signalrConnected,
    _signalrSelectCert,
    isSignalRReady,
    reconnectSignalR,
  } = useCommonContext();
  const [serialNumber, setSerialNumber] = useState("");
  const [kySoSuccessHoaDonSuccessResult, setkySoSuccessHoaDonSuccessResult] =
    useState();
  const [kySoBienBanSuccessResult, setKySoBienBanSuccessResult] = useState();
  useEffect(() => {
    if (props.base64BienBan) {
      if (kySoSuccessHoaDonSuccessResult && kySoBienBanSuccessResult) {
        if ((user?.serial_number ?? "") === "" && serialNumber) {
          hanldeUpdateSerialNumerIfEmpty();
        }
        onSuccess(kySoSuccessHoaDonSuccessResult, kySoBienBanSuccessResult);
      }
    } else {
      if (kySoSuccessHoaDonSuccessResult) {
        if ((user?.serial_number ?? "") === "" && serialNumber) {
          hanldeUpdateSerialNumerIfEmpty();
        }
        onSuccess(kySoSuccessHoaDonSuccessResult);
      }
    }
  }, [
    kySoSuccessHoaDonSuccessResult,
    kySoBienBanSuccessResult,
    props.base64BienBan,
    onSuccess,
    serialNumber,
    user,
  ]);
  // const {
  //   _signalrConnected,
  //   createUUID,
  //   _signalrHubProxy,
  //   _signalrSelectCert,
  //   _signalrSignLogin,
  //   getMSTFromCertSubject,
  // } = useCommonContext();
  useEffect(() => {
    setSerialNumber(user?.serial_number ?? "");
  }, [user]);
  const [selectedTab, setSelectedTab] = useState<"usb" | "remote_siging">(
    localStorage.getItem("ky_so_mode") === "remote_siging"
      ? "remote_siging"
      : "usb"
  );
  useEffect(() => {
    localStorage.setItem("ky_so_mode", selectedTab);
  }, [selectedTab]);
  const isDisabled = useMemo(() => {
    if (!user) return true;
    if (selectedTab === "usb" && _signalrConnected) return false;
    if (
      selectedTab === "remote_siging" &&
      user.is_serial_remote_signing_verified === true &&
      user.serial_remote_signing_numner
    )
      return false;
    return true;
  }, [selectedTab, user, _signalrConnected]);
  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        console.log({
          data,
        });
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
      });
    }
  }, [_signalrConnected, _signalrHubProxy]);

  const _codeRef = useRef<any>();
  const _codeRefBienBan = useRef<any>();
  const hanldeUpdateSerialNumerIfEmpty = async () => {
    const res = await userApi.updateSerialNumber({
      serial: serialNumber,
    });
    if (res.is_success) {
      NotifyHelper.Success("Đã cập nhật số serila cho user");
    }
  };
  function Send() {
    try {
      var code = createUUID().replace(/-/g, "");
      _codeRef.current = code;
      // const code = '95182a97-5f81-4928-9101-dccb14b9a336';

      var content = code + "|" + serialNumber + "|" + base64 + "|XML";

      // if (!isSignalRReady()) {
      //   NotifyHelper.Error("Chưa kết nối server. Đang kết nối lại...");
      //   reconnectSignalR();
      //   return; // KHÔNG SEND, chờ reconnect
      // }
      // debugger
      _signalrHubProxy
        .invoke("send", content)
        .done(function () {
          // console.log({
          //   sendSuccess: content,
          // });
          CheckAndSendBienBan();
        })
        .fail(function (error: any) {
          NotifyHelper.Error("Có lỗi");
          console.log("Invocation failed. Error: " + error);
        });
    } catch (error) {
      window.location.reload();
    }
  }
  function CheckAndSendBienBan() {
    if ((props.base64BienBan ?? "") === "") return;
    try {
      var code = createUUID().replace(/-/g, "");
      _codeRefBienBan.current = code;
      // const code = '95182a97-5f81-4928-9101-dccb14b9a336';

      var content =
        code +
        "|" +
        serialNumber +
        "|" +
        (props.base64BienBan ?? "") +
        "|XML|NDBBan|NBan";
      // debugger
      _signalrHubProxy
        .invoke("send", content)
        .done(function () {
          // console.log({
          //   sendSuccess: content,
          // });
        })
        .fail(function (error: any) {
          NotifyHelper.Error("Có lỗi");
          console.log("Invocation failed. Error: " + error);
        });
    } catch (error) {
      window.location.reload();
    }
  }

  useEffect(() => {
    console.log({
      _signalrConnected,
    });

    if (_signalrConnected) {
      const shownErrorCodes = new Set<string>();
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        if (eventName === "SERVER") {
          const ketquas = data.split("|");
          const [returnCode, code, signedtext] = ketquas;

          if (code === _codeRef.current) {
            if (returnCode === "1") {
              // onSuccess(signedtext);
              console.log({
                bienBan: 0,
                ketquas,
                signedtext,
              });
              setkySoSuccessHoaDonSuccessResult(signedtext);
            } else {
              if (!shownErrorCodes.has(code)) {
                NotifyHelper.Error(signedtext ?? "Có lỗi");
                shownErrorCodes.add(code);
              }
            }
          }
          if (code === _codeRefBienBan.current) {
            if (returnCode === "1") {
              // debugger
              console.log({
                bienBan: 1,
                ketquas,
                signedtext,
              });
              setKySoBienBanSuccessResult(signedtext);
              // onSuccess(signedtext);
            } else {
              if (!shownErrorCodes.has(code)) {
                NotifyHelper.Error(signedtext ?? "Có lỗi");
                shownErrorCodes.add(code);
              }
            }
          }
        }
      });
    }
  }, [_signalrConnected, _signalrHubProxy]);

  return (
    <Modal
      title={"Ký số"}
      onClose={() => {
        onClose();
      }}
      isOpen={true}
      width="xlarge"
      height={"auto"}
    >
      <Box>
        {/* <Box>
          <UnderlineNav
            aria-label="Repository"
            sx={{
              display: "flex",
              justifyContent: "center",
            }}
          >
            <UnderlineNav.Item
              aria-current={selectedTab === "usb" ? "page" : undefined}
              icon={PasskeyFillIcon}
              onClick={() => {
                setSelectedTab("usb");
              }}
            >
              Ký trực tiếp
            </UnderlineNav.Item>
            <UnderlineNav.Item
              aria-current={
                selectedTab === "remote_siging" ? "page" : undefined
              }
              icon={RssIcon}
              onClick={() => {
                setSelectedTab("remote_siging");
              }}
            >
              Remote signing
            </UnderlineNav.Item>
          </UnderlineNav>
        </Box> */}
        <Box
          sx={{
            mt: 3,
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          {selectedTab === "usb" && (!serialNumber || serialNumber === "") && (
            <Box sx={{ display: "grid", gap: 2 }}>
              <Box>Tài khoản chưa được gán serial</Box>
              <Box sx={{ fontWeight: 600 }}>
                Anh/Chị có thể chọn serial để tiếp tục ký số, Vui lòng đảm bảo
                đã mở ứng dụng chữ ký số
              </Box>
              <Button
                text="Chọn serial"
                variant="primary"
                onClick={_signalrSelectCert}
              />
            </Box>
          )}

          {/* {selectedTab === "usb" && <UsbSigingConfig />} */}
          {/* {selectedTab === "remote_siging" && <RemoteSigningConfig />} */}
          {serialNumber && serialNumber !== "" && !isDisabled && (
            <Box sx={{ fontWeight: 600 }}>Vui lòng ấn xác nhận để ký số</Box>
          )}

          {serialNumber && serialNumber !== "" && isDisabled && (
            <Box
              sx={{
                color: "red",
                flex: 2,
                textAlign: "center",
                fontSize: 14,
                fontWeight: "bold",
              }}
            >
              <Text
                text="Bạn chưa chạy tool ký số."
                sx={{
                  display: "block",
                }}
              ></Text>
              <Text
                text="Vui lòng chạy tool ký số để sử dụng tính năng này."
                sx={{
                  display: "block",
                }}
              ></Text>

              <Box
                id="right"
                sx={{
                  display: "flex",
                  textAlign: "center",
                  justifyContent: "center",
                  mt: 4,
                }}
              >
                <Button
                  text="Tải về tool ký số"
                  variant="primary"
                  leadingVisual={DownloadIcon}
                  size="medium"
                  sx={{}}
                  onClick={() => {
                    window.open(
                      "https://hsdt.nacencomm.vn/downloads/setup.msi",
                      "_blank"
                    );
                  }}
                />
              </Box>
            </Box>
          )}
        </Box>
        <ModalActions>
          <Button
            onClick={() => {
              onClose();
            }}
            text="Đóng"
          />
          <Button
            variant="primary"
            onClick={() => {
              if (
                selectedTab === "usb" &&
                serialNumber &&
                serialNumber !== ""
              ) {
                Send();
              }
            }}
            text={"Xác nhận"}
            disabled={isDisabled}
          />
        </ModalActions>
      </Box>
    </Modal>
  );
};

export default KySoModal;
