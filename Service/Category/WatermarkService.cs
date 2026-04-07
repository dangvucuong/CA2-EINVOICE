using Contracts.Service.Category;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class WatermarkService : CRUDServiceWithCache<watermark_template>, IWatermarkService
    {
        public WatermarkService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.WatermarkTemplate;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "watermark_template:";
        }
    }
}