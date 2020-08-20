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
	}
}