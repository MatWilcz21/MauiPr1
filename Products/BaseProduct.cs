using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Products;

public abstract partial class BaseProduct : ObservableObject
{

	[ObservableProperty] public partial float Count { get; set; }

	private string name = "";

	public string Name
	{
		get => name;
		set
		{
			if (SetProperty(ref name, value))
				OnPropertyChanged(nameof(DisplayName));
		}
	}

	public string DisplayName => Name.Capitalize();
	[ObservableProperty] public partial Unit Unit { get; set; }


}
public static class BaseProductExtensionMethods
{
	public static void Increment(this BaseProduct product)
	{
		product.Count++;
	}

	public static void Decrement(this BaseProduct product)
	{
		if (product.Count <= 1)
		{
			product.Count = 1;
			return;
		}

		product.Count--;
	}

	public static async Task ChangeName(this BaseProduct product, IEnumerable<BaseProduct> products)
	{

		string? NewName = await Shell.Current.DisplayPromptAsync(
		"Change name",
		"Enter new name:",
		"OK",
		"Cancel",
		product.Name.Capitalize());

		if (string.IsNullOrWhiteSpace(NewName)) return;

		NewName = NewName.GetTrimmedProductName();

		if (products.FirstOrDefault(e => e.Name == NewName) is not null) return;

		product.Name = NewName;
	}

	public static string GetTrimmedProductName(this string s)
	{
		s = s.ToLower();
		s = s.Trim();
		return s;
	}
}
