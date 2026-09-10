using MauiApp1.Products;

namespace MauiApp1;

static class CommonListOperations
{

	public static BaseProduct GetProductIfExistInList(this IEnumerable<BaseProduct> enumerable, string productName)
	{
		return enumerable.FirstOrDefault(p => p.Name == productName)!;
	}

}
