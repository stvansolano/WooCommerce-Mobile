using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using eCommerce.Core.Http;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class MainPageViewModel : Prism.Mvvm.BindableBase, IInitialize, IInitializeAsync, INavigationAware
	{
		public ICommand RefreshCommand { get; set; }
		public ICommand SelectedItemCommand { get; set; }
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

		public MainPageViewModel(IContainerProvider dependencyProvider, INavigationService navigationService)
		{
			NavigationService = navigationService;
			ProductService = dependencyProvider.Resolve<IHttpFactory<Product>>();

			CategoryService = dependencyProvider.Resolve<IHttpFactory<ProductCategory>>();

			RefreshCommand = new DelegateCommand(async() => await RefreshDataAsync());
			SelectedItemCommand = new DelegateCommand<Product>(async selectedItem => {
				var parameters = new NavigationParameters();
				parameters.Add("Product", selectedItem);

				await NavigationService.NavigateAsync("ProductDetail", parameters);
			});
		}

		private async Task RefreshDataAsync()
		{
			var store = await ProductService.GetAsync();

			Products.Clear();
			foreach (Product item in store.Result)
			{
				Products.Add(item);
			}

			var categories = await CategoryService.GetAsync("/products/categories");

			Console.WriteLine($"Products: {(store?.Result ?? new Product[0]).Length}");
			Console.WriteLine($"Categories: {(categories?.Result ?? new ProductCategory[0]).Length}");
		}

		public INavigationService NavigationService { get; }
		public IHttpFactory<Product> ProductService { get; }

		public IHttpFactory<ProductCategory> CategoryService { get; }

		public void Initialize(INavigationParameters parameters)
		{
			ProductService.BaseUrl = App.Constants.UrlEndpoint;
		}

		public Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public async void OnNavigatedTo(INavigationParameters parameters)
		{
			await RefreshDataAsync();
		}
	}
}
