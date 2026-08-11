using MauiApp1.Pages;

namespace MauiApp1
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(AddNewSavedProductPage), typeof(AddNewSavedProductPage));
			Routing.RegisterRoute(nameof(SelectRecipePage), typeof(SelectRecipePage));
			Routing.RegisterRoute(nameof(MergeToListPage), typeof(MergeToListPage));
			Routing.RegisterRoute(nameof(EditSelectedRecipePage), typeof(EditSelectedRecipePage));
		}
	}
}
