using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Logic.Http;
using eCommerce.Services;
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

		private object _parent;
		public object Parent
		{
			get => _parent;
			set
			{
				_parent = value;
				RaisePropertyChanged(nameof(Parent));
			}
		}
		public ProductListingViewModel(IContainerProvider dependencyProvider, INavigationService navigationService)
		{
			NavigationService = navigationService;
			ProductService = dependencyProvider.Resolve<IHttpProductFactory>();

			GoBackCommand = new DelegateCommand(async() => await NavigationService.GoBackAsync());

			SelectedItemCommand = new DelegateCommand<Product>(async selectedItem =>
			{
				var parameters = new NavigationParameters();
				parameters.Add("Product", selectedItem);

				await NavigationService.NavigateAsync("ProductDetail", parameters);
			});
		}

		public INavigationService NavigationService { get; }
		public IHttpProductFactory ProductService { get; }
		public ICommand GoBackCommand { get; set; }

		public void Initialize(INavigationParameters parameters) { }

		public Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public async void OnNavigatedTo(INavigationParameters parameters)
		{
			var parent = parameters.GetValue<object>("Parent");

			var asyncTask = FilterResults(parent);
			var storeResult = await asyncTask.ConfigureAwait(false);

			Products.Clear();

			foreach (Product item in storeResult.Result)
			{
#if USE_MOCKS
				Decorate(item);
#endif
				if (parent is ProductCategory category && item.categories != null)
				{
					if (item.categories.Any(c => c.id == category.id))
					{
						Parent = parent;
						Products.Add(item);
						continue;
					}
				}
				else
				{
					Products.Add(item);
				}
			}

			Console.WriteLine($"Products: {(storeResult?.Result ?? new Product[0]).Length}");
		}

		private Task<HttpResponse<Product[]>> FilterResults(object parent)
		{
			if (parent is ProductCategory category && category.id.HasValue)
			{
				return ProductService.GetByCategoryId(category.id.GetValueOrDefault(0));
			}
			return ProductService.GetAsync();
		}

		private void Decorate(Product item)
		{
			if (item.images == null || item.images.Any() == false)
			{
				item.images = new System.Collections.Generic.List<ProductImage>();
				item.images.Add(new ProductImage
				{
					src = "https://static.ah.nl/image-optimization/static/product/AHI_43545239353939383432_1_LowRes_JPG.JPG?options=399,q85"
				});
			}
		}
	}
}
