namespace MauiApp1.Communication.Contacts;

public class ContactsLogic
{

	public ContactsLogic()
	{
		//TO_DO gdy contacty są puste wywala blad przy probie wyswietlenia listy
		Task.Run(() => LoadContacts()).Wait();

		//ContactPersons.Add("Al", new ContactPerson("Al", new PersonSMSData(SecretPhoneNumbers.Ale)));
		//ContactPersons.Add("Ma", new ContactPerson("Ma", new PersonSMSData(SecretPhoneNumbers.Mat)));

		//Task.Run(() => SaveContacts()).Wait();

		//ChangeSelectSavedPerson("Al");
	}

	public ContactPerson? SelectedContactPerson { get; set; }

	public Dictionary<string, ContactPerson> ContactPersons { get; set; }

	public void ChangeSelectSavedPerson(string name)
	{
		if (name is null)
		{
			SelectedContactPerson = null;
			return;
		}

		ContactPerson? c = ContactPersons.GetValueOrDefault(name)!;
		if (c is null)
		{
			SelectedContactPerson = null;
			return;
		}

		SelectedContactPerson = c;

		SaveContacts();
	}

	async Task LoadContacts()
	{

		PackedContacts? p = await JsonHandler.LoadJson<PackedContacts>(nameof(PackedContacts));

		if (p is not null)
		{
			ContactPersons = p.ContactPersons;
			ChangeSelectSavedPerson(p.SelectedContact);
			return;
		}

		ContactPersons = new();
	}

	public async Task SaveContacts()
	{

		PackedContacts p = new PackedContacts(ContactPersons, SelectedContactPerson.Name);

		await JsonHandler.SaveJson(p, nameof(PackedContacts));
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

public record class PackedContacts(Dictionary<string, ContactPerson> ContactPersons, string SelectedContact);
