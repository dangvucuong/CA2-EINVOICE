using Contracts.Repository.Base;

namespace Contracts.Repository.ToKhai
{
    public interface IToKhaiRepositoryWrapper : IBaseRepositoryWrapper
    {
        IToKhaiLogRepository ToKhaiLog { get; }
        IToKhaiLogTypeRepository ToKhaiLogType { get; }
        IToKhaiStatusRepository ToKhaiStatus { get; }
        IToKhaiRepository ToKhai { get; }
        IToKhaiCTSRepository ToKhaiCTS { get; }
    }
}