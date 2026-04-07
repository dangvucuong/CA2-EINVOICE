using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Respone.Upload;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/upload")]
    [MustLogged]
    public class UploadController : BaseController
    {
        public UploadController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpPost]
        public async Task<ContentResult> UploadAsync(IFormFile form_file)
        {
            var uploadResult = await _serviceWrapper.Upload.UploadAsync(form_file);
            if (uploadResult.is_success)
            {
                var respone = new UploadRespone()
                {
                    url = uploadResult.data,
                    file_name = form_file.FileName
                };
                return this.OK(respone);
            }
            else
            {
                return this.BadRequest(uploadResult.message);
            }
        }
        [HttpPost]
        [Route("cert")]
        public async Task<ContentResult> UploadCerAsync(IFormFile form_file)
        {
            var uploadResult = await _serviceWrapper.Upload.UploadCertAsync(form_file);
            if (uploadResult.is_success)
            {
                return this.OK(uploadResult.data);
            }
            else
            {
                return this.BadRequest(uploadResult.message);
            }
        }
        // [HttpPost]
        // [Route("/api/link")]
        // public async Task<ContentResult> GetPublicLinkAsync([FromBody] CreatePublicLinkRequest request)
        // {

        //     using (var s3Client = new AmazonS3Client(AppSettings.AWSS3Config.AccessKey, AppSettings.AWSS3Config.SecretKey, Amazon.RegionEndpoint.APSoutheast1))
        //     {
        //         var keyName = request.url.Replace(AppSettings.AWSS3Config.Domain + "/", "");
        //         var requestS3 = new GetPreSignedUrlRequest
        //         {
        //             BucketName = AppSettings.AWSS3Config.BucketName,
        //             Key = keyName,
        //             Expires = DateTime.UtcNow.AddHours(1)
        //         };
        //         var url = s3Client.GetPreSignedURL(requestS3);
        //         return this.OK(new
        //         {
        //             url = url
        //         });
        //     }

        // }
    }
}

