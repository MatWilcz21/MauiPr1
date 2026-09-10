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

			ReceiveSMS.ProcessReceivedSmsMessage(message);
			
		}
	}
}
#endif
