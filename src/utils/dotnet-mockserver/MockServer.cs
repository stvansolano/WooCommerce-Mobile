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
using WooCommerceNET.WooCommerce.v3;
using System.Collections.Generic;

namespace MockServer
{
	// ./ngrok http 0.0.0.0:7071
	// Windows: func start --csharp
	// Windows start C:\dev\ngrok\ngrok.exe http 0.0.0.0:7071
	public static class MockServer
	{
		[FunctionName("MockServer")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "")] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			string url = req.Query["url"];

			url = url ?? string.Empty;
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			//name = name ?? data?.name;

			Console.WriteLine("URL {0}", url);
			Console.WriteLine("Body {0}" + Environment.NewLine, data);

			if (url.EndsWith("/wp-json/wc/v3/products/categories"))
			{
				return await FromFile("Categories.json");
			}
			if (url.EndsWith("/wp-json/wc/v3/products/tags"))
			{
				return await FromFile("Tags.json");
			}
			if (url.EndsWith("/wp-json/wc/v3/products"))
			{
				return await FromFile("Products.json");
			}
			if (url.Contains("/wp-json/wc/v3/products/") && url.EndsWith("/variations"))
			{
				return new CustomStatusCodeResult(HttpStatusCode.OK,
					new Variation[] {
						new Variation { image = new VariationImage { src = DEFAULT_IMAGE } },
						new Variation { image = new VariationImage { src = DEFAULT_IMAGE } }
					});
			}
			if (url.Contains(CATEGORY_ID_URL))
			{
				var id = GetParameterId(url, CATEGORY_ID_URL);

				return new CustomStatusCodeResult(HttpStatusCode.OK,
					new Product[] {
						new Product {
							id = 799,
							name="Ship Your Idea",
							description = DESCRIPTION,
							slug="ship-your-idea-22",
							images = new List<ProductImage>{ new ProductImage { src = DEFAULT_IMAGE } },
							categories = new List<ProductCategoryLine> {
								new ProductCategoryLine { id = id, name = "Category" + id
							}
						}
					} });
			}
			if (url.Contains(SEARCH_URL))
			{
				var keyword = GetParameter(url, SEARCH_URL);

				return new CustomStatusCodeResult(HttpStatusCode.OK,
					new Product[] {
						new Product {
							id = 800,
							name=$"Ship Your Idea {keyword}",
							description = DESCRIPTION,
							slug="ship-your-idea-22",
							images = new List<ProductImage>{ new ProductImage { src = DEFAULT_IMAGE } }
					} });
			}
			var response = new CustomStatusCodeResult(HttpStatusCode.OK,
				new
				{
					Success = true,
					Result = $"Not found mapping for {url}"
				});

			return response;
		}

		private static object GetParameter(string url, string templateContent)
			=> url.Split(templateContent)[1];


		private static int GetParameterId(string url, string templateContent)
			=> int.Parse(url.Split(templateContent)[1]);

		private static async Task<IActionResult> FromFile(string fileName)
		{
			var jsonContents = await WooCommerce.Mocks.Resources.GetContentAsync(fileName);

			var result = new CustomStatusCodeResult(HttpStatusCode.OK, jsonContents);

			return result;
		}

		private static string CATEGORY_ID_URL = "/wp-json/wc/v3/products?categoryId=";
		private static string DEFAULT_IMAGE = "https://static.ah.nl/image-optimization/static/product/AHI_43545239353939383432_1_LowRes_JPG.JPG?options=399,q85";
		private static string SEARCH_URL = "/wp-json/wc/v3/products?search=";
		private static string DESCRIPTION = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
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
