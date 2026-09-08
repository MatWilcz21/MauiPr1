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

	public static bool GetTrimmedProductNameIfValidString(out string r, string s)
	{
		r = s;

		if (string.IsNullOrWhiteSpace(s)) return false;

		r = s.ToLower();
		r = r.Trim();
		return true;
	}


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

		if (!BaseProduct.GetTrimmedProductNameIfValidString(out string newTrimmedName, NewName)) return;


		if (products.FirstOrDefault(e => e.Name == newTrimmedName) is not null) return;

		product.Name = newTrimmedName;
	}
}
