using Contracts.Service.Core;
using Model.Enum;
using Model.Table;
using Service.Base;

namespace Service.Core
{
    public class LocalizedResourceService : CRUDService<localized_resource>, ILocalizedResourceService
    {
        public LocalizedResourceService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.Core.LocalizedResource;
        }

        public async Task<string> GetValueByKeyAsync(e_localized_resource_scope scope, string code, string language)
        {
            //cần triển khai cache
            var list = await this.SelectAllAsync();
            var obj = list.Where(x => x.scope == scope.ToString() && x.language == language && x.code == code).FirstOrDefault();
            if (obj != null)
            {
                return obj.value;
            }
            return code;
        }

        public async Task<IEnumerable<localized_resource>> SelectAsync(e_localized_resource_scope scope, string language)
        {
            //cần triển khai cache
            var list = await this.SelectAllAsync();
            return list.Where(x => x.scope == scope.ToString() && x.language == language).ToList();
        }
    }
}