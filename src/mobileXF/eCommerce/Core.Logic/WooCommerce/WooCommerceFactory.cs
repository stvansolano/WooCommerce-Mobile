using System.Net;
using Core.Logic.Http;

namespace Core.Logic.Services
{
	public abstract class WooCommerceFactory<T> : HttpFactory<T>
		where T : class, new()
	{
		protected WooCommerceFactory(WooComerceApi apiInstance)
		{
			ApiInstance = apiInstance;
		}

		public WooComerceApi ApiInstance { get; }

		protected HttpResponse<T[]> Ok(T[] result)
		{
			return new HttpResponse<T[]>(result, HttpStatusCode.OK);
		}
	}
}
