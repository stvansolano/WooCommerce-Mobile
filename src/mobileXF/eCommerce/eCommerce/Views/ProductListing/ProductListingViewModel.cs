using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Logic.Http;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce.ViewModels
{
	public class ProductListingViewModel : Prism.Mvvm.BindableBase,
							IInitialize, IInitializeAsync,
							INavigationAware
	{
		public ICommand SelectedItemCommand { get; set; }
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

		public ProductListingViewModel(IContainerProvider dependencyProvider, INavigationService navigationService)
		{
			NavigationService = navigationService;
			ProductService = dependencyProvider.Resolve<IHttpFactory<Product>>();

			GoBackCommand = new DelegateCommand(async() => await NavigationService.GoBackAsync());

			SelectedItemCommand = new DelegateCommand<Product>(async selectedItem =>
			{
				var parameters = new NavigationParameters();
				parameters.Add("Product", selectedItem);

				await NavigationService.NavigateAsync("ProductDetail", parameters);
			});
		}

		public INavigationService NavigationService { get; }
		public IHttpFactory<Product> ProductService { get; }
		public ICommand GoBackCommand { get; set; }

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
			var store = await ProductService.GetAsync();

			Products.Clear();

			foreach (Product item in store.Result)
			{
#if USE_MOCKS
				if (item.images == null || item.images.Any() == false)
				{
					item.images = new System.Collections.Generic.List<ProductImage>();
					item.images.Add(new ProductImage
					{
						src = "https://static.ah.nl/image-optimization/static/product/AHI_43545239353939383432_1_LowRes_JPG.JPG?options=399,q85"
					});
				}
#endif
				Products.Add(item);
			}

			Console.WriteLine($"Products: {(store?.Result ?? new Product[0]).Length}");
		}
	}
}
