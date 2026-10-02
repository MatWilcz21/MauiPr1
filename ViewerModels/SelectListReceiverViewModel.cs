using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Communication.Contacts;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewerModels;

public partial class FriendFromList : ObservableObject
{
	public FriendFromList(string _name)
	{
		Name = _name;
	}

	[ObservableProperty] public partial string Name { get; set; }


}
[QueryProperty(nameof(mainViewModel), nameof(MainViewModel))]
public partial class SelectListReceiverViewModel : ObservableObject, IStartableViewModel
{
	[ObservableProperty] public partial string NewContactNameEntry { get; set; } = string.Empty;
	[ObservableProperty] public partial string SelectedFriendName { get; set; }
	[ObservableProperty] public partial ObservableCollection<FriendFromList> Friends { get; set; } = new();

	public MainViewModel mainViewModel { get; set; } = null!;

	public async Task Start()
	{

		ContactsLogic.ThresholdReached += RefreshSelectedContactName;

		RefreshList();
		RefreshSelectedContactName(this, EventArgs.Empty);
		await Task.CompletedTask;
	}

	public void RefreshSelectedContactName(object? sender, EventArgs e)
	{

		if (mainViewModel.ContactsLogicClass.SelectedContactPerson is not null)
		{
			SelectedFriendName = mainViewModel.ContactsLogicClass.SelectedContactPerson.Name;
			return;
		}

		SelectedFriendName = "Not selected";
	}

	public void RefreshList()
	{
		Friends.Clear();

		foreach (ContactPerson contactPerson in mainViewModel.ContactsLogicClass.ContactPersons.Values)
		{
			Friends.Insert(0, new FriendFromList(contactPerson.Name));
		}

	}

}

public partial class SelectListReceiverViewModel
{
	[RelayCommand]
	async Task Add()
	{

		if (string.IsNullOrWhiteSpace(NewContactNameEntry)) return;

		await mainViewModel.ContactsLogicClass.Add(NewContactNameEntry.ToLower());
		NewContactNameEntry = string.Empty;

		RefreshList();
	}

	[RelayCommand]
	private async Task ChangeName(FriendFromList friend)
	{
		await mainViewModel.ContactsLogicClass.ChangeSelectSavedPerson(friend.Name);
		await Shell.Current.GoToAsync("../..");
	}

	[RelayCommand]
	private async Task DeleteContact(FriendFromList friend)
	{

		bool answer = await Shell.Current.DisplayAlert(
		"Confirmation",
		$"""Are you sure you want to delete "{friend.Name}"?""",
		"Yes",
		"No");

		if (!answer)
			return;

		await mainViewModel.ContactsLogicClass.DeleteContact(friend.Name);
		RefreshList();
	}
}