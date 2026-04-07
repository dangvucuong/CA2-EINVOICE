using Contracts.Repository.Base;
using Contracts.Repository.User;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class VenderRepository : CRUDRepository<vender>, IVenderRepository
    {
        public VenderRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}