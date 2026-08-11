using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Pages;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MainViewModel : ObservableObject
{

	public MainViewModel()
	{
		Items = new();
		ChangeProductsListFromOutside = new ChangeProductsListFromOutside(this);
		Text = "";
		try
		{
			Task.Run(() => ItemListUpdater.LoadListFromJson(this)).Wait();
		}
		catch
		{
			//TO_DO trzeba to ogarnąć
		}

	}

	[ObservableProperty] public partial ObservableCollection<ProductView> Items { get; set; } //TO_DO zmienić nazwę

	[ObservableProperty] public partial string Text { get; set; } //TO_DO zmienić nazwę

	public ChangeProductsListFromOutside ChangeProductsListFromOutside { get; } = null!;

	[RelayCommand]
	void Add()
	{

		if (string.IsNullOrWhiteSpace(Text)) return;

		Text = Text.ToLower();
		Text = Text.Trim();

		if (ProductExists(Text)) return;

		ChangeProductsListFromOutside.StandardProductAddition(Items, Text);

		Text = string.Empty;
		ItemListUpdater.SaveListToJson(this);

		bool ProductExists(string productName)
		{

			ProductView? existingProduct = Items.FirstOrDefault(p => p.GetName() == productName);

			if (existingProduct is null) return false;

			int existingProductOldID = Items.IndexOf(existingProduct!);

			Items.Move(existingProductOldID, 0);

			Text = string.Empty;

			return true;

		}
	}

	[RelayCommand]
	private void Delete(ProductView product)
	{
		Items.Remove(product);
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void DeleteAll(ProductView product)
	{
		Items = new();
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void Increment(ProductView product)
	{
		product.Increment();
		ItemListUpdater.SaveListToJson(this);
	}


	[RelayCommand]
	private void Decrement(ProductView product)
	{
		product.Decrement();
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	private void ChangeStatus(ProductView product)
	{
		product.IsInCart = !product.IsInCart;
		ItemListUpdater.SaveListToJson(this);
	}

	[RelayCommand]
	async Task SaveProduct(ProductView product)
	{
		var parameters = new Dictionary<string, object>
		{
			{ "Product", product },
			{ "ProductList", Items }, //TO_DO po zmianie nazw dodać nameof()
			{ nameof(MainViewModel), this}
		};

		await Shell.Current.GoToAsync(nameof(AddNewSavedProductPage), parameters);
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

public class ChangeProductsListFromOutside(MainViewModel mainViewModel)
{

	public void SaveList()
	{
		ItemListUpdater.SaveListToJson(mainViewModel);
	}

	public static ProductView StandardProductAddition(ObservableCollection<ProductView> collection, string productName)
	{

		if (ProductExists(productName)) return collection.First(p => p.GetName() == productName);


		ProductView newProductView = null!;


		if (SavedProducts.Products.FirstOrDefault(p => p.Name == productName) is null)
			newProductView = new CustomProductView(productName);
		else
			newProductView = new DefinedProductView(productName);

		collection.Insert(0, newProductView);

		return newProductView;

		bool ProductExists(string productName)
		{

			ProductView? existingProduct = collection.FirstOrDefault(p => p.GetName() == productName);

			if (existingProduct is null) return false;

			int existingProductOldID = collection.IndexOf(existingProduct!);

			collection.Move(existingProductOldID, 0);

			productName = string.Empty;

			return true;

		}

	}

	public void ForceSetProduct(string productName, int productCount)
	{
		ProductView productView = StandardProductAddition(mainViewModel.Items, productName);
		productView.Count = productCount;
	}
}

static class ItemListUpdater
{
	public static void SaveListToJson(MainViewModel mainViewModel)
	{

		PackedProduct[] productsToSave = new PackedProduct[mainViewModel.Items.Count];


		for (int i = 0; i < productsToSave.Length; i++)
		{

			ProductClassEnum pClass = ProductClassEnum.CustomProductView;

			if (mainViewModel.Items[i] is DefinedProductView)
				pClass = ProductClassEnum.DefinedProductView;
			else if (mainViewModel.Items[i] is CustomProductView)
				pClass = ProductClassEnum.CustomProductView;

			productsToSave[i] = new PackedProduct(pClass, mainViewModel.Items[i].GetName(), mainViewModel.Items[i].Count, mainViewModel.Items[i].IsInCart);
		}

		Task.Run(() => JsonHandler.SaveJson(productsToSave, nameof(mainViewModel.Items))).Wait();
	}

	public static async Task LoadListFromJson(MainViewModel mainViewModel)
	{

		var o = await JsonHandler.LoadJson<PackedProduct[]>(nameof(mainViewModel.Items));

		if (o is null)
		{
			return;
		}

		for (int i = 0; i < o.Length; i++)
		{

			PackedProduct packedProduct = o[i];

			ProductView productView = null!;

			if (o[i].ProductClass == ProductClassEnum.DefinedProductView)
				productView = new DefinedProductView(packedProduct.Name, packedProduct.Count, packedProduct.IsInCart);
			else if (o[i].ProductClass == ProductClassEnum.CustomProductView)
				productView = new CustomProductView(packedProduct.Name, packedProduct.Count, packedProduct.IsInCart);

			mainViewModel.Items.Add(productView);
		}
	}
}
