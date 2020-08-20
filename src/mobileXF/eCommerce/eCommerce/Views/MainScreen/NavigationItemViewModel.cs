using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace eCommerce.Views.MainScreen
{
	public class NavigationItemViewModel : BindableBase
	{
		public NavigationItemViewModel(object data, INavigationService navigationService)
		{
			Data = data;

			Navigation = navigationService;

			GoToDetailCommand = new DelegateCommand(async () => {
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("Parent", Data);
				await navigationService.NavigateAsync("ProductListing", navigationParameters);
			});
		}

		public object Data { get; }
		public INavigationService Navigation { get; }
		public ICommand GoToDetailCommand { get; }
	}
}
