using System;
using System.Net;
using System.Threading.Tasks;
using Core.Logic.Http;
using Newtonsoft.Json;

namespace WooCommerce.Mocks
{
    public class MockHttpFactory<T> : HttpFactory<T>
        where T : class, new()
    {
        protected string _defaultEndpoint;

        public MockHttpFactory(string endpoint)
        {
            _defaultEndpoint = endpoint;
        }

        public MockHttpFactory() { }

        public override Task<HttpResponse<T[]>> GetAsync(string endpointName = null, HttpRequest request = null)
            => base.GetAsync(endpointName ?? _defaultEndpoint, request);

        public async Task<HttpResponse<SubT[]>> GetSubTypeAsync<SubT>(string endpointName = null, HttpRequest request = null)
            where SubT : class
        {
			try
            {
                using (var client = GetClient(GetUrl(endpointName)))
                {
                    var response = await client.GetStringAsync(string.Empty).ConfigureAwait(false);
                    return new HttpResponse<SubT[]>(JsonConvert.DeserializeObject<SubT[]>(response));
                }
            }
            catch (Exception ex)
            {
                //if (!string.IsNullOrEmpty(apex.Response))
                //{
                //    apex.ObjectResponse = JsonConvert.DeserializeObject<CSResponse>(apex.Response);
                //}
                return new HttpResponse<SubT[]>(Array.Empty<SubT>(), HttpStatusCode.InternalServerError, ex);
            }
        }

        protected override string GetUrl(string endpointName = null)
            => MockUtils.GetMockerverUrl(endpointName ?? _defaultEndpoint);
    }

	public static class MockUtils
    {
        public static string BaseUrl { get; set; }

        public static string GetMockerverUrl(string endpointName)
        {
            const string API_SEGMENT = "/wp-json/wc/v3";

            var encoded = WebUtility.UrlEncode(API_SEGMENT + endpointName);
            var result = BaseUrl + $"/api/MockServer?url={encoded}";

            return result;
        }
    }
}
