namespace eCommerce.Services
{
	public class HttpRequest<T>
		where T : class, new()
	{
	}

	public class HttpResponse<T>
		where T : class, new()
	{
		public HttpResponse(T instance)
		{

		}
	}
}