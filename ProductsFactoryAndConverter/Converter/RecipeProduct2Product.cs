using MauiApp1.Recipes;
using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Converter;

class RecipeProduct2Product
{


	public static ProductView Convert(RecipeProduct recipeProduct)
	{
		if (SavedProducts.Products.FirstOrDefault(p => p.Name == recipeProduct.Name) is null)
			return new CustomProductView(recipeProduct.Name, recipeProduct.Count, false);
		else
			return new DefinedProductView(recipeProduct.Name, recipeProduct.Count, false);
	}

}
