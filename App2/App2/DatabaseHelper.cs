using App2.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DatabaseHelper
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseHelper(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<ItemModel>().Wait();  // Create table for ItemModel if it doesn't exist
    }

    // Save item to the database
    public Task<int> SaveItemAsync(ItemModel item)
    {
        return _database.InsertAsync(item);
    }

    // Get all items from the database
    public Task<List<ItemModel>> GetItemsAsync()
    {
        return _database.Table<ItemModel>().ToListAsync();
    }

    // Delete item from the database
    public Task<int> DeleteItemAsync(ItemModel item)
    {
        return _database.DeleteAsync(item);
    }

    // Delete all items from the database
    public Task<int> ClearAllItemsAsync()
    {
        return _database.DeleteAllAsync<ItemModel>();
    }
}
