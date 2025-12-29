import { PlusIcon, TrashIcon } from "@primer/octicons-react";

import { Box, IconButton, TextInput } from "@primer/react";
import SelectBoxHoaDon from "../../component-data/selectbox-hoa-don";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading";
import { eSize } from "../../models/commons/eSize";
import moment from "moment";
interface IThongBaoSaiSotDSChungTuProps {
  data: any[];
  onValueChanged: (data: any[]) => void;
  allowSelect?: boolean;
}
const PlusIconAccent = () => {
  return (
    <Box sx={{ color: "accent.fg" }}>
      <PlusIcon />
    </Box>
  );
};
const ThongBaoSaiSotDSChungTu = (props: IThongBaoSaiSotDSChungTuProps) => {
  const { data = [] } = props;
  console.log(data);

  return (
    <Box>
      <Box sx={{ mb: 3, display: "flex", alignItems: "center" }}>
        <Box sx={{ mr: 2, flex: 1 }}>
          <Heading
            text="Danh sách chứng từ cần thông báo"
            size={eSize.medium}
          />
        </Box>
      </Box>
      <Box>
        {/* <TextInputNumber /> */}
        <table className="myTable">
          <thead>
            <tr>
              <td style={{ textAlign: "center", width: "50px" }}>STT</td>
              <td>Ký hiệu mẫu số</td>
              <td>Ký hiệu chứng từ</td>
              <td>Số chứng từ</td>
              <td>Ngày lập chứng từ</td>
              <td>Lý do</td>
              <td style={{ textAlign: "center", width: "50px" }}></td>
            </tr>
          </thead>
          <tbody>
            {props.data.map((item, idx) => {
              return (
                <tr className="tr-no-padding" key={idx}>
                  <td style={{ textAlign: "center", width: "50px" }}>
                    {idx + 1}
                  </td>
                  <td>
                    {/* {item.hoa_don_dang_ky_phat_hanh_mau_so ?? ""} */}
                    <TextInput
                      className="noborder"
                      block
                      defaultValue={item.mau_so}
                      onChange={(e) => {
                        props.onValueChanged(
                          props.data.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                mau_so: e.target.value,
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
                    <TextInput
                      className="noborder"
                      block
                      defaultValue={item.ky_hieu}
                      onChange={(e) => {
                        props.onValueChanged(
                          props.data.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                ky_hieu: e.target.value,
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
                    <TextInput
                      className="noborder"
                      block
                      defaultValue={item.so_chung_tu}
                      onChange={(e) => {
                        props.onValueChanged(
                          props.data.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                so_chung_tu: e.target.value,
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
                    {/* {item.ngay_hoa_don ? moment(item.ngay_hoa_don).format("DD/MM/YYYY") : ""} */}
                    <TextInput
                      className="noborder"
                      block
                      type="date"
                      defaultValue={item.ngay_lap}
                      onChange={(e) => {
                        props.onValueChanged(
                          props.data.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                ngay_lap: e.target.value,
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
                    {/* {item.ma_cqt_cap} */}
                    <TextInput
                      className="noborder"
                      block
                      // type='date'
                      defaultValue={item.ly_do}
                      onChange={(e) => {
                        props.onValueChanged(
                          props.data.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                ly_do: e.target.value,
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
                  <td style={{ textAlign: "center" }}>
                    <Box
                      sx={{
                        m: -2,
                      }}
                    >
                      <IconButton
                        aria-label={`Remove: ${idx}`}
                        title={`Remove: ${idx}`}
                        icon={TrashIcon}
                        variant="invisible"
                        onClick={() => {
                          const arr = props.data.filter(
                            (x) => x?.so_chung_tu !== item?.so_chung_tu
                          );
                          props.onValueChanged(arr);
                        }}
                      />
                    </Box>
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
                    text="Thêm thủ công"
                    variant="invisible"
                    size="medium"
                    sx={{
                      color: "accent.fg",
                    }}
                    onClick={() => {
                      const fake: any = {};
                      props.onValueChanged([...props.data, fake]);
                    }}
                  />
                </Box>
              </td>
            </tr>
          </tbody>
        </table>
      </Box>
    </Box>
  );
};

export default ThongBaoSaiSotDSChungTu;
