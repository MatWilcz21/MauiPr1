using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Products;

public partial class MergeProduct : BaseProduct
{

	public MergeProduct(string _name, float _oldCount, float _newCount, bool _merge)
	{
		Name = _name;
		OldCount = _oldCount;
		NewCount = _newCount;
		Merge = _merge;
	}

	[ObservableProperty] public partial float OldCount { get; set; }
	[ObservableProperty] public partial float NewCount { get; set; }
	[ObservableProperty] public partial bool Merge { get; set; }

}
