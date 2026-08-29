using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Pages;
using MauiApp1.Recipes;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class SelectRecipeViewerModel : ObservableObject, IQueryAttributable
{

	public SelectRecipeViewerModel()
	{
		RecipesList = new();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(nameof(MainViewModel), out var value))
		{
			mainViewModel = value as MainViewModel ?? throw new Exception(nameof(MainViewModel));
		}

		try
		{
			Task.Run(() => LoadRecipes().Wait());
		}
		catch
		{
			//TO_DO trzeba to ogarnąć
		}
	}

	[ObservableProperty] public partial string EnterNewRecipeName { get; set; } = null!;

	[ObservableProperty] public partial ObservableCollection<Recipe> RecipesList { get; set; }

	MainViewModel mainViewModel = null!;

	[RelayCommand]
	async Task AddNewRecipe()
	{

		RecipesList.Insert(0, new Recipe(EnterNewRecipeName));

		await GoToEditSelectedRecipePage(EnterNewRecipeName);

		EnterNewRecipeName = string.Empty;
	}

	[RelayCommand]
	private void Delete(Recipe recipe)
	{
		RecipesList.Remove(recipe);
	}

	[RelayCommand]
	async Task LoadRecipes()
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

		await Shell.Current.GoToAsync(nameof(MergeToListPage), parameters);
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

		await Shell.Current.GoToAsync(nameof(EditSelectedRecipePage), parameters);
	}
}
