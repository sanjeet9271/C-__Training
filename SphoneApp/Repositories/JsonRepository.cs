using System.Text.Json;
using SphoneApp.Interfaces;

namespace SphoneApp.Repositories;


public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string _filePath;
    private List<T> _data;

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
        _data = new List<T>();
        LoadData();
    }

    public List<T> GetAll()
    {
        return _data;
    }

    public void Add(T entity)
    {
        _data.Add(entity);
    }

    public void SaveChanges()
    {
        try
        {
            string json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(ConstantStrings.WARNING_SAVE_CONTACTS, ex.Message));
        }
    }

    public void LoadData()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _data = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not load data from {_filePath}: {ex.Message}");
            _data = new List<T>();
        }
    }
}

