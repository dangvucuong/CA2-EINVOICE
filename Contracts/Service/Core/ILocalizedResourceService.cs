using Contracts.Service.Base;
using Model.Enum;
using Model.Table;

namespace Contracts.Service.Core
{
    public interface ILocalizedResourceService : ICRUDService<localized_resource>
    {
        Task<string> GetValueByKeyAsync(e_localized_resource_scope scope, string code, string language);
        Task<IEnumerable<localized_resource>> SelectAsync(e_localized_resource_scope scope, string language);
    }
}