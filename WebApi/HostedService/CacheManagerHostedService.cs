using System;
using System.Threading;
using System.Threading.Tasks;
using Contract.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using Model.Respone.Upload;
using Contracts.Repository;
using System.IO;
using Service.HoaDon.XuLyThongDiep;
using System.Xml.Xsl;
namespace WebApi.HostedService
{
    public class CacheManagerHostedService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create 
        // the scoped service, MyDbContext
        private readonly IServiceProvider _serviceProvider;
        // private readonly IConfiguration _configuration;
        public CacheManagerHostedService(IServiceProvider serviceProvider
            // IConfiguration configuration
            )
        {

            _serviceProvider = serviceProvider;
            // _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            // var p = "test123".GenerateBcrypt();
            var scope = this._serviceProvider.CreateScope();
            var _serviceWrapper = scope.ServiceProvider.GetRequiredService<IServiceWrapper>();
            var _repositoryWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
            // var xmlData = File.ReadAllText("103/check.xml");
            // var xsltArgumentList = new XsltArgumentList();
            // var resultKhach = await _serviceWrapper.Xslt.FillDataAsXmlAsync("Template/2400901475-002/538bd1e9-b68c-4720-8e2e-25b5d8a31c9f.xslt", xmlData, xsltArgumentList);
            // var resultNCM = await _serviceWrapper.Xslt.FillDataAsXmlAsync("Template/0103930279-999/1a650350-0a5c-422a-bbb9-2c05f435c901.xslt", xmlData, xsltArgumentList);
            // var yeuCauKy = await _repositoryWrapper.HoaDon.RsYeuCauKyRepository.SelectByCodeAsync("138f34f209ce40eb8ced57122b03349b");
            // var hoaDon = await _serviceWrapper.Core.Account.XuLyThongDiepKyRSAsync(yeuCauKy);
            // var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(309979);
            // await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
            // var result = await _serviceWrapper.Core.Account.LoginRSAsync(new Model.Request.Account.LoginRSRequest()
            // {
            //     rs_ma_but_ky = "536128",
            //     reCaptchaToken = "",
            //     session_id=""
            // });
            // var html = await _serviceWrapper.HoaDon.HoaDon.GetHtmlPrintAsync(349536);
            // var html = await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot.CreateXmlKySoAsync(152);
            // await _serviceWrapper.HoaDon.HoaDon.SaoChepHoaDonNghichDaoAsync();
            // await _serviceWrapper.HoaDon.HoaDon.UpdateHoaDonPhatHanhLoiNhieuLanAsync();
            // await _serviceWrapper.HoaDon.HoaDon.UpdateHoaDonPhatHanhLoiChuaPhatHanhAsync();
            // await _serviceWrapper.HoaDon.HoaDon.XuLyLoiMaKhongLienTiepAsync();
            // var toKhai = await _serviceWrapper.ToKhaiSerivceWrapper.ToKhai.CreateXmlKySoAsync(689);

            // var thongDiep = File.ReadAllText("103/473.xml");
            // var toKhai = await _repositoryWrapper.ToKhaiWrapper.ToKhai.SelectByIdAsync(473);
            // var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            // await _serviceWrapper.ToKhaiSerivceWrapper.ToKhai.XuLyThongDiepAsync(toKhai, ketQuaThongDiepRespone, thongDiep);
            // if (hoaDon != null)
            // {
            //     await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
            // }

            // var thongDiep = File.ReadAllText("103/tbss-217.xml");
            // var thongBaoSaiSot = await _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSot.SelectByIdAsync(217);
            // var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            // var result =
            //        await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot.XuLyThongDiepAsync(thongBaoSaiSot,
            //            ketQuaThongDiepRespone, thongDiep);

            // var thongDiep = File.ReadAllText("103/mttien-204loi.xml");
            // var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            // await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepKetQuaPhanHanhAsync( ketQuaThongDiepRespone, thongDiep);

            // var thongDiep = File.ReadAllText("103/70717273.xml");
            // var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            // await _serviceWrapper.HoaDon.HoaDon.XuLyThongDiepAsync(thongDiep);


            // var fromId = 251404;
            // var toId = 251423;
            // var hoaDonCoMaService = new XuLyHoaDonCoMaService(_serviceProvider);
            // for (var id = fromId; id <= toId; id++)
            // {
            //     if (fromId != 251423)
            //     {
            //         var thongDiep = File.ReadAllText($"hoa-don-20/{id}.xml");
            //         var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            //         var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(id);
            //         var result = await hoaDonCoMaService.XuLyThongDiepAsync(hoaDon, ketQuaThongDiepRespone, thongDiep);
            //     }
            //     var ma_tra_cuu = myExtension.CreateMaTraCuu(id);
            //     await _repositoryWrapper.HoaDon.HoaDon.UpdateMaTraCuuAsync(id, ma_tra_cuu);

            // }
            // var html = await _serviceWrapper.HoaDon.HoaDon.GetHtmlPrintAsync(349536);
            // var _hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            // var hoaDon = await _hoaDonService.SelectByIdAsync(433351);
            // if (hoaDon != null)
            // {
            //     var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
            //     if (donVi != null && donVi.total_cks_con_lai <= 0)
            //     {
            //         var x = 0;
            //     }
            //     if (hoaDon.hoa_don_hinh_thuc_code == "M")
            //     {
            //         var base64Result = await _hoaDonService.CreateBase64MTTAsync(hoaDon);

            //     }
            //     else
            //     {
            //         var xml = await _hoaDonService.CreateXmlKySoAsync(hoaDon);
            //         var base64 = xml.ConvertToBase64();

            //     }

            // }

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // noop
            return Task.CompletedTask;
        }


    }
}