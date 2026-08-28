using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Pages;
using MauiApp1.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MainViewModel : ObservableObject
{

	public MainViewModel()
	{

		Text = "";
		MainProductsListClass = new MainProductsListClass(this);
	}

	[ObservableProperty] public partial MainProductsListClass MainProductsListClass { get; set; }

	[ObservableProperty] public partial string Text { get; set; } //TO_DO zmienić nazwę

	[RelayCommand]
	void Add()
	{
		MainProductsListClass.Add(Text);
		Text = string.Empty;
	}

	[RelayCommand]
	async Task DeleteAll()
	{
		await MainProductsListClass.DeleteAll();
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
}

public partial class MainProductsListClass : ObservableObject
{


	public MainProductsListClass(MainViewModel _mainViewModel)
	{

		mainViewModel = _mainViewModel;

		Products = new();
		ChangeProductsListFromOutside = new ChangeProductsListFromOutside(this);

		Task.Run(() => ItemListUpdater.LoadListFromJson(this)).Wait();
	}


	[ObservableProperty] public partial ObservableCollection<MainListProduct> Products { get; set; } //TO_DO zmienić nazwę
	public ChangeProductsListFromOutside ChangeProductsListFromOutside { get; } = null!;

	MainViewModel mainViewModel;

	public void Add(string name)
	{

		if (string.IsNullOrWhiteSpace(name)) return;

		name = name.ToLower();
		name = name.Trim();

		float productCount = 1; //TO_DO parsuj czy nie ma dopisanej ilosci produktu

		ChangeProductsListFromOutside.StandardProductAddition(Products, name, productCount);
		ItemListUpdater.SaveListToJson(this);
	}

	public async Task DeleteAll()
	{

		bool answer = await Shell.Current.DisplayAlert(
		"Confirmation",
		"Are you sure you want to delete all products?",
		"Yes",
		"No");

		if (!answer)
			return;

		Products.Clear();
		ItemListUpdater.SaveListToJson(this);
	}

	void SortProductsByStatus(MainListProduct product, bool toCart)
	{

		int lastOutOfCartProduct = Products.Count(e => e.IsInCart == false);

		int currentIndex = Products.IndexOf(product);

		if (!toCart)
		{
			//Products.Move(currentIndex, Math.Max(0, lastOutOfCartProduct - 1));
			Products.Move(currentIndex, 0);
			return;
		}

		Products.Move(currentIndex, lastOutOfCartProduct);

	}

	#region Commands

	[RelayCommand]
	private void Delete(MainListProduct product)
	{
		Products.Remove(product);
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void Increment(MainListProduct product)
	{
		product.Increment();
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private async Task ChangeName(MainListProduct product)
	{
		await product.ChangeName(Products);
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private async Task SetCustomCount(MainListProduct product)
	{

		string? result = await Shell.Current.DisplayPromptAsync(
		"Enter custom value",
		"Enter a number:",
		"OK",
		"Cancel",
		keyboard: Keyboard.Numeric);

		if (float.TryParse(result, out float value))
			product.Count = value;


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

		SortProductsByStatus(product, product.IsInCart);

		ItemListUpdater.SaveListToJson(this);
	}

	#endregion

}

public class ChangeProductsListFromOutside
{

	public ChangeProductsListFromOutside(MainProductsListClass _mainViewModel)
	{
		mainViewModel = _mainViewModel;
	}

	MainProductsListClass mainViewModel;

	public void SaveList()
	{
		ItemListUpdater.SaveListToJson(mainViewModel);
	}

	public void StandardProductAddition(ObservableCollection<MainListProduct> collection, string productName, float count)
	{

		MainListProduct newProductView = GetProductIfExistInList(mainViewModel.Products, productName);

		if (newProductView is not null)
		{
			int existingProductOldID = mainViewModel.Products.IndexOf(newProductView!);
			mainViewModel.Products.Move(existingProductOldID, 0);
			return;
		}

		mainViewModel.Products.Insert(0, new MainListProduct(productName, count));

	}
	MainListProduct GetProductIfExistInList(ObservableCollection<MainListProduct> products, string name)
	{
		return products.FirstOrDefault(p => p.Name == name)!;
	}

	public void ForceSetProduct(string productName, float productCount)
	{

		MainListProduct product = GetProductIfExistInList(mainViewModel.Products, productName);

		if (product is null)
		{

			StandardProductAddition(mainViewModel.Products, productName, productCount);
			return;
		}

		product.Count = productCount;
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
