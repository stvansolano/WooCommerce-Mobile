using System.Linq;
using Prism.Navigation;
using WooCommerceNET.WooCommerce.v3;

namespace eCommerce.ViewModels
{
	public class ProductViewModel : ViewModels.NavigationItemViewModel
	{
		public ProductViewModel(Product data, INavigationService navigationService) : base(data, navigationService)
		{
			ImageUrl = data.images?.FirstOrDefault()?.src;
		}

		public string ImageUrl { get; }

		public decimal SubTotal
		{
			get
			{
				if (Data is Product p)
				{
					return p.price.GetValueOrDefault(0) * Quantity;
				}
				return 0;
			}
		}

		public int Quantity { get; set; }
		public string Sku { get => Data is Product p ? p.sku : string.Empty; }
	}
}
