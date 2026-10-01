using MauiApp1.AppSettings;

namespace MauiApp1.Communication.Contacts;

public class ContactsLogic
{

	public ContactsLogic(ApplicationSettings _settings)
	{

		contacts = _settings.ApplicationContacts;

		Task.Run(() => LoadContacts()).Wait();

		//ContactPersons.Add("Al", new ContactPerson("Al", new PersonSMSData(SecretPhoneNumbers.Ale)));
		//ContactPersons.Add("Ma", new ContactPerson("Ma", new PersonSMSData(SecretPhoneNumbers.Mat)));

		//Task.Run(() => SaveContacts()).Wait();
	}

	public ContactPerson? SelectedContactPerson { get; set; } = null;

	public Dictionary<string, ContactPerson> ContactPersons { get; set; } = new();

	ApplicationContacts contacts;

	public void SelectSavedPerson()
	{
		if (contacts.SavedReceiverName is null)
		{
			SelectedContactPerson = null;
			return;
		}

		if (!ContactPersons.ContainsKey(contacts.SavedReceiverName))
		{
			SelectedContactPerson = null;
			contacts.SavedReceiverName = null;
			return;
		}

		SelectedContactPerson = ContactPersons[contacts.SavedReceiverName];
	}

	public void ChangeSelectSavedPerson(string nameToSelect)
	{
		contacts.SavedReceiverName = nameToSelect;
	}

	async Task LoadContacts()
	{
		ContactPersons = await JsonHandler.LoadJson<Dictionary<string, ContactPerson>>(nameof(ContactPersons)) ?? new();
		SelectSavedPerson();
	}

	public async Task SaveContacts()
	{
		await JsonHandler.SaveJson(ContactPersons, nameof(ContactPersons));
	}

	public async Task Add(string name)
	{
		ContactPersons.Add(name, new ContactPerson(name, null!));
		await SaveContacts();
	}

	public async Task DeleteContact(string name)
	{
		ContactPersons.Remove(name);
		await SaveContacts();
	}

}
