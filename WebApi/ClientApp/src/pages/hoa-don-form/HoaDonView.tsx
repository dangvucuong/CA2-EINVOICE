import { DownloadIcon } from "@primer/octicons-react";
import { Box, Checkbox, Link, Pagination } from "@primer/react";
import { useEffect, useMemo, useRef, useState } from "react";
import { useReactToPrint } from "react-to-print";
import { useDebounce } from "use-debounce";
import { HOA_DON_API, hoaDonApi } from "../../api/hoa-don/hoaDonApi";
import Button from "../../component-ui/button";
import FormGroupInline from "../../component-ui/form-group-inline";
import PrintIcon from "../../component-ui/icon/print";
import PlaceHolder from "../../component-ui/place-holder";
import TextInput from "../../component-ui/text-input";
import { NotifyHelper } from "../../helpers/toast";
import { useAuth } from "../../hooks/useAuth";
import styles from "./HoaDonView.module.css";
import BackButton from "../../component-ui/back-button";
import { appInfo } from "../../AppInfo";

interface IHoaDonViewProps {
  id: number;
  showBackButton?: boolean;
  hinhThucHoaDonId?: number;
}

const HoaDonView = (props: IHoaDonViewProps) => {
  const { showBackButton = true, hinhThucHoaDonId = 1 } = props;
  const [htmlData, setHtmlData] = useState<string>("");
  const [isLoading, setIsLoading] = useState(false);
  const [isShowLoading] = useDebounce(isLoading, 300);
  const [htmPages, setHtmPages] = useState<string[]>([""]);
  const [pageIndex, setPageIndex] = useState(0);
  const [isShowPaging, setIsShowPaging] = useState(true);
  const [pageSize, setPageSize] = useState(10);
  const [inChuyenDoi, setInChuyenDoi] = useState(false);

  const { user } = useAuth();

  const printPdfUrl = useMemo(() => {
    if (props.id) {
      return `${appInfo.baseApiURL}/${HOA_DON_API}/${props.id}/pdf?page_size=${
        pageSize ?? 10
      }&${inChuyenDoi ? `chuyen_doi=${inChuyenDoi}` : ""}`;
    }
    return "#";
  }, [props.id, pageSize, inChuyenDoi]);

  const printPdfBienBanUrl = useMemo(() => {
    if (props.id) {
      return `${appInfo.baseApiURL}/${HOA_DON_API}/${props.id}/pdf-bien-ban`;
    }
    return "#";
  }, [props.id]);

  useEffect(() => {
    setHtmPages(htmlData.split('<div class="page-break"></div>'));
  }, [htmlData]);

  // ✅ Sử dụng contentRef thay vì callback function
  const contentRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setPageIndex(0);
    handlePrintAsync(pageSize, inChuyenDoi);
  }, [props.id, pageSize, inChuyenDoi]);

  const handlePrintAsync = async (page_size: number, inChuyenDoi: boolean) => {
    setIsLoading(true);
    const res = await hoaDonApi.getPrintHtml(props.id, page_size, inChuyenDoi);
    if (res.is_success) {
      setHtmlData(res.data);
    } else {
      NotifyHelper.Error(res.message ?? "Error");
    }
    setIsLoading(false);
  };

  // ✅ API mới của react-to-print v3+
  const handlePrint = useReactToPrint({
    contentRef, // ✅ Sử dụng contentRef thay vì content callback
    onAfterPrint: () => {
      setIsShowPaging(true);
    },
  });

  console.log(props);

  return (
    <Box>
      {showBackButton && <BackButton />}
      {isShowLoading && <PlaceHolder line_number={10} />}
      {!isShowLoading && (
        <Box>
          <Box
            sx={{
              flex: 1,
              p: 3,
              justifyContent: "center",
              display: "flex",
            }}
          >
            {isShowPaging && (
              <Box
                dangerouslySetInnerHTML={{ __html: htmPages[pageIndex] }}
                sx={{
                  width: "100%",
                  display: "flex",
                  justifyContent: "center",
                }}
              />
            )}
            {!isShowPaging && (
              <Box
                id="htmlView"
                dangerouslySetInnerHTML={{ __html: htmlData }}
                ref={contentRef} // ✅ Giữ nguyên ref
                sx={{
                  width: "100%",
                }}
              />
            )}
          </Box>
          <Box
            sx={{
              display: "flex",
              position: "sticky",
              bottom: user ? "-100px" : "-16px",
              alignItems: "center",
              backgroundColor: "#f6f8fa",
              zIndex: 1000,
              p: 3,
              m: -3,
              mt: 3,
            }}
          >
            {htmlData && (
              <Box sx={{ display: "flex", flex: 1 }}>
                <Button
                  text="In hóa đơn"
                  onClick={() => {
                    setIsShowPaging(false);
                    setTimeout(() => {
                      handlePrint();
                    }, 300);
                  }}
                  variant="invisible"
                  size="medium"
                  leadingVisual={PrintIcon}
                />
                <Link href={printPdfUrl}>
                  <Button
                    text="Tải xuống"
                    onClick={() => {}}
                    variant="invisible"
                    size="medium"
                    leadingVisual={DownloadIcon}
                  />
                </Link>

                {hinhThucHoaDonId !== 1 && (
                  <Link href={printPdfBienBanUrl}>
                    <Button
                      text="Tải biên bản"
                      onClick={() => {}}
                      variant="invisible"
                      size="medium"
                      leadingVisual={DownloadIcon}
                    />
                  </Link>
                )}

                <Link
                  href={`${appInfo.baseApiURL}/hoa-don/${props.id}/download`}
                >
                  <Button
                    text="Tải xuống XML"
                    variant="invisible"
                    size="medium"
                    leadingVisual={DownloadIcon}
                  />
                </Link>
              </Box>
            )}
            {isShowPaging && (
              <Box
                sx={{
                  mt: -3,
                  mb: -3,
                  display: "flex",
                  flexDirection: "row",
                  alignItems: "center",
                }}
              >
                <FormGroupInline label="In chuyển đổi">
                  <Checkbox
                    checked={inChuyenDoi}
                    onChange={(e) => {
                      setInChuyenDoi(e.target.checked);
                    }}
                  />
                </FormGroupInline>
                <Box
                  sx={{
                    mr: 3,
                    ml: 3,
                  }}
                >
                  <FormGroupInline label="Số dòng/trang">
                    <TextInput
                      type="number"
                      min={1}
                      max={20}
                      defaultValue={pageSize}
                      onBlur={(e) => {
                        if (!isNaN(parseInt(e.target.value ?? "0"))) {
                          setPageSize(parseInt(e.target.value));
                        }
                      }}
                    />
                  </FormGroupInline>
                </Box>
                <Box className={styles.pagingContainer}>
                  <Pagination
                    pageCount={htmPages.length}
                    currentPage={pageIndex + 1}
                    surroundingPageCount={4}
                    onPageChange={(e, n) => {
                      setPageIndex(n - 1);
                    }}
                  />
                </Box>
              </Box>
            )}
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default HoaDonView;
