using System;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace Core.Logic
{
	public class WooComerceApi
	{
		public WooComerceApi(string websiteRoot, string client, string secret)
		{
			Endpoint = websiteRoot;
			Client = client;
			Secret = secret;
		}

		private WCObject GetObject()
		{
			RestAPI rest = new RestAPI($"{Endpoint}/wp-json/wc/v3/", Client, Secret);
			WCObject wc = new WCObject(rest);

			return wc;
		}

		public string Endpoint { get; }
		public string Client { get; }
		public string Secret { get; }

		public async Task<Product[]> GetProducts()
		{
			var obj = GetObject();

			var result = await obj.Product.GetAll();

			return result.ToArray();
		}

		public async Task<ProductCategory[]> GetCategories()
		{
			var obj = GetObject();

			var result = await obj.Category.GetAll();

			return result.ToArray();
		}

		public async Task<ProductTag[]> GetTags()
		{
			var obj = GetObject();

			var result = await obj.Tag.GetAll();

			return result.ToArray();
		}
	}
}
