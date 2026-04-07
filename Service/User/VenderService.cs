using Contracts.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class VenderService : CRUDServiceWithCache<vender>, IVenderService
    {
        public VenderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.Vender;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "vender:";
        }
    }
}