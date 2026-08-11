using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class MergeProduct : ObservableObject
{

	public MergeProduct(string _name, int _oldCount, int _newCount, bool _merge)
	{
		Name = _name;
		DisplayName = _name.Capitalize();

		OldCount = _oldCount;
		NewCount = _newCount;
		Merge = _merge;
	}

	public string Name { get; set; }
	[ObservableProperty] public partial string DisplayName { get; set; }
	[ObservableProperty] public partial int OldCount { get; set; }
	[ObservableProperty] public partial int NewCount { get; set; }
	[ObservableProperty] public partial bool Merge { get; set; }

}

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
			int currentProductCount = GetCurrentProductCount(recipe.ProductsList[i].Name);
			MergeProductsList.Add(recipe.ProductsList[i].ConvertToMergeProduct(currentProductCount));
		}

		int GetCurrentProductCount(string productName)
		{
			ProductView? productView = mainViewModel.Items.FirstOrDefault(p => p.GetName() == productName);

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
