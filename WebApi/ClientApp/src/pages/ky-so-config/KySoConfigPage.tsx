import { PencilIcon, PlusIcon } from "@primer/octicons-react";
import { Box, IconButton, Truncate } from "@primer/react";
import { useEffect, useState } from "react";
import { Helmet } from "react-helmet";
import { donViCtsApi } from "../../api/category/donViCtsApi";
import Button from "../../component-ui/button";
import { DataTable } from "../../component-ui/data-table";
import Heading from "../../component-ui/heading";
import { NotifyHelper } from "../../helpers/toast";
import { IKhachHang } from "../../models/responses/category/IKhachHang";
import EditModal from "./EditModal";

const KySoConfigPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [dataSoursce, setDataSoursce] = useState<any[]>([]);
  const [isShowEditModal, setIsShowEditModal] = useState(false);
  const [editingData, setEditingData] = useState<any>();

  useEffect(() => {
    handleReload();
  }, []);
  const handleReload = async () => {
    setIsLoading(true);
    const res = await donViCtsApi.getAll();
    if (res.is_success) {
      setDataSoursce(res.data);
    } else {
      NotifyHelper.Error(res.message ?? "Có lỗi");
    }
    setIsLoading(false);
  };
  return (
    <Box>
      <Helmet>
        <title>Quản lý serial</title>
      </Helmet>
      <DataTable
        titleComponent={<Heading text="Danh sách chữ ký số" />}
        subTitle={`Tổng số: ${dataSoursce.length.toLocaleString()}`}
        data={dataSoursce}
        height={window.innerHeight - 100}
        isLoading={isLoading}
        actionComponent={
          <>
            <Button
              text="Thêm mới Chữ ký số"
              variant="primary"
              leadingVisual={PlusIcon}
              onClick={() => {
                setIsShowEditModal(true);
                setEditingData(undefined);
              }}
            />
          </>
        }
        columns={[
          {
            header: "Số serial",
            field: "serial_number",
            rowHeader: true,
            minWidth: "200px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Người sở hữu",
            field: "subject",
            minWidth: "250px",
            // sortBy: "alphanumeric"
          },
          {
            header: "Tổ chức phát hành",
            field: "issuer",
            rowHeader: false,
            width: "200px",
          },
          {
            header: "Hiệu lực từ ngày",
            field: "not_before",
            rowHeader: false,
            width: "200px",
          },
          {
            header: "Hiệu lực đến ngày",
            field: "not_after",
            rowHeader: false,
            width: "200px",
          },

          {
            id: "actions",
            header: "",
            width: "50px",
            renderCell: (row: any) => {
              return (
                <>
                  <Box
                    sx={{
                      mt: -2,
                      mb: -2,
                    }}
                  >
                    <IconButton
                      aria-label={`Edit: ${row.name}`}
                      title={`Edit: ${row.name}`}
                      icon={PencilIcon}
                      variant="invisible"
                      onClick={() => {
                        setEditingData(row);
                        setIsShowEditModal(true);
                      }}
                    />
                  </Box>
                </>
              );
            },
          },
        ]}
      />
      {isShowEditModal && (
        <EditModal
          onClose={() => {
            setIsShowEditModal(false);
          }}
          onSuccess={() => {
            setIsShowEditModal(false);
            handleReload();
          }}
          data={editingData}
        />
      )}
    </Box>
  );
};

export default KySoConfigPage;
