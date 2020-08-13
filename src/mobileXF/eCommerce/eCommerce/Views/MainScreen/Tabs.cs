using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace eCommerce.Views.MainScreen
{	
	public class TabTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CategoryTemplate { get; set; }
		public DataTemplate TagTemplate { get; set; }
		public DataTemplate PopularTemplate { get; set; }
		public DataTemplate SearchTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object tab, BindableObject container)
		{
			if (tab is AllItemsTab)
				return TagTemplate;

			if (tab is AllTagsTab)
				return TagTemplate;

			if (tab is PopularTab)
				return PopularTemplate;

			if (tab is SearchTab)
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
			Items = new ObservableCollection<object>{
						"test1", "test2", "test3"
					};

			Title = "All";
		}
	}

	public class AllTagsTab : Tab
	{
		public AllTagsTab()
		{
			Items = new ObservableCollection<object>{
						"tag1", "tag2", "tag3"
					};

			Title = "Explore";
		}
	}

	public class PopularTab : Tab
	{
		public PopularTab()
		{
			Items = new ObservableCollection<object>{
						"item1", "item2", "item3"
					};

			Title = "Popular";
		}
	}

	public class SearchTab : Tab
	{
		public SearchTab()
		{
			Items = new ObservableCollection<object>{
						"result1", "result2", "result3"
					};

			Title = "Search";
		}
	}
}