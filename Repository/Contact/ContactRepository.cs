using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Dapper;
using Model.FuncResult;
using Model.Request.Contact;
using Model.Table;
using Repository.Base;
using Common;
namespace Repository.Contact
{
    public class ContactRepository : CRUDRepository<contact>, IContactRepository
    {
        public ContactRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<PagingResult<IEnumerable<contact>>> SelectAsync(ContactSelectRequest request)
        {
            var param = new DynamicParameters();
            param.Add("@contact_status_id", request.contact_status_id);
            param.Add("@page_index", request.page_index);
            param.Add("@page_size", request.page_size);
            param.Add("@sort_by", request.sort_by.ConvertToString());
            param.Add("@sort_mode", request.sort_mode.ConvertToString());
            param.Add("@search_key", request.search_key.ConvertToString());
            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<contact>("contact_select_paging", param);
            var total_count = param.Get<long>("@total_count");
            var page_size = request?.page_size ?? 1;
            if (page_size == 0) page_size = 1;
            var page_count = (int)total_count / page_size;
            var pagingResultSummaries = new PagingResultSummary()
            {
                page_count = page_count * page_size < total_count ? (page_count + 1) : page_count,
                page_number = request?.page_index ?? 0,
                page_size = request?.page_size ?? 0,
                total_count = total_count
            };
            return new PagingResult<IEnumerable<contact>>(pagingResultSummaries, list);
        }

        public async Task<int> SelectCountContactByStatusAsync(int contact_status_id)
        {
            var param = new DynamicParameters();
            param.Add("contact_status_id", contact_status_id);
            var result = await _dbConnection.SelectAsync<int>("contact_select_count_by_status", param);
            return result.FirstOrDefault();
        }
    }
}