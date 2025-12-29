import { DownloadIcon } from "@primer/octicons-react";
import { saveAs } from "file-saver";
import { useState } from "react";
import * as XLSX from "xlsx";
import Button from "../../component-ui/button";
// import React from 'react';
interface IExportToExcelBtnProps {
  fetchDataPromise: () => any;
  formatDataFunction: (data: any[]) => any;
  fileName: string;
  text?: string;
  teamplate?: boolean;
  teamplateFunction?: (data: any[]) => any;
}
const ExportToExcelBtn = (props: IExportToExcelBtnProps) => {
  const {
    fetchDataPromise,
    formatDataFunction,
    fileName,
    teamplate = false,
    teamplateFunction = () => {},
  } = props;
  const [isLoading, setisLoading] = useState(false);
  const handleExport = async () => {
    setisLoading(true);
    try {
      const data = await fetchDataPromise();
      if (data) {
        if (teamplate) {
          await teamplateFunction(data);
        } else {
          const formatData = await formatDataFunction(data);
          const worksheet = XLSX.utils.json_to_sheet(formatData);
          const workbook = XLSX.utils.book_new();
          XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

          const excelBuffer = XLSX.write(workbook, {
            bookType: "xlsx",
            type: "array",
          });
          const blob = new Blob([excelBuffer], {
            type: "application/octet-stream",
          });

          saveAs(blob, fileName + ".xlsx");
        }
      }
    } catch (error) {
      console.log({
        error,
      });
    }
    setisLoading(false);
  };
  return (
    <Button
      text={props.text ?? "Xuất excel"}
      leadingVisual={DownloadIcon}
      variant="default"
      size="medium"
      onClick={handleExport}
      isLoading={isLoading}
    />
  );
};

export default ExportToExcelBtn;
