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
	// ./ngrok http 7071 --host-header=localhost:7071
	public static class MockServer
	{
		[FunctionName("MockServer")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			string url = req.Query["url"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			//name = name ?? data?.name;

			Console.WriteLine("URL {0}", url);
			Console.WriteLine("Body {0}" + Environment.NewLine, data);

			var response = new CustomStatusCodeResult(HttpStatusCode.NotFound,
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
