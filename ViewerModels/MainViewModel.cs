using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Pages;
using MauiApp1.Products;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MainViewModel : ObservableObject
{

	public MainViewModel()
	{
		Items = new();
		ChangeProductsListFromOutside = new ChangeProductsListFromOutside(this);

		Task.Run(() => ItemListUpdater.LoadListFromJson(this)).Wait();


		Text = "";

	}

	[ObservableProperty] public partial ObservableCollection<MainListProduct> Items { get; set; } //TO_DO zmienić nazwę

	[ObservableProperty] public partial string Text { get; set; } //TO_DO zmienić nazwę

	public ChangeProductsListFromOutside ChangeProductsListFromOutside { get; } = null!;

	[RelayCommand]
	void Add()
	{

		if (string.IsNullOrWhiteSpace(Text)) return;

		Text = Text.ToLower();
		Text = Text.Trim();

		float productCount = 1; //TO_DO parsuj czy nie ma dopisanej ilosci produktu

		ChangeProductsListFromOutside.StandardProductAddition(Items, Text, productCount);

		Text = string.Empty;
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void Delete(MainListProduct product)
	{
		Items.Remove(product);
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void DeleteAll(MainListProduct product)
	{
		Items = new();
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void Increment(MainListProduct product)
	{
		product.Increment();
		ItemListUpdater.SaveListToJson(this);
	}


	[RelayCommand]
	private void Decrement(MainListProduct product)
	{
		product.Decrement();
		ItemListUpdater.SaveListToJson(this);
	}
	[RelayCommand]
	private void ChangeStatus(MainListProduct product)
	{
		product.IsInCart = !product.IsInCart;
		ItemListUpdater.SaveListToJson(this);
	}


	/*[RelayCommand]
	async Task SaveProduct(ProductView product)
	{
		var parameters = new Dictionary<string, object>
		{
			{ "Product", product },
			{ "ProductList", Items }, //TO_DO po zmianie nazw dodać nameof()
			{ nameof(MainViewModel), this}
		};

		await Shell.Current.GoToAsync(nameof(AddNewSavedProductPage), parameters);
	}*/

	[RelayCommand]
	async Task GoToSelectRecipePage()
	{

		var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};

		await Shell.Current.GoToAsync(nameof(SelectRecipePage), parameters);
	}
}

public class ChangeProductsListFromOutside
{

	public ChangeProductsListFromOutside(MainViewModel _mainViewModel)
	{
		mainViewModel = _mainViewModel;
	}

	MainViewModel mainViewModel;

	public void SaveList()
	{
		ItemListUpdater.SaveListToJson(mainViewModel);
	}

	public void StandardProductAddition(ObservableCollection<MainListProduct> collection, string productName, float count)
	{

		MainListProduct newProductView = GetProductIfExistInList(mainViewModel.Items, productName);

		if (newProductView is not null)
		{
			int existingProductOldID = mainViewModel.Items.IndexOf(newProductView!);
			mainViewModel.Items.Move(existingProductOldID, 0);
			return;
		}

		mainViewModel.Items.Insert(0, new MainListProduct(productName, count));




	}
	MainListProduct GetProductIfExistInList(ObservableCollection<MainListProduct> products, string name)
	{
		return products.FirstOrDefault(p => p.Name == name)!;
	}

	public void ForceSetProduct(string productName, float productCount)
	{

		MainListProduct product = GetProductIfExistInList(mainViewModel.Items, productName);

		if (product is null)
		{

			StandardProductAddition(mainViewModel.Items, productName, productCount);
			return;
		}

		product.Count = productCount;
	}
}

static class ItemListUpdater
{
	public static void SaveListToJson(MainViewModel mainViewModel)
	{

		Task.Run(() => JsonHandler.SaveJson(mainViewModel.Items, nameof(mainViewModel.Items))).Wait();
	}

	public static async Task LoadListFromJson(MainViewModel mainViewModel)
	{

		mainViewModel.Items = await JsonHandler.LoadJson<ObservableCollection<MainListProduct>>(nameof(mainViewModel.Items)) ?? new();
	}
}
