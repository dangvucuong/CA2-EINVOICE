using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Service.HoaDon;
using Model.Enum;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class RsYeuCauKyService : CRUDService<rs_yeu_cau_ky>, IRsYeuCauKyService
    {
        public RsYeuCauKyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.RsYeuCauKyRepository;
        }

        public async Task<bool> SaveYeuCauKyAsync(string code, string user_id, e_rs_yeu_cau_ky_type type, string type_key)
        {
            var userId = this.GetCurrentUserId();
            var obj = new rs_yeu_cau_ky()
            {
                code = code,
                user_id = user_id,
                type = type.ToString(),
                type_key = type_key,
                ket_qua_ky = ""
            };
            obj.SetInsertInfo(userId);
            obj.id = await this.InsertAsync(obj);
            await _serviceWrapper.Cache.SetDataAsync<rs_yeu_cau_ky>(code, obj, DateTime.Now.AddDays(1));
            return true;
        }

        public Task<rs_yeu_cau_ky> SelectByCodeAsync(string code)
        {
            return _repositoryWrapper.HoaDon.RsYeuCauKyRepository.SelectByCodeAsync(code);
        }
    }
}