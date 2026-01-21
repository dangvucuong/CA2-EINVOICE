using Contracts.Service.Category;
using Model.Static;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class DonViService : CRUDServiceWithCache<donvi>, IDonViService
    {
        public DonViService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DonVi;
        }

        //public async Task<donvi> GetGipInfoAsync(string ma_dv)
        //{
        //    try
        //    {
        //        using (var client = new GipNcm.CA2_GIPSoapClient(GipNcm.CA2_GIPSoapClient.EndpointConfiguration.CA2_GIPSoap))
        //        {
        //            client.ClientCredentials.UserName.UserName = AppSettings.WSInterTRCA2Config.Username;
        //            client.ClientCredentials.UserName.Password = AppSettings.WSInterTRCA2Config.Password;
        //            await client.OpenAsync();

        //            var data = await client.Laythongtin_NNTAsync(ma_dv);
        //            if (data != null)
        //            {
        //                var diaBanHanhChinhInfo = await _repositoryWrapper.Category.DiaBanHanhChinh.SelectByMaDiaBanAsync(data.MA_HUYEN);
        //                var resutl = new donvi()
        //                {
        //                    mst = data.MST,
        //                    ten_dv = data.TEN_NNT,
        //                    co_quan_thu_id_chuquan = 0,
        //                    donvi_chuquan = "",
        //                    dia_chi = $"{data.MOTA_DIACHI}, {diaBanHanhChinhInfo?.Ten_QuanHuyen ?? ""}, {diaBanHanhChinhInfo?.Ten_TinhThanhPho ?? ""}",

        //                };
        //                return resutl;
        //            }

        //        }
        //        return null;
        //    }

        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}


        public async Task<donvi> GetGipInfoAsync(string ma_dv)
        {
            try
            {
                using (var client = new GipNcm_V2.CA2_GIPSoapClient(GipNcm_V2.CA2_GIPSoapClient.EndpointConfiguration.CA2_GIPSoap))
                {
                    client.ClientCredentials.UserName.UserName = AppSettings.GipNcm_V2Config.Username;
                    client.ClientCredentials.UserName.Password = AppSettings.GipNcm_V2Config.Password;
                    await client.OpenAsync();

                    var myHeader = new GipNcm_V2.AuthHeader();
                    // Gán user/pass vào header này (Bạn gõ dấu chấm để xem thuộc tính chính xác là gì, thường là Username/Password)
                    myHeader.Username = AppSettings.GipNcm_V2Config.Username;
                    myHeader.Password = AppSettings.GipNcm_V2Config.Password;

                    var data = await client.Laythongtin_NNTAsync(myHeader, ma_dv);

                    var realData = data.Laythongtin_NNTResult;

                    if (realData != null)
                    {
                        var resultl = new donvi()
                        {
                            // Phải chấm qua realData (tức là data.Laythongtin_NNTResult)
                            mst = realData.MST,
                            ten_dv = realData.TEN_NNT,
                            co_quan_thu_id_chuquan = 0,
                            donvi_chuquan = "",

                            // Các trường khác tương tự
                            dia_chi = $"{realData.DIACHI_DAYDU}"
                        };
                        return resultl;
                    }

                }
                return null;
            }

            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<donvi> SelectByMaDonViAsync(string ma_dv)
        {
            var cachedData = await this.SelectByOptionKeyAsync(ma_dv);
            if (cachedData != null)
            {
                return cachedData;
            }
            return await _repositoryWrapper.Category.DonVi.SelectByMaDonViAsync(ma_dv);
        }

        public async Task<donvi> SyncTotalChuKySoDaMuaAsync(string ma_dv)
        {
            var donVi = await this.SelectByMaDonViAsync(ma_dv);
            if (donVi != null)
            {
                var total_cks_con_lai = await _repositoryWrapper.Category.DonVi.CalculateTongCKSConLaiAsync(ma_dv);
                donVi.total_cks_con_lai = total_cks_con_lai;
                await this.UpdateAsync(donVi);
            }
            return null;
        }

        protected override void ConfigKey()
        {
            this._keyPrefix = "donvi:";
            this._itemKeyField = "id";
            this._itemKeyFieldOption = "ma_dv";
        }
    }
}