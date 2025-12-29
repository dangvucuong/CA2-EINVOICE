using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IRsYeuCauKyRepository : ICRUDRepository<rs_yeu_cau_ky>
    {
        Task<rs_yeu_cau_ky> SelectByCodeAsync(string code);
        

    }
}