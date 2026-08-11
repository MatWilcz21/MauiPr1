namespace MauiApp1.ViewerModels.Products;

public partial class DefinedProductView : ProductView
{

	public DefinedProductView() { }

	public DefinedProductView(string _productName)
	{
		ProductDefinition = FindProductDefinition(_productName);

		DisplayName = GetName().Capitalize();
		Count = 1;
		ShowAddToSavedProductsButton = false;

		Unit = ProductDefinition.Unit;

		IsInCart = false;
	}

	public DefinedProductView(string _productName, int _productCount, bool _isItemInCart)
	{
		ProductDefinition = FindProductDefinition(_productName);
		DisplayName = GetName().Capitalize();

		Count = _productCount;
		ShowAddToSavedProductsButton = false;

		Unit = ProductDefinition.Unit;

		IsInCart = _isItemInCart;
	}

	public ProductDefinition ProductDefinition { get; protected set; }

	public override ProductView GetInRecipeEditProductX(ProductView productView)
	{
		throw new NotImplementedException();
		return productView;

		//DefinedProductView product = (DefinedProductView)productView;

		//return new DefinedInRecipeEditProduct(product.ProductDefinition, product.Count, true);
	}

	public override string GetName()
	{
		return ProductDefinition.Name;
	}

	protected ProductDefinition FindProductDefinition(string productName)
	{
		return SavedProducts.Products.First(p => p.Name == productName);
	}
}
