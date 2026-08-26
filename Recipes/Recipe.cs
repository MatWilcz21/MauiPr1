using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MauiApp1.Recipes;

public partial class Recipe : ObservableObject
{

	public Recipe() { }

	public Recipe(string _name)
	{
		Name = _name;
	}

	[ObservableProperty] public partial string Name { get; set; }

	[ObservableProperty] public partial ObservableCollection<PackedRecipeProduct> ProductsList { get; set; } = new();
}

public record PackedRecipeProduct(string Name, float Count, bool MergeByDefault);
