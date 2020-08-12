using System;
using System.Threading.Tasks;
using eCommerce.Services;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class MainPageViewModel : Prism.Mvvm.BindableBase, IInitialize, IInitializeAsync
	{
		public MainPageViewModel(IContainerProvider dependencyProvider)
		{
			Factory = dependencyProvider.Resolve<IHttpFactory<Product>>();
		}

		public IHttpFactory<Product> Factory { get; }

		public void Initialize(INavigationParameters parameters) { }

		public async Task InitializeAsync(INavigationParameters parameters)
		{
			Factory.BaseUrl = App.Constants.UrlEndpoint;

			var result = await Factory.GetAsync("/products", new HttpRequest());
			Console.WriteLine($"Results: {(result?.Result ?? new Product[0]).Length}");
		}
	}
}
