using MauiApp1.Products;
using MauiApp1.Products.MainProductsList;
using MauiApp1.ViewerModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

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

				string[] values = Regex.Matches(dataToUpdateList[i], @"(?:""([^""]*)""|(\S+))")
				  .Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value)
				  .ToArray();

				if (values.Length != 3) continue;

				if (!float.TryParse(values[1], out float productCount))
					productCount = 1;

				string productName = values[0].ToLower();
				string productUnit = values[2];

				mainLists!.AddProductToList(productName, productCount, mainProductsListClass);

			}

			mainProductsListClass.SaveList();
		}
	}

}
