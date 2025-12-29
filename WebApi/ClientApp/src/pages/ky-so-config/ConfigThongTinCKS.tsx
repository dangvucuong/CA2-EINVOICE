import React, { useEffect, useState } from "react";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { rootAction } from "../../state/actions/rootAction";
import { useAppSelector } from "../../hooks/useAppSelector";
import { toKhaiApi } from "../../api/to-khai/toKhaiApi";
import moment from "moment";
import Heading from "../../component-ui/heading";
import { Box } from "@primer/react";
import { DataTable } from "@primer/react/lib-esm/DataTable";

function ConfigThongTinCKS() {
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
      sx={{
        display: "flex",
        flexDirection: "column",
        borderRadius: 2,
        border: "1px",
        borderStyle: "solid",
        borderColor: "border.default",
        p: 3,
        // pb: 4,
        // pt: 4,
        width: "500px",
        // height:"200px",
        justifyContent: "center",
        mt: 3,
      }}
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

        {/* {list_cts?.length !== 0 &&
        list_cts?.map((item: any, index: number) => (
          <Box key={index} sx={{ mb: 3 }}></Box>
        ))} */}

        <table></table>
      </Box>
    </Box>
  );
}

export default ConfigThongTinCKS;
