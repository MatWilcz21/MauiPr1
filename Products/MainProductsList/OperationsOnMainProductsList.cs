using System.Collections.ObjectModel;

namespace MauiApp1.Products.MainProductsList;

public static class OperationsOnMainProductsList
{

	public static void AddProductToList(this ObservableCollection<MainListProduct> list, string productName, float productCount, MainProductsListClass mainProductsListClass)
	{

		productName = productName.ToLower();

		MainListProduct product = (MainListProduct)list.GetProductIfExistInList(productName);

		if (product is null)
		{
			list.Insert(0, new MainListProduct(productName, productCount));
			return;

		}

		if (product.Count == productCount) return;

		product.Count = productCount;
		//product.Unit = productUnit;

		product.IsInCart = false;


		mainProductsListClass.SortProductsByStatus(product, product.IsInCart);
	}
}
