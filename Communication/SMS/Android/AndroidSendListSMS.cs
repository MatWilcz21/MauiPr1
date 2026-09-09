#if ANDROID
using Android.Telephony;
#endif

namespace MauiApp1.Communication.SMS.Android;

class AndroidSendListSMS : IProductListDelivery
{

	public async Task Receive()
	{
		throw new NotImplementedException();
	}

	public async Task Send(string phoneNumber, string message)
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
				phoneNumber,
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
