using System.Threading.Tasks;
using System.Windows.Input;
using eCommerce.Core.Http;
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
			ProductService = dependencyProvider.Resolve<IHttpFactory<Product>>();

			GoBackCommand = new DelegateCommand(async() => await NavigationService.GoBackAsync());
		}

		public INavigationService NavigationService { get; }
		public IHttpFactory<Product> ProductService { get; }
		public ICommand GoBackCommand { get; set; }

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

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			var product = parameters.GetValue<Product>("Product");

			if (product != null)
			{
				Product = product;
			}
		}
	}
}
