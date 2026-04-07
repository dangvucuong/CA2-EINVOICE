import { PlusIcon, TrashIcon } from '@primer/octicons-react';
import { Box, FormControl, IconButton } from '@primer/react';
import { Controller } from 'react-hook-form';
import SelectBoxThueSuat from '../../component-data/selectbox-thue-suat';
import SelectBoxTinhChatHangHoa from '../../component-data/selectbox-tinh-chat-hang-hoa';
import TextInputMaHangHoa from '../../component-data/text-ma-hang-hoa-search';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import TextInput from '../../component-ui/text-input';
import TextInputNumber from '../../component-ui/text-input-number/TextInputNumber';
import { eSize } from '../../models/commons/eSize';
import { IHoaDonHangHoa } from '../../models/responses/hoa-don/IHoaDonHangHoa';
import { numberWithCommas } from '../../helpers/common';
import { useEffect } from 'react';

interface IHangHoaDieuChinhThueProps {
  tienTe?: string;
  hangHoas: IHoaDonHangHoa[];
  isHoaDonBanHang: boolean;
  isSoAm?: boolean;
  limit?: number;
  onValueChanged: (hangHoas: IHoaDonHangHoa[]) => void;
  control: any;
  watch: any;
  error: any;
  giam_thue_ty_le: number;
  onGiamThueTyLeChanged: (ty_le: number) => void;
}

const PlusIconAccent = () => {
  return (
    <Box sx={{ color: "accent.fg" }}>
      <PlusIcon />
    </Box>
  );
};
const HangHoaDieuChinhThue = (props: IHangHoaDieuChinhThueProps) => {
  const { hangHoas } = props;
  const setHangHoas = (hangHoas: IHoaDonHangHoa[]) => {
    props.onValueChanged(hangHoas);
  };
  // useEffect(() => {
  //   if (props.hangHoas) {

  //     setHangHoas(props.hangHoas.map(h => ({ ...h, so_luong: 0, don_gia: 0, thanh_tien: 0 })))
  //   }
  // }, [])
  const tong_tien_thue = props.watch("tong_tien_thue")
  return (
    <Box>
      <Box sx={{ mr: 2, flex: 1 }}>
        <Heading text="Danh sách hàng hóa" size={eSize.medium} />
        <Box sx={{ mt: 2 }}>
          <table className="myTable">
            <thead>
              <tr>
                <td style={{ width: "50px" }}></td>
                <td style={{ textAlign: "center", width: "50px" }}>STT</td>
                <td style={{ width: "120px" }}>Mã hàng hóa</td>
                <td style={{ minWidth: "200px" }}>Tên hàng hóa</td>
                <td style={{ width: "150px" }}>Tính chất</td>
                <td style={{ width: "80px" }}>ĐVT</td>
                <td style={{ width: "93px" }}>Thuế suất</td>
              </tr>
            </thead>
            <tbody>
              {hangHoas.map((hangHoa, idx) => {
                return (
                  <tr key={idx} className="tr-no-padding">
                    <td style={{ width: 50 }}>
                      <IconButton
                        icon={TrashIcon}
                        aria-label={`Delete:`}
                        title={`Delete:`}
                        variant="invisible"
                        onClick={() => {
                          let arr = [...hangHoas];
                          arr.splice(idx, 1);
                          setHangHoas(arr);
                        }}
                      />
                    </td>
                    <td style={{ textAlign: "center", width: "50px" }}>
                      <>{idx + 1}</>
                    </td>
                    <td>
                      <TextInputMaHangHoa
                        className="noborder"
                        value={hangHoa.ma_hang}
                        onValueChanged={(data) => {
                          if (data.hang_hoa) {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    ma_hang: data.text,
                                    ten_hang: data.hang_hoa?.ten_hang_hoa ?? "",
                                    dvt: data.hang_hoa?.dvt ?? "",
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          } else {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    ma_hang: data.text,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          }
                        }}
                      />
                    </td>
                    <td>
                      <TextInput
                        block
                        value={hangHoa.ten_hang}
                        className="noborder"
                        onChange={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  ten_hang: e.target.value,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>
                    <td>
                      <SelectBoxTinhChatHangHoa
                        sx={{
                          border: 0,
                          boxShadow: "none",
                        }}
                        onValueChanged={(id) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  hang_hoa_tinh_chat_id: id,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                        value={hangHoa.hang_hoa_tinh_chat_id}
                      />
                    </td>
                    <td>
                      <TextInput
                        className="noborder"
                        defaultValue={hangHoa.dvt}
                        onChange={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  dvt: e.target.value,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>

                    <td>
                      <SelectBoxThueSuat
                        sx={{
                          border: 0,
                          boxShadow: "none",
                        }}
                        isReadOnly={props.isHoaDonBanHang}
                        onValueChanged={(id) => {
                          if (!props.isHoaDonBanHang) {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    thue_vat: id,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          }
                        }}
                        value={
                          props.isHoaDonBanHang ? "0%" : hangHoa.thue_vat ?? ""
                        }
                      />
                    </td>



                  </tr>
                );
              })}
              <tr>
                <td colSpan={7}>
                  <Box
                    sx={{
                      width: "100%",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                    }}
                  >
                    <Button
                      leadingVisual={PlusIconAccent}
                      text="Thêm hàng hóa"
                      variant="invisible"
                      size="medium"
                      sx={{
                        color: "accent.fg",
                      }}
                      onClick={() => {
                        const newHangHoa: any = {
                          hang_hoa_tinh_chat_id: 1,
                          ty_le_chiet_khau: 0,
                        };
                        setHangHoas([...hangHoas, newHangHoa]);
                      }}
                    />
                  </Box>
                </td>
              </tr>
              <tr>
                <td colSpan={3}>Cộng tiền VAT:</td>

                <td colSpan={6} style={{ textAlign: "right" }}>
                  <Controller
                    control={props.control}
                    rules={{
                      required: true
                      // validate: (value) => {
                      //   const tong_tang_giam =
                      //     (value ? parseInt(value) : 0) +
                      //     so_tien_tang_giam +
                      //     so_tien_tang_giam_tien_thue;
                      //   if (value) {
                      //     if (value < -5 || value > 5) {
                      //       return "Số tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                      //     }
                      //     if (tong_tang_giam < -5 || tong_tang_giam > 5) {
                      //       return "Tổng tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                      //     }
                      //   }
                      //   return true;
                      // },
                    }}
                    name="tong_tien_thue"
                    render={({ field }) => {
                      return (

                        <Box
                          sx={{
                            display: "flex",
                            gap: 2,
                            justifyContent: "flex-end",
                            width: "100%"
                          }}
                        >
                          <TextInputNumber
                            placeholder='Cộng tiền VAT'
                            defaultValue={field.value}
                            onValueChanged={(value) => {
                              field.onChange(value);
                            }}
                          // value={field.value}
                          // onChange={(e) => {
                          //   field.onChange(e);
                          // }}
                          />
                          {props.error &&
                            props.error["tong_tien_thue"] && (
                              <FormControl.Validation variant="error"></FormControl.Validation>
                            )}
                        </Box>
                      );
                    }}
                  />
                </td>
              </tr>
              <tr>
                <td colSpan={3}>Cộng tiền hàng:</td>
                <td
                  colSpan={6}
                  style={{
                    textAlign: "right",
                    fontWeight: "600",
                    paddingRight: "24px"
                  }}>
                  {numberWithCommas(tong_tien_thue ?? 0)}
                </td>
              </tr>
            </tbody>
          </table>
        </Box>
      </Box>
    </Box>
  );
};

export default HangHoaDieuChinhThue;