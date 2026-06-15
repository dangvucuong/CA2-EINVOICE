using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.BackgroupJob
{
    // public class RenewCacheBackgroundService : IHostedService//BackgroundService
    public class RenewCacheBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public RenewCacheBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var scope = this._serviceProvider.CreateScope();
            var _serviceWrapper = scope.ServiceProvider.GetRequiredService<IServiceWrapper>();
            string content = "123";
            await _serviceWrapper.HoaDon.HoaDon.XuLyThongDiepAsync(content);
            //await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            // await _serviceWrapper.Category.DonVi.EnsureCachedDateUpdatedAsync();
            await Task.WhenAll(
               // _serviceWrapper.Category.DonVi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.Contact.CompanySize.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.Contact.ContactStatus.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

               //_serviceWrapper.HoaDon.LoaiHoaDon.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.HoaDon.LoaiHoaDonCT.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

               //_serviceWrapper.User.RoleApi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.User.Api.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
               //_serviceWrapper.User.Role.EnsureCachedDateUpdatedByLastUpdatTimeAsync()
             
               
            );


            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // var soTien = (decimal)123.4;
            // var soTienText = await Common.ReadNumberWebSerivce.DocSoAsync(soTien, "USD");
            var scope = this._serviceProvider.CreateScope();
            var _serviceWrapper = scope.ServiceProvider.GetRequiredService<IServiceWrapper>();
            var _repositoryWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
            // await _serviceWrapper.Category.DonVi.EnsureCachedDateUpdatedByLastUpdatTimeAsync();
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            await Task.WhenAll(
                _serviceWrapper.Category.DonVi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.Category.Watermark.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.Contact.CompanySize.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.Contact.ContactStatus.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

                _serviceWrapper.HoaDon.LoaiHoaDon.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.HoaDon.LoaiHoaDonCT.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

                _serviceWrapper.User.RoleApi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.User.Api.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.User.Role.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                _serviceWrapper.User.Vender.EnsureCachedDateUpdatedByLastUpdatTimeAsync()
            );

            // var xmlData = File.ReadAllText("103/hoaDonTest.xml");
            // // xslt v1
            // var result = await _serviceWrapper.Xslt.FillDataAsXmlAsync("Template/0103930279/0103930279_WLKZU88B_hdgtgtnthue.xslt", xmlData, null);
            // var resultV1 = await _serviceWrapper.Xslt.FillDataAsXmlAsync("103/ver1.xslt", xmlData, null);
            // // xslt v2
            // var result2 = await _serviceWrapper.Xslt.FillDataAsXmlAsync("Template/5aea9112-6a56-4a09-8277-6a4d85b720ac.xslt", xmlData, null);

            // var _userRepository = _repositoryWrapper.User.User;
            // var users = await _userRepository.SelectToUpdatePWFromV1Async();
            // // var tasks = new List<Task<bool>>();
            // foreach (var user in users)
            // {
            //     var newHasedPW = user.title.GenerateBcrypt();
            //     await _userRepository.ChangePWAsync(user.id, newHasedPW);
            // }
            


            await Task.CompletedTask;
        }
    }
}