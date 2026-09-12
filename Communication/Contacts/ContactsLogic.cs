namespace MauiApp1.Communication.Contacts;

public class ContactsLogic
{

	public ContactsLogic()
	{
		ContactPersons.Add("Skarb", new ContactPerson("Skarb", new PersonSMSData("537870143")));
		ContactPersons.Add("Ja", new ContactPerson("Ja", new PersonSMSData("515623758")));

		SelectedContactPerson = ContactPersons["Skarb"];
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
