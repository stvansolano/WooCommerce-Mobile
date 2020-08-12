using System;
using eCommerce.Services;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public partial class App
	{
		internal class Constants
		{
			// ./ngrok 0.0.0.0:7071
			public const string UrlEndpoint = "https://9bc23ef03c98.ngrok.io"; // "without /"
		}

		public App(IPlatformInitializer initializer = null) : base(initializer) {}

		protected override async void OnInitialized()
		{
			try
			{
				InitializeComponent();

				var result = await NavigationService.NavigateAsync("MainPage");

				if (!result.Success)
				{
					SetMainPageFromException(result.Exception);
				}
			}
			catch (Exception ex)
			{

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

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterInstance(Container);

			//containerRegistry.Register<ILoggerFacade, Services.DebugLogger>();
			containerRegistry.RegisterSingleton<IHttpFactory<Product>, MockHttpFactory<Product>>();

			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

		}
	}
}
