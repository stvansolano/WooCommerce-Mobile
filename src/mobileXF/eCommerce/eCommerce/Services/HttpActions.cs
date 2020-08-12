using System;
using System.Net;

namespace eCommerce.Services
{
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