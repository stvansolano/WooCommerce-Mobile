using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using WooCommerce;
using WooCommerceNET.WooCommerce.v3;
using System.Collections.Generic;

namespace MockServer
{
	// ./ngrok http 0.0.0.0:7071
	public static class MockServer
	{
		[FunctionName("MockServer")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "")] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			string url = req.Query["url"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			//name = name ?? data?.name;

			Console.WriteLine("URL {0}", url);
			Console.WriteLine("Body {0}" + Environment.NewLine, data);

			if (url.EndsWith("/wp-json/wc/v3/products"))
			{
				return new CustomStatusCodeResult(HttpStatusCode.OK,
					new [] {
						new Product {
							id = 1,
							name = "Product 1",
							description = "description",
							short_description = "short description",
							sale_price = 100,
							categories = new List<ProductCategoryLine>   {
								new ProductCategoryLine {
								 id = 1,
								 name = "category 1",
								 slug = "slug"
								}
							}
						},
						new Product {
							id = 1,
							name = "Product 2",
							description = "description",
							short_description = "short description",
							sale_price = 100,
							categories = new List<ProductCategoryLine>   {
								new ProductCategoryLine {
								 id = 1,
								 name = "category 1",
								 slug = "slug"
								}
							}
						},
						new Product {
							id = 1,
							name = "Product 3",
							description = "description",
							short_description = "short description",
							sale_price = 100,
							categories = new List<ProductCategoryLine>   {
								new ProductCategoryLine {
								 id = 1,
								 name = "category 1",
								 slug = "slug"
								}
							}
						}
					});
			}
			var response = new CustomStatusCodeResult(HttpStatusCode.OK,
				new
				{
					Success = true,
					Result = $"Not found mapping for {url}"
				});

			return response;
		}
	}

	public class CustomStatusCodeResult : ObjectResult
	{
		public CustomStatusCodeResult(HttpStatusCode code, object value) : base(value)
		{
			base.StatusCode = (int)code;
		}

		public HeaderDictionary Headers { get; private set; } = new HeaderDictionary();

		public override void ExecuteResult(ActionContext context)
		{
			base.ExecuteResult(context);

			foreach (var item in Headers)
			{
				context.HttpContext.Response.Headers.Add(item.Key, item.Value);
			}
		}
	}
}
