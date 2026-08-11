using MauiApp1.ViewerModels;

namespace MauiApp1;

public partial class AddNewSavedProductPage : ContentPage
{
	public AddNewSavedProductPage(AddNewSavedProductViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}