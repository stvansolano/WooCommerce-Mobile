using System;
using System.Diagnostics;

using Core.Logic;
using Core.Logic.Http;
using eCommerce.Services;
using eCommerce.ViewModels;
using WooCommerce.Mocks;

using Prism.Ioc;

using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;

namespace eCommerce
{
	public partial class App
	{
		internal class Constants
		{
			internal const string UrlEndpoint = "https://my-woo-store.azurewebsites.net/";
		}

		static App()
		{
			// ./ngrok 0.0.0.0:7071
			// http://localhost:7071/api/MockServer?url=/wp-json/wc/v3/products
			WooCommerce.Mocks.MockUtils.BaseUrl = "https://69d81be48da1.ngrok.io"; // "without /"
		}

		private WooComerceApi _api = new WooComerceApi(websiteRoot: eCommerce.Helpers.Secrets.Website,
													   client: eCommerce.Helpers.Secrets.ClientId,
													   secret: eCommerce.Helpers.Secrets.ClientSecret);

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterInstance(Container);
			containerRegistry.RegisterInstance(_api);

			containerRegistry.RegisterSingleton<IHttpFactory<Product>, HttpProductFactory>();
															//new MockHttpFactory<Product>("/products")
															//);

			containerRegistry.RegisterSingleton<IHttpFactory<ProductTag>, HttpProductTagFactory>();
															//new MockHttpFactory<ProductTag>("/products/tags")
															//);

			containerRegistry.RegisterSingleton<IHttpFactory<ProductCategory>, HttpProductCategoryFactory>();

			containerRegistry.RegisterForNavigation<NavigationPage>();

			// ProductListing
			containerRegistry.RegisterForNavigation<ProductListingPage, ProductListingViewModel>("ProductListing");
			containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailViewModel>("ProductDetail");

			containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
		}

		protected override async void OnInitialized()
		{
			try
			{
				InitializeComponent();

				var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

				if (!result.Success)
				{
					SetMainPageFromException(result.Exception);
				}
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
}
