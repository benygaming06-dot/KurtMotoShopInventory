using KurtDhylanMotoShopInventory.Data;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly List<CartItem> _cart = new();

    public MainPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshLists();
    }

    private async Task RefreshLists()
    {
        var products = await _db.GetProducts(ProductSearch.Text);
        var services = await _db.GetServices(ServiceSearch.Text);
        ProductsView.ItemsSource = products;
        ServicesView.ItemsSource = services;
        ProductCountLabel.Text = products.Count.ToString();
        ServiceCountLabel.Text = services.Count.ToString();
        CartCountLabel.Text = _cart.Sum(x => x.Quantity).ToString();
        CartTotalLabel.Text = $"₱{_cart.Sum(x => x.Amount):N2}";
    }

    private async void ProductSearchChanged(object sender, TextChangedEventArgs e) => await RefreshLists();
    private async void ServiceSearchChanged(object sender, TextChangedEventArgs e) => await RefreshLists();

    private void AddProductToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is Product p)
        {
            if (p.Stocks <= 0) { DisplayAlert("Out of Stock", $"{p.Name} has no available stock.", "OK"); return; }
            var existing = _cart.FirstOrDefault(x => !x.IsService && x.ProductId == p.Id);
            if (existing != null)
            {
                if (existing.Quantity >= p.Stocks) { DisplayAlert("Stock Limit", "You cannot add more than the available stock.", "OK"); return; }
                existing.Quantity++;
            }
            else _cart.Add(new CartItem { ProductId=p.Id, Name=p.Name, Price=p.Price });
            _ = RefreshLists();
        }
    }

    private void AddServiceToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is ServiceItem s)
        {
            var existing = _cart.FirstOrDefault(x => x.IsService && x.ServiceId == s.Id);
            if (existing != null) existing.Quantity++;
            else _cart.Add(new CartItem { ServiceId=s.Id, IsService=true, Name=s.LaborName, Price=s.Price });
            _ = RefreshLists();
        }
    }

    private async void OpenCartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CartPage(_db, _cart, RefreshLists));
    }

    private async void AddProductClicked(object sender, EventArgs e)
    {
        var page = new ProductEditorPage(_db, null);
        await Navigation.PushAsync(page);
    }

    private async void AddServiceClicked(object sender, EventArgs e)
    {
        var page = new ServiceEditorPage(_db, null);
        await Navigation.PushAsync(page);
    }

    private async void ProductMenuClicked(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is Product p)
        {
            var action = await DisplayActionSheet(p.Name, "Cancel", "Delete", "Edit");
            if (action == "Edit") await Navigation.PushAsync(new ProductEditorPage(_db, p));
            if (action == "Delete")
            {
                if (await DisplayAlert("Delete Product", $"Delete {p.Name}?", "Delete", "Cancel"))
                {
                    await _db.DeleteProduct(p.Id);
                    await RefreshLists();
                }
            }
        }
    }
}
