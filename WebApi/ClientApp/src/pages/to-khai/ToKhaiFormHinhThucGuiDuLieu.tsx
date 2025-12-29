import { InfoIcon } from "@primer/octicons-react";
import {
  Box,
  Checkbox,
  CheckboxGroup,
  Flash,
  FormControl,
  Octicon,
} from "@primer/react";
import { Control, Controller, UseFormWatch } from "react-hook-form";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
interface ToKhaiFormHinhThucGuiDuLieuProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
  watch: UseFormWatch<IToKhai>;
}
const ToKhaiFormHinhThucGuiDuLieu = (
  props: ToKhaiFormHinhThucGuiDuLieuProps
) => {
  const { register, errors, control, watch } = props;
  const is_hoadon_khong_co_ma_cqt = watch("is_hoadon_khong_co_ma_cqt");
  const is_doanh_nghiep_vvn_kho_khan = watch("is_doanh_nghiep_vvn_kho_khan");
  const is_doanh_nghiep_vvn_khac = watch("is_doanh_nghiep_vvn_khac");
  const is_chuyen_du_lieu_truc_tiep = watch("is_chuyen_du_lieu_truc_tiep");
  const is_chuyen_lieu_thong_qua_to_chuc = watch(
    "is_chuyen_lieu_thong_qua_to_chuc"
  );
  // console.log({
  //   is_chuyen_du_lieu_truc_tiep,
  //   is_chuyen_lieu_thong_qua_to_chuc,
  // });

  return (
    <Box sx={{ display: "grid", gap: 2 }}>
      <Flash variant="warning">
        <Box
          sx={{
            display: "flex",
          }}
        >
          <Box>
            <Octicon icon={InfoIcon} />
          </Box>
          <Box>
            Vui lòng bỏ qua nếu bạn không thuộc đối tượng nào trong các đối
            tượng dưới đây
          </Box>
        </Box>
      </Flash>
      <CheckboxGroup disabled={is_hoadon_khong_co_ma_cqt}>
        <FormControl>
          <Checkbox
            readOnly
            disabled={is_hoadon_khong_co_ma_cqt}
            checked={is_doanh_nghiep_vvn_khac || is_doanh_nghiep_vvn_kho_khan}
          />
          <FormControl.Label>
            Trường hợp sử dụng hóa đơn điện tử có mã không phải trả tiền dịch vụ
          </FormControl.Label>
          <FormControl.Caption>
            (Theo khoản 1 điều 14 của Nghị định)
          </FormControl.Caption>
        </FormControl>
        <Box sx={{ ml: 5, display: "grid", gap: 2 }}>
          <Controller
            control={control}
            name="is_doanh_nghiep_vvn_kho_khan"
            render={({ field }) => {
              return (
                <FormControl>
                  <Checkbox
                    disabled={is_hoadon_khong_co_ma_cqt}
                    checked={field.value}
                    onChange={(e) => {
                      field.onChange(e.target.checked);
                    }}
                  />
                  <FormControl.Label sx={{ fontWeight: "unset" }}>
                    Doanh nghiệp vừa và nhỏ, hợp tác xã, hộ, cá nhân kinh doanh
                    tại địa bàn có điều kiện kinh tế xã hội khó khăn và đặc biệt
                    khó khăn
                  </FormControl.Label>
                </FormControl>
              );
            }}
          />
          <Controller
            control={control}
            name="is_doanh_nghiep_vvn_khac"
            render={({ field }) => {
              return (
                <FormControl>
                  <Checkbox
                    disabled={is_hoadon_khong_co_ma_cqt}
                    checked={field.value}
                    onChange={(e) => {
                      field.onChange(e.target.checked);
                    }}
                  />
                  <FormControl.Label sx={{ fontWeight: "unset" }}>
                    Doanh nghiệp vừa và nhỏ khác theo đề nghị của UBND tỉnh,
                    thành phố trực thuộc trung ương gửi Bộ tài chính trừ doanh
                    nghiệp hoạt động tại khu kinh tế, khu công nghiệp, khu công
                    nghệ cao
                  </FormControl.Label>
                </FormControl>
              );
            }}
          />
        </Box>
      </CheckboxGroup>
      <CheckboxGroup disabled={!is_hoadon_khong_co_ma_cqt}>
        <FormControl>
          <Checkbox
            readOnly
            disabled={!is_hoadon_khong_co_ma_cqt}
            checked={
              is_chuyen_du_lieu_truc_tiep || is_chuyen_lieu_thong_qua_to_chuc
            }
          />
          <FormControl.Label>
            {" "}
            Trường hợp sử dụng hóa đơn điện tử không có mã của cơ quan thuế
          </FormControl.Label>
        </FormControl>
        <Box sx={{ ml: 5, display: "grid", gap: 2 }}>
          <Controller
            control={control}
            name="is_chuyen_du_lieu_truc_tiep"
            render={({ field }) => {
              return (
                <FormControl>
                  <Checkbox
                    disabled={!is_hoadon_khong_co_ma_cqt}
                    checked={field.value}
                    onChange={(e) => {
                      field.onChange(e.target.checked);
                    }}
                  />
                  <FormControl.Label sx={{ fontWeight: "unset" }}>
                    Chuyển dữ liệu hóa đơn điện tử trực tiếp đến cơ quan thuế
                  </FormControl.Label>
                </FormControl>
              );
            }}
          />
          <Controller
            control={control}
            name="is_chuyen_lieu_thong_qua_to_chuc"
            render={({ field }) => {
              return (
                <FormControl>
                  <Checkbox
                    disabled={!is_hoadon_khong_co_ma_cqt}
                    checked={field.value}
                    onChange={(e) => {
                      field.onChange(e.target.checked);
                    }}
                  />
                  <FormControl.Label sx={{ fontWeight: "unset" }}>
                    Thông qua tổ chức cung cấp dịch vụ hóa đơn điện tử
                  </FormControl.Label>
                </FormControl>
              );
            }}
          />
        </Box>
      </CheckboxGroup>
      <Controller
        control={control}
        name="is_co_quan_xu_ly_tai_san_cong"
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Cơ quan thuế hoặc cơ quan được giao nhiệm vụ tổ chức, xử lý tài
                sản công theo quy định pháp luật về quản lý, sử dụng tài sản
                công (khoản 11 điều 1 Nghị định 70)
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
    </Box>
  );
};

export default ToKhaiFormHinhThucGuiDuLieu;
