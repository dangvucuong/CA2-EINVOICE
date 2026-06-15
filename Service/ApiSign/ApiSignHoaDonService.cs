using Contracts.Service.ApiSign;
using Service.Base;
using ApiSign;
using Model.Respone.ApiSign;
using Common;
using Model.Table;
using WebApp;
using System.Xml;

namespace Service.ApiSign
{

    public class ApiSignHoaDonService : BaseService, IApiSignHoaDonService
    {
        // private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public ApiSignHoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiSignResultModel> SignAsync(string base64Xml, string mst, string serial)
        {
            try
            {
                // LogWriter.Writer(base64Xml, "ApiSignHoaDonService/SignAsync", "Start");
                // await _semaphore.WaitAsync();
                try
                {
                    using (var client = new wsCA2EinvoiceSoapClient(wsCA2EinvoiceSoapClient.EndpointConfiguration.wsCA2EinvoiceSoap))
                    {

                        await client.OpenAsync();

                        var data = await client.CA2KySo_HDAsync(base64Xml, mst, serial);
                        if (data != null)
                        {
                            var json = ((XmlNode[])data)[0].InnerText;
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiSignResultModel>(json);
                            return result;
                        }
                    }
                    return null;
                }
                finally
                {
                    // _semaphore.Release();
                    // LogWriter.Writer("", "ApiSignHoaDonService/SignAsync", "End");
                }
            }
            catch (Exception ex)
            {
                ex.SaveLog("ApiSignHoaDonService/SignAsync");
                return null;
            }
        }

        // public Task<ApiSignResultModel> SignAsync(string base64Xml,string mst, string serial)
        // {
        //     return this.SignAsync(base64Xml, AppSettings.ApiSignHd.MST, AppSettings.ApiSignHd.Serial);
        // }

        public async Task<ApiSignResultModel> SignHoaDonAsync(int hoaDonId, string serial)
        {
            var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoaDonId);
            if (hoaDon != null)
            {
                var result = await SignHoaDonAsync(hoaDon, hoaDon.donvi_ma_dv, serial);

                return result;
            }
            return null;
        }
        private async Task<ApiSignResultModel> SignHoaDonAsync(hoa_don hoaDon, string mst, string serial)
        {
            var base64 = "";
            if (hoaDon.hoa_don_hinh_thuc_code == "M")
            {
                var base64Result = await _serviceWrapper.HoaDon.HoaDon.CreateBase64MTTAsync(hoaDon);
                base64 = base64Result.is_success ? base64Result.data : "";
            }
            else
            {
                var xmlResult = await _serviceWrapper.HoaDon.HoaDon.CreateXmlKySoAsync(hoaDon);
                if (!xmlResult.is_success) return null;
                base64 = xmlResult.data.ConvertToBase64();

            }
            if (base64.ConvertToString() != string.Empty)
            {
                var signResultModel = await this.SignAsync(base64, mst, serial);
                if (signResultModel != null && signResultModel.Macode == 1)
                {
                    signResultModel.HoadonId = hoaDon.id;
                    await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                    {
                        id = hoaDon.id,
                        signed_text = signResultModel.SignedData
                    });
                }

                return signResultModel;
            }
            return null;
        }

        public async Task<IEnumerable<ApiSignResultModel>> SignHoaDonsAsync(List<int> hoaDonIds, string serial)
        {

            var hoaDons = await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(hoaDonIds);
            var tasks = hoaDons.Select(hoaDon => SignHoaDonAsync(hoaDon, hoaDon.donvi_ma_dv, serial)).ToList();
            var results = await Task.WhenAll(tasks);
            return results.Where(result => result != null);

        }
    }
}