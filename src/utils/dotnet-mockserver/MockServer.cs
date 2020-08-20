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
			var response = new CustomStatusCodeResult(HttpStatusCode.OK,
				new
				{
					Success = true,
					Result = $"Not found mapping for {url}"
				});

			return response;
		}

		private static async Task<IActionResult> FromFile(string fileName)
		{
			var jsonContents = await WooCommerce.Mocks.Resources.GetContentAsync(fileName);

			var result = new CustomStatusCodeResult(HttpStatusCode.OK, jsonContents);

			return result;
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
