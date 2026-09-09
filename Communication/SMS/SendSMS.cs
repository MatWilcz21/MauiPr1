using MauiApp1.Communication.SMS.Android;
using MauiApp1.Products;
using MauiApp1.ViewerModels;
using System.Collections.ObjectModel;

namespace MauiApp1.Communication.SMS;

class SendSMS
{
	public SendSMS(MainProductsListClass ml)
	{
		mainProductsListClass = ml;

#if ANDROID
		listDelivery = new AndroidSendListSMS();
#endif

	}

	IProductListDelivery listDelivery;
	MainProductsListClass mainProductsListClass;

	public async Task Send(string phoneNumber, ObservableCollection<MainListProduct> message)
	{

		List<string> ms = new();

		for (int i = 0; i < message.Count; i++)
		{

			MainListProduct mlp = message[i];

			ms.Add($"""<"{mlp.DisplayName}" {mlp.Count} "{mlp.Unit}">\n""");
		}

		string text = string.Join("", ms);

		text = text.Replace(@"\n", Environment.NewLine);

		await listDelivery.Send(phoneNumber, text);
	}


}

public interface IProductListDelivery
{
	public Task Send(string phoneNumber, string message);
	public Task Receive();
}
