using Contract.Service.Core;
using Service.Base;

namespace Service.Core
{
    public class ExceptionService : CRUDService<Model.Table.exception>, IExceptionService
    {
        public ExceptionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Core.Exception;
        }
    }
}

