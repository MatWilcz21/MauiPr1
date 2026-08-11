using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.ProductsFactoryAndConverter.Converter;
using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class DefinedInRecipeEditProduct : DefinedProductView, Ica
{

	public DefinedInRecipeEditProduct(string _productName)
	{
		ProductDefinition = FindProductDefinition(_productName);

		DisplayName = GetName().Capitalize();
		Count = 1;
		MergeByDefault = true;
	}

	public DefinedInRecipeEditProduct(string _productName, int _count, bool _mergeByDefault)
	{
		ProductDefinition = FindProductDefinition(_productName);
		Count = _count;
		MergeByDefault = _mergeByDefault;

		DisplayName = GetName().Capitalize();
		Unit = ProductDefinition.Unit;
	}

	[ObservableProperty] public partial bool MergeByDefault { get; set; }

	public void inc()
	{
		MergeByDefault = !MergeByDefault;
	}

	public RecipeProduct ToRecipeProduct()
	{
		return new RecipeProduct(GetName(), (byte)Count, MergeByDefault);
	}
}

public partial class CustomInRecipeEditProduct : CustomProductView, Ica
{

	public CustomInRecipeEditProduct(string _productName)
	{

		ProductName = _productName;

		DisplayName = GetName();
		Count = 1;
		MergeByDefault = true;
	}


	public CustomInRecipeEditProduct(string _productName, int _count, bool _mergeByDefault)
	{

		ProductName = _productName;

		DisplayName = GetName();

		Count = _count;

		MergeByDefault = _mergeByDefault;
	}

	[ObservableProperty] public partial bool MergeByDefault { get; set; }

	public void inc()
	{
		MergeByDefault = !MergeByDefault;
	}

	public RecipeProduct ToRecipeProduct()
	{
		return new RecipeProduct(GetName(), (byte)Count, MergeByDefault);
	}
}

public interface Ica
{
	public void inc();
	public RecipeProduct ToRecipeProduct();
}

public partial class EditSelectedRecipeViewModel : ObservableObject, IQueryAttributable
{

	public EditSelectedRecipeViewModel()
	{
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{

		if (query.TryGetValue(nameof(SelectRecipeViewerModel), out var value))
		{
			selectRecipeViewerModel = value as SelectRecipeViewerModel ?? throw new Exception(nameof(SelectRecipeViewerModel));
		}

		if (query.TryGetValue("selectedRecipeName", out var valuex))
		{
			selectedRecipeName = valuex as string ?? throw new Exception(nameof(selectedRecipeName));
		}

		SetRecipe();
	}

	[ObservableProperty] public partial string EnterNewProductName { get; set; }
	[ObservableProperty] public partial ObservableCollection<ProductView> InRecipeEditProducts { get; set; } = new();

	public SelectRecipeViewerModel selectRecipeViewerModel;

	public string selectedRecipeName;

	void SetRecipe()
	{
		InRecipeEditProducts = new();
		sss(selectedRecipeName);

	}


	[RelayCommand]
	private void Add()
	{
		ProductView inRecipeEditProduct = ChangeProductsListFromOutside.StandardProductAddition(InRecipeEditProducts, EnterNewProductName.ToLower());

		int x = InRecipeEditProducts.IndexOf(inRecipeEditProduct);

		if (inRecipeEditProduct is DefinedProductView)
		{
			InRecipeEditProducts[x] = new DefinedInRecipeEditProduct(EnterNewProductName.ToLower(), 1, true);
		}
		else if (inRecipeEditProduct is CustomProductView)
		{
			InRecipeEditProducts[x] = new CustomInRecipeEditProduct(EnterNewProductName.ToLower(), 1, true);
		}
		else
			throw new Exception();

		EnterNewProductName = string.Empty;
	}

	[RelayCommand]
	private void Delete(ProductView product)
	{
		InRecipeEditProducts.Remove(product);
	}
	[RelayCommand]
	private void Increment(ProductView product)
	{
		product.Increment();
	}

	[RelayCommand]
	private void Decrement(ProductView product)
	{
		product.Decrement();
	}


	[RelayCommand]
	private void ChangeStatus(ProductView recipeProduct)
	{

		if (recipeProduct is CustomInRecipeEditProduct)
		{
			CustomInRecipeEditProduct pr = (CustomInRecipeEditProduct)recipeProduct;
			pr.inc();
		}
		else if (recipeProduct is DefinedInRecipeEditProduct)
		{
			DefinedInRecipeEditProduct ins = (DefinedInRecipeEditProduct)recipeProduct;
			ins.inc();
		}
	}

	[RelayCommand]
	async Task GetPackedRecipe()
	{
		GetPackedRecipe(selectedRecipeName, InRecipeEditProducts);
		await selectRecipeViewerModel.SaveRecipes();
		await Shell.Current.GoToAsync("..");
	}

	void GetPackedRecipe(string name, ObservableCollection<ProductView> products)
	{

		Recipe recipe = selectRecipeViewerModel.RecipesList.First(r => r.Name == name);

		recipe.ProductsList = new();
		for (int i = 0; i < products.Count; i++)
		{
			recipe.ProductsList.Add(InRecipeProduct2RecipeProduct.Convert(products[i]));
		}
	}

	void sss(string name)
	{

		Recipe recipe = selectRecipeViewerModel.RecipesList.First(r => r.Name == name);


		for (int i = 0; i < recipe.ProductsList.Count; i++)
		{
			ProductView pr = RecipeProduct2InRecipeProduct.Convert(recipe.ProductsList[i]);

			InRecipeEditProducts.Add(pr);


		}

	}

}
