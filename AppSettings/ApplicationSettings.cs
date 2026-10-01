namespace MauiApp1.AppSettings;

public class ApplicationSettings
{
	public string Language { get; set; } = "en";
	public ApplicationAppearance ApplicationAppearance { get; set; } = null!;
	public ApplicationContacts ApplicationContacts { get; set; } = new ApplicationContacts();

	public async Task LoadSettings()
	{
		// wczytanie ustawień z pliku / Preferences itd.

		await Task.CompletedTask;
	}
}
