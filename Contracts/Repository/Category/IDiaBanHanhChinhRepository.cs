using Contracts.Repository.Base;
using Model.Respone.Category;

namespace Contracts.Repository.Category
{
    public interface IDiaBanHanhChinhRepository : IBaseRepository
    {
        Task<DiaBanHanhChinh> SelectByMaDiaBanAsync(string maDiaBan);
    }
}