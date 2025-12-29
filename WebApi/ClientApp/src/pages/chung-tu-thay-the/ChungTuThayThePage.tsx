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
import SelectBoxMauSoChungTuPhatHanh from "../../component-data/selectbox-mau-so-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import TextInput from "../../component-ui/text-input";
import Button from "../../component-ui/button";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";

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
    mau_so: "03/TNCN",
    loai_chung_tu_dien_tu: "Chứng từ khấu trừ thuế thu nhập cá nhân theo ND70",
    ky_hieu: "",
    ky_hieu_thay_the: "",
  });

  const onSubmit = async (data: any) => {
    let isValid = true;

    if (
      !dataForm?.loai_chung_tu_dien_tu ||
      dataForm?.loai_chung_tu_dien_tu === ""
    ) {
      setError("loai_chung_tu_dien_tu", {
        type: "manual",
      });
      isValid = false;
    }

    if (!dataForm?.mau_so || dataForm?.mau_so === "") {
      setError("mau_so", {
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
      mau_so: dataForm?.mau_so,
      ky_hieu: dataForm?.ky_hieu,
      so_chung_tu_goc: data?.so_chung_tu_goc,
      loai_chung_tu: dataForm?.loai_chung_tu,
    });
  };

  const checkChungTu = async (payload: any) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <CheckChungTuThayTheDieuChinh xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <mau_so>${payload?.mau_so}</mau_so>
      <kyhieu>${payload?.ky_hieu}</kyhieu>
      <sochungtu>${payload?.so_chung_tu_goc}</sochungtu>
      <TinhchatCT>${payload?.loai_chung_tu}</TinhchatCT>
    </CheckChungTuThayTheDieuChinh>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      history.push(
        `../../chung-tu/form/0?tinhchatct=${payload?.loai_chung_tu}&mact_goc=${parseRes.data}`
      );
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
                        setDataForm({
                          ...dataForm,
                          loai_chung_tu: 1,
                        });
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
                        setDataForm({
                          ...dataForm,
                          loai_chung_tu: 2,
                        });
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
                    setDataForm({
                      ...dataForm,
                      loai_chung_tu_dien_tu: value,
                    });

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
                <SelectBoxMauSoChungTuPhatHanh
                  value={dataForm?.mau_so}
                  onValueChanged={(value: string) => {
                    setDataForm({ ...dataForm, mau_so: value });
                    clearErrors("mau_so");
                  }}
                  loai_chung_tu={dataForm.loai_chung_tu_dien_tu}
                />

                {errors && errors["mau_so"] && (
                  <FormControl.Validation id={"mau_so"} variant="error">
                    Vui lòng chọn mẫu số chứng từ gốc
                  </FormControl.Validation>
                )}
              </FormControl>

              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu chứng từ gốc" />
                </FormControl.Label>
                <SelectBoxKyHieuChungTuPhatHanh
                  value={dataForm.ky_hieu}
                  onValueChanged={(value: string) => {
                    setDataForm({ ...dataForm, ky_hieu: value });
                    clearErrors("ky_hieu");
                  }}
                  mau_so={dataForm.mau_so}
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
                    setDataForm({ ...dataForm, ky_hieu_thay_the: value });
                    clearErrors("ky_hieu_thay_the");
                  }}
                  mau_so={dataForm.mau_so}
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
