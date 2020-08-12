using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace eCommerce.Services
{
    public interface IHttpFactory<T>
		where T : class, new()
    {
        string BaseUrl { get; set; }

        Task<HttpResponse<T[]>> GetAsync(string endpointName, HttpRequest request);
    }

    public class HttpFactory<T> : IHttpFactory<T>
        where T : class, new()
    {
        public string BaseUrl { get; set; }

        public virtual async Task<HttpResponse<T[]>> GetAsync(string endpointName, HttpRequest request)
        {
            try
            {
                using (var client = GetClient(GetUrl(endpointName)))
				{
					var response = await client.GetStringAsync(string.Empty).ConfigureAwait(false);
                    return new HttpResponse<T[]>(JsonConvert.DeserializeObject<T[]>(response));
                }
            }
            catch (Exception ex)
            {
				//if (!string.IsNullOrEmpty(apex.Response))
				//{
				//    apex.ObjectResponse = JsonConvert.DeserializeObject<CSResponse>(apex.Response);
				//}
                return new HttpResponse<T[]>(Array.Empty<T>(), HttpStatusCode.InternalServerError, ex);
            }
        }

		protected HttpClient GetClient(string url = null)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(url ?? BaseUrl))
            {
                client.BaseAddress = new Uri($"{url ?? BaseUrl}");
            }

            return client;
        }

        protected virtual string GetUrl(string serviceEndpointName)
		{
            return string.Empty;
		}
    }

	/*
    public class RefitFactory : IHttpFactory
    {
        public string BaseUrl { get; set; }

		public RefitFactory()
		{
            _instance = new Lazy<IDomainApi>(
                () => RestService.For<IDomainApi>(BaseUrl)
            );
        }

        private Lazy<IDomainApi> _instance;
	}*/

    public interface IDomainApi
    {

    }

	public class MockHttpFactory<T> : HttpFactory<T>
        where T : class, new()
    {
        protected override string GetUrl(string endpointName)
        {
            const string API_SEGMENT = "/wp-json/wc/v3";

            const string TEMP_MOCK_URL = App.Constants.UrlEndpoint;
            // http://localhost:7071/api/MockServer?url=/wp-json/wc/v3/products

            var encoded = WebUtility.UrlEncode(API_SEGMENT + endpointName);
            var result = TEMP_MOCK_URL + $"/api/MockServer?url={encoded}";

            return result;
        }
    }
}