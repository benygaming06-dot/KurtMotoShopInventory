using KurtDhylanMotoShopInventory.Data;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory;

public partial class ProductEditorPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly Product? _existing;

    public ProductEditorPage(DatabaseService db, Product? existing)
    {
        InitializeComponent();
        _db = db; _existing = existing;
        if (existing != null)
        {
            BrandEntry.Text = existing.Brand; ModelEntry.Text = existing.Model;
            NameEntry.Text = existing.Name; PriceEntry.Text = existing.Price.ToString();
            StocksEntry.Text = existing.Stocks.ToString();
            Title = "Edit Product";
        }
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        if (!decimal.TryParse(PriceEntry.Text, out var price) ||
            !int.TryParse(StocksEntry.Text, out var stocks) ||
            string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Invalid", "Please enter a product name, valid price and stocks.", "OK");
            return;
        }

        var p = _existing ?? new Product();
        p.Brand = BrandEntry.Text?.Trim() ?? "";
        p.Model = ModelEntry.Text?.Trim() ?? "";
        p.Name = NameEntry.Text.Trim();
        p.Price = price; p.Stocks = stocks;
        await _db.SaveProduct(p);
        await Navigation.PopAsync();
    }
}
