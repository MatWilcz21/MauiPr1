namespace MauiApp1;


public class StartableViewModel
{
	public static async Task ChangePage(string pageName, Dictionary<string, object> parameters)
	{
		await Shell.Current.GoToAsync(pageName, parameters);

		Page page = Shell.Current.CurrentPage;

		IStartableViewModel startableViewModel = page.BindingContext as IStartableViewModel ?? throw new Exception($"{pageName} does not derive from {nameof(IStartableViewModel)}");

		await startableViewModel.Start();
	}
}

interface IStartableViewModel
{
	Task Start();
}
