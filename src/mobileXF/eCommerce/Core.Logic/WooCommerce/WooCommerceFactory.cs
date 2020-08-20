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

		protected HttpResponse<TObject[]> Ok<TObject>(TObject[] result)
			where TObject : class, new()
		{
			return new HttpResponse<TObject[]>(result, HttpStatusCode.OK);
		}
	}
}
