using MauiApp1.Recipes;
using MauiApp1.ViewerModels;
using MauiApp1.ViewerModels.Products;

namespace MauiApp1.ProductsFactoryAndConverter.Converter;

class InRecipeProduct2RecipeProduct
{
	public static RecipeProduct Convert(ProductView product)
	{
		if (SavedProducts.Products.FirstOrDefault(p => p.Name == product.GetName()) is null)
		{
			CustomInRecipeEditProduct pr = (CustomInRecipeEditProduct)product;
			return new RecipeProduct(pr.GetName(), (byte)pr.Count, pr.MergeByDefault);
		}
		else
		{
			DefinedInRecipeEditProduct pr = (DefinedInRecipeEditProduct)product;
			return new RecipeProduct(pr.GetName(), (byte)pr.Count, pr.MergeByDefault);

		}
	}

}
