using Common;
using Contracts.Service.HoaDon.PushMessageToVender;
using Model.Enum;
using Model.Request.HoaDon.PushMessageToVender;
using Model.Static;
using Model.Table;
using Service.Base;
using WebApp;

namespace Service.HoaDon.PushMessageToVender
{
    public class PushMessageToVenderService : BaseService, IPushMessageToVenderService
    {

        public PushMessageToVenderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<bool> CheckAndPushMessageAsync(hoa_don hoaDon)
        {
            try
            {
                LogWriter.Writer($"Hóa đơn: {hoaDon.id}", "CheckAndPushMessageAsync", "");
                if (hoaDon.vender_id.ConvertToString().Trim() == "") return true;


                var Status = 0;
                var Message = "";

                switch (hoaDon.hoa_don_trang_thai_id)
                {
                    case 2:
                        Status = 1;
                        Message = "Hóa đơn phát hành thành công, đã được cấp mã";
                        break;
                    case 3:
                        if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_HUY)
                        {
                            Status = 5;
                            Message = "Hóa Đã hủy hóa đơn, gửi thông báo sai sót thành công và được CQT duyệt";
                        }
                        if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
                        {
                            Status = 8;
                            Message = "Hóa Đã hủy nội bộ";
                        }
                        break;
                    case 7:
                        Status = 6;
                        Message = "Hóa đơn không đủ điều kiện cấp mã";
                        break;

                    case 8:
                        Status = 6;
                        Message = "Hóa đơn không đủ điều kiện cấp mã";
                        break;
                    default:
                        break;
                }
                if (hoaDon.is_deleted)
                {
                    Status = 7;
                    Message = "Xóa hóa đơn nháp chưa có số";
                }


                await this.PushMessageAsync(new PushMessageToVenderRequest()
                {
                    EInvoiceIssueDate = hoaDon.ngay_hoa_don.ToString("yyyy-MM-dd HH:mm:ss"),
                    Key = hoaDon.invoice_id.ConvertToString().StartsWith($"{hoaDon.donvi_ma_dv}-")
                    ? hoaDon.invoice_id.ConvertToString().Substring(hoaDon.donvi_ma_dv.Length + 1)
                    : hoaDon.invoice_id.ConvertToString(),
                    Message = Message,
                    Status = Status,
                    MST = hoaDon.nguoi_ban_mst,
                    ResponseEInvoiceNumber = hoaDon.ma_so_hoa_don.ConvertToString(),
                    VenderId = hoaDon.vender_id.ConvertToString()

                });

                return true;
            }
            catch (System.Exception ex)
            {
                LogWriter.Writer(ex.Message, "PushMessageToVenderService/CheckAndPushMessageAsync", "");
                return false;
            }
        }

        public Task<bool> PushMessageAsync(PushMessageToVenderRequest request)
        {
            try
            {
                var config = AppSettings.RabbitMqHoaDonMessageToVender;
                config.QueueName = request.VenderId;
                config.VirtualHost = request.VenderId;
                // config.QueueName = "tichhopssc";
                // config.VirtualHost = "tichhopssc";
                _serviceWrapper.Core.RabitMQ.SendMessage(request, config);
                // this._taskQueueService.EnqueueTask(async cancellationToken =>
                //  {
                //      _serviceWrapper.Core.RabitMQ.SendMessage(request, config);
                //  });
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(true);
            }
        }
    }
}