using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Logic.Services;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce.ViewModels
{
	public class ProductDetailViewModel : Prism.Mvvm.BindableBase,
							IInitialize, IInitializeAsync,
							INavigationAware
	{

		public ProductDetailViewModel(IContainerProvider dependencyProvider, INavigationService navigationService)
		{
			NavigationService = navigationService;
			ProductService = dependencyProvider.Resolve<IProductService>();

			GoBackCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
		}

		public INavigationService NavigationService { get; }
		public ICommand GoBackCommand { get; set; }
		public ObservableCollection<Variation> Variations { get; private set; } = new ObservableCollection<Variation>();
		public IProductService ProductService { get; private set; }

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

		public void Initialize(INavigationParameters parameters){ }

		public Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		public void OnNavigatedFrom(INavigationParameters parameters){ }

		public async void OnNavigatedTo(INavigationParameters parameters)
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
			}
		}
	}
}
