using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Logic.Http;
using eCommerce.ViewModels;
using eCommerce.Views.MainScreen;
using eCommerce.Views.SearchScreen;
using eCommerce.Views.ShoppingCart;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class MainViewModel : Prism.Mvvm.BindableBase, IInitialize, IInitializeAsync, INavigationAware
	{
		public ICommand RefreshCommand { get; set; }
		public INavigationService NavigationService { get; }
		public IHttpFactory<ProductCategory> CategoryService { get; }

		private Tab _item;
		public Tab Item
		{
			get => _item;
			set
			{
				_item = value;
				RaisePropertyChanged(nameof(Item));
			}
		}

		public AllItemsTab AllItems { get; }
		public PopularTab AllPopular { get; }
		public AllTagsTab AllTags { get; }
		public SearchTab Search { get; }
		public ObservableCollection<Tab> TabItems { get; private set; } = new ObservableCollection<Tab>();
		public IHttpFactory<ProductTag> TagsService { get; private set; }

		public SearchViewModel SearchViewModel { get; }
		public ShoppingCartViewModel ShoppingCart { get; }

		public MainViewModel(IContainerProvider dependencyProvider,
							 INavigationService navigationService,
							 IEventAggregator eventAggregator)
		{
			NavigationService = navigationService;

			CategoryService = dependencyProvider.Resolve<IHttpFactory<ProductCategory>>();
			TagsService = dependencyProvider.Resolve<IHttpFactory<ProductTag>>();

			ShoppingCart = new ShoppingCartViewModel(dependencyProvider, eventAggregator, navigationService);
			SearchViewModel = new SearchViewModel(dependencyProvider, navigationService, dependencyProvider.Resolve<IShoppingCartService>());

			RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync(forceRefresh: true));

			TabItems.Add(AllItems = new AllItemsTab { Selected = true });
			//TabItems.Add(AllTags = new AllTagsTab());
			//TabItems.Add(AllPopular = new PopularTab());
			TabItems.Add(Search = new SearchTab());

			TabItems.OfType<SearchTab>().FirstOrDefault().Items?.Add(this.SearchViewModel);
		}

		private async Task RefreshDataAsync(bool? forceRefresh = false)
		{
			await RefreshCategories(forceRefresh);
			//await RefreshTags();
		}

		private async Task RefreshCategories(bool? forceRefresh = false)
		{
			if (AllItems.Items.Any() && !forceRefresh.GetValueOrDefault(false))
			{
				return;
			}
            try
            {
				var categories = await CategoryService.GetAsync("/products/categories");
				Console.WriteLine($"Categories: {(categories?.Result ?? new ProductCategory[0]).Length}");

				AllItems.Items.Clear();

				foreach (var item in categories.Result)
				{
					var navigableItem = new NavigationItemViewModel(item, NavigationService);

					if (item.image == null)
					{
						item.image = new WooCommerceNET.WooCommerce.v2.ProductCategoryImage();
						item.image.src = "https://my-woo-store.azurewebsites.net/wp-content/uploads/woocommerce-placeholder-324x324.png";
					}
					AllItems.Items.Add(navigableItem);
				}
			}
			catch (System.Net.WebException ex) 
			{
				Debug.WriteLine($"{ex.Message} - Unable to connect: {CategoryService.BaseUrl}");
			}
			catch (Exception ex)
            {
				Debug.WriteLine(ex.Message);
            }
		}

		private async Task RefreshTags()
		{
			var tags = await TagsService.GetAsync();
			Console.WriteLine($"Tags: {(tags?.Result ?? new ProductTag[0]).Length}");

			AllTags.Items.Clear();

			foreach (var item in tags.Result)
			{
				var navigableItem = new NavigationItemViewModel(item, NavigationService);

				AllTags.Items.Add(navigableItem);
			}
		}

		public void Initialize(INavigationParameters parameters) { }

		public Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		public void OnNavigatedFrom(INavigationParameters parameters) { }
		
		public async void OnNavigatedTo(INavigationParameters parameters)
		{
			await RefreshDataAsync();
		}
	}
}
