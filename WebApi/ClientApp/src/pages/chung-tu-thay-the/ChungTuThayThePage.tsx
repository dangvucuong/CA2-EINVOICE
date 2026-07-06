import { PlusIcon } from "@primer/octicons-react";
import { Helmet } from "react-helmet";

import {
  Box,
  FormControl,
  Link as LinkHref,
  Radio,
  RadioGroup,
} from "@primer/react";
import { useHistory } from "react-router-dom";
import { useForm } from "react-hook-form";
import Text from "../../component-ui/text";
import { useState } from "react";
import SelectBoxLoaiChungTuPhatHanh from "../../component-data/selectbox-loai-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import TextInput from "../../component-ui/text-input";
import Button from "../../component-ui/button";
import { checkChungTuThayTheDieuChinh } from "../../helpers/toKhaiChungTuHelper";
import {
  getChungTuMadonvi,
  MAU_SO_CHUNG_TU_TNCN,
} from "../../helpers/chungTuConstants";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import SelectBoxKyHieuChungTuQuanLy from "../../component-data/selectbox-ky-hieu-chung-tu-quan-ly";

const ChungTuThayThePage = () => {
  const history = useHistory();
  const { user } = useAuth();
  const {
    register,
    clearErrors,
    setError,
    handleSubmit,
    formState: { errors },
  } = useForm<any>({});
  const [dataForm, setDataForm] = useState<any>({
    loai_chung_tu: 1,
    mau_so: MAU_SO_CHUNG_TU_TNCN,
    loai_chung_tu_dien_tu: MAU_SO_CHUNG_TU_TNCN,
    ky_hieu: "",
    ky_hieu_thay_the: "",
  });

  const onSubmit = async (data: any) => {
    let isValid = true;

    if (!dataForm?.loai_chung_tu_dien_tu ||
      dataForm?.loai_chung_tu_dien_tu === ""
    ) {
      setError("loai_chung_tu_dien_tu", {
        type: "manual",
      });
      isValid = false;
    }

    if (!dataForm?.ky_hieu || dataForm?.ky_hieu === "") {
      setError("ky_hieu", {
        type: "manual",
      });
      isValid = false;
    }

    if (!dataForm?.ky_hieu_thay_the || dataForm?.ky_hieu_thay_the === "") {
      setError("ky_hieu_thay_the", {
        type: "manual",
      });
      isValid = false;
    }

    if (!isValid) return;
    console.log("data");

    await checkChungTu({
      mau_so: MAU_SO_CHUNG_TU_TNCN,
      ky_hieu: dataForm?.ky_hieu,
      so_chung_tu_goc: data?.so_chung_tu_goc,
      loai_chung_tu: dataForm?.loai_chung_tu,
    });
  };

  const checkChungTu = async (payload: any) => {
    const parseRes = await checkChungTuThayTheDieuChinh(getChungTuMadonvi(user), {
      mau_so: payload?.mau_so,
      ky_hieu: payload?.ky_hieu,
      so_chung_tu_goc: payload?.so_chung_tu_goc,
      loai_chung_tu: payload?.loai_chung_tu,
    });

    if (parseRes.status === "success") {
      const params = new URLSearchParams({
        tinhchatct: String(payload?.loai_chung_tu),
        mact_goc: String(parseRes.data),
      });
      if (dataForm?.ky_hieu_thay_the) {
        params.set("ky_hieu_thay_the", dataForm.ky_hieu_thay_the);
      }
      params.set("mau_so", MAU_SO_CHUNG_TU_TNCN);
      history.push(`../../chung-tu/form/0?${params.toString()}`);
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  return (
    <Box>
      <Helmet>
        <title>Chứng từ thay thế/ điều chỉnh</title>
      </Helmet>

      <Box>
        <form onSubmit={handleSubmit(onSubmit)}>
          <Box
            display={"grid"}
            sx={{
              gap: 2,
            }}
          >
            <FormControl>
              <FormControl.Label></FormControl.Label>
              <RadioGroup name="loai_chung_tu">
                <FormControl>
                  <Radio
                    value="left"
                    checked={dataForm.loai_chung_tu === 1}
                    onChange={(e) => {
                      if (e.target.checked) {
                        setDataForm((prev: any) => ({
                          ...prev,
                          loai_chung_tu: 1,
                        }));
                      }
                    }}
                  />
                  <FormControl.Label>Lập chứng từ thay thế</FormControl.Label>
                </FormControl>
                <FormControl>
                  <Radio
                    value="right"
                    checked={dataForm.loai_chung_tu === 2}
                    onChange={(e) => {
                      if (e.target.checked) {
                        setDataForm((prev: any) => ({
                          ...prev,
                          loai_chung_tu: 2,
                        }));
                      }
                    }}
                  />
                  <FormControl.Label>Lập chứng từ điều chỉnh</FormControl.Label>
                </FormControl>
              </RadioGroup>
            </FormControl>

            <Box
              sx={{
                display: "flex",
                gap: 20,
              }}
            >
              <FormControl>
                <FormControl.Label>
                  <Text text="Chọn loại chứng từ" />
                </FormControl.Label>
                <SelectBoxLoaiChungTuPhatHanh
                  value={dataForm?.loai_chung_tu_dien_tu}
                  onValueChanged={(value) => {
                    setDataForm((prev: any) => ({
                      ...prev,
                      loai_chung_tu_dien_tu: value,
                      mau_so: MAU_SO_CHUNG_TU_TNCN,
                      ky_hieu: "",
                      ky_hieu_thay_the: "",
                    }));
                    clearErrors("loai_chung_tu_dien_tu");
                  }}
                />

                {errors && errors["loai_chung_tu_dien_tu"] && (
                  <FormControl.Validation
                    id={"loai_chung_tu_dien_tu"}
                    variant="error"
                  >
                    Vui lòng chọn loại chứng từ
                  </FormControl.Validation>
                )}
              </FormControl>
              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu mẫu số chứng từ gốc" />
                </FormControl.Label>
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    height: 32,
                    px: 2,
                    border: "1px solid",
                    borderColor: "border.default",
                    borderRadius: 2,
                    minWidth: 120,
                  }}
                >
                  <Text text={MAU_SO_CHUNG_TU_TNCN} />
                </Box>
              </FormControl>

              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu chứng từ gốc" />
                </FormControl.Label>
                <SelectBoxKyHieuChungTuQuanLy
                  value={dataForm.ky_hieu}
                  onValueChanged={(value: string) => {
                    setDataForm((prev: any) => ({ ...prev, ky_hieu: value }));
                    clearErrors("ky_hieu");
                  }}
                  mau_so={MAU_SO_CHUNG_TU_TNCN}
                />
                {errors && errors["ky_hieu"] && (
                  <FormControl.Validation id={"ky_hieu"} variant="error">
                    Vui lòng chọn ký hiệu chứng từ gốc
                  </FormControl.Validation>
                )}
              </FormControl>

              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu chứng từ thay thế" />
                </FormControl.Label>
                <SelectBoxKyHieuChungTuPhatHanh
                  value={dataForm.ky_hieu_thay_the}
                  onValueChanged={(value: string) => {
                    setDataForm((prev: any) => ({
                      ...prev,
                      ky_hieu_thay_the: value,
                    }));
                    clearErrors("ky_hieu_thay_the");
                  }}
                  mau_so={MAU_SO_CHUNG_TU_TNCN}
                />
                {errors && errors["ky_hieu_thay_the"] && (
                  <FormControl.Validation
                    id={"ky_hieu_thay_the"}
                    variant="error"
                  >
                    Vui lòng chọn ký hiệu chứng từ thay thế
                  </FormControl.Validation>
                )}
              </FormControl>
            </Box>

            <FormControl>
              <FormControl.Label>
                <Text text="Số chứng từ gốc" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="so_chung_tu_goc"
                required
                validateMessage="Vui lòng điền số chứng từ gốc"
                errors={errors}
              />
            </FormControl>
          </Box>

          <Button
            type="submit"
            variant="primary"
            size="large"
            sx={{ width: 200, mt: 4 }}
          >
            Tạo chứng từ
          </Button>
        </form>
      </Box>
    </Box>
  );
};

export default ChungTuThayThePage;
