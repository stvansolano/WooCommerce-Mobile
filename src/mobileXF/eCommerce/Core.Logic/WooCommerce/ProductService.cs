using System.Threading.Tasks;
using Core.Logic.Http;
using WooCommerceNET.WooCommerce.v3;

namespace Core.Logic.Services
{
	public class ProductService : WooCommerceFactory<Product>, IProductService
	{
		public ProductService(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<Product[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetProducts());

		public async Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId)
		{
			return Ok(await ApiInstance.GetProducts(categoryId));
		}

		public async Task<HttpResponse<Variation[]>> GetVariations(int productId)
		{
			return Ok(await ApiInstance.GetProductVariations(productId));
		}

		public async Task<HttpResponse<Product[]>> Search(string criteria)
		{
			return Ok(await ApiInstance.SearchProducts(criteria));
		}
	}
}
