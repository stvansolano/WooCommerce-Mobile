using System.Net;
using System.Threading.Tasks;
using eCommerce.Core.Http;
using eCommerce.Http;
using eCommerce.Services;

namespace eCommerce.Mocks
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

        protected override string GetUrl(string endpointName = null)
            => MockUtils.GetMockerverUrl(endpointName ?? _defaultEndpoint);
    }

    public static class MockUtils
    {
        public static string GetMockerverUrl(string endpointName)
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
