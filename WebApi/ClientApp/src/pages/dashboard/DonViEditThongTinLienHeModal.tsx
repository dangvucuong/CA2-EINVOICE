import { Box, FormControl, useConfirm } from "@primer/react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { donViApi } from "../../api/category/donViApi";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { IDonVi } from "../../models/responses/category/IDonVi";
import SelectBoxCoQuanThue from "../../component-data/selectbox-co-quan-thue";

interface IThongTinBatBuoc {
  ten_dv: string;
  dia_chi: string;
  email: string;
  dien_thoai: string;
  co_quan_thu_id_chuquan: number;
  donvi_chuquan: string;
}

interface IDonViEditThongTinLienHeModalProps {
  donVi: IDonVi;
  onSuccess: () => void;
  onCancel: () => void;
}

const DonViEditThongTinLienHeModal = (
  props: IDonViEditThongTinLienHeModalProps
) => {
  const dispatch = useAppDispatch();
  const donViEditing = props.donVi;
  const [coQuanThueId, setCoQuanThueId] = useState(
    donViEditing?.co_quan_thu_id_chuquan ?? 0
  );
  const [tenDonViChuQuan, setTenDonViChuQuan] = useState(
    donViEditing?.donvi_chuquan ?? ""
  );
  const [isSaving, setIsSaving] = useState(false);
  const confirm = useConfirm();

  const {
    register,
    handleSubmit,
    clearErrors,
    setError,
    formState: { errors },
  } = useForm<IDonVi>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...donViEditing,
    },
  });

  const checkthongtinbatbuoc = (
    oldData: IThongTinBatBuoc,
    newData: IThongTinBatBuoc
  ): boolean => {
    const fields: (keyof IThongTinBatBuoc)[] = [
      "ten_dv",
      "dia_chi",
      "email",
      "dien_thoai",
      "co_quan_thu_id_chuquan",
      "donvi_chuquan",
    ];

    return fields.some((field) => oldData[field] !== newData[field]);
  };

  const onSubmit = async (data: any) => {
    setIsSaving(true);
    const res = await donViApi.updateThongTinLienHe({
      id: donViEditing.id,
      ...data,
      donvi_chuquan: tenDonViChuQuan,
      co_quan_thu_id_chuquan: coQuanThueId,
    });
    if (res.is_success) {
      const thaydoithongtinbb = checkthongtinbatbuoc(
        {
          ten_dv: donViEditing.ten_dv,
          dia_chi: donViEditing.dia_chi,
          email: donViEditing.email,
          dien_thoai: donViEditing.dien_thoai ?? "",
          co_quan_thu_id_chuquan: coQuanThueId ?? "",
          donvi_chuquan: tenDonViChuQuan ?? "",
        },
        data
      );
      NotifyHelper.Success("Cập nhật thành công");
      if (thaydoithongtinbb) {
        await confirm({
          content:
            "Bạn đã thay đổi thông tin bắt buộc trên tờ khai. Bạn có muốn nộp lại tờ khai Đăng ký sử dụng?",
          title: "Xác nhận",
          cancelButtonContent: (
            <div
              onClick={() => {
                props.onSuccess();
              }}
            >
              Hủy
            </div>
          ),
          confirmButtonContent: (
            <div
              onClick={() => {
                window.location.href = "../../to-khai/0";
              }}
            >
              Đồng ý
            </div>
          ),
          confirmButtonType: "primary",
        });
      } else {
        props.onSuccess();
      }
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
  };
  return (
    <Modal
      title={(donViEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
      onClose={() => {
        props.onCancel();
      }}
      isOpen={true}
      width={"large"}
      height={"auto"}
      key={donViEditing?.id ?? 0}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box>
          <Box className="row">
            <Box
              className="col-md-12"
              sx={{
                borderRight: "1px",
                borderRightStyle: "dashed",
                borderRightColor: "border.default",
                pr: 4,
              }}
            >
              <FormControl>
                <FormControl.Caption>Thông tin cơ bản</FormControl.Caption>
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Tên đơn vị" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="ten_dv"
                  required
                  block
                  // width={150}
                  validateMessage="Vui lòng điền Tên đơn vị"
                  errors={errors}
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Địa chỉ" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="dia_chi"
                  required
                  block
                  errors={errors}
                  validateMessage="Vui lòng điền Địa chỉ"
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Cơ quan thuế quản lý" />
                </FormControl.Label>

                <SelectBoxCoQuanThue
                  maxWidth={"300px"}
                  onValueChanged={(id, data) => {
                    setCoQuanThueId(id);
                    setTenDonViChuQuan(data?.ten ?? "");
                  }}
                  value={coQuanThueId}
                />
                {/* {
                                    errors && errors["donvi_chuquan"] &&
                                    <FormControl.Validation id={"donvi_chuquan"} variant="error">
                                        <>Vui lòng chọn đơn vị chủ quản</>
                                    </FormControl.Validation>
                                } */}
              </FormControl>
              <FormControl sx={{ mt: 3 }}>
                <FormControl.Caption>Thông tin liên hệ</FormControl.Caption>
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Điện thoại" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="dien_thoai"
                  // required
                  width={150}
                  errors={errors}
                  validateMessage="Vui lòng điền số điện thoại"
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Fax" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="fax"
                  // required
                  width={200}
                  // block
                  errors={errors}
                  // validateMessage='Vui lòng điền ngân hàng'
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Website" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="website"
                  // required
                  // width={150}
                  block
                  errors={errors}
                  validateMessage="Vui lòng điền website"
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Email" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="email"
                  // required
                  // width={150}
                  block
                  errors={errors}
                  validateMessage="Vui lòng điền Email"
                />
              </FormControl>

              <FormControl sx={{ mt: 3 }}>
                <FormControl.Caption>Thông tin ngân hàng</FormControl.Caption>
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Số tài khoản" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="stk"
                  block
                  // required
                  // width={200}
                  errors={errors}
                  validateMessage="Vui lòng điền số tài khoản"
                />
              </FormControl>
              <FormControl sx={{ mt: 2 }}>
                <FormControl.Label>
                  <Text text="Tại ngân hàng" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="ngan_hang"
                  // required
                  // width={200}
                  block
                  errors={errors}
                  validateMessage="Vui lòng điền ngân hàng"
                />
              </FormControl>
            </Box>
          </Box>

          <ModalActions>
            <Button
              onClick={() => {
                props.onCancel();
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={(donViEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
              isLoading={isSaving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default DonViEditThongTinLienHeModal;
