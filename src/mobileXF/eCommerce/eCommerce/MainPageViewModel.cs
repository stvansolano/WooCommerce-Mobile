using System;
using System.Threading.Tasks;
using eCommerce.Services;
using Prism.Ioc;
using Prism.Navigation;


using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class MainPageViewModel : Prism.Mvvm.BindableBase, IInitialize, IInitializeAsync
	{
		public MainPageViewModel(IHttpFactory<Product> dependencyService)
		{
			Factory = dependencyService;
		}

		public IHttpFactory<Product> Factory { get; }

		public void Initialize(INavigationParameters parameters)
		{
		}

		public async Task InitializeAsync(INavigationParameters parameters)
		{
			Factory.BaseUrl = "https://6ed5bcbfc3a9.ngrok.io/api/";
			var result = await Factory.GetAsync("/products", new HttpRequest<Product>());

			Console.WriteLine(result);
		}
	}
}
