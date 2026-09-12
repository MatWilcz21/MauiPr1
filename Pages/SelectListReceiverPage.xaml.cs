using MauiApp1.ViewerModels;

namespace MauiApp1.Pages;

public partial class SelectListReceiverPage : ContentPage
{
	public SelectListReceiverPage(SelectListReceiverViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}