using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using eCommerce.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace eCommerce.Views.ShoppingCart
{
	public class ShoppingCartViewModel : ViewModelBase
	{
		public ObservableCollection<ProductViewModel> CartContents { get; } = new ObservableCollection<ProductViewModel>();
		public IEventAggregator EventAggregator { get; }
		public ICommand ContinueShoppingCommand { get; }

		public ShoppingCartViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			EventAggregator = eventAggregator;
			Navigation = navigationService;

			ContinueShoppingCommand = new DelegateCommand(() => { }, () => CanContinue)
										.ObservesProperty(() => TotalPrice);

			EventAggregator.GetEvent<AddToCartEvent>()
						   .Subscribe((AddToCartEventArgs newEntry) => {

				var existing = CartContents.FirstOrDefault(element => element.Sku == newEntry.Product.sku);

				if (existing == null)
				{
					CartContents.Add(new ProductViewModel(newEntry.Product, Navigation)
					{
						Quantity = newEntry.Quantity
					});
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
