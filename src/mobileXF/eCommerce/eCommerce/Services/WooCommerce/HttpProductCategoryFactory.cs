using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WooCommerceNET.WooCommerce.v3;
using System.Linq;
using eCommerce.Core.Http;
using eCommerce.Http;

namespace eCommerce.Services
{
	public class HttpProductCategoryFactory : HttpFactory<ProductCategory>
	{
		public override async Task<HttpResponse<ProductCategory[]>> GetAsync(string endpointName = null, HttpRequest request = null)
		{
			endpointName = "/products/categories";

			try
			{
				using (var client = GetClient(GetUrl(endpointName)))
				{
					var jsonContent = await client.GetStringAsync(string.Empty).ConfigureAwait(false);

					var result = JsonConvert.DeserializeObject<ProductCategoryDto[]>(jsonContent);

					var response = result.Select(item => new ProductCategory {
						name = item.Name
					}).ToArray();

					return new HttpResponse<ProductCategory[]>(response);
				}
			}
			catch (Exception ex)
			{
				return new HttpResponse<ProductCategory[]>(Array.Empty<ProductCategory>(), HttpStatusCode.InternalServerError, ex);
			}
		}

		public class ProductCategoryDto
		{
			public string Name { get; set; }
		}
	}
}