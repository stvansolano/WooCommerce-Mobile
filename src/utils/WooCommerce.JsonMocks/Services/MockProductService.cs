using System.Net;
using System.Threading.Tasks;
using Core.Logic.Http;
using Core.Logic.Services;
using WooCommerceNET.WooCommerce.v3;

namespace WooCommerce.Mocks
{
	public class MockProductService : MockHttpFactory<Product>, IProductService
	{
		public async Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId)
		{
			var mockedCall = await base.GetAsync();

			return mockedCall;
		}

		public async Task<HttpResponse<Variation[]>> GetVariations(int productId)
		{
			await Task.Delay(500);

			return new HttpResponse<Variation[]>(new Variation[0], HttpStatusCode.OK);
		}
	}
}