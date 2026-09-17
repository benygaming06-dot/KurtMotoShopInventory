using KurtDhylanMotoShopInventory.Data;

namespace KurtDhylanMotoShopInventory;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>();

        builder.Services.AddSingleton<DatabaseService>();

        return builder.Build();
    }
}
