using Contracts.Repository.Base;
using Contracts.Repository.User;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class OTPRepository : CRUDRepository<app_otp>, IOTPRepository
    {
        public OTPRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<app_otp> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@phone_number", phone_number);
            return _dbConnection.SelectFirstOrDefaultAsync<app_otp>("otp_select_by_phonenumber", param);
        }

        public Task<IEnumerable<app_otp>> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number, DateTime date)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@phone_number", phone_number);
            param.Add("@date", date);
            return _dbConnection.SelectAsync<app_otp>("otp_select_by_phonenumber_date", param);
        }


    }
}