using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Model.Table;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiLogTypeRepository : CRUDRepository<to_khai_log_type>, IToKhaiLogTypeRepository
    {
        public ToKhaiLogTypeRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}