using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.AppSettings;
using MauiApp1.Communication.Contacts;
using MauiApp1.Pages;
using MauiApp1.Products.MainProductsList;

namespace MauiApp1.ViewerModels;

public partial class MainViewModel : ObservableObject
{

	public MainViewModel(ApplicationSettings _settings)
	{
		settings = _settings;
		Task.Run(() => _settings.LoadSettings()).Wait();
		MainProductsListClass = new MainProductsListClass(this);
		ContactsLogicClass = new ContactsLogic(this);
		ReceiveListUpdate.SendMainViewModel(this);

	}

	[ObservableProperty] public partial MainProductsListClass MainProductsListClass { get; set; }
	[ObservableProperty] public partial string NewProductNameEntry { get; set; } = string.Empty;

	[ObservableProperty] public partial ContactsLogic ContactsLogicClass { get; set; }

	private readonly ApplicationSettings settings;


}
// RelayCommands
public partial class MainViewModel
{
	[RelayCommand]
	void Add()
	{
		MainProductsListClass.Add(NewProductNameEntry);
		NewProductNameEntry = string.Empty;
	}

	[RelayCommand]
	async Task DeleteAll()
	{
		await MainProductsListClass.DeleteAll();
	}

	[RelayCommand]
	async Task Update()
	{
		await MainProductsListClass.Update();
	}

	[RelayCommand]
	async Task GoToSelectRecipePage()
	{

		var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};

		await StartableViewModel.ChangePage(nameof(SelectRecipePage), parameters);
	}

	[RelayCommand]
	async Task GoToSelectListReceiverPage()
	{
		var parameters = new Dictionary<string, object>
		{
			{ nameof(MainViewModel), this },
		};

		await StartableViewModel.ChangePage(nameof(SelectListReceiverPage), parameters);
	}
}
