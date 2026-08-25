using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Products;
using MauiApp1.Recipes;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MergeToListViewModel : ObservableObject, IQueryAttributable
{
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		MergeProductsList = new();
		if (query.TryGetValue(nameof(MainViewModel), out var value))
		{
			mainViewModel = value as MainViewModel ?? throw new Exception(nameof(MainViewModel));
		}
		if (query.TryGetValue(nameof(Recipe), out var valuex))
		{
			recipe = valuex as Recipe ?? throw new Exception(nameof(Recipe));
		}

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
			BaseProduct? productView = mainViewModel.Items.FirstOrDefault(p => p.Name == productName);

			if (productView is null) return 0;

			return productView.Count;
		}

	}

	[ObservableProperty] public partial ObservableCollection<MergeProduct> MergeProductsList { get; set; }

	MainViewModel mainViewModel = null!;
	Recipe recipe = null!;

	MergeHandler mergeHandler = null!;


	[RelayCommand]
	private void ChangeStatus(MergeProduct mergeProduct)
	{

		mergeProduct.Merge = !mergeProduct.Merge;
	}

	[RelayCommand]
	async Task Merge()
	{

		mergeHandler.CreateMerge();


		/*var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};*/

		//await Shell.Current.GoToAsync(nameof(SelectRecipePage), parameters);
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

			mainViewModel.ChangeProductsListFromOutside.ForceSetProduct(mergeProduct.Name, mergeProduct.NewCount);

		}

		mainViewModel.ChangeProductsListFromOutside.SaveList();

	}

}
