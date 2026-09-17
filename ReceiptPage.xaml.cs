using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory;

public partial class ReceiptPage : ContentPage
{
    public ReceiptPage(List<CartItem> items, decimal total, decimal payment, decimal change)
    {
        InitializeComponent();
        DateLabel.Text = DateTime.Now.ToString("MMM dd, yyyy • hh:mm tt");
        TotalLabel.Text = $"₱{total:N2}";
        PaymentLabel.Text = $"₱{payment:N2}";
        ChangeLabel.Text = $"₱{change:N2}";

        foreach (var item in items)
        {
            var row = new Grid { ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto)
            }};
            row.Add(new Label { Text = $"{item.Name} x{item.Quantity}", TextColor = Colors.White }, 0, 0);
            row.Add(new Label { Text = $"₱{item.Amount:N2}", TextColor = Color.FromArgb("#F20D18"), FontAttributes = FontAttributes.Bold }, 1, 0);
            ItemsLayout.Add(row);
        }
    }

    private async void DoneClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
