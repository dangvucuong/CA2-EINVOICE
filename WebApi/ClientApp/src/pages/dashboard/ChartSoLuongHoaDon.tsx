import { Box, Link } from "@primer/react";
import { CreditCardIcon } from "@primer/octicons-react";
import React, { useEffect, useState } from "react";
import Heading from "../../component-ui/heading";
import { Doughnut } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import ChartDataLabels from "chartjs-plugin-datalabels";
import Button from "../../component-ui/button";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { useDebounce } from "use-debounce";
import PlaceHolder from "../../component-ui/place-holder";
import LichSuMuaChuKySoModal from "./LichSuMuaChuKySoModal";
ChartJS.register(ArcElement, Tooltip, Legend, ChartDataLabels);
const ChartSoLuongHoaDon = () => {
  const { status, data } = useAppSelector(
    (x) => x.dashBoard.tongSoHoaDonReport
  );
  const dispatch = useAppDispatch();
  const isLoading = status === eReducerStatusBase.is_loading;
  const [isShowLoading] = useDebounce(isLoading, 300);
  const [isShowLichSuMuaCKS, setIsShowLichSuMuaCKS] = useState(false);

  const handleSelectReport = () => {
    dispatch(rootAction.dashBoard.tongSoHoaDonReportLoadStart());
  };
  useEffect(() => {
    if (status === eReducerStatusBase.is_not_initialization) {
      handleSelectReport();
    }
  }, [status]);
  // let delayed: any;
  const options: any = {
    responsive: true,
    plugins: {
      legend: {
        position: "top",
        display: false,
      },
      datalabels: {
        color: "#ffffff", // Set label color to white
        // anchor: 'end',
        // align: 'start',
        // offset: -10,
      },
      tooltip: {
        callbacks: {
          label: function (context: any) {
            let label = context.label === "used" ? "Đã sử dụng" : "Còn lại";
            if (label) {
              label += ": ";
            }
            if (context.parsed !== null) {
              label += context.parsed;
            }
            return label;
          },
        },
      },
    },
  };

  const chartDataSource = {
    labels: ["used", "available"],
    datasets: [
      {
        label: "# of Votes",
        data: [
          data.tong_so_luong_da_su_dung,
          data.tong_so_luong_da_mua - data.tong_so_luong_da_su_dung,
        ],
        backgroundColor: ["#C53104", "#FFAB90"],
        borderColor: ["#C53104", "#FFAB90"],
        borderWidth: 1,
        borderRadius: 2,
        // barThickness: 10, // Set bar width
        // maxBarThickness: 3, // You can use this instead to set the maximum bar width
      },
    ],
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
          height: "100px",
        }}
      >
        <Box sx={{ flex: 1 }}>
          <Heading text="Hóa đơn" />
        </Box>
        <Box
          sx={{
            textAlign: "right",
          }}
        >
          <Box>Số lượng hóa đã mua</Box>
          <Link
            sx={{
              cursor: "pointer",
            }}
            onClick={() => {
              setIsShowLichSuMuaCKS(true);
            }}
          >
            <Heading text={`${data.tong_so_luong_da_mua.toLocaleString()}`} />
          </Link>
        </Box>
      </Box>
      <Box
        sx={{
          height: "200px",
          display: "flex",
          justifyContent: "center",
        }}
      >
        {isShowLoading && <PlaceHolder line_number={3} />}
        {!isShowLoading && (
          <Doughnut
            data={chartDataSource}
            options={options}
            // height={100}
            // width={100}
          />
        )}
      </Box>
      <Box sx={{ display: "flex", height: "100px", alignItems: "center" }}>
        <Box
          sx={{
            flex: 1,
          }}
        >
          <Box sx={{ display: "flex" }}>
            <Box
              sx={{
                backgroundColor: "#FFAB90",
                mr: 2,
                height: "20px",
                width: "20px",
                borderRadius: 2,
              }}
            ></Box>
            <Box>
              Hóa đơn còn:{" "}
              {(
                data.tong_so_luong_da_mua - data.tong_so_luong_da_su_dung
              ).toLocaleString()}
            </Box>
          </Box>
          <Box sx={{ display: "flex", mt: 2 }}>
            <Box
              sx={{
                backgroundColor: "#C53104",
                mr: 2,
                height: "20px",
                width: "20px",
                borderRadius: 2,
              }}
            ></Box>
            <Box>
              Hóa đơn đã sử dụng:{" "}
              {data.tong_so_luong_da_su_dung.toLocaleString()}
            </Box>
          </Box>
        </Box>
        <Box>
          <Link
            href="https://nacencomm.vn/product/chu-ky-so-usb-token"
            target="_blank"
          >
            <Button
              text="Đăng ký mua"
              variant="invisible"
              leadingVisual={CreditCardIcon}
            />
          </Link>
        </Box>
      </Box>
      {isShowLichSuMuaCKS && (
        <LichSuMuaChuKySoModal
          onCancel={() => {
            setIsShowLichSuMuaCKS(false);
          }}
        />
      )}
    </Box>
  );
};

export default ChartSoLuongHoaDon;
