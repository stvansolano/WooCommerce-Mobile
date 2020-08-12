using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using eCommerce.Core.Http;
using Newtonsoft.Json;

namespace eCommerce.Http
{
    public class HttpFactory<T> : IHttpFactory<T>
        where T : class, new()
    {
        public string BaseUrl { get; set; }

        public virtual async Task<HttpResponse<T[]>> GetAsync(string endpointName = null, HttpRequest request = null)
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

}