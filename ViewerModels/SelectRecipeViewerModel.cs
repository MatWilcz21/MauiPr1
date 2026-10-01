using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Pages;
using MauiApp1.Recipes;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

[QueryProperty(nameof(mainViewModel), nameof(MainViewModel))]
public partial class SelectRecipeViewerModel : ObservableObject, IStartableViewModel
{

	[ObservableProperty] public partial string EnterNewRecipeName { get; set; } = null!;

	[ObservableProperty] public partial ObservableCollection<Recipe> RecipesList { get; set; }
	public MainViewModel mainViewModel { get; set; } = null!;

	public async Task Start()
	{
		await LoadRecipes();
	}


}

public partial class SelectRecipeViewerModel
{
	[RelayCommand]
	async Task AddNewRecipe()
	{

		RecipesList.Insert(0, new Recipe(EnterNewRecipeName));

		await GoToEditSelectedRecipePage(EnterNewRecipeName);

		EnterNewRecipeName = string.Empty;
	}

	[RelayCommand]
	private async Task Delete(Recipe recipe)
	{
		RecipesList.Remove(recipe);
		await SaveRecipes();
	}

	[RelayCommand]
	public async Task LoadRecipes()
	{
		RecipesList = await JsonHandler.LoadJson<ObservableCollection<Recipe>>("Recipes") ?? new();
	}
	[RelayCommand]
	public async Task SaveRecipes()
	{
		await JsonHandler.SaveJson(RecipesList, "Recipes");
	}

	[RelayCommand]
	async Task GoToMergeRecipeToList(Recipe selectedRecipe)
	{

		var parameters = new Dictionary<string, object>
		{
			{ nameof(Recipe), selectedRecipe },
			{ nameof(MainViewModel), mainViewModel },
		};

		await StartableViewModel.ChangePage(nameof(MergeToListPage), parameters);
	}
	[RelayCommand]
	async Task EditThisRecipe(Recipe selectedRecipe)
	{
		await GoToEditSelectedRecipePage(selectedRecipe.Name);
	}

	[RelayCommand]
	async Task GoToEditSelectedRecipePage(string selectedRecipeName)
	{

		var parameters = new Dictionary<string, object>
		{
			{ nameof(SelectRecipeViewerModel), this },
			{ "selectedRecipeName", selectedRecipeName },
		};

		await StartableViewModel.ChangePage(nameof(EditSelectedRecipePage), parameters);
	}
}
