namespace MauiApp1.Communication.Contacts;

public record ContactPerson(string Name, PersonSMSData PersonSMSData);

public record PersonSMSData(string PhoneNumber);

