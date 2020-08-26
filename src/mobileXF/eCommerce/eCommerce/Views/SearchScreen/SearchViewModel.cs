using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Logic.Services;
using eCommerce.ViewModels;
using eCommerce.Views.ShoppingCart;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

namespace eCommerce.Views.SearchScreen
{
	public class SearchViewModel : ViewModelBase
	{
		public string Title { get; set; } = "Search";
		public ICommand SearchCommand { get; set; }
		public IProductService SearchService { get; set; }
		public IShoppingCartService ShoppingCart { get; }
		public ICommand SelectedItemCommand { get; }
		public ObservableCollection<ProductViewModel> Results { get; set; } = new ObservableCollection<ProductViewModel>();

		private string _criteria;
		public string Criteria
		{
			get => _criteria;
			set => SetProperty(ref _criteria, value);
		}

		public SearchViewModel(IContainerProvider dependencyProvider,
							   INavigationService navigationService,
							   IShoppingCartService shoppingCart)
		{
			Navigation = navigationService;
			SearchService = dependencyProvider.Resolve<IProductService>();
			ShoppingCart = shoppingCart;

			SelectedItemCommand = new DelegateCommand<ProductViewModel>(
							   async selectedItem =>
							   {
								   var parameters = new NavigationParameters();
								   parameters.Add("Product", selectedItem);
								   parameters.Add("QuantityInCart", GetQuantityInCart(selectedItem));

								   await Navigation.NavigateAsync("ProductDetail", parameters);
							   });

			SearchCommand = new DelegateCommand(async() => {

				var searchResult = await SearchService.Search(Criteria);

				Results.Clear();

				foreach (var item in searchResult.Result)
				{
					Results.Add(new ProductViewModel(item, Navigation));
				}
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
	}
}
