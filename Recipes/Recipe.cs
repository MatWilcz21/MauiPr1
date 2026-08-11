using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.ViewerModels;
using System.Collections.ObjectModel;

namespace MauiApp1.Recipes;

public partial class Recipe : ObservableObject
{

	public Recipe()
	{

	}

	public Recipe(string _name)
	{
		Name = _name;
	}

	[ObservableProperty] public partial string Name { get; set; }

	[ObservableProperty] public partial ObservableCollection<RecipeProduct> ProductsList { get; set; } = new();
}

public record RecipeProduct(string Name, byte Count, bool MergeByDefault);

public static class RecipeProductExtension
{
	public static MergeProduct ConvertToMergeProduct(this RecipeProduct recipeProduct, int oldCount)
	{
		return new MergeProduct(recipeProduct.Name, oldCount, oldCount + recipeProduct.Count, recipeProduct.MergeByDefault);
	}
}
