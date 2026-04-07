using Contracts.Service.Base;
using Model.Base;
using Model.RemoteSigning;

namespace Contracts.Service.RemoteSigning
{
    public interface IRemoteSigningSerivce : IBaseService
    {
        Task<FunctionResult<string>> GetCertInfoAsync(int ma_but_ky);
        Task<FunctionResult<string>> GuiYeuCauKyAsync<T>(T request);
        // Task<FunctionResult<T>> GetKetQuaKyAsync<T>(string code);
        Task<FunctionResult<string>> TryGetKetQuaKyThenClearAsync(string code, string user_id,CancellationToken cancellationToken);
        Task<FunctionResult<string>> DangNhapAsync(DangNhapRequest request);
        Task<FunctionResult<string>> DangNhapCodeAsync(DangNhapRequest request);
        Task<FunctionResult<string>> KySoAsync(BaseRequest request);
        Task<FunctionResult<string>> TryGetKetQuaKyThenClearAsync(string code);
        Task<bool> UpdateYeuCauKyThanhCongAsync(string code);
    }
}