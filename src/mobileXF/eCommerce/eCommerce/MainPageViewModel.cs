using System;
using System.Threading.Tasks;
using eCommerce.Core.Http;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class MainPageViewModel : Prism.Mvvm.BindableBase, IInitialize, IInitializeAsync
	{
		public MainPageViewModel(IContainerProvider dependencyProvider)
		{
			Products = dependencyProvider.Resolve<IHttpFactory<Product>>();

			Categories = dependencyProvider.Resolve<IHttpFactory<ProductCategory>>();
		}

		public IHttpFactory<Product> Products { get; }

		public IHttpFactory<ProductCategory> Categories { get; }

		public void Initialize(INavigationParameters parameters) { }

		public async Task InitializeAsync(INavigationParameters parameters)
		{
			Products.BaseUrl = App.Constants.UrlEndpoint;

			var result = await Products.GetAsync();
			Console.WriteLine($"Results: {(result?.Result ?? new Product[0]).Length}");

			var categories = await Categories.GetAsync("/products/categories");

			Console.WriteLine($"Products: {(result?.Result ?? new Product[0]).Length}");
			Console.WriteLine($"Categories: {(categories?.Result ?? new ProductCategory[0]).Length}");
		}
	}
}
