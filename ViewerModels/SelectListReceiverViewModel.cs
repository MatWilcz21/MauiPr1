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

public partial class SelectListReceiverViewModel : ObservableObject, IQueryAttributable
{
	[ObservableProperty] public partial string NewContactNameEntry { get; set; } = string.Empty;
	[ObservableProperty] public partial string SelectedFriendName { get; set; }
	[ObservableProperty] public partial ObservableCollection<FriendFromList> Friends { get; set; } = new();

	MainViewModel mainViewModel = null!;

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{

		if (query.TryGetValue(nameof(MainViewModel), out var value))
		{
			mainViewModel = value as MainViewModel ?? throw new Exception(nameof(MainViewModel));
		}

		SelectedFriendName = mainViewModel.ContactsLogicClass.SelectedContactPerson.Name;

		RefreshList();
	}

	void RefreshList()
	{
		Friends.Clear();

		foreach (ContactPerson contactPerson in mainViewModel.ContactsLogicClass.ContactPersons.Values)
		{
			Friends.Insert(0, new FriendFromList(contactPerson.Name));
		}

	}

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
		mainViewModel.ContactsLogicClass.SelectPerson(friend.Name);
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