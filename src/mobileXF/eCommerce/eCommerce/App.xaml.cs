using System;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

using WooCommerceNET.WooCommerce.v3;
using eCommerce.Mocks;
using System.Diagnostics;
using eCommerce.Core.Http;
using eCommerce.Views;
using eCommerce.ViewModels;

namespace eCommerce
{
	public partial class App
	{
		internal class Constants
		{
			// ./ngrok 0.0.0.0:7071
			public const string UrlEndpoint = "https://835b1bee481e.ngrok.io"; // "without /"
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterInstance(Container);

			//containerRegistry.Register<ILoggerFacade, Services.DebugLogger>();
			containerRegistry.RegisterInstance<IHttpFactory<Product>>(
															new MockHttpFactory<Product>("/products")
															);

			containerRegistry.RegisterSingleton<IHttpFactory<ProductCategory>, MockHttpFactory<ProductCategory>>();

			containerRegistry.RegisterForNavigation<NavigationPage>();

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
