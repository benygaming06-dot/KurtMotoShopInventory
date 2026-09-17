using SQLite;

namespace KurtDhylanMotoShopInventory.Models;

public class ServiceItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string LaborName { get; set; } = "";
    public decimal Price { get; set; }
}
