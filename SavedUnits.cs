namespace MauiApp1;

public record Unit(string Name);

public static class SavedUnits
{
    static List<Unit> units = null!;
    public static List<Unit> Units
    {
        get
        {
            if (units is null)
                LoadUnitsDefinitions();

            return units!;
        }
        private set { units = value; }
    }

    static void LoadUnitsDefinitions()
    {
        Units = new();

        Units.Add(new Unit("Szt"));
        Units.Add(new Unit("L"));
    }

    public static int UnitNameToID(string us)
    {

        Unit un = Units.FirstOrDefault(u => u.Name == us);

        return units.IndexOf(un);
    }


}
