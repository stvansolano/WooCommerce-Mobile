using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;

namespace eCommerce.ViewModels
{
	public class ViewModelBase : BindableBase,
								 IInitialize, IInitializeAsync,
								 INavigationAware

	{
		private INavigationService _navigation;

		public INavigationService Navigation
		{
			get
			{
				Debug.Assert(_navigation != null);
				return _navigation;
			}
			protected set => _navigation = value;
		}

		public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

		public virtual void Initialize(INavigationParameters parameters) { }

		public virtual Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		public virtual void OnNavigatedTo(INavigationParameters parameters) { }
	}
}
