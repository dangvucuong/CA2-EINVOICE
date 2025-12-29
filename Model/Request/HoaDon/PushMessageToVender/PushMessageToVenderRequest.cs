namespace Model.Request.HoaDon.PushMessageToVender
{
    public class PushMessageToVenderRequest
    {
        /// <summary>
        /// id của bên tích hợp
        /// </summary>
        public string VenderId { get; set; }
        /// <summary>
        /// mã số thuế của hóa đơn
        /// </summary>
        public string MST { get; set; }
        /// <summary>
        /// invoice_id, id hóa đơn của bên tích hợp
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Mã số hóa đơn
        /// </summary>
        public string ResponseEInvoiceNumber { get; set; }
        /// <summary>
        /// Ngày hóa đơn
        /// </summary>
        public string EInvoiceIssueDate { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// message
        ///1: Hóa đơn phát hành thành công, đã được cấp mã
        ///2: Hóa đơn phát hành không thành công, chưa có phản hồi của CQT
        ///5: Đã hủy hóa đơn, gửi thông báo sai sót thành công và được CQT duyệt
        ///6: Hóa đơn không đủ điều kiện cấp mã
        ///7: Xóa hóa đơn nháp chưa có số
        /// </summary>
        public string Message { get; set; }
    }
}