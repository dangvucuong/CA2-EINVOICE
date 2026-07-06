using System.Linq;
using System.Text.RegularExpressions;
using Model.Request.ToKhai;

namespace Service.Helper
{
    public static class NguoiMuaValidationHelper
    {
        public static string? ValidateAndNormalize(HoaDonAddOrEditModel model)
        {
            var mst = model.nguoi_mua_mst?.Trim() ?? "";
            var cccd = model.nguoi_mua_cccd?.Trim() ?? "";

            if (string.IsNullOrEmpty(mst) && string.IsNullOrEmpty(cccd))
                return null;

            if (!string.IsNullOrEmpty(mst))
            {
                if (mst.Length > 14)
                    return "Mã số thuế người mua không được vượt quá 14 ký tự";

                if (!Regex.IsMatch(mst, @"^[\d-]+$"))
                    return "Mã số thuế chỉ được chứa số và dấu gạch ngang (-)";

                var digits = mst.Replace("-", "");
                if (digits.Length != 10 && digits.Length != 12 && digits.Length != 13)
                    return $"Mã số thuế người mua không hợp lệ: phải có 10, 12 hoặc 13 chữ số (đang nhập {digits.Length} số)";

                if (!string.IsNullOrEmpty(cccd) && cccd != mst)
                    return "Số Căn cước công dân không khớp với Mã số thuế người mua";

                if (string.IsNullOrWhiteSpace(model.nguoi_mua_ten_donvi))
                    return "Vui lòng nhập Đơn vị mua hàng khi có Mã số thuế";

                if (string.IsNullOrWhiteSpace(model.nguoi_mua_dia_chi))
                    return "Vui lòng nhập Địa chỉ người mua hàng khi có Mã số thuế";
            }

            if (!string.IsNullOrEmpty(cccd))
            {
                if (cccd.Length != 12 || !cccd.All(char.IsDigit))
                    return "Căn cước công dân người mua phải đúng 12 chữ số";

                if (string.IsNullOrEmpty(mst))
                {
                    if (string.IsNullOrWhiteSpace(model.nguoi_mua_dia_chi))
                        return "Vui lòng nhập Địa chỉ người mua khi sử dụng CCCD";

                    if (string.IsNullOrWhiteSpace(model.nguoi_mua_ten) &&
                        string.IsNullOrWhiteSpace(model.nguoi_mua_ten_donvi))
                        return "Vui lòng nhập Họ tên người mua hoặc Đơn vị mua hàng khi sử dụng CCCD";
                }
            }

            return null;
        }
    }
}
