using System.Collections.Generic;
using Common;
using Model.Respone.MauHoaDon;
using Model.Table;

namespace Service.Helper
{
    public static class MauHoaDonAdvancedSettingsHelper
    {
        public static string ApplyToContent(string content, mau_hoa_don mauHoaDon)
        {
            if (string.IsNullOrEmpty(content) || mauHoaDon == null)
                return content;

            var advancedSettings = mauHoaDon.advanced_settings_json
                .ConvertToString()
                .TryDeserializeObject<CssEditorElementData[]>();

            if (advancedSettings == null)
                return content;

            foreach (var ad in advancedSettings)
            {
                if (ad == null || string.IsNullOrWhiteSpace(ad.elementId))
                    continue;

                var keyCss = $"{ad.elementId}_css;";
                var keyCssDisplay = $"{ad.elementId}_css_display;";
                var css = new List<string>()
                {
                    $"font-weight:{(ad.cssValue?.isBold == true ? "bold" : "normal")}",
                    $"font-style:{(ad.cssValue?.isItalic == true ? "italic" : "normal")}",
                    $"font-size:{ad.cssValue?.fontSize ?? 12}px",
                    $"color:{ad.cssValue?.color ?? "#1E1E1E"}",
                    $"text-align:{ad.cssValue?.align ?? "left"}"
                }.Join(";");

                content = content.Replace(keyCss, css);
                content = content.Replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
            }

            return content;
        }
    }
}
