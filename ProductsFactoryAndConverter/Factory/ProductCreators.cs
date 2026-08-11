using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Factory;

class ProductCreators
{

	public static ProductView CreateFromName(string name)
	{
		if (SavedProducts.Products.FirstOrDefault(p => p.Name == name) is null)
			return new CustomProductView(name);
		else
			return new DefinedProductView(name);
	}

}
