using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Products;

public partial class RecipeProduct : BaseProduct
{

	public RecipeProduct() { }

	public RecipeProduct(string _name)
	{
		Name = _name;
		//DisplayName = _name.Capitalize();

		Count = 1;
		MergeByDefault = true;
	}

	public RecipeProduct(string _name, float _count, bool _mergeByDefault)
	{
		Name = _name;
		//DisplayName = _name.Capitalize();

		Count = _count;
		MergeByDefault = MergeByDefault;
	}

	[ObservableProperty] public partial bool MergeByDefault { get; set; }
}
