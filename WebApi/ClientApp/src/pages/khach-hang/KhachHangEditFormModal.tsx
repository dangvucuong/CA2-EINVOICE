import { Box, FormControl } from "@primer/react";
import { useForm } from "react-hook-form";
import Modal from "../../component-ui/modal";
import Text from "../../component-ui/text";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { IKhachHang } from "../../models/responses/category/IKhachHang";
import { rootAction } from "../../state/actions/rootAction";
import TextInput from "../../component-ui/text-input";
import ModalActions from "../../component-ui/modal/ModalActions";
import Button from "../../component-ui/button";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { useAuth } from "../../hooks/useAuth";

const KhachHangEditFormModal = () => {
  const dispatch = useAppDispatch();
  const { user } = useAuth();
  const { khachHangEditing, status } = useAppSelector(
    (x) => x.category.khachHangReducer,
  );
  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
  } = useForm<IKhachHang>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...khachHangEditing,
      donvi_ma_dv: khachHangEditing?.donvi_ma_dv ?? user?.donvi_ma_dv,
    },
  });

  const onSubmit = async (data: any) => {
    if (!data.ten_don_vi || data.ten_don_vi.trim() === "") {
      setError("ten_khach_hang", {
        type: "manual",
        message: "Vui lòng điền tên đơn vị mua hàng",
      });
      return;
    }

    dispatch(
      rootAction.category.khachHangAction.saveStart({
        ...data,
        ma_dv_ngan_sach: data.ma_dv_ngan_sach || "",
      }),
    );
  };
  return (
    <Modal
      title={(khachHangEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
      onClose={() => {
        dispatch(rootAction.category.khachHangAction.closeEditModal());
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={khachHangEditing?.id ?? 0}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <FormControl>
            <FormControl.Label>
              <Text text="Mã đơn vị bán hàng" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="donvi_ma_dv"
              disabled
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Tên đơn vị mua hàng" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ten_don_vi"
              // required
              block
              // validateMessage="Vui lòng điền tên đơn vị mua hàng"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Tên người mua hàng" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ten_khach_hang"
              // required
              block
              validateMessage="Vui lòng điền tên người mua hàng"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Địa chỉ" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="dia_chi"
              // required
              block
              validateMessage="Vui lòng điền Địa chỉ"
              errors={errors}
            />
          </FormControl>
          <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
            <FormControl sx={{ pr: 2 }}>
              <FormControl.Label>
                <Text text="Mã số thuế" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="mst"
                // required
                block
                validateMessage="Vui lòng điền Mã số thuế"
                errors={errors}
              />
            </FormControl>
            <FormControl>
              <FormControl.Label>
                <Text text="Email" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="email"
                width={250}
                // required
                block
                validateMessage="Vui lòng điền Email"
                errors={errors}
              />
            </FormControl>
          </Box>
          <FormControl>
            <FormControl.Label>
              <Text text="Số tài khoản" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="stk"
              // required
              block
              validateMessage="Vui lòng điền Số tài khoản"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Mã quan hệ ngân sách" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ma_dv_ngan_sach"
              minLength={7}
              maxLength={7}
              // required
              block
              // validateMessage="Vui lòng điền Số tài khoản"
              errors={errors}
            />
          </FormControl>

          <FormControl>
            <FormControl.Label>
              <Text text="Căn cước công dân" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ccdan"
              minLength={9}
              maxLength={12}
              // required
              block
              // validateMessage="Vui lòng điền Số tài khoản"
              errors={errors}
            />
          </FormControl>

          <ModalActions>
            <Button
              onClick={() => {
                dispatch(rootAction.category.khachHangAction.closeEditModal());
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={(khachHangEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
              isLoading={status === eReducerStatusBase.is_saving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default KhachHangEditFormModal;
