namespace MauiApp1.ViewerModels.Products;

public partial class CustomProductView : ProductView
{

	public string ProductName { get; set; } = null!;

	public CustomProductView() { }

	public CustomProductView(string _productName)
	{
		DisplayName = _productName;
		ProductName = _productName;
		Count = 1;
		ShowAddToSavedProductsButton = true;
		Unit = SavedUnits.Units[0];
	}

	public CustomProductView(string _productName, int _productCount, bool _isItemInCart)
	{
		DisplayName = _productName;
		ProductName = _productName;
		Count = _productCount;
		ShowAddToSavedProductsButton = true;
		Unit = SavedUnits.Units[0];

		IsInCart = _isItemInCart;
	}

	public override string GetName()
	{
		return ProductName;
	}

	public override ProductView GetInRecipeEditProductX(ProductView productView)
	{
		CustomProductView product = (CustomProductView)productView;

		return new CustomInRecipeEditProduct(product.GetName(), product.Count, true);
	}
}
