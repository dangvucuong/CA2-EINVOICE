using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Service.Base;
using Model.Enum;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IRsYeuCauKyService : ICRUDService<rs_yeu_cau_ky>
    {
        Task<rs_yeu_cau_ky> SelectByCodeAsync(string code);
        Task<bool> SaveYeuCauKyAsync(string code, string user_id, e_rs_yeu_cau_ky_type type, string type_key);
    }
}