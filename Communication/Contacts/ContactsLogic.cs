namespace MauiApp1.Communication.Contacts;

public class ContactsLogic
{

	public ContactsLogic()
	{

		Task.Run(() => LoadContacts()).Wait();

		//ContactPersons.Add("Al", new ContactPerson("Al", new PersonSMSData(SecretPhoneNumbers.Ale)));
		//ContactPersons.Add("Ma", new ContactPerson("Ma", new PersonSMSData(SecretPhoneNumbers.Mat)));

		//Task.Run(() => SaveContacts()).Wait();

		SelectPerson("Al");
	}

	public ContactPerson SelectedContactPerson { get; set; } = null!;

	public Dictionary<string, ContactPerson> ContactPersons { get; set; } = new();

	public void SelectPerson(string name)
	{
		if (name is null)
			goto dott;

		ContactPerson c = ContactPersons.GetValueOrDefault(name)!;

		if (c is not null)
		{
			SelectedContactPerson = c;
			return;
		}

	dott:

		SelectedContactPerson = ContactPersons.FirstOrDefault().Value;
	}

	async Task LoadContacts()
	{
		ContactPersons = await JsonHandler.LoadJson<Dictionary<string, ContactPerson>>(nameof(ContactPersons)) ?? new();
		SelectPerson(null!);
	}

	public async Task SaveContacts()
	{
		await JsonHandler.SaveJson(ContactPersons, nameof(ContactPersons));
		SelectPerson(null!);
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
