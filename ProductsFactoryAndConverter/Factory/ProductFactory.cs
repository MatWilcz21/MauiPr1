using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Factory;

public class ProductFactory
{
	public static ProductView CreateFromName(string name)
	{
		return ProductCreators.CreateFromName(name);
	}
}
