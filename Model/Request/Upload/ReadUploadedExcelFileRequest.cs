namespace Model.Request.Upload
{
    public class ReadUploadedExcelFileRequest
    {
        public string file_path { get; set; }
        public int sheetIndex { get; set; }
    }
}