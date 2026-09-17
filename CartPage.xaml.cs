using KurtDhylanMotoShopInventory.Data;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory;

public partial class CartPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly List<CartItem> _cart;
    private readonly Func<Task> _refreshHome;
    private decimal Total => _cart.Sum(x => x.Amount);

    public CartPage(DatabaseService db, List<CartItem> cart, Func<Task> refreshHome)
    {
        InitializeComponent();
        _db = db; _cart = cart; _refreshHome = refreshHome;
        CartView.ItemsSource = _cart;
        TotalLabel.Text = $"₱{Total:N2}";
    }

    private async void CheckoutClicked(object sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            await DisplayAlert("Empty Cart", "Add products or services first.", "OK");
            return;
        }

        if (!decimal.TryParse(PaymentEntry.Text, out var payment) || payment < Total)
        {
            await DisplayAlert("Payment", $"Payment must be at least ₱{Total:N2}.", "OK");
            return;
        }

        // Verify and decrease product stocks.
        foreach (var item in _cart.Where(x => !x.IsService))
        {
            var product = await _db.GetProduct(item.ProductId);
            if (product == null || product.Stocks < item.Quantity)
            {
                await DisplayAlert("Stock Changed", $"{item.Name} no longer has enough stock.", "OK");
                return;
            }
        }

        foreach (var item in _cart.Where(x => !x.IsService))
            await _db.DecreaseStock(item.ProductId, item.Quantity);

        var change = payment - Total;
        ChangeLabel.Text = $"₱{change:N2}";

        await Navigation.PushAsync(new ReceiptPage(_cart.ToList(), Total, payment, change));
        _cart.Clear();
        await _refreshHome();
    }
}
