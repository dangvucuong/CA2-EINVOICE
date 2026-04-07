import { Box } from "@primer/react";
import { memo, useEffect, useMemo, useState } from "react";
import Heading from "../../component-ui/heading";
import { useAuth } from "../../hooks/useAuth";
import styles from "./ThongTinDonvi.module.css";
import { useAppSelector } from "../../hooks/useAppSelector";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { rootAction } from "../../state/actions/rootAction";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { toKhaiApi } from "../../api/to-khai/toKhaiApi";
import moment from "moment";
import Button from "../../component-ui/button";
const ThongTinCKS = () => {
  const { status, toKhais } = useAppSelector((x) => x.toKhai.toKhaiReducer);
  const [list_cts, setListCts] = useState<any>([]);
  const dispatch = useAppDispatch();

  useEffect(() => {
    const dstokhaidaguithue = toKhais
      ?.filter((x) => x.to_khai_status_id === 4)
      .sort(
        (a, b) =>
          new Date(b?.ngay_lap).getTime() - new Date(a?.ngay_lap).getTime()
      );

    if (dstokhaidaguithue?.length > 0) {
      handleGetDetailByIdAsync(dstokhaidaguithue);
    }
  }, [toKhais]);

  useEffect(() => {
    if (
      status === eReducerStatusBase.is_not_initialization ||
      status === eReducerStatusBase.is_need_reload
    ) {
      dispatch(rootAction.toKhai.toKhaiAction.loadStart());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [status]);

  const handleGetDetailByIdAsync = async (dstokhaidaguithue: any) => {
    // Chỉ lấy tờ khai mới nhất thay vì tất cả
    const latestToKhai = dstokhaidaguithue[0]; // Đã sort theo ngày giảm dần

    const res = await toKhaiApi.getViewModel(latestToKhai?.id);

    if (res.is_success && res?.data?.list_cts) {
      const listCks = res.data.list_cts.map((item: any) => ({
        not_after: moment(item.not_after).format("DD/MM/YYYY"),
        not_after_raw: item.not_after,
        serial_number: item.serial_number,
        ten_cks: item.subject
          .split(",")
          .map((x: any) => x.trim())[0]
          .replace("CN=", ""),
      }));

      setListCts(listCks);
      return listCks;
    }

    setListCts([]);
    return [];
  };

  return (
    <Box
      sx={
        {
          // width: "400px"
        }
      }
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Box sx={{ flex: 1, display: "flex", flexDirection: "column", mb: 2 }}>
          <Heading text="Thông tin chữ ký số" />
        </Box>

        {list_cts?.length !== 0 &&
          list_cts?.map((item: any, index: number) => (
            <Box key={index} sx={{ mb: 3 }}>
              <table>
                <tr className={styles.tr}>
                  <td className={styles.labelTd}>Tên chứng thư</td>
                  <td>{<b>{item?.ten_cks}</b>}</td>
                </tr>
                <tr className={styles.tr}>
                  <td className={styles.labelTd}>Số serial</td>
                  <td>{<b>{item?.serial_number}</b>}</td>
                </tr>

                <tr className={styles.tr}>
                  <td className={styles.labelTd}>
                    <div>
                      <p
                        style={{
                          margin: 0,
                        }}
                      >
                        Ngày hết hạn
                      </p>
                      {moment(item?.not_after).isBefore(moment()) && (
                        <span style={{ color: "red", fontWeight: "bold" }}>
                          (CKS đã hết hạn)
                        </span>
                      )}
                      {moment(item?.not_after).isAfter(moment()) &&
                        moment(item?.not_after).diff(moment(), "months") <
                          3 && (
                          <span style={{ color: "red", fontWeight: "bold" }}>
                            (CKS sắp hết hạn)
                          </span>
                        )}
                    </div>
                  </td>
                  <td>
                    <Box
                      sx={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                      }}
                    >
                      <b>{item?.not_after}</b>

                      {/* nếu còn 3 tháng hoặc hết hạn thì mới hiển thị nút gia hạn  */}
                      {(moment(item?.not_after).isBefore(moment()) ||
                        (moment(item?.not_after).isAfter(moment()) &&
                          moment(item?.not_after).diff(moment(), "months") <
                            3)) && (
                        <Button
                          text="Gia hạn Chữ ký số"
                          size="small"
                          sx={{
                            ml: 3,
                            backgroundColor: "red",
                            color: "white",
                          }}
                          onClick={() => {
                            window.open("https://nacencomm.vn/", "_blank");
                          }}
                        />
                      )}
                    </Box>
                  </td>
                </tr>
              </table>
            </Box>
          ))}
      </Box>
    </Box>
  );
};

export default memo(ThongTinCKS);
