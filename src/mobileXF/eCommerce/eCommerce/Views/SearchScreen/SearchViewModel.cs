using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Logic.Services;
using eCommerce.ViewModels;
using eCommerce.Views.MainScreen;
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

		public ObservableCollection<NavigationItemViewModel> Results { get; set; } = new ObservableCollection<NavigationItemViewModel>();

		private string _criteria;
		public string Criteria
		{
			get => _criteria;
			set => SetProperty(ref _criteria, value);
		}

		public SearchViewModel(IContainerProvider dependencyProvider, INavigationService navigation, IProductService searchService)
		{
			Navigation = dependencyProvider.Resolve<INavigationService>();
			SearchService = searchService;

			SearchCommand = new DelegateCommand(async() => {

				var searchResult = await SearchService.Search(Criteria);

				Results.Clear();

				foreach (var item in searchResult.Result)
				{
					Results.Add(new NavigationItemViewModel(item, Navigation));
				}
			});
		}
	}
}
