using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Products;
using MauiApp1.Products.MainProductsList;
using MauiApp1.Recipes;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

[QueryProperty(nameof(mainViewModel), nameof(MainViewModel))]
[QueryProperty(nameof(recipe), nameof(Recipe))]
public partial class MergeToListViewModel : ObservableObject, IStartableViewModel
{

	[ObservableProperty] public partial ObservableCollection<MergeProduct> MergeProductsList { get; set; }

	public MainViewModel mainViewModel { get; set; } = null!;
	public Recipe recipe { get; set; } = null!;

	MergeHandler mergeHandler = null!;

	public async Task Start()
	{
		MergeProductsList = new();


		mergeHandler = new MergeHandler(mainViewModel, this);

		for (int i = 0; i < recipe.ProductsList.Count; i++)
		{
			float currentProductCount = GetCurrentProductCount(recipe.ProductsList[i].Name);

			PackedRecipeProduct packedRecipeProduct = recipe.ProductsList[i];

			MergeProduct newMergeProduct = new MergeProduct(packedRecipeProduct.Name, currentProductCount, currentProductCount + packedRecipeProduct.Count, packedRecipeProduct.MergeByDefault);

			MergeProductsList.Add(newMergeProduct);
		}

		float GetCurrentProductCount(string productName)
		{
			BaseProduct? productView = mainViewModel.MainProductsListClass.Products.FirstOrDefault(p => p.Name == productName);

			if (productView is null) return 0;

			return productView.Count;
		}
	}

}

public partial class MergeToListViewModel
{
	[RelayCommand]
	private void ChangeStatus(MergeProduct mergeProduct)
	{
		mergeProduct.Merge = !mergeProduct.Merge;
	}

	[RelayCommand]
	async Task Merge()
	{
		mergeHandler.CreateMerge();
		await Shell.Current.GoToAsync("../..");
	}
}

class MergeHandler(MainViewModel mainViewModel, MergeToListViewModel mergeToListViewModel)
{

	public void CreateMerge()
	{
		for (int i = 0; i < mergeToListViewModel.MergeProductsList.Count; i++)
		{
			MergeProduct mergeProduct = mergeToListViewModel.MergeProductsList[i];

			if (!mergeProduct.Merge) continue;

			mainViewModel.MainProductsListClass.Products.AddProductToList(mergeProduct.Name, mergeProduct.NewCount, mainViewModel.MainProductsListClass);

		}
		mainViewModel.MainProductsListClass.SaveList();
	}

}
