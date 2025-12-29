using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class RsYeuCauKyRepository : CRUDRepository<rs_yeu_cau_ky>, IRsYeuCauKyRepository
    {
        public RsYeuCauKyRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<rs_yeu_cau_ky> SelectByCodeAsync(string code)
        {
            var param = new DynamicParameters();
            param.Add("@code", code);
            return _dbConnection.SelectFirstOrDefaultAsync<rs_yeu_cau_ky>("rs_yeu_cau_ky_select", param);
        }
    }
}