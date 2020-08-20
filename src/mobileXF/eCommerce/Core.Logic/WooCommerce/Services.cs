using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;
using Core.Logic.Http;

namespace Core.Logic.Services
{
	public interface IProductService : IHttpFactory<Product>
	{
		Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId);
	}

	public class ProductCategoryService : WooCommerceFactory<ProductCategory>
	{
		public ProductCategoryService(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<ProductCategory[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetCategories());
	}

	public class ProductTagService : WooCommerceFactory<ProductTag>
	{
		public ProductTagService(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<ProductTag[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetTags());
	}

	public class ProductService : WooCommerceFactory<Product>, IProductService
	{
		public ProductService(WooComerceApi apiInstance) : base(apiInstance) { }

		public override async Task<HttpResponse<Product[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		 => Ok(await ApiInstance.GetProducts());

		public async Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId)
		{
			return Ok(await ApiInstance.GetProducts(categoryId));
		}
	}
}