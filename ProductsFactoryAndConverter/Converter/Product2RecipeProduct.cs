using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Converter;

class Product2RecipeProduct
{

	public static RecipeProduct Convert(ProductView product)
	{
		return new RecipeProduct(product.GetName(), (byte)product.Count, true);
	}
}
