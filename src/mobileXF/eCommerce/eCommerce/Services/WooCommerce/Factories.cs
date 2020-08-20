using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;
using Core.Logic.Http;
using Core.Logic;
using WooCommerce.Services;

namespace eCommerce.Services
{
	public interface IHttpProductFactory : IHttpFactory<Product>
	{
		Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId);
	}

	public class HttpProductCategoryFactory : WooCommerceFactory<ProductCategory>
	{
		public HttpProductCategoryFactory(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<ProductCategory[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetCategories());
	}

	public class HttpProductTagFactory : WooCommerceFactory<ProductTag>
	{
		public HttpProductTagFactory(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<ProductTag[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetTags());
	}

	public class HttpProductFactory : WooCommerceFactory<Product>, IHttpProductFactory
	{
		public HttpProductFactory(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<Product[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetProducts());

		public async Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId)
		{
			return Ok(await ApiInstance.GetProducts(categoryId));
		}
	}
}