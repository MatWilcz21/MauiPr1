using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Communication.Contacts;
using MauiApp1.Pages;
using MauiApp1.Products;
using MauiApp1.Products.MainProductsList;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MainViewModel : ObservableObject
{

	public MainViewModel()
	{
		//new CommunicationTest();
		MainProductsListClass = new MainProductsListClass(this);
		ContactsLogicClass = new ContactsLogic();
		ReceiveListUpdate.SendMainViewModel(this);
	}

	[ObservableProperty] public partial MainProductsListClass MainProductsListClass { get; set; }
	[ObservableProperty] public partial string NewProductNameEntry { get; set; } = string.Empty;

	[ObservableProperty] public partial ContactsLogic ContactsLogicClass { get; set; }

	[RelayCommand]
	void Add()
	{
		MainProductsListClass.Add(NewProductNameEntry);
		NewProductNameEntry = string.Empty;
	}

	[RelayCommand]
	async Task DeleteAll()
	{
		await MainProductsListClass.DeleteAll();
	}

	[RelayCommand]
	async Task Update()
	{
		await MainProductsListClass.Update();
	}

	[RelayCommand]
	async Task GoToSelectRecipePage()
	{

		var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};

		await Shell.Current.GoToAsync(nameof(SelectRecipePage), parameters);
	}

	[RelayCommand]
	async Task GoToSelectListReceiverPage()
	{
		var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};

		await Shell.Current.GoToAsync(nameof(SelectListReceiverPage), parameters);
	}
}

static class ItemListUpdater
{
	public static void SaveListToJson(MainProductsListClass mainViewModel)
	{

		Task.Run(() => JsonHandler.SaveJson(mainViewModel.Products, nameof(mainViewModel.Products))).Wait();
	}

	public static async Task LoadListFromJson(MainProductsListClass mainViewModel)
	{

		mainViewModel.Products = await JsonHandler.LoadJson<ObservableCollection<MainListProduct>>(nameof(mainViewModel.Products)) ?? new();
	}
}

