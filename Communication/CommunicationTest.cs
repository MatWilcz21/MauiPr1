namespace MauiApp1.Communication;

#if ANDROID
using Android.Telephony;
using Microsoft.Maui.ApplicationModel;
#endif

class CommunicationTest
{

	public async Task Send(string phonenbr, string message)
	{
#if ANDROID

		var status = await Permissions.RequestAsync<SmsPermission>();

		if (status != PermissionStatus.Granted)
		{
			await Shell.Current.DisplayAlert(
				"Brak uprawnienia",
				"Aplikacja nie ma pozwolenia na wysyłanie/odbieranie SMS.",
				"OK");

			return;
		}

		try
		{
			SmsManager smsM = SmsManager.Default;

			smsM.SendTextMessage(
				phonenbr,
				null,
				message,
				null,
				null);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert(
				"Błąd",
				$"Nie udało się wysłać SMS:\n{ex.Message}",
				"OK");
		}

#else

        await Task.CompletedTask;

#endif
	}
}


#if ANDROID

class SmsPermission : Permissions.BasePlatformPermission
{
	public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
		new[]
		{
			(global::Android.Manifest.Permission.SendSms, true),
			(global::Android.Manifest.Permission.ReceiveSms, true)
		};
}

#endif