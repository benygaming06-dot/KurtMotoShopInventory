using SQLite;

namespace KurtDhylanMotoShopInventory.Models;

public class Product
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Stocks { get; set; }
    public string ImagePath { get; set; } = "";
}
