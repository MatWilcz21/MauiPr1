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

		PopulateList(mainViewModel.ContactsLogicClass.ContactPersons);
	}

	void PopulateList(Dictionary<string, ContactPerson> contactPersons)
	{
		Friends.Clear();

		foreach (ContactPerson contactPerson in contactPersons.Values)
		{
			Friends.Add(new FriendFromList(contactPerson.Name));
		}

	}

	[RelayCommand]
	private async Task ChangeName(FriendFromList friend)
	{
		mainViewModel.ContactsLogicClass.SelectedContactPerson = mainViewModel.ContactsLogicClass.ContactPersons[friend.Name];
		await Shell.Current.GoToAsync("../..");
	}
}