using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Model.Table;
using Repository.Base;

namespace Repository.Category
{
    public class WatermarkTemplateRepository : CRUDRepository<watermark_template>, IWatermarkTemplateRepository
    {
        public WatermarkTemplateRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}