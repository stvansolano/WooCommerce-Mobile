using System;
using System.Net;
using System.Threading.Tasks;

namespace eCommerce.Core.Http
{
    public interface IHttpFactory<T>
		where T : class, new()
    {
        string BaseUrl { get; set; }

        Task<HttpResponse<T[]>> GetAsync(string endpointName = null, HttpRequest request = null);
    }

    public class HttpRequest
    {
    }

    public class HttpResponse<T>
    {
        public HttpResponse(T instance, HttpStatusCode statusCode = HttpStatusCode.OK, Exception ex = null)
        {
            Result = instance;
            StatusCode = statusCode;
            Exception = ex;
        }

        public T Result { get; }
        public HttpStatusCode StatusCode { get; }
        public Exception Exception { get; }
    }
}