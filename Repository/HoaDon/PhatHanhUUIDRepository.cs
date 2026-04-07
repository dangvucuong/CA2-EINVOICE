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
    public class PhatHanhUUIDRepository : CRUDRepository<phat_hanh_uuid>, IPhatHanhUUIDRepository
    {
        public PhatHanhUUIDRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<bool> SaveLogUuidAsync(string uuid, string type_name, int user_id)
        {
            var obj = new phat_hanh_uuid()
            {
                uuid = uuid,
                type_name = type_name
            };
            obj.SetInsertInfo(user_id);
            obj.id = await this.InsertAsync(obj);
            return true;
        }

        public Task<phat_hanh_uuid> SelectByUuIdAsync(string uuid)
        {
            var param = new DynamicParameters();
            param.Add("@uuid", uuid);
            return _dbConnection.SelectFirstOrDefaultAsync<phat_hanh_uuid>("phat_hanh_uuid_select", param);
        }
    }
}