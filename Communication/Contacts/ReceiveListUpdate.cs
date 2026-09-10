using MauiApp1.Products;
using MauiApp1.ViewerModels;
using System.Collections;
using System.Collections.ObjectModel;

namespace MauiApp1.Communication.Contacts;

public static class ReceiveListUpdate
{

	static Dictionary<string, IEnumerable> listsToUpdate = new();

	static public MainViewModel MainViewModel { get; set; }

	public static void UpdateList(string listName, List<string> dataToUpdateList)
	{
		IUpdateList up = listName switch
		{
			"MainList" => new UpdateMainList(MainViewModel),
			_ => throw new NotImplementedException()
		};

		up.Update(dataToUpdateList, listsToUpdate[listName]);
	}

	public static void AddThisList(IEnumerable list)
	{
		listsToUpdate.Add("MainList", list);
	}

	public static void ST(MainViewModel mv)
	{
		MainViewModel = mv;
	}

}

interface IUpdateList
{

	public void Update(List<string> dataToUpdateList, IEnumerable targetList);

}


class UpdateMainList : IUpdateList
{

	public UpdateMainList(MainViewModel mv)
	{
		mainProductsListClass = mv.MainProductsListClass;
	}

	static MainProductsListClass mainProductsListClass;

	public void Update(List<string> dataToUpdateList, IEnumerable targetList)
	{

		ObservableCollection<MainListProduct> mainLists = targetList as ObservableCollection<MainListProduct>;

		Merge();


		mainProductsListClass.SaveList();

		void Merge()
		{

			for (int i = 0; i < dataToUpdateList.Count; i++)
			{
				string[] values = dataToUpdateList[i].Replace("\"", "").Split(' ');

				string productName = values[0];
				float productCount = float.Parse(values[1]);
				string productUnit = values[2];

				MainListProduct product = mainLists!.FirstOrDefault(p => p.Name.ToLower() == productName.ToLower())!;

				if (product is null)
				{
					mainLists!.Add(new Products.MainListProduct(productName, productCount, productUnit));
					continue;
				}

				if (product.Count == productCount) continue;

				product.Count = productCount;
				product.Unit = productUnit;

				product.IsInCart = false;

				if (mainProductsListClass is null) continue;
				mainProductsListClass.SortProductsByStatus(product, product.IsInCart);
			}
		}

		void Replace()
		{
			mainLists.Clear();
			for (int i = 0; i < dataToUpdateList.Count; i++)
			{
				string[] values = dataToUpdateList[i].Replace("\"", "").Split(' ');

				string productName = values[0];
				string productCount = values[1];
				string productUnit = values[2];

				mainLists.Add(new Products.MainListProduct(productName, float.Parse(productCount), productUnit));
			}
		}
	}


}
