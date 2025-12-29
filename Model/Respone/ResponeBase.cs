using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Model.Respone
{
    [Serializable]
    public class ResponeBase<T>
    {
        public HttpStatusCode status_code { get; set; }
        public bool is_success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public ResponeBase() { }

        public ResponeBase(HttpStatusCode status_code, T data, string message = "")
        {
            this.status_code = status_code;
            this.is_success = status_code == HttpStatusCode.OK;
            this.message = message;
            this.data = data;
        }

        public ResponeBase(HttpStatusCode status_code, string message = "")
        {
            this.status_code = status_code;
            this.is_success = status_code == HttpStatusCode.OK;
            this.message = message;

        }
        public ResponeBase(bool is_success, T data, string message = "")
        {
            this.status_code = is_success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            this.is_success = is_success;
            this.message = message;
            this.data = data;
        }
        public ResponeBase(bool is_success, string message = "")
        {
            this.status_code = is_success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            this.is_success = is_success;
            this.message = message;
        }
        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                StatusCode = (int)this.status_code,
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(this),
                ContentType = "json"
            };
        }
        public Task<ContentResult> ToContentResultAsync()
        {
            return Task.FromResult(new ContentResult()
            {
                StatusCode = (int)this.status_code,
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(this),
                ContentType = "json"
            });
        }
    }
    public class ResponeBaseSuccess : ResponeBase<object>
    {

        public ResponeBaseSuccess(object data, string message = "")
        {
            this.status_code = HttpStatusCode.OK;
            this.is_success = true;
            this.message = message;
            this.data = data;
        }
        public ResponeBaseSuccess(string message = "")
        {
            this.status_code = HttpStatusCode.OK;
            this.is_success = true;
            this.message = message;
        }
    }
    public class ResponeBaseErr
    {
        public HttpStatusCode status_code { get; set; }
        public bool is_success { get; set; }
        public string message { get; set; }


        public ResponeBaseErr(string message = "")
        {
            this.status_code = HttpStatusCode.BadRequest;
            this.is_success = false;
            this.message = message;
        }
        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                StatusCode = (int)this.status_code,
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(this),
                ContentType = "json"
            };
        }
        public Task<ContentResult> ToContentResultAsync()
        {
            return Task.FromResult(new ContentResult()
            {
                StatusCode = (int)this.status_code,
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(this),
                ContentType = "json"
            });
        }
    }
}

