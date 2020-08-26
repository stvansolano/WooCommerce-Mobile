using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace eCommerce.ViewModels
{
	public class NavigationItemViewModel : ViewModelBase
	{
		public NavigationItemViewModel(object data, INavigationService navigationService)
		{
			Data = data;

			Navigation = navigationService;

			GoBackCommand = new DelegateCommand(async () =>
				await Navigation.GoBackAsync());

			GoToDetailCommand = new DelegateCommand(async () => {

				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("Parent", Data);

				await navigationService.NavigateAsync("ProductListing", navigationParameters);
			});
		}

		public object Data { get; }
		public ICommand GoToDetailCommand { get; }
		public ICommand GoBackCommand { get; set; }
	}
}
