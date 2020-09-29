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

			if (url.EndsWith("/wp-json/wc/v3/products/categories"))
			{
				return new CustomStatusCodeResult(HttpStatusCode.OK, _categories);
			}

			var response = new CustomStatusCodeResult(HttpStatusCode.OK,
				new
				{
					Success = true,
					Result = $"Not found mapping for {url}"
				});

			return response;
		}

		private static ProductCategory[] _categories = JsonConvert.DeserializeObject<ProductCategory[]>(@"
            {
				""id"": 15,
				""name"": ""Albums"",
				""slug"": ""albums"",
				""parent"": 11,
				""description"": """",
				""display"": ""default"",
				""image"":
				{
				  ""id"": 795,
				  ""date_created"": ""2017 -03-23T14:03:08"",
				  ""date_created_gmt"": ""2017 -03-23T20:03:08"",
				  ""date_modified"": ""2017 -03-23T14:03:08"",
				  ""date_modified_gmt"": ""2017 -03-23T20:03:08"",
				  ""src"": ""https://static.ah.nl/image-optimization/static/product/AHI_43545239353939383432_1_LowRes_JPG.JPG?options=399,q85"",
				  ""name"": """",
				  ""alt"": """"
				},
			  },
			  {
				""id"": 9,
				""name"": ""Clothing"",
				""slug"": ""clothing"",
				""parent"": 0,
				""description"": """",
				""display"": ""default"",
				""image"":
				{
				  ""id"": 795,
				  ""date_created"": ""2017 -03-23T14:03:08"",
				  ""date_created_gmt"": ""2017 -03-23T20:03:08"",
				  ""date_modified"": ""2017 -03-23T14:03:08"",
				  ""date_modified_gmt"": ""2017 -03-23T20:03:08"",
				  ""src"": ""https://static.ah.nl/image-optimization/static/product/AHI_43545239353939383432_1_LowRes_JPG.JPG?options=399,q85"",
				  ""name"": """",
				  ""alt"": """"
				}");
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
