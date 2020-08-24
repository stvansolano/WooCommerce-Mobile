namespace eCommerce
{
	using eCommerce.Views;
	using Xamarin.Forms;

	public class ProductDetailPage : ApplicationPage
	{
		public ProductDetailPage() => Content = new Views.ProductListing.ProductDetailView();
	}

	public class ProductListingPage : ApplicationPage
	{
		public ProductListingPage() => Content = new Views.ProductListing.ProductListingView();
	}

	public class MainPage : ApplicationPage
	{
		public MainPage() => Content = new MainView();
	}

	public abstract class ApplicationPage : ContentPage
	{
		public ApplicationPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}
