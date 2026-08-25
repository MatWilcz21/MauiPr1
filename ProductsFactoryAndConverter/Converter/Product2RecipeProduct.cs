using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Converter;

class Product2RecipeProduct
{

	public static PackedRecipeProduct Convert(ProductView product)
	{
		return new PackedRecipeProduct(product.GetName(), (byte)product.Count, true);
	}
}
