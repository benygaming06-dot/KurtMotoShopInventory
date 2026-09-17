namespace KurtDhylanMotoShopInventory;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage(new Data.DatabaseService()))
        {
            BarBackgroundColor = Color.FromArgb("#0B0B0D"),
            BarTextColor = Colors.White
        };
    }
}
