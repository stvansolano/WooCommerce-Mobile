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

        Task<HttpResponse<T>> GetAsync<RequestType>(string endpointName, HttpRequest<RequestType> request)
            where RequestType : class, new();

    }

    public class HttpFactory<T> : IHttpFactory<T>
		where T: class, new()
    {
        public string BaseUrl { get; set; }

        public virtual Task<HttpResponse<T>> GetAsync<RequestType>(string endpointName, HttpRequest<RequestType> request)
            where RequestType : class, new()
        {
            return null;
		}

		public virtual HttpClient GetClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(BaseUrl))
            {
                client.BaseAddress = new Uri($"{BaseUrl}/");
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
        protected override string GetUrl(string SERVICE_URL)
        {
            const string TEMP_MOCK_URL = "https://44595e64cbd9.ngrok.io/api/MockServer";

            return TEMP_MOCK_URL + $"?url={WebUtility.UrlEncode(SERVICE_URL)}";
        }


        public override async Task<HttpResponse<T>> GetAsync<RequestType>(string endpointName, HttpRequest<RequestType> request)
        {
            try
            {
                var url = GetUrl(endpointName);

                var client = GetClient();

                var response = await client.GetStringAsync(endpointName).ConfigureAwait(false);

                //HandleResponse(Response, AuthBehavior.NoAuth, out String ResponseString);
                return new HttpResponse<T>(JsonConvert.DeserializeObject<T>(response));
            }
            catch (Exception ex)
            {
                //if (!string.IsNullOrEmpty(apex.Response))
                //{
                //    apex.ObjectResponse = JsonConvert.DeserializeObject<CSResponse>(apex.Response);
                //}
                throw ex;
            }
        }
    }
}