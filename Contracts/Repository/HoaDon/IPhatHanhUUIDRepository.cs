using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IPhatHanhUUIDRepository : ICRUDRepository<phat_hanh_uuid>
    {
        Task<phat_hanh_uuid> SelectByUuIdAsync(string uuid);
        Task<bool> SaveLogUuidAsync(string uuid, string type_name, int user_id);

    }
}