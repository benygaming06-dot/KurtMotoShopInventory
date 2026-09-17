using SQLite;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory.Data;

public class DatabaseService
{
    private SQLiteAsyncConnection? _db;
    private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "kurt_motoshop.db3");

    private async Task Init()
    {
        if (_db != null) return;
        _db = new SQLiteAsyncConnection(_dbPath);
        await _db.CreateTableAsync<Product>();
        await _db.CreateTableAsync<ServiceItem>();

        // Seed a few sample records only when the database is brand new.
        if (await _db.Table<Product>().CountAsync() == 0 &&
            await _db.Table<ServiceItem>().CountAsync() == 0)
        {
            await _db.InsertAllAsync(new[]
            {
                new Product { Brand="Honda", Model="Click V3", Name="Engine Oil", Price=280, Stocks=10 },
                new Product { Brand="Yamaha", Model="Mio", Name="Spark Plug", Price=120, Stocks=8 }
            });
            await _db.InsertAllAsync(new[]
            {
                new ServiceItem { LaborName="Change Engine Oil", Price=100 },
                new ServiceItem { LaborName="CVT Cleaning", Price=350 }
            });
        }
    }

    public async Task<List<Product>> GetProducts(string search = "")
    {
        await Init();
        var list = await _db!.Table<Product>().ToListAsync();
        if (string.IsNullOrWhiteSpace(search)) return list.OrderBy(p => p.Name).ToList();
        search = search.Trim().ToLowerInvariant();
        return list.Where(p =>
            p.Name.ToLowerInvariant().Contains(search) ||
            p.Brand.ToLowerInvariant().Contains(search) ||
            p.Model.ToLowerInvariant().Contains(search)).OrderBy(p => p.Name).ToList();
    }

    public async Task<List<ServiceItem>> GetServices(string search = "")
    {
        await Init();
        var list = await _db!.Table<ServiceItem>().ToListAsync();
        if (string.IsNullOrWhiteSpace(search)) return list.OrderBy(s => s.LaborName).ToList();
        search = search.Trim().ToLowerInvariant();
        return list.Where(s => s.LaborName.ToLowerInvariant().Contains(search))
                   .OrderBy(s => s.LaborName).ToList();
    }

    public async Task<int> SaveProduct(Product item)
    {
        await Init();
        return item.Id == 0 ? await _db!.InsertAsync(item) : await _db!.UpdateAsync(item);
    }

    public async Task<int> SaveService(ServiceItem item)
    {
        await Init();
        return item.Id == 0 ? await _db!.InsertAsync(item) : await _db!.UpdateAsync(item);
    }

    public async Task<Product?> GetProduct(int id)
    {
        await Init();
        return await _db!.Table<Product>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteProduct(int id)
    {
        await Init();
        return await _db!.DeleteAsync<Product>(id) > 0;
    }

    public async Task<bool> DeleteService(int id)
    {
        await Init();
        return await _db!.DeleteAsync<ServiceItem>(id) > 0;
    }

    public async Task<bool> DecreaseStock(int productId, int quantity)
    {
        await Init();
        var p = await GetProduct(productId);
        if (p == null || p.Stocks < quantity) return false;
        p.Stocks -= quantity;
        await _db!.UpdateAsync(p);
        return true;
    }
}
