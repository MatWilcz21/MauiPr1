using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Products;
using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

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

		LoadRecipe(InRecipeEditProducts);
	}

	[ObservableProperty] public partial string EnterNewProductName { get; set; }
	[ObservableProperty] public partial ObservableCollection<RecipeProduct> InRecipeEditProducts { get; set; } = new();

	public SelectRecipeViewerModel selectRecipeViewerModel;

	public string selectedRecipeName;


	[RelayCommand]
	private void Add()
	{

		InRecipeEditProducts.Add(new RecipeProduct(EnterNewProductName.ToLower())); //TO_DO trzeba to poprawic


		EnterNewProductName = string.Empty;
	}

	[RelayCommand]
	private void Delete(RecipeProduct product)
	{
		InRecipeEditProducts.Remove(product);
	}
	[RelayCommand]
	private void Increment(RecipeProduct product)
	{
		product.Increment();
	}

	[RelayCommand]
	private void Decrement(RecipeProduct product)
	{
		product.Decrement();
	}


	[RelayCommand]
	private void ChangeStatus(RecipeProduct recipeProduct)
	{

		recipeProduct.MergeByDefault = !recipeProduct.MergeByDefault;
	}

	[RelayCommand]
	async Task GetPackedRecipe()
	{
		SaveRecipe(InRecipeEditProducts);
		await selectRecipeViewerModel.SaveRecipes();
		await Shell.Current.GoToAsync("..");
	}

	void SaveRecipe(ObservableCollection<RecipeProduct> products)
	{

		Recipe recipe = selectRecipeViewerModel.RecipesList.First(r => r.Name == selectedRecipeName);

		recipe.ProductsList.Clear();
		for (int i = 0; i < products.Count; i++)
		{
			recipe.ProductsList.Add(ConvertToPackedRecipeProduct(products[i]));
		}

		PackedRecipeProduct ConvertToPackedRecipeProduct(RecipeProduct recipeProduct)
		{
			return new PackedRecipeProduct(recipeProduct.Name, recipeProduct.Count, recipeProduct.MergeByDefault);
		}
	}


	void LoadRecipe(ObservableCollection<RecipeProduct> products)
	{

		Recipe recipe = selectRecipeViewerModel.RecipesList.First(r => r.Name == selectedRecipeName);

		products.Clear();

		for (int i = 0; i < recipe.ProductsList.Count; i++)
		{
			PackedRecipeProduct packedRecipeProduct = recipe.ProductsList[i];

			RecipeProduct newRecipeProduct = new RecipeProduct(packedRecipeProduct.Name, packedRecipeProduct.Count, packedRecipeProduct.MergeByDefault);

			newRecipeProduct.MergeByDefault = packedRecipeProduct.MergeByDefault; //TO_DO ogarnac too/ nie dotykac potrzebne

			products.Add(newRecipeProduct);
		}

	}

}
