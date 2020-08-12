using System;
using System.IO;
using System.Threading.Tasks;

namespace WooCommerce.JsonMocks
{
	public static class Resources
	{
		public static async Task CopyTo(string resourceName, string outputPath)
		{
			var stream = typeof(Resources).Assembly.GetManifestResourceStream(resourceName);

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
				File.WriteAllBytes(outputPath, memoryStream.ToArray());
            }
		}

		public static async Task<string> GetContentAsync(string resourceName)
		{
			using (var memoryStream = typeof(Resources).Assembly.GetManifestResourceStream("WooCommerce.JsonMocks." + resourceName))
			{
				if (memoryStream == null)
				{
					return string.Empty;
				}
				return await new StreamReader(memoryStream).ReadToEndAsync();
			}
		}
	}
}
