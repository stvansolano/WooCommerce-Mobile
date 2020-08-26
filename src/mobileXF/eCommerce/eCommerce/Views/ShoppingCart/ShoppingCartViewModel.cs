using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using eCommerce.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

namespace eCommerce.Views.ShoppingCart
{
	public interface IShoppingCartService
	{
		ObservableCollection<ProductViewModel> CartContents { get; }
	}

	public class ShoppingCartService : IShoppingCartService
	{
		public ObservableCollection<ProductViewModel> CartContents { get; } = new ObservableCollection<ProductViewModel>();
	}

	public class ShoppingCartViewModel : ViewModelBase
	{
		public ObservableCollection<ProductViewModel> CartContents { get; } = new ObservableCollection<ProductViewModel>();
		public IEventAggregator EventAggregator { get; }
		public IShoppingCartService ShoppingCartService { get; }
		public ICommand ContinueShoppingCommand { get; }

		public ShoppingCartViewModel(IContainerProvider provider, IEventAggregator eventAggregator, INavigationService navigationService)
		{
			EventAggregator = eventAggregator;
			Navigation = navigationService;

			ShoppingCartService = provider.Resolve<IShoppingCartService>();

			ContinueShoppingCommand = new DelegateCommand(() => { }, () => CanContinue)
										.ObservesProperty(() => TotalPrice);

			CartContents = ShoppingCartService.CartContents;

			EventAggregator.GetEvent<AddToCartEvent>()
						   .Subscribe((AddToCartEventArgs newEntry) => {

				var existing = ShoppingCartService.CartContents.FirstOrDefault(element => element.Sku == newEntry.Product.sku);

				if (existing == null)
				{
					var newOne = new ProductViewModel(newEntry.Product, Navigation)
					{
						Quantity = newEntry.Quantity
					};	

					CartContents.Add(newOne);
				}
				else
				{
					existing.Quantity = newEntry.Quantity;
				}

				RaisePropertyChanged(nameof(TotalPrice));
				RaisePropertyChanged(nameof(CanContinue));
			});
		}

		public bool CanContinue { get => CartContents.Any() && TotalPrice > 0; }
		public decimal TotalPrice { get => CartContents.Sum(item => item.SubTotal); }
	}
}
