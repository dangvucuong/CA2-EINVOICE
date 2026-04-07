import { Box, FormControl } from "@primer/react";
import { useForm } from "react-hook-form";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { IDaiLy } from "../../models/responses/category/IDaiLy";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { NotifyHelper } from "../../helpers/toast";
import { daiLyApi } from "../../api/category/daiLyApi";

const DaiLyEditFormModal = () => {
  const dispatch = useAppDispatch();
  const { user } = useAuth();
  const { daiLyEditing, status, daiLys, filter } = useAppSelector(
    (x) => x.category.daiLyReducer
  );

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<IDaiLy>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...daiLyEditing,
      donvi_ma_dv: daiLyEditing?.donvi_ma_dv ?? user?.donvi_ma_dv,
    },
  });

  const handleGetDaiLy = async (ma_dai_ly: string) => {
    const res = await daiLyApi.getByDonViPaging({
      ...filter,
      search_key: ma_dai_ly,
    });

    if (res.is_success) {
      return res.data.data;
    }
  };

  const onSubmit = async (data: any) => {
    const listDaily: IDaiLy[] = await handleGetDaiLy(data.ma_dai_ly);

    const checkExist = listDaily?.find((x) => x.ma_dai_ly === data.ma_dai_ly);

    if (checkExist && (daiLyEditing?.id ?? 0) === 0) {
      NotifyHelper.Error("Đại lý đã tồn tại!");
      return;
    }

    dispatch(
      rootAction.category.daiLyAction.saveStart({
        ...data,
      })
    );
  };

  return (
    <Modal
      title={(daiLyEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
      onClose={() => {
        dispatch(rootAction.category.daiLyAction.closeEditModal());
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={daiLyEditing?.id ?? 0}
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
              <Text text="Mã đơn vị" />
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
              <Text text="Mã đại lý" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ma_dai_ly"
              required
              width={150}
              validateMessage="Vui lòng điền mã đại lý"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Tên đại lý" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="ten_dai_ly"
              required
              block
              validateMessage="Vui lòng điền tên đại lý"
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
              block
              // required
              // width={100}
              validateMessage="Vui lòng điền Email"
              errors={errors}
            />
          </FormControl>

          <FormControl>
            <FormControl.Label>
              <Text text="Số tài khoản" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="so_tai_khoan"
              // width={150}
              block
              validateMessage="Vui lòng điền Số tài khoản"
              errors={errors}
            />
          </FormControl>

          <ModalActions>
            <Button
              onClick={() => {
                dispatch(rootAction.category.daiLyAction.closeEditModal());
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={(daiLyEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
              isLoading={status === eReducerStatusBase.is_saving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default DaiLyEditFormModal;
