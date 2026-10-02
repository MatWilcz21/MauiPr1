namespace MauiApp1.AppSettings;
public class ApplicationSettingsMainBody
{

	//public ApplicationSettings ApplicationSettings { get; set; } = null!;


	public string Language { get; set; } = null!;
	public ApplicationAppearance ApplicationAppearance { get; set; } = null!;
}

public class ApplicationSettings
{

	public ApplicationSettingsMainBody ApplicationSettingsMainBody { get; set; } = null!;

	public async Task LoadSettings()
	{

		//ApplicationSettingsMainBody? outt = null;
		ApplicationSettingsMainBody? outt = await JsonHandler.LoadJson<ApplicationSettingsMainBody>(nameof(ApplicationSettings));


		if (outt is not null)
		{
			ApplicationSettingsMainBody = outt;
			return;
		}


		DefaultAppSettings.SetDefaultSettings(this);
	}
	public async Task SaveSettings()
	{
		await JsonHandler.SaveJson(ApplicationSettingsMainBody, nameof(ApplicationSettings));
	}

}

public class DefaultAppSettings
{
	public static void SetDefaultSettings(ApplicationSettings applicationSettings)
	{

		ApplicationSettingsMainBody aps = new ApplicationSettingsMainBody();

		aps.Language = "en";
		aps.ApplicationAppearance = new ApplicationAppearance();


		applicationSettings.ApplicationSettingsMainBody = aps;

	}

}
