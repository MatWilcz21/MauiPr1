using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Products;

public abstract partial class BaseProduct : ObservableObject
{

	[ObservableProperty] public partial float Count { get; set; }

	[ObservableProperty] public partial string DisplayName { get; set; }
	[ObservableProperty] public partial Unit Unit { get; set; }

	public string Name { get; set; } = null!;

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
}
