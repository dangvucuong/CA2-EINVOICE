import { Box, CounterLabel, FormControl } from "@primer/react";
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import SelectBoxRole from "../../component-data/selectbox-role";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input/TextInput";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { IUserEditModel } from "../../models/responses/user/IUserEditModel";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { useCommonContext } from "../../contexts/common";
import { USER_API_ENDPOINT } from "../../api/user/userApi";

const UserEditFormModal = () => {
  const { userEditing, userEditingForm, status } = useAppSelector(
    (x) => x.user.userReducer
  );
  const [roleIds, setRoleIds] = useState<number[]>(
    userEditingForm?.role_ids ?? []
  );
  const userEditingId = useMemo(() => {
    return userEditing?.id ?? 0;
  }, [userEditing]);
  const { user } = useAuth();
  const { checkAccesiableTo } = useCommonContext();
  const dispatch = useAppDispatch();
  const isCanViewAll = useMemo(() => {
    return checkAccesiableTo(USER_API_ENDPOINT + "/all", "GET");
  }, []);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<IUserEditModel>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...userEditingForm,
      donvi_ma_dv: userEditingForm?.donvi_ma_dv ?? user?.donvi_ma_dv,
    },
  });
  const onSubmit = async (data: any) => {
    dispatch(
      rootAction.user.userAction.saveFormStart({
        ...data,
        password: data.password ?? "",
        id: userEditingId,
        role_ids: roleIds,
      })
    );
  };
  return (
    <Modal
      title={
        (userEditing?.id ?? 0) === 0 ? "Thêm người dùng" : "Sửa người dùng"
      }
      onClose={() => {
        dispatch(rootAction.user.userAction.closeEditModal());
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={userEditing?.id ?? 0}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
            <FormControl>
              <FormControl.Label>
                <Text text="Mã đơn vị" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="donvi_ma_dv"
                width={150}
                required
                disabled={!isCanViewAll || userEditingId > 0}
                validateMessage="Vui lòng điền Mã đơn vị"
                errors={errors}
              />
            </FormControl>
            <FormControl>
              <FormControl.Label>
                <Text text="Email đăng nhập" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="username"
                width={150}
                required
                validateMessage="Vui lòng điền Email đăng nhập"
                errors={errors}
              />
            </FormControl>
          </Box>
          <FormControl>
            <FormControl.Label>
              <Text text="Họ và tên" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="full_name"
              block
              required
              validateMessage="Vui lòng điền họ và tên"
              errors={errors}
            />
          </FormControl>
          {/* <FormControl>
            <FormControl.Label>
              <Text text="Email" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="email"
              width={250}
              required
              validateMessage="Vui lòng điền email"
              errors={errors}
            />
          </FormControl> */}
          {isCanViewAll && (
            <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
              <FormControl>
                <FormControl.Label>
                  <Text text="Serial" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="serial_number"
                  // required
                  width={320}
                  validateMessage="Vui lòng điền Serial number"
                  errors={errors}
                />
              </FormControl>
            </Box>
          )}
          {(!userEditing || userEditing.id <= 0) && (
            <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
              <FormControl>
                <FormControl.Label>
                  <Text text="Mật khẩu" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="password"
                  type="password"
                  minLength={6}
                  // width={320}
                  validateMessage="Vui lòng điền mật khẩu"
                  errors={errors}
                />
              </FormControl>
            </Box>
          )}
          <FormControl>
            <FormControl.Label>
              Vai trò
              {roleIds.length > 0 && (
                <CounterLabel sx={{ marginLeft: 1 }}>
                  {roleIds.length}
                </CounterLabel>
              )}
            </FormControl.Label>
            <SelectBoxRole
              onValueChanged={(ids) => {
                setRoleIds(ids);
              }}
              value={roleIds}
              maxWidth={"400px"}
            />
          </FormControl>

          <ModalActions>
            <Button
              onClick={() => {
                dispatch(rootAction.user.userAction.closeEditModal());
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={(userEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
              isLoading={status === eReducerStatusBase.is_saving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default UserEditFormModal;
