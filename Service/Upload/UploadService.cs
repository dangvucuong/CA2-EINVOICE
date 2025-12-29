using Contracts.Service.Upload;
using Microsoft.AspNetCore.Http;
using Model.Base;
using Model.Static;
using Service.Base;
using Common;
using Amazon.S3;
using Amazon.S3.Model;
using Model.Respone.Upload;
using System.Security.Cryptography.X509Certificates;
using Model.Request.Upload;
using ExcelDataReader;
namespace Service.Upload
{
    public class UploadService : BaseService, IUploadService
    {
        public UploadService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        private bool IsValidFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[^1];
            var acceptFiles = new List<string>(){
                ".xls",
                ".xlsx",
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".svg"
            };
            return acceptFiles.Contains(extension);
        }
        private bool IsValidCertFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[^1];
            var acceptFiles = new List<string>(){
                ".cer"  , ".crt"            };
            return acceptFiles.Contains(extension);
        }
        private async Task<string> UploadFileToAWSAsync(Stream fileStream, string fileNameTarget, string subFolder = "")
        {
            var result = "";
            try
            {
                var keyName = AppSettings.AWSS3Config.DefaultFolder.ConvertToString();
                using (var client = new AmazonS3Client(AppSettings.AWSS3Config.AccessKey, AppSettings.AWSS3Config.SecretKey, Amazon.RegionEndpoint.APSoutheast1))
                {
                    if (!string.IsNullOrEmpty(subFolder)) keyName += "/" + subFolder.Trim();
                    keyName += "/" + fileNameTarget;

                    var TagSet = new List<Tag> { new Tag() { Key = "public", Value = "yes" } };

                    var request = new Amazon.S3.Model.PutObjectRequest
                    {
                        BucketName = AppSettings.AWSS3Config.BucketName,
                        Key = keyName,
                        InputStream = fileStream,
                        // CannedACL = S3CannedACL.PublicRead,
                        // TagSet = TagSet
                    };
                    await client.PutObjectAsync(request);


                    result = AppSettings.AWSS3Config.Domain + "/"
                        + AppSettings.AWSS3Config.DefaultFolder
                        + (subFolder != string.Empty ? ("/" + subFolder) : "")
                        + "/" + fileNameTarget;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }
        private async Task<string> WriteFile(IFormFile file, string uploadFolder)
        {
            try
            {
                var fileName = string.Empty;
                var extension = "." + file.FileName.Split('.')[^1];
                fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    return AppSettings.FixedValue.FileDomain != "" ? $"{AppSettings.FixedValue.FileDomain}/{filePath}" : filePath;
                }
                // using (var stream = file.OpenReadStream())
                // {
                //     var url = await this.UploadFileToAWSAsync(stream, fileName, uploadFolder);
                //     return url;
                // }
            }
            catch
            {
                return string.Empty;
            }
        }
        public async Task<FunctionResult<string>> UploadAsync(IFormFile formFile)
        {
            if (IsValidFile(formFile))
            {
                var uploadToFolder = "Upload";

                var url = await WriteFile(formFile, uploadToFolder);
                if (url != string.Empty)
                {
                    return new SuccessResult<string>() { data = url };
                }
                else
                {
                    return new ErrorResult<string>() { data = string.Empty };
                }
            }
            else
            {
                return new ErrorResult<string>() { message = "File type is not valid" };
            }
        }

        public async Task<FunctionResult<UploadCerRespone>> UploadCertAsync(IFormFile formFile)
        {
            if (IsValidCertFile(formFile))
            {
                var uploadToFolder = "Upload/Cer";

                var url = await WriteFile(formFile, uploadToFolder);
                if (url != string.Empty)
                {
                    var cerInfo = await this.ReadCerInfoAsync(url);
                    if (cerInfo != null) return new SuccessResult<UploadCerRespone>(
                        new UploadCerRespone
                        {
                            cer_info = cerInfo,
                            file_name = formFile.FileName,
                            url = url
                        }
                    );

                }
                return new ErrorResult<UploadCerRespone>() { };
            }
            else
            {
                return new ErrorResult<UploadCerRespone>() { message = "File type is not valid" };
            }
        }
        private async Task<CerInfo> ReadCerInfoAsync(string certFilePath)
        {
            try
            {
                if (AppSettings.FixedValue.FileDomain != "")
                {
                    certFilePath = certFilePath.Replace(AppSettings.FixedValue.FileDomain + "/", "");
                }
                X509Certificate2 cert = new X509Certificate2(certFilePath);
                return new CerInfo()
                {
                    serial_number = cert.SerialNumber,
                    issuer = cert.Issuer,
                    not_after = cert.NotAfter,
                    not_before = cert.NotBefore,
                    public_key = cert.PublicKey.Key.ToXmlString(false),
                    signature_algorithm = cert.SignatureAlgorithm.FriendlyName,
                    version = cert.Version.ToString(),
                    extensions = Newtonsoft.Json.JsonConvert.SerializeObject(cert.Extensions.Select(x => x.Oid.FriendlyName)),
                    subject = cert.Subject,
                    thumbprint = cert.Thumbprint

                };
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<System.Data.DataTable> ReadUploadedExcelFile(ReadUploadedExcelFileRequest request)
        {
            try
            {
                if (AppSettings.FixedValue.FileDomain != "")
                {
                    request.file_path = request.file_path.Replace(AppSettings.FixedValue.FileDomain + "/", "");
                }
                using (FileStream fileStream = new FileStream(request.file_path, FileMode.Open, FileAccess.Read))
                {

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);

                        // Auto-detect format, supports:
                        //  - Binary Excel files (2.0-2003 format; *.xls)
                        //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                        using (var reader = ExcelReaderFactory.CreateReader(memStream))
                        {
                            // 2. Use the AsDataSet extension method
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                // Gets or sets a value indicating whether to set the DataColumn.DataType 
                                // property in a second pass.
                                UseColumnDataType = true,

                                // Gets or sets a callback to determine whether to include the current sheet
                                // in the DataSet. Called once per sheet before ConfigureDataTable.
                                FilterSheet = (tableReader, sheetIndex) => true,

                                // Gets or sets a callback to obtain configuration options for a DataTable. 
                                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                                {
                                    // Gets or sets a value indicating the prefix of generated column names.
                                    EmptyColumnNamePrefix = "Column",

                                    // Gets or sets a value indicating whether to use a row from the 
                                    // data as column names.
                                    UseHeaderRow = false,

                                    // Gets or sets a callback to determine which row is the header row. 
                                    // Only called when UseHeaderRow = true.
                                    ReadHeaderRow = (rowReader) =>
                                    {
                                        // F.ex skip the first row and use the 2nd row as column headers:
                                        rowReader.Read();
                                    },

                                    // Gets or sets a callback to determine whether to include the 
                                    // current row in the DataTable.
                                    FilterRow = (rowReader) =>
                                    {
                                        return true;
                                    },

                                    // Gets or sets a callback to determine whether to include the specific
                                    // column in the DataTable. Called once per column after reading the 
                                    // headers.
                                    FilterColumn = (rowReader, columnIndex) =>
                                    {
                                        return true;
                                    }
                                }
                            });
                            var tables = result.Tables;
                            if (tables.Count > request.sheetIndex)
                            {
                                var dt = tables[request.sheetIndex];
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        dt.Columns[i].ColumnName = dt.Rows[0][i].ConvertToString() == "" ? i.ToString() : dt.Rows[0][i].ConvertToString();
                                    }
                                    dt.Rows.RemoveAt(0);
                                }
                                dt.Columns.Add("ID", typeof(int));
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["ID"] = i;
                                }
                                return dt;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ex.SaveLog("UploadService/ReadUploadedExcelFile");
                return null;
            }
        }
    }
}