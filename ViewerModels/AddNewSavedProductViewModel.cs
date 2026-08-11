using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class AddNewSavedProductViewModel : ObservableObject, IQueryAttributable
{

	public AddNewSavedProductViewModel()
	{
		Units = SavedUnits.Units!.Select(u => u.Name).ToList();
	}

	[ObservableProperty] public partial ProductView ProductViewX { get; set; } = null!;

	[ObservableProperty] public partial ObservableCollection<ProductView> ItemsX { get; set; }

	[ObservableProperty] public partial List<string> Units { get; set; }

	[ObservableProperty] public partial string SelectedUnit { get; set; }

	MainViewModel mainViewModel;

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Product", out var value))
		{
			ProductViewX = value as ProductView ?? throw new Exception(nameof(ProductView));
		}

		if (query.TryGetValue("ProductList", out var value2))
		{
			ItemsX = value2 as ObservableCollection<ProductView> ?? throw new Exception(nameof(MainViewModel));
		}

		if (query.TryGetValue(nameof(MainViewModel), out var value3))
		{
			mainViewModel = value3 as MainViewModel ?? throw new Exception(nameof(MainViewModel));
		}
	}

	[RelayCommand]
	async Task SaveProduct()
	{

		SavedProducts.Products.Add(new ProductDefinition(ProductViewX.GetName(), SavedUnits.Units[SavedUnits.UnitNameToID(SelectedUnit)]));

		ReplaceCustomProductWithDefinedProduct();
		ItemListUpdater.SaveListToJson(mainViewModel);

		await SavedProducts.SaveProductsList();

		await Shell.Current.GoToAsync("..");

	}

	void ReplaceCustomProductWithDefinedProduct()
	{

		DefinedProductView definedProduct = new DefinedProductView(ProductViewX.GetName(), ProductViewX.Count, ProductViewX.IsInCart);

		int index = ItemsX.IndexOf(ProductViewX);

		ItemsX.Remove(ProductViewX);
		ItemsX.Insert(index, definedProduct);

	}

}
