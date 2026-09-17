using KurtDhylanMotoShopInventory.Data;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory;

public partial class ServiceEditorPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly ServiceItem? _existing;

    public ServiceEditorPage(DatabaseService db, ServiceItem? existing)
    {
        InitializeComponent();
        _db = db; _existing = existing;
        if (existing != null)
        {
            LaborEntry.Text = existing.LaborName;
            PriceEntry.Text = existing.Price.ToString();
            Title = "Edit Service";
        }
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LaborEntry.Text) || !decimal.TryParse(PriceEntry.Text, out var price))
        {
            await DisplayAlert("Invalid", "Please enter labor name and a valid price.", "OK");
            return;
        }
        var s = _existing ?? new ServiceItem();
        s.LaborName = LaborEntry.Text.Trim();
        s.Price = price;
        await _db.SaveService(s);
        await Navigation.PopAsync();
    }
}
