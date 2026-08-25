using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.ViewerModels;

public partial class AddNewSavedProductViewModel : ObservableObject, IQueryAttributable
{


	MainViewModel mainViewModel;

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{

		if (query.TryGetValue(nameof(MainViewModel), out var value3))
		{
			mainViewModel = value3 as MainViewModel ?? throw new Exception(nameof(MainViewModel));
		}
	}

	[RelayCommand]
	async Task SaveProduct()
	{
		ItemListUpdater.SaveListToJson(mainViewModel);

		await SavedProducts.SaveProductsList();

		await Shell.Current.GoToAsync("..");

	}

}
