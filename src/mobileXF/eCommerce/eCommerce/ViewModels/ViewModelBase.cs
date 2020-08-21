using System.Diagnostics;
using Prism.Mvvm;
using Prism.Navigation;

namespace eCommerce.ViewModels
{
	public class ViewModelBase : BindableBase
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
	}
}
