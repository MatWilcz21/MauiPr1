using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Products;

public partial class MainListProduct : BaseProduct
{

	public MainListProduct() { }

	public MainListProduct(string _name, float _count)
	{
		Name = _name;
		Count = _count;
		Unit = "Szt";
	}

	public MainListProduct(string _name, float _count, string _unit)
	{
		Name = _name;
		Count = _count;
		Unit = _unit;
	}

	[ObservableProperty] public partial bool IsInCart { get; set; }


}
