using MauiApp1.Products;
using MauiApp1.Products.MainProductsList;
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

	public static void SendMainViewModel(MainViewModel mv)
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

		void Merge()
		{

			for (int i = dataToUpdateList.Count - 1; i >= 0; i--)
			{
				string[] values = dataToUpdateList[i].Replace("\"", "").Split(' ');

				string productName = values[0].ToLower();
				float productCount = float.Parse(values[1]);
				string productUnit = values[2];

				mainLists!.AddProductToList(productName, productCount, mainProductsListClass);


			}

			mainProductsListClass.SaveList();
		}
	}

}
