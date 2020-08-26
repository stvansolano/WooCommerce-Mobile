using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Logic.Http;
using Core.Logic.Services;
using eCommerce.Views.ShoppingCart;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce.ViewModels
{
	public class ProductListingViewModel : ViewModelBase
	{
		public IProductService ProductService { get; }
		public ICommand GoBackCommand { get; set; }
		public ICommand SelectedItemCommand { get; set; }

		public ObservableCollection<ProductViewModel> Products { get; set; } = new ObservableCollection<ProductViewModel>();
		public IShoppingCartService ShoppingCart { get; }

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

		public ProductListingViewModel(IContainerProvider dependencyProvider,
									  INavigationService navigationervice)
		{
			Navigation = navigationervice;
			ProductService = dependencyProvider.Resolve<IProductService>();
			ShoppingCart = dependencyProvider.Resolve<IShoppingCartService>();

			GoBackCommand = new DelegateCommand(async() => await Navigation.GoBackAsync());

			SelectedItemCommand = new DelegateCommand<ProductViewModel>(
							   async selectedItem =>
							   {
								   var parameters = new NavigationParameters();
								   parameters.Add("Product", selectedItem);
								   parameters.Add("QuantityInCart", GetQuantityInCart(selectedItem));

								   await Navigation.NavigateAsync("ProductDetail", parameters);
							   });
		}

		private int GetQuantityInCart(ProductViewModel selectedItem)
		{
			if (selectedItem == null)
			{
				return 0;
			}
			foreach (var item in ShoppingCart.CartContents)
			{
				if (item.Sku == selectedItem.Sku)
				{
					return item.Quantity;
				}
			}
			return 0;
		}

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			if (Parent != null)
			{
				return;
			}
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
						Products.Add(new ProductViewModel(item, Navigation));
						continue;
					}
				}
				else
				{
					Products.Add(new ProductViewModel(item, Navigation));
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

		#if USE_MOCKS

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

		#endif
	}
}
