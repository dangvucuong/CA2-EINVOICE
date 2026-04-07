import { PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, FormControl, IconButton } from "@primer/react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import Button from "../../component-ui/button";
import { DataTable } from "../../component-ui/data-table";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
import TextInput from "../../component-ui/text-input";
import moment from "moment";
interface ToKhaiFormTTTNProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
  watch: UseFormWatch<IToKhai>;
  setValue: UseFormSetValue<IToKhai>;
}
const ToKhaiFormTTTN = (props: ToKhaiFormTTTNProps) => {
  const { control } = props;
  const defaultTTTNObj = {
    TTCTN: "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM",
    MSTTCTN: "0103930279",
    TNgay: "2021-12-01",
    DNgay: "2030-12-31",
    isReadOnly: true,
  };
  return (
    <Box>
      <Controller
        control={control}
        name="to_chuc_truyen_nhan_json"
        rules={{
          validate: (data) => {
            return true;
          },
        }}
        render={({ field }) => {
          const dataSource = field.value
            ? JSON.parse(field.value)
            : [{ ...defaultTTTNObj }].map((x, idx) => ({ ...x, id: idx }));

          return (
            <FormControl>
              <Box sx={{ width: "100%" }}>
                <DataTable
                  titleComponent={
                    <Box>
                      <Button
                        leadingVisual={PlusIcon}
                        text="Thêm tổ chức"
                        onClick={() => {
                          field.onChange(
                            JSON.stringify([
                              ...dataSource,
                              {
                                id: dataSource.length,
                                TTCTN: "",
                                MSTTCGP: "",
                                TNgay: "",
                                DNgay: "",
                              },
                            ])
                          );
                        }}
                      />
                    </Box>
                  }
                  data={dataSource}
                  key={"id"}
                  columns={[
                    {
                      header: "STT",
                      field: "id",
                      width: "50px",
                      renderCell: (data: any) => {
                        return <Box>{data.id + 1}</Box>;
                      },
                    },
                    {
                      header: "Tên tổ chức",
                      field: "TTCTN",
                      renderCell: (data: any) => {
                        return (
                          <TextInput
                            block
                            value={data.TTCTN}
                            className="noborder"
                            onChange={(e) => {
                              if (!data.isReadOnly) {
                                field.onChange(
                                  JSON.stringify(
                                    dataSource.map((x: any) => {
                                      if (x.id === data.id) {
                                        return { ...x, TTCTN: e.target.value };
                                      }
                                      return x;
                                    })
                                  )
                                );
                              }
                            }}
                          />
                        );
                      },
                    },
                    {
                      header: "Mã số thuế",
                      field: "MSTTCTN",
                      width: "200px",
                      renderCell: (data: any) => {
                        return (
                          <TextInput
                            block
                            value={data.MSTTCTN}
                            className="noborder"
                            onChange={(e) => {
                              if (!data.isReadOnly) {
                                field.onChange(
                                  JSON.stringify(
                                    dataSource.map((x: any) => {
                                      if (x.id === data.id) {
                                        return {
                                          ...x,
                                          MSTTCTN: e.target.value,
                                        };
                                      }
                                      return x;
                                    })
                                  )
                                );
                              }
                            }}
                          />
                        );
                      },
                    },
                    {
                      header: "Từ ngày",
                      field: "TNgay",
                      width: "150px",
                      renderCell: (data: any) => {
                        return (
                          <TextInput
                            block
                            type="date"
                            value={
                              data.TNgay
                                ? moment(data.TNgay).format("YYYY-MM-DD")
                                : undefined
                            }
                            className="noborder"
                            onChange={(e) => {
                              if (!data.isReadOnly) {
                                field.onChange(
                                  JSON.stringify(
                                    dataSource.map((x: any) => {
                                      if (x.id === data.id) {
                                        return {
                                          ...x,
                                          // TNgay: e.target.value
                                          TNgay: moment(e.target.value).format(
                                            "YYYY-MM-DD"
                                          ),
                                        };
                                      }
                                      return x;
                                    })
                                  )
                                );
                              }
                            }}
                          />
                        );
                      },
                    },
                    {
                      header: "Đến ngày",
                      field: "DNgay",
                      width: "150px",
                      renderCell: (data: any) => {
                        return (
                          <TextInput
                            block
                            // value={data.DNgay}
                            type="date"
                            value={
                              data.DNgay
                                ? moment(data.DNgay).format("YYYY-MM-DD")
                                : undefined
                            }
                            className="noborder"
                            onChange={(e) => {
                              if (!data.isReadOnly) {
                                field.onChange(
                                  JSON.stringify(
                                    dataSource.map((x: any) => {
                                      if (x.id === data.id) {
                                        return {
                                          ...x,
                                          // DNgay: e.target.value
                                          DNgay: moment(e.target.value).format(
                                            "YYYY-MM-DD"
                                          ),
                                        };
                                      }
                                      return x;
                                    })
                                  )
                                );
                              }
                            }}
                          />
                        );
                      },
                    },
                    {
                      header: "",
                      id: "cmd",
                      width: "80px",
                      renderCell: (data: any) => {
                        return (
                          <IconButton
                            aria-label=""
                            icon={TrashIcon}
                            variant="invisible"
                            onClick={() => {
                              if (!data.isReadOnly) {
                                field.onChange(
                                  JSON.stringify(
                                    dataSource
                                      .filter((x: any) => x.id !== data.id)
                                      .map((x: any) => ({ ...x }))
                                  )
                                );
                              }
                            }}
                          />
                        );
                      },
                    },
                  ]}
                />
              </Box>
            </FormControl>
          );
        }}
      />
    </Box>
  );
};

export default ToKhaiFormTTTN;
