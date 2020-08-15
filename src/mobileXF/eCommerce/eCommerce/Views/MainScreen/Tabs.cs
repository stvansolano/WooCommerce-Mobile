using System;
using System.Collections.ObjectModel;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;

namespace eCommerce.Views.MainScreen
{	
	public class TabTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CategoryTemplate { get; set; }
		public DataTemplate TagTemplate { get; set; }
		public DataTemplate PopularTemplate { get; set; }
		public DataTemplate SearchTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object model, BindableObject container)
		{
			if (model is NavigationItemViewModel navigable1
				&& navigable1.Data is ProductCategory)
				return CategoryTemplate;

			if (model is NavigationItemViewModel navigable2
				&& navigable2.Data is ProductTag)
				return TagTemplate;

			if (model is PopularTab)
				return PopularTemplate;

			if (model is SearchTab)
				return SearchTemplate;

			return null;
		}
	}

	public class Tab
	{
		public string Title { get; set; }
		public string Id { get; set; }
		public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();
		public bool Selected { get; set; }
	}

	public class AllItemsTab : Tab
	{
		public AllItemsTab()
		{
			Id = "A";
			Items = new ObservableCollection<object>();
			Title = "All";
		}
	}

	public class AllTagsTab : Tab
	{
		public AllTagsTab()
		{
			Id = "T";
			Items = new ObservableCollection<object>();
			Title = "Explore";
		}
	}

	public class PopularTab : Tab
	{
		public PopularTab()
		{
			Id = "P";
			Items = new ObservableCollection<object>();
			Title = "Popular";
		}
	}

	public class SearchTab : Tab
	{
		public SearchTab()
		{
			Id = "S";
			Items = new ObservableCollection<object>();
			Title = "Search";
		}
	}
}