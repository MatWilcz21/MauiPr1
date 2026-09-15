namespace MauiApp1.Communication.Contacts;

public class ContactsLogic
{

	public ContactsLogic()
	{
		ContactPersons.Add("Al", new ContactPerson("Al", new PersonSMSData("537870143")));
		ContactPersons.Add("Ma", new ContactPerson("Ma", new PersonSMSData("515623758")));

		SelectedContactPerson = ContactPersons["Al"];
	}

	public ContactPerson SelectedContactPerson { get; set; } = null!;

	public Dictionary<string, ContactPerson> ContactPersons { get; set; } = new();

	/*async Task LoadContacts()
	{
		contactPersons.Clear();
	}

	public async Task SaveContacts()
	{
		await JsonHandler.SaveJson(contactPersons, nameof(contactPersons));
	}*/

}
