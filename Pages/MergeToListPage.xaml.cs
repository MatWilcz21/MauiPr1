using MauiApp1.ViewerModels;

namespace MauiApp1.Pages;

public partial class MergeToListPage : ContentPage
{
    public MergeToListPage(MergeToListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}