using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.ViewerModels.Products;

public abstract partial class ProductView : ObservableObject
{
	[ObservableProperty] public partial int Count { get; set; }
	[ObservableProperty] public partial string DisplayName { get; protected set; }
	[ObservableProperty] public partial bool ShowAddToSavedProductsButton { get; set; }
	[ObservableProperty] public partial Unit Unit { get; set; }
	[ObservableProperty] public partial bool IsInCart { get; set; }
	public abstract string GetName();
}
public enum ProductClassEnum
{
	DefinedProductView,
	CustomProductView
}

public record PackedProduct(ProductClassEnum ProductClass, string Name, int Count, bool IsInCart);

public static class ProductViewExtensionMethods
{
	public static void Increment(this ProductView productView)
	{
		productView.Count++;
	}

	public static void Decrement(this ProductView product)
	{
		if (product.Count <= 1)
		{
			product.Count = 1;
			return;
		}

		product.Count--;
	}
}

