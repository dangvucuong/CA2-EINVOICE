using System.Text;
using Common;
using Contract.Service;
using Microsoft.Extensions.Hosting;
using Model.Enum;
using Model.Respone.RS;
using Model.Static;
using Model.Table;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebApp;

namespace Service.Consumer
{
    public class RemoteSigningConsummer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        private IServiceWrapper _serviceWrapper;
        public RemoteSigningConsummer(IServiceProvider serviceProvider, IServiceWrapper serviceWrapper)
        {
            _serviceProvider = serviceProvider;

            _serviceWrapper = serviceWrapper;

            InitializeRabbitMqWithRetry();

        }
        private void InitializeRabbitMqWithRetry()
        {
            while (true)
            {
                try
                {
                    LogWriter.Writer("Trying to connect to RabbitMQ...", "", "");
                    if (InitializeRabbitMq())
                    {
                        LogWriter.Writer("Connected to RabbitMQ", "", "");
                        break;
                    }
                    Task.Delay(5000).Wait();
                }
                catch (Exception ex)
                {
                    LogWriter.Writer("Failed to connect to RabbitMQ: {message}", ex.Message, "");
                    Task.Delay(5000).Wait();
                }
            }
        }
        private bool InitializeRabbitMq()
        {
            var _rabbitMqConfiguration = AppSettings.RabbitMqRemoteSigning;
            try
            {

                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqConfiguration.HostName,
                    UserName = _rabbitMqConfiguration.UserName,
                    Password = _rabbitMqConfiguration.Password,
                    Port = _rabbitMqConfiguration.Port,
                    VirtualHost = _rabbitMqConfiguration.VirtualHost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                    RequestedHeartbeat = TimeSpan.FromSeconds(30) //// Tăng heartbeat cho kết nối từ xa
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.BasicQos(0, 10, false); // Giới hạn 10 message mỗi lần để tránh quá tải

                _channel.ExchangeDeclare(_rabbitMqConfiguration.ExchangeName, ExchangeType.Direct, true, false, null);

                // declare queue
                //_channel.QueueDeclare(_rabbitMqConfiguration.QueueName, true, false, false, null);
                _channel.QueueDeclare(_rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                // bind queue
                _channel.QueueBind(_rabbitMqConfiguration.QueueName, _rabbitMqConfiguration.ExchangeName, string.Empty, null);
                LogWriter.Writer("RemoteSigningConsummer Started", Newtonsoft.Json.JsonConvert.SerializeObject(_rabbitMqConfiguration), "");
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.Writer(ex.Message, Newtonsoft.Json.JsonConvert.SerializeObject(_rabbitMqConfiguration), "");
                LogWriter.Writer(ex.Message, "RemoteSigningConsummer", "");
                ex.SaveLog("RemoteSigningConsummer");
                return false;
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var _rabbitMqConfiguration = AppSettings.RabbitMqRemoteSigning;
                stoppingToken.ThrowIfCancellationRequested();
                _connection.ConnectionShutdown += async (sender, args) =>
                {
                    {
                        try
                        {
                            LogWriter.Writer("ConnectionShutdown", "ConnectionShutdown", "");
                            LogWriter.Writer($"{args.ReplyText} - {args.ReplyCode}", "ConnectionShutdown", "");
                        }
                        finally
                        {
                            await RetryConnectionAsync(stoppingToken);
                        }
                    }
                };
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (ch, ea) =>
                {

                    try
                    {
                        var content = Encoding.UTF8.GetString(ea.Body.Span);
                        if (AppSettings.FixedValue.LogThongDiep)
                        {
                            LogWriter.Writer(content, "", "");
                        }
                        var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<RemoteSigningMessage>(content);
                        if (payload != null)
                        {

                            try
                            {
                                //kiểm tra yêu cầu ký của chức năng nào
                                //gửi đến service tương ứng
                                var yeuCauKyCached = await _serviceWrapper.Cache.GetDataAsync<rs_yeu_cau_ky>(payload.code);
                                if (yeuCauKyCached == null)
                                {
                                    yeuCauKyCached = await _serviceWrapper.HoaDon.RsYeuCauKy.SelectByCodeAsync(payload.code);
                                }
                                if (yeuCauKyCached != null)
                                {
                                    yeuCauKyCached.ket_qua_ky = payload.ket_qua_ky;
                                    yeuCauKyCached.SetUpdateInfo(0);
                                    await _serviceWrapper.HoaDon.RsYeuCauKy.UpdateAsync(yeuCauKyCached);
                                    if (yeuCauKyCached.type == e_rs_yeu_cau_ky_type.DANG_NHAP.ToString())
                                    {
                                        await _serviceWrapper.Core.Account.XuLyThongDiepKyRSAsync(yeuCauKyCached);
                                    }
                                    if (yeuCauKyCached.type == e_rs_yeu_cau_ky_type.KY_SO_HOA_DON.ToString())
                                    {
                                        await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepKySoHoaDonAsync(yeuCauKyCached);
                                    }
                                    if (yeuCauKyCached.type == e_rs_yeu_cau_ky_type.PHAT_HANH_MTT_HOA_DON.ToString())
                                    {
                                        await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepPhatHanhMttHoaDonAsync(yeuCauKyCached);
                                    }
                                    if (yeuCauKyCached.type == e_rs_yeu_cau_ky_type.KY_SO_VA_PHAT_HANH_HOA_DON.ToString())
                                    {
                                        await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepKySoVaPhatHanhHoaDonAsync(yeuCauKyCached);
                                    }
                                    if (yeuCauKyCached.type == e_rs_yeu_cau_ky_type.KY_SO_VA_PHAT_HANH_BANG_KE.ToString())
                                    {
                                        await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepKySoVaPhatHanhHoaDonBangKeAsync(yeuCauKyCached);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                LogWriter.Writer(ex.Message, "Xu ly yeu cau ky", payload.code.ConvertToString());
                            }
                            finally
                            {
                                await _serviceWrapper.RemoteSigningSerivce.UpdateYeuCauKyThanhCongAsync(payload.code.ConvertToString());
                            }
                        }
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        LogWriter.Writer($"Error processing message: {ex.Message}", "", "");
                        ex.SaveLog("RemoteSigningConsummer/Received");
                        _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue nếu lỗi
                    }
                };
                consumer.ConsumerCancelled += (model, ea) =>
                            {
                                LogWriter.Writer("Consumer cancelled by server!", "", "");
                            };
                _channel.BasicConsume(_rabbitMqConfiguration.QueueName, autoAck: false, consumer: consumer);
                LogWriter.Writer("Consumer started", "", "");
                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                ex.SaveLog("RemoteSigningConsummer");
            }

        }
        private async Task RetryConnectionAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (stoppingToken != null && stoppingToken.IsCancellationRequested) { break; }
                try
                {
                    LogWriter.Writer("Trying to reconnect to RabbitMQ...", "", "");
                    var isConnected = InitializeRabbitMq();
                    if (isConnected)
                    {
                        LogWriter.Writer("Reconnected to RabbitMQ", "", "");
                        // Khởi động lại consumer
                        await ExecuteAsync(stoppingToken);
                        break; // Thoát vòng lặp sau khi consumer chạy lại thành công
                    }
                    else
                    {
                        await Task.Delay(5000);
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Writer("Failed to reconnect to RabbitMQ with message: {message}", ex.Message, "");
                    await Task.Delay(5000);
                }
            }
        }
        public override void Dispose()
        {
            try
            {
                if (_channel?.IsOpen == true) _channel.Close();
                if (_connection?.IsOpen == true) _connection.Close();
            }
            catch (Exception ex)
            {
                LogWriter.Writer($"Dispose error: {ex.Message}", "", "");
            }
            base.Dispose();
        }
    }
}