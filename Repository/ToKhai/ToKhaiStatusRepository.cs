using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Model.Table;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiStatusRepository : CRUDRepository<to_khai_status>, IToKhaiStatusRepository
    {
        public ToKhaiStatusRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}