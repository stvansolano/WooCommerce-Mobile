using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Core.Logic.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce.ViewModels
{
	public class ProductDetailViewModel : ViewModelBase
	{
		public ProductDetailViewModel(IContainerProvider dependencyProvider,
									 INavigationService navigationService,
									 IEventAggregator eventAggregator)
		{
			NavigationService = navigationService;
			ProductService = dependencyProvider.Resolve<IProductService>();
			EventAggregator = eventAggregator;

			GoBackCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());

			IncreaseQuantityCommand = new DelegateCommand(() => {
				OrderedQuantity++;
			});

			DecreaseQuantityCommand = new DelegateCommand(() => {
				OrderedQuantity--;
			},
				() => OrderedQuantity > 0)
				.ObservesProperty(() => OrderedQuantity);

			AddToCartCommand = new DelegateCommand(() => {
				AddToCartText = UPDATE_CART;

				EventAggregator.GetEvent<AddToCartEvent>().Publish(new AddToCartEventArgs(Product, OrderedQuantity));
			});
		}

		public override void Initialize(INavigationParameters parameters)
		{
			base.Initialize(parameters);

			QuantityInCart = parameters.GetValue<int>("QuantityInCart");
			OrderedQuantity = QuantityInCart == 0 ? 1 : QuantityInCart;

			AddToCartText = QuantityInCart == 0 ? ADD_TO_CART : UPDATE_CART;
		}

		public INavigationService NavigationService { get; }

		public bool HasVariations { get => Variations.Any(); }
		public ObservableCollection<Variation> Variations { get; private set; } = new ObservableCollection<Variation>();

		public ICommand GoBackCommand { get; set; }
		public ICommand DecreaseQuantityCommand { get; set; }
		public ICommand IncreaseQuantityCommand { get; set; }

		public ICommand AddToCartCommand { get; set; }

		public IProductService ProductService { get; private set; }
		public IEventAggregator EventAggregator { get; }

		private string _addToCartText;
		public string AddToCartText
		{
			get => _addToCartText;
			set
			{
				_addToCartText = value;
				RaisePropertyChanged(nameof(AddToCartText));
			}
		}

		private Product _product;
		public Product Product
		{
			get => _product;
			set
			{
				_product = value;
				RaisePropertyChanged(nameof(Product));
			}
		}

		public int QuantityInCart { get; private set; }

		private int _orderedQuantity;
		public int OrderedQuantity
		{
			get => _orderedQuantity;
			set
			{
				_orderedQuantity = value;
				RaisePropertyChanged(nameof(OrderedQuantity));
			}
		}

		public const string ADD_TO_CART = "Add to cart";
		public const string UPDATE_CART = "Update cart";

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			var product = parameters.GetValue<NavigationItemViewModel>("Product");

			if (product?.Data is Product p && !Variations.Any())
			{
				Product = p;
				
				var variationsResult = await ProductService.GetVariations(Product.id.GetValueOrDefault(0));

				Variations.Clear();

				foreach (var item in variationsResult.Result)
				{
					Variations.Add(item);
				}

				RaisePropertyChanged(nameof(HasVariations));
			}
		}
	}
}
