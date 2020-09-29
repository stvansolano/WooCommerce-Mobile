using System;
using System.Diagnostics;

using Core.Logic;
using Core.Logic.Http;
using Core.Logic.Services;
using eCommerce.ViewModels;

using Prism.Ioc;

using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;
using eCommerce.Views.SearchScreen;
using Prism.Navigation;
using eCommerce.Views.ShoppingCart;
using Prism.Mvvm;
using WooCommerce.Mocks;

namespace eCommerce
{
	public partial class App
	{
		public IContainerRegistry Registry { get; private set; }
		private const string _mockBaseUrl = ""; // "without /"

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			Registry = containerRegistry;

			containerRegistry.RegisterInstance(Container);
			containerRegistry.RegisterServices(Container, NavigationService);
			containerRegistry.RegisterForNavigation<NavigationPage>();

			// ProductListing
			containerRegistry.RegisterForNavigation<ProductListingPage, ProductListingViewModel>("ProductListing");
			containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailViewModel>("ProductDetail");

			//ViewModelLocationProvider.Register<ShoppingCartView, ShoppingCartViewModel>();
			//ViewModelLocationProvider.Register<SearchView, SearchViewModel>();

			containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
		}

		protected override async void OnInitialized()
		{
			try
			{
				InitializeComponent();

				var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

				if (result.Success)
				{
					return;
				}
				SetMainPageFromException(result.Exception);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private void SetMainPageFromException(Exception ex)
		{
			var layout = new StackLayout
			{
				Padding = new Thickness(40)
			};
			layout.Children.Add(new Label
			{
				Text = ex?.GetType()?.Name ?? "Unknown Error encountered",
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			});

			layout.Children.Add(new ScrollView
			{
				Content = new Label
				{
					Text = $"{ex}",
					LineBreakMode = LineBreakMode.WordWrap
				}
			});

			MainPage = new ContentPage
			{
				Content = layout
			};
		}
	}

	public static class AppExtensions
	{
		public static void RegisterServices(this IContainerRegistry containerRegistry,
											IContainerProvider provider,
											INavigationService navigation)
		{
			containerRegistry.RegisterSingleton<IShoppingCartService, ShoppingCartService>();

			var api = new WooComerceApi(websiteRoot: eCommerce.Helpers.Secrets.Website,
													 client: eCommerce.Helpers.Secrets.ClientId,
													 secret: eCommerce.Helpers.Secrets.ClientSecret);

			containerRegistry.RegisterInstance(api);

			var productService = new ProductService(api);

			// Products
			containerRegistry.RegisterInstance<IHttpFactory<Product>>(productService);
			containerRegistry.RegisterInstance<IProductService>(productService);

			// Tags
			containerRegistry.RegisterSingleton<IHttpFactory<ProductTag>, ProductTagService>();
			//new MockHttpFactory<ProductTag>("/products/tags")
			//);

			// Categories
			containerRegistry.RegisterSingleton<IHttpFactory<ProductCategory>, ProductCategoryService>();
		}
	}
}
