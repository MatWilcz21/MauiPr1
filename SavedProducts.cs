namespace MauiApp1;

public record ProductDefinition(string _name, Unit _unit)
{
    public string Name { get; set; } = _name;
    public Unit Unit { get; set; } = _unit;
    public bool HasPreviewImage { get; set; } = false;
}

public static class SavedProducts
{

    static List<ProductDefinition> products = null!;

    public static List<ProductDefinition> Products
    {
        get
        {
            if (products is null)
                Task.Run(() => LoadProductsList()).Wait();

            return products!;
        }
        private set { products = value; }
    }

    static void LoadProductDefinitions()
    {
        products = new();

        products.Add(new ProductDefinition("woda", SavedUnits.Units[1]));

    }

    public static async Task SaveProductsList()
    {
        await JsonHandler.SaveJson(products, nameof(SavedProducts));
    }

    public static async Task LoadProductsList()
    {
        var o = await JsonHandler.LoadJson<List<ProductDefinition>>(nameof(SavedProducts));

        if (o is null)
        {
            LoadProductDefinitions();
            return;
        }

        products = o;
    }

}
