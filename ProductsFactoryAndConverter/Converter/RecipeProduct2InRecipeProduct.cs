using MauiApp1.Recipes;
using MauiApp1.ViewerModels;
using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Converter;

class RecipeProduct2InRecipeProduct
{
	public static ProductView Convert(RecipeProduct recipeProduct)
	{
		if (SavedProducts.Products.FirstOrDefault(p => p.Name == recipeProduct.Name) is null)
		{
			return new CustomInRecipeEditProduct(recipeProduct.Name, recipeProduct.Count, recipeProduct.MergeByDefault);
		}
		else
		{
			return new DefinedInRecipeEditProduct(recipeProduct.Name, recipeProduct.Count, recipeProduct.MergeByDefault);

		}
	}
}
