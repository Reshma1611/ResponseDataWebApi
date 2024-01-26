using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace ResponseDataWebAPI
{
    //[InvalidModelStateHandler]
    public class BaseController : Controller
    {
        

        public const string CONTENT_TYPE_APPLICATION_JSON = "application/json";
        public const string CONTENT_TYPE_MULTIPART_FORMDATA = "multipart/form-data";
        public BaseController() { }

        #region Api Result Helpers

        #region OK Result
        protected IActionResult OkResult() => StatusCode((int)HttpStatusCode.OK);

        protected IActionResult OkResult(int status) => Json(PrepareResultObject(status, null, (object?)null));

        protected IActionResult OkResult<T>(T data) => Json(PrepareResultObject(null, null, data));

        protected IActionResult OkResult(string message) => Json(PrepareResultObject(null, message, (object?)null));

        protected IActionResult OkResult<T>(string message, T data) => Json(PrepareResultObject(null, message, data));

        protected IActionResult OkResult<T>(int status, T data) => Json(PrepareResultObject(status, null, data));

        protected IActionResult OkResult(int status, string message) => Json(PrepareResultObject(status, message, (object?)null));

        protected IActionResult OkResult<T>(int status, string message, T data) => Json(PrepareResultObject(status, message, data));
        #endregion

        #region Other Result
        protected IActionResult OtherResult(HttpStatusCode code) => StatusCode((int)code);

        protected IActionResult OtherResult(HttpStatusCode code, int status)
        {
           
        var res = new JsonResult(PrepareResultObject(status, null, (object?)null))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult(HttpStatusCode code, int status, string message)
        {
            var res = new JsonResult(PrepareResultObject(status, message, (object?)null))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult(HttpStatusCode code, string message)
        {
            var res = new JsonResult(PrepareResultObject(null, message, (object?)null))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult<T>(HttpStatusCode code, int status, T data)
        {
            var res = new JsonResult(PrepareResultObject(status, null, data))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult<T>(HttpStatusCode code, T data)
        {
            var res = new JsonResult(PrepareResultObject(null, null, data))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult<T>(HttpStatusCode code, int status, string message, T data)
        {
            var res = new JsonResult(PrepareResultObject(status, message, data))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected IActionResult OtherResult<T>(HttpStatusCode code, string message, T data)
        {
            var res = new JsonResult(PrepareResultObject(null, message, data))
            {
                StatusCode = (int)code,
                ContentType = CONTENT_TYPE_APPLICATION_JSON,
            };

            return res;
        }

        protected async Task<IActionResult> FileResultAsync(string filepath, string contentDispositionHeaderValue = "attachment")
        {
            var memStream = new MemoryStream();
            using (var stream = new FileStream(filepath, FileMode.Open))
                await stream.CopyToAsync(memStream);

            memStream.Position = 0;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out string? contentType))
                contentType = "application/octet-stream";

            var filename = Path.GetFileName(filepath);
            var contentDisposition = new ContentDispositionHeaderValue(contentDispositionHeaderValue);
            contentDisposition.SetHttpFileName(filename);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return File(memStream, contentType, filename);
        }
        #endregion

        private static ResponseData<T> PrepareResultObject<T>(int? status, string? message, T? data)
        {
            var resObj = new ResponseData<T>()
            {
                Data = data,
                Message = message,
                Status = status
            };

            return resObj;
        }
        #endregion
    }

    //public class InvalidModelStateHandlerAttribute : ActionFilterAttribute
    //{
    //    public const string CONTENT_TYPE_APPLICATION_JSON = "application/json";
    //    public const string CONTENT_TYPE_MULTIPART_FORMDATA = "multipart/form-data";
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        if (!context.ModelState.IsValid)
    //        {
    //            var data = context.ModelState.Where(x => x.Value?.Errors.Count > 0)
    //                .Select(x => new FieldError(
    //                    x.Key,
    //                    x.Value?.Errors?.Select(x => x.ErrorMessage).ToArray() ?? new string[] { "" }
    //                    ))
    //                .ToArray();
    //            var resData = new ResponseData<FieldError[]>()
    //            {
    //                Data = data
    //            };
    //            context.Result = new JsonResult(resData)
    //            {
    //                StatusCode = (int)HttpStatusCode.BadRequest,
    //                ContentType = CONTENT_TYPE_APPLICATION_JSON,
    //            };
    //        }

    //        base.OnActionExecuting(context);
    //    }
    //}
    
    public class FieldError
    {
        public FieldError(string property, string[] errors)
        {
            Property = property;
            Errors = errors;
        }

        public FieldError(string property, string error)
        {
            Property = property;
            Errors = new string[] { error };
        }

        public string Property { get; set; }
        public string[]? Errors { get; set; }
    }

    public class ResponseData<T>
    {
        public int? Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }

    public class ResponseData
    {
        public int? Status { get; set; }
        public string? Message { get; set; }
    }

   
}
