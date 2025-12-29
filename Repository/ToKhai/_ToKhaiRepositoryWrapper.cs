using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiRepositoryWrapper : BaseRepositoryWrapper, IToKhaiRepositoryWrapper
    {
        private IToKhaiLogRepository _toKhaiLogRepository;
        private IToKhaiLogTypeRepository _toKhaiLogTypeRepository;
        private IToKhaiStatusRepository _toKhaiStatusRepository;
        private IToKhaiRepository _toKhaiRepository;
        private IToKhaiCTSRepository _toKhaiCTSRepository;
        public ToKhaiRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public IToKhaiLogRepository ToKhaiLog => _toKhaiLogRepository ??= new ToKhaiLogRepository(_defaultConnection);

        public IToKhaiLogTypeRepository ToKhaiLogType => _toKhaiLogTypeRepository ??= new ToKhaiLogTypeRepository(_defaultConnection);

        public IToKhaiStatusRepository ToKhaiStatus => _toKhaiStatusRepository ??= new ToKhaiStatusRepository(_defaultConnection);

        public IToKhaiRepository ToKhai => _toKhaiRepository??= new ToKhaiRepository(_defaultConnection);

        public IToKhaiCTSRepository ToKhaiCTS => _toKhaiCTSRepository??= new ToKhaiCTSRepository(_defaultConnection);
    }
}