using MauiApp1.ViewerModels;

namespace MauiApp1.Pages;

public partial class EditSelectedRecipePage : ContentPage
{
	public EditSelectedRecipePage(EditSelectedRecipeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}