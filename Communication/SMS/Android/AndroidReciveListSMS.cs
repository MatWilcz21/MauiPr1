using System.Text.RegularExpressions;
using MauiApp1.ViewerModels;


#if ANDROID
using Android.App;
using Android.Content;
using Android.Provider;
#endif

namespace MauiApp1.Communication.SMS.Android;

#if ANDROID

[BroadcastReceiver(
Enabled = true,
Exported = true,
Permission = "android.permission.BROADCAST_SMS")]
[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]

class AndroidReciveListSMS : BroadcastReceiver
{
	public override void OnReceive(Context? context, Intent? intent)
	{
		if (intent?.Action != "android.provider.Telephony.SMS_RECEIVED")
			return;

		var messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

		foreach (var sms in messages)
		{
			string? sender = sms.OriginatingAddress;
			string? message = sms.MessageBody;

			List<string> results = Regex.Matches(message, @"<([^>]*)>").Select(m => m.Groups[1].Value).ToList();

			MainProductsListClass mainProductsListClass =
			MauiApplication.Current.Services.GetService<MainViewModel>().MainProductsListClass; //TO_DO to do ogarniecia

			mainProductsListClass.Products.Clear();
			for (int i = 0; i < results.Count; i++)
			{
				string[] values = results[i]
	.Replace("\"", "")
	.Split(' ');

				string a = values[0];
				string b = values[1];
				string c = values[2];

				mainProductsListClass.Products.Add(new Products.MainListProduct(a, float.Parse(b), c));
			}
		}
	}
}
#endif
