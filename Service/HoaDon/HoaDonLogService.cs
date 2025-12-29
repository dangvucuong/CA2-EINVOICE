using System.Text.RegularExpressions;
using Contracts.Service.HoaDon;
using Model.Base;
using Model.Enum;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonLogService : CRUDService<hoa_don_log>, IHoaDonLogService
    {
        public HoaDonLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.HoaDonLog;
        }

        public async Task<FunctionResult<string>> SaveFromPhatHanhAsync(int hoa_don_id, string noi_dung_thuc_hien, string xmlResult, bool isCQTChapNhan)
        {
            //save xmlResult to file
            //save log
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await File.WriteAllTextAsync(filePath, xmlResult);
            var mltdiep = "";
            string pattern = @"<MLTDiep>(.*?)</MLTDiep>";
            var match = Regex.Match(xmlResult, pattern, RegexOptions.Singleline);
            if (match.Success)
            {
                mltdiep = match.Groups[1].Value;
            }

            var log = new hoa_don_log()
            {
                file_thong_diep_url = filePath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = "Cơ quan thuế",
                noi_dung_thuc_hien = noi_dung_thuc_hien,
                hoa_don_id = hoa_don_id,
                hoa_don_log_type_id = isCQTChapNhan ? (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN : (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN,
                mltdiep = mltdiep
            };
            log.SetInsertInfo(0);
            await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
            return new SuccessResult<string>("", filePath);
        }

        public async Task<FunctionResult<string>> SaveFromPhatHanhBangKeAsync(int hoa_don_id, string noi_dung_thuc_hien, string fileXmlPath, bool isCQTChapNhan)
        {
            var log = new hoa_don_log()
            {
                file_thong_diep_url = fileXmlPath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = "Cơ quan thuế",
                noi_dung_thuc_hien = noi_dung_thuc_hien,
                hoa_don_id = hoa_don_id,
                hoa_don_log_type_id = isCQTChapNhan ? (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN : (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN
            };
            log.SetInsertInfo(0);
            await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
            return new SuccessResult<string>("", fileXmlPath);
        }

        public Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id)
        {
            return _repositoryWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(hoa_don_id);
        }

        public Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id, int hoa_don_log_type_id)
        {
            return _repositoryWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(hoa_don_id, hoa_don_log_type_id);
        }
    }
}