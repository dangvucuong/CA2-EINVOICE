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
}
const HoaDonView = (props: IHoaDonViewProps) => {
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

  useEffect(() => {
    setHtmPages(htmlData.split('<div class="page-break"></div>'));
  }, [htmlData]);
  const contentRef = useRef<HTMLDivElement>(null); // ✅ Thêm type cho ref
  // console.log({
  //     htmlData
  // });

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
  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {
      setIsShowPaging(true);
      // console.log({
      //     onAfterPrint: "xxx"
      // });
    },
  });
  const handleExportWithFunction = () => {};
  const handleExportWithFunction2 = async () => {
    // const element = document.getElementById('htmlView');
    // html2pdf(element, {
    //     filename: 'output.pdf',
    //     image: { type: 'jpeg', quality: 0.98 },
    //     html2canvas: { dpi: 192, letterRendering: true },
    //     jsPDF: { unit: 'pt', format: 'a4', orientation: 'portrait' }
    // });
    // return;
    // const input = contentRef.current;
    // const canvas = await html2canvas(input, {
    //     useCORS: true,
    //     backgroundColor: "#ffff",
    //     // scale:1
    // });
    // const imgData = canvas.toDataURL('image/jpeg', 1);
    // console.log({ imgData });
    // const pdf = new jsPDF('p', 'mm', 'a4');
    // const imgWidth = 210;
    // const pageHeight = 297;
    // let imgHeight = (canvas.height * imgWidth) / canvas.width;
    // let heightLeft = imgHeight;
    // let position = 0;
    // pdf.addImage(imgData, 'JPEG', 0, position, imgWidth, imgHeight);
    // heightLeft -= (pageHeight);
    // console.log({
    //     position: position,
    //     imgWidth,
    //     imgHeight,
    //     heightLeft
    // });
    // while (heightLeft >= 0) {
    //     position = heightLeft - imgHeight;
    //     pdf.addPage();
    //     pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
    //     heightLeft -= pageHeight;
    //     console.log({
    //         position: position,
    //         imgWidth,
    //         imgHeight,
    //         heightLeft
    //     });
    // }
    // pdf.save(`HD${props.id}.pdf`);
    // setIsShowPaging(true)
  };

  return (
    <Box>
      <BackButton />
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
                }}
              />
            )}
            {!isShowPaging && (
              <Box
                id="htmlView"
                dangerouslySetInnerHTML={{ __html: htmlData }}
                ref={contentRef}
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
              // mb: -4
            }}
          >
            {htmlData && (
              <Box sx={{ display: "flex", flex: 1 }}>
                <Button
                  text="In chứng từ"
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
                    onClick={() => {
                      // setIsShowPaging(false);
                      // setTimeout(() => {
                      //     handleExportWithFunction();
                      // }, 300)
                    }}
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
                      // console.log({ e, n });
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
