import { ArrowLeftIcon } from "@primer/octicons-react";
import { Box, FormControl, SubNav } from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { useHistory, useParams } from "react-router-dom";
import { useReactToPrint } from "react-to-print";
import { loaiHoaDonCTTemplateApi } from "../../api/hoa-don/loaiHoaDonCTTemplateApi";
import { mauHoaDonApi } from "../../api/hoa-don/mauHoaDonApi";

import { DownloadIcon } from "@primer/octicons-react";

import Button from "../../component-ui/button";
import { ICssEditorElementData } from "../../component-ui/css-editor-element/CssEditorElement";
import Heading from "../../component-ui/heading";
import { PrintIcon } from "../../component-ui/icon";
import ModalActions from "../../component-ui/modal/ModalActions";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { useWindowSize } from "../../hooks/useWindowSize";
import { ILoaiHoaDonCTTemplate } from "../../models/responses/hoa-don/ILoaiHoaDonCTTemplate";
import { IMauHoaDon } from "../../models/responses/hoa-don/IMauHoaDon";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import AdvancedSettings from "./AdvancedSettings";
import BasicSettings, { IIBasicSettingsData } from "./BasicSettings";
import styles from "./MauHoaDonPage.module.css";
import MauHoaDonSelectTemplate from "./MauHoaDonSelectTemplate";
import axios from "axios";
import { appInfo } from "../../AppInfo";
import { applyMauHoaDonWatermarkToHtml } from "../../helpers/mauHoaDonWatermarkHelper";
const fixedAdvancedSettings = [
  { id: "ten_cong_ty", text: "Tên công ty", type: "nguoi_ban" },
  { id: "dia_chi", text: "Địa chỉ", type: "nguoi_ban" },
  { id: "so_tai_khoan", text: "Số tài khoản", type: "nguoi_ban" },
  { id: "dien_thoai", text: "Điện thoại", type: "nguoi_ban" },
  { id: "mst", text: "Mã số thuế", type: "nguoi_ban" },
  { id: "fax", text: "Fax", type: "nguoi_ban" },
  { id: "website", text: "Website", type: "nguoi_ban" },
  { id: "email", text: "Email", type: "nguoi_ban" },

  { id: "ho_ten_nguoi_mua", text: "Họ tên người mua", type: "nguoi_mua" },
  { id: "don_vi_mua_hang", text: "Đơn vị mua hàng", type: "nguoi_mua" },
  { id: "mst_nguoi_mua", text: "Mã số thuế", type: "nguoi_mua" },
  { id: "dia_chi_nguoi_mua", text: "Địa chỉ", type: "nguoi_mua" },
  { id: "so_tai_khoan_nguoi_mua", text: "Số tài khoản", type: "nguoi_mua" },
];
const MauHoDonPage = () => {
  const { id: pId }: any = useParams();
  const history = useHistory();
  const { user } = useAuth();
  const { status } = useAppSelector((x) => x.hoaDon.mauHoaDonReducer);
  const dispatch = useAppDispatch();
  const [loaiHoaDonCTTemplate, setLoaiHoaDonCTTemplate] =
    useState<ILoaiHoaDonCTTemplate>();
  const [isLoading, setIsLoading] = useState(false);
  const { width } = useWindowSize();
  const [previewData, setPreviewData] = useState<string>("");
  const [mauHoaDonEditing, setMauHoaDonEditing] = useState<IMauHoaDon>();
  const [isThietLapMode, setIsThietLapMode] = useState<"basic" | "advanced">(
    "basic"
  );
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  const [isExporting, setIsExporting] = useState(false);

  const [basicSetings, setBasicSetings] = useState<IIBasicSettingsData>({
    isShowLogoOrWatermark: "logo",
    isShowWatterMarkInnerTable: false,
    opacity: 50,
    logoFile: undefined,
    waterMarkFile: undefined,
    logoPosition: "left",
  });
  const [advancedSettings, setAdvancedSettings] = useState<
    ICssEditorElementData[]
  >(
    fixedAdvancedSettings.map((x) => {
      return {
        elementId: x.id,
        elementText: x.text,
        isDisplay: true,
        type: x.type,
        cssValue: {
          color: "#1E1E1E",
          fontSize: 12,
          align: "left",
          isBold: false,
          isItalic: false,
        },
      };
    })
  );
  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {
      // setIsShowPaging(true);
      // console.log({
      //     onAfterPrint: "xxx"
      // });
    },
  });

  const handleExportWithFunction = async () => {
    setIsExporting(true);
    // const url = `${domain ? domain : appInfo.baseApiURL}/${path}`

    const response = await axios.get(
      `${appInfo.baseApiURL}/mau-hoa-don/${mauHoaDonEditing?.id ?? 0}/pdf`,
      // {
      //     ...filter,
      //     tu_ngay: filter.tu_ngay === "" ? undefined : filter.tu_ngay,
      //     den_ngay: filter.den_ngay === "" ? undefined : filter.den_ngay,
      // },
      {
        headers: {
          Authorization: `Bearer ${localStorage.access_token}`,
          language: localStorage.getItem("language"),
        },
        responseType: "blob", // Important for handling binary data
      }
    );

    // Create a URL for the file blob
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", `${mauHoaDonEditing?.name ?? "Mau"}.pdf`); // File name
    document.body.appendChild(link);
    link.click();
    link.remove();
    setIsExporting(false);
  };

  const getPreviewHtml = () => {
    let html = previewData;
    html = applyMauHoaDonWatermarkToHtml(html, {
      watermarkUrl: basicSetings.waterMarkFile?.url,
      opacity: basicSetings.opacity,
      isShowWatterMarkInnerTable: basicSetings.isShowWatterMarkInnerTable,
      logoUrl: basicSetings.logoFile?.url,
    });
    if (basicSetings.vienFile) {
      html = html.replace(
        "{paramVien}",
        basicSetings.vienFile?.url?.replace("\\", "/") ?? ""
      );
    }
    if (basicSetings.logoPosition === "right") {
      html = html.replace("paramOpacityHeaderFlexDirection;", "row-reverse");
    }
    advancedSettings.forEach((ad) => {
      const keyCss = `${ad.elementId}_css;`;
      const keyCssDisplay = `${ad.elementId}_css_display;`;
      const css = [
        `font-weight:${ad.cssValue?.isBold ? "bold" : "normal"}`,
        `font-style:${ad.cssValue?.isItalic ? "italic" : "normal"}`,
        `font-size:${ad.cssValue?.fontSize}pt`,
        `color:${ad.cssValue?.color}`,
        `text-align:${ad.cssValue?.align}`,
      ].join(";");
      html = html.replace(keyCss, css);
      html = html.replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
    });
    return html;
  };

  const mauHoaDonId = useMemo(() => {
    return parseInt(pId) ?? 0;
  }, [pId]);
  useEffect(() => {
    if (mauHoaDonId > 0) {
      handleGetDetail();
    }
  }, [mauHoaDonId]);
  // const handleInputChange = (e: any) => {
  //     const { name, value } = e.target;
  //     setFormData({
  //         ...formData,
  //         [name]: value,
  //     });
  // };
  useEffect(() => {
    if (mauHoaDonEditing) {
      reset({
        ...mauHoaDonEditing,
        ngay_qd: mauHoaDonEditing?.ngay_qd
          ? moment(mauHoaDonEditing?.ngay_qd).format("YYYY-MM-DD")
          : undefined,
      });
      setBasicSetings({
        ...basicSetings,
        isShowWatterMarkInnerTable:
          mauHoaDonEditing.is_show_wattermark_inner_table ?? false,
        logoPosition:
          mauHoaDonEditing.logo_position === "right" ? "right" : "left",
        opacity: mauHoaDonEditing.watermark_opacity ?? 50,
        vienFile: mauHoaDonEditing.vien_path
          ? {
              file_name: "",
              url: mauHoaDonEditing.vien_path,
            }
          : undefined,
        logoFile: mauHoaDonEditing.logo_path
          ? {
              file_name: "",
              url: mauHoaDonEditing.logo_path,
            }
          : undefined,
        waterMarkFile: mauHoaDonEditing.watermark_path
          ? {
              file_name: "",
              url: mauHoaDonEditing.watermark_path,
            }
          : undefined,
      });
      try {
        const advanced_settings_json = JSON.parse(
          mauHoaDonEditing?.advanced_settings_json ?? ""
        );
        setAdvancedSettings(advanced_settings_json);
      } catch (error) {
        console.log({
          error,
        });
      }
      // console.log({
      //     xyz: JSON.parse(mauHoaDonEditing?.advanced_settings_json??"")
      // });

      // if (mauHoaDonEditing.logo_path) {
      //     setLogoFile({
      //         file_name: "",
      //         url: mauHoaDonEditing.logo_path
      //     })
      // }
      // if (mauHoaDonEditing.watermark_path) {
      //     setWaterMarkFile({
      //         file_name: "",
      //         url: mauHoaDonEditing.watermark_path
      //     })
      //     setIsShowWatterMarkInnerTable(mauHoaDonEditing.is_show_wattermark_inner_table ?? false)
      // }
      handleCreatePreviewDataAsync();
    }
  }, [mauHoaDonEditing]);
  const handleGetDetail = async () => {
    const res = await mauHoaDonApi.getById(mauHoaDonId);
    if (res.is_success) {
      setMauHoaDonEditing(res.data);
    }
  };
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...mauHoaDonEditing,
    },
  });
  useEffect(() => {
    if (status === eReducerStatusBase.is_saved) {
      history.push("../../../mau-hoa-don");
    }
  }, [status]);
  const onSubmit = (data: any) => {
    dispatch(
      rootAction.hoaDon.mauHoaDonAction.saveStart({
        ...mauHoaDonEditing,
        ...data,
        donvi_ma_dv: user?.donvi_ma_dv ?? "",
        loai_hoa_don_ct_template_id:
          loaiHoaDonCTTemplate?.id ??
          mauHoaDonEditing?.loai_hoa_don_ct_template_id,
        logo_path: basicSetings.logoFile?.url ?? "",
        vien_path: basicSetings.vienFile?.url ?? "",
        watermark_path: basicSetings.waterMarkFile?.url ?? "",
        is_show_wattermark_inner_table: basicSetings.isShowWatterMarkInnerTable,
        logo_position: basicSetings?.logoPosition ?? "left",
        advanced_settings_json: JSON.stringify(advancedSettings),
        watermark_opacity: basicSetings.opacity,
      })
    );
  };
  const handleCreatePreviewDataAsync = async () => {
    setIsLoading(true);
    const res = await loaiHoaDonCTTemplateApi.createPreviewData({
      donvi_ma_dv: "",
      id: mauHoaDonId ?? 0,
      loai_hoa_don_ct_template_id:
        mauHoaDonEditing?.loai_hoa_don_ct_template_id ??
        loaiHoaDonCTTemplate?.id ??
        0,
      logo_path: basicSetings.logoFile?.url ?? "",
      vien_path: basicSetings.vienFile?.url ?? "",
      name: "",
      ngay_qd: "2020-01-01",
      so_qd: "",
      watermark_path: basicSetings.waterMarkFile?.url ?? "",
      is_show_wattermark_inner_table: basicSetings.isShowWatterMarkInnerTable,
      watermark_opacity: basicSetings.opacity,
      is_active: true,
      xslt_path: mauHoaDonEditing?.xslt_path ?? "",
    });
    if (res.is_success) {
      setPreviewData(res.data);
    } else {
      setPreviewData("");
    }
    setIsLoading(false);
  };
  useEffect(() => {
    if (mauHoaDonId <= 0 && loaiHoaDonCTTemplate) {
      handleCreatePreviewDataAsync();
    }
  }, [mauHoaDonId, loaiHoaDonCTTemplate]);
  return (
    <Box>
      <Box sx={{ mb: 3 }}>
        <Button
          text="Quay lại"
          leadingVisual={ArrowLeftIcon}
          variant="invisible"
          onClick={() => {
            history.goBack();
          }}
        />
      </Box>
      {!loaiHoaDonCTTemplate && !mauHoaDonEditing && mauHoaDonId === 0 && (
        <MauHoaDonSelectTemplate
          onSelectionChanged={(data) => {
            setLoaiHoaDonCTTemplate(data);
          }}
        />
      )}
      {(loaiHoaDonCTTemplate || mauHoaDonEditing) && (
        <Box
          sx={{
            display: "flex",
            flexWrap: width < 1400 ? "wrap" : "nowrap",
            // alignItems:"baseline"
          }}
        >
          <Box
            className={styles.setup}
            sx={{
              width: "320px",
              // minHeight: window.innerHeight - 100,
              borderRightStyle: "solid",
              borderColor: "border.default",
              borderWidth: "1px",
              pr: 3,

              // backgroundColor:"rebeccapurple"
            }}
          >
            <form onSubmit={handleSubmit(onSubmit)}>
              <Box
                display={"grid"}
                sx={{
                  gap: 2,
                }}
              >
                <Heading text="Thiết lập" />
                <FormControl
                  sx={{
                    mb: 2,
                  }}
                >
                  <SubNav aria-label="Main">
                    <SubNav.Links>
                      <SubNav.Link
                        selected={isThietLapMode === "basic"}
                        sx={{
                          cursor: "pointer",
                        }}
                        onClick={() => {
                          setIsThietLapMode("basic");
                        }}
                      >
                        Thiết lập cơ bản
                      </SubNav.Link>
                      <SubNav.Link
                        selected={isThietLapMode === "advanced"}
                        sx={{
                          cursor: "pointer",
                        }}
                        onClick={() => {
                          setIsThietLapMode("advanced");
                        }}
                      >
                        Thiết lập nâng cao
                      </SubNav.Link>
                    </SubNav.Links>
                  </SubNav>
                </FormControl>
                {isThietLapMode === "basic" && (
                  <BasicSettings
                    data={basicSetings}
                    errors={errors}
                    register={register}
                    onValueChanged={(data) => {
                      setBasicSetings(data);
                    }}
                  />
                )}
                {isThietLapMode === "advanced" && (
                  <>
                    <AdvancedSettings
                      cssElements={advancedSettings}
                      onValueChanged={(data) => {
                        setAdvancedSettings(data);
                      }}
                    />
                  </>
                )}

                <ModalActions>
                  <Button
                    text={`${
                      mauHoaDonEditing ? "Lưu mẫu hóa đơn" : "Lưu mẫu hóa đơn"
                    }`}
                    variant="primary"
                    size="large"
                    type="submit"
                    isLoading={status === eReducerStatusBase.is_saving}
                  />
                </ModalActions>
              </Box>
            </form>
          </Box>
          <Box
            className={styles.preview}
            sx={{
              flex: 1,
              p: 3,
              width: width - 80 - 200 - 320 - 10,
              overflowX: "auto",
              // display:"flex",
              // flexDirection:"column"
              // minWidth: "900px"
            }}
          >
            <Box
              dangerouslySetInnerHTML={{ __html: getPreviewHtml() }}
              ref={contentRef}
            />
            <Box
              sx={{
                display: "flex",
                // justifyContent: "center"
              }}
            >
              <Button
                text="In mẫu"
                onClick={() => {
                  handlePrint();
                }}
                variant="invisible"
                size="medium"
                leadingVisual={PrintIcon}
              />
              <Button
                text="Tải xuống"
                disabled={(mauHoaDonEditing?.id ?? 0) <= 0}
                onClick={() => {
                  handleExportWithFunction();
                }}
                isLoading={isExporting}
                variant="invisible"
                size="medium"
                leadingVisual={DownloadIcon}
              />
            </Box>
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default MauHoDonPage;
