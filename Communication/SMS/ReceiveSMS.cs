using MauiApp1.Communication.Contacts;
using MauiApp1.ViewerModels;
using System.Text.RegularExpressions;

namespace MauiApp1.Communication.SMS.Android;

public class ReceiveSMS
{

	public ReceiveSMS(MainViewModel mv)
	{
		viewModel = mv;
	}

	static MainViewModel viewModel;

	public static void ProcessReceivedSmsMessage(string message)
	{

		/*bool answer = Shell.Current.DisplayAlert(
		"repl",
		"Are you sure you want to delete all products?",
		"Yes",
		"No").Result; //TO_DO asyncccc

		if (!answer)
			return;
		*/

		List<string> results = Regex.Matches(message, @"<([^>]*)>").Select(m => m.Groups[1].Value).ToList();

		ReceiveListUpdate.UpdateList("MainList", results);
	}

}
