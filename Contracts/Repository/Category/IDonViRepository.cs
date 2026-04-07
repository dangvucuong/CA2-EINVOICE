using Contracts.Repository.Base;
using Model.Table;

namespace Contract.Repository.Category
{
    public interface IDonViRepository : ICRUDRepository<donvi>
    {
        Task<donvi> SelectByMaDonViAsync(string ma_dv);
        Task<int> CalculateTongCKSConLaiAsync(string ma_dv);
    }
}