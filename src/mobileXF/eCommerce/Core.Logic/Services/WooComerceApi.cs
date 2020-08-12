using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;

namespace Core.Logic
{
	public class WooComerceApi
	{
		public async Task<List<Product>> Get()
		{
			//RestAPI rest = new RestAPI("http://www.yourstore.co.nz/wp-json/wp/v2/", "<Client_Key>", "<Client_Secret>");

			//using OAuth
//			RestAPI rest = new RestAPI("http://www.yourstore.co.nz/wp-json/wp/v2/", "<Client_Key>", "<Client_Secret>");
			//rest.oauth_token = "<OAuth_Token>";
			//rest.oauth_token_secret = "<OAuth_Token_Secret>";


			RestAPI rest = new RestAPI("http://www.yourstore.co.nz/wp-json/wc/v3/", "<WooCommerce Key>", "<WooCommerce Secret");
			WCObject wc = new WCObject(rest);

			//Get all products
			var products = await wc.Product.GetAll();

			return products;
		}
	}
}
