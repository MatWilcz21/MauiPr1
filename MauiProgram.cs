using MauiApp1.Pages;
using MauiApp1.ViewerModels;
using Microsoft.Extensions.Logging;

namespace MauiApp1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddTransient<AddNewSavedProductPage>();
		builder.Services.AddTransient<AddNewSavedProductViewModel>();

		builder.Services.AddSingleton<SelectRecipePage>();
		builder.Services.AddSingleton<SelectRecipeViewerModel>();

		builder.Services.AddSingleton<MergeToListPage>();
		builder.Services.AddSingleton<MergeToListViewModel>();

		builder.Services.AddSingleton<EditSelectedRecipePage>();
		builder.Services.AddSingleton<EditSelectedRecipeViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
