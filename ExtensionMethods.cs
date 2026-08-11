namespace MauiApp1;

public static class ExtensionMethods
{

	public static string Capitalize(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
			return s;

		return char.ToUpper(s[0]) + s.Substring(1);
	}


}
