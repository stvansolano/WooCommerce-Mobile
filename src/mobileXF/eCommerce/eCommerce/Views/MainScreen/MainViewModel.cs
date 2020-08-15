using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using eCommerce.Core.Http;
using eCommerce.Views.MainScreen;
using Prism.Commands;
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

		public MainViewModel(IContainerProvider dependencyProvider, INavigationService navigationService)
		{
			NavigationService = navigationService;

			CategoryService = dependencyProvider.Resolve<IHttpFactory<ProductCategory>>();

			RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync());

			TabItems.Add(AllItems = new AllItemsTab { Selected = true });
			TabItems.Add(AllTags = new AllTagsTab());
			TabItems.Add(AllPopular = new PopularTab());
			TabItems.Add(Search = new SearchTab());
		}

		private async Task RefreshDataAsync()
		{
			var categories = await CategoryService.GetAsync("/products/categories");
			Console.WriteLine($"Categories: {(categories?.Result ?? new ProductCategory[0]).Length}");

			AllItems.Items.Clear();

			foreach (var item in categories.Result)
			{
				var navigableItem = new NavigationItemViewModel(item, NavigationService);

				AllItems.Items.Add(navigableItem);
			}
		}

		public void Initialize(INavigationParameters parameters)
		{
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
			await RefreshDataAsync();
		}
	}
}
