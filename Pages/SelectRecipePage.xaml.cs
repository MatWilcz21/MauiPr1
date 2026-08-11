using MauiApp1.ViewerModels;

namespace MauiApp1.Pages;

public partial class SelectRecipePage : ContentPage
{
    public SelectRecipePage(SelectRecipeViewerModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}