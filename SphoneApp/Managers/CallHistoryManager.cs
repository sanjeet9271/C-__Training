using System.Text.Json;
using SphoneApp.Models;

namespace SphoneApp.Managers;
public class CallHistoryManager
{
    private List<CallHistoryEntry> _callHistory;

    public CallHistoryManager()
    {
        _callHistory = new List<CallHistoryEntry>();
        LoadHistory();
    }

    private void LoadHistory()
    {
        try
        {
            if (File.Exists(Constants.CallHistoryFilePath))
            {
                string json = File.ReadAllText(Constants.CallHistoryFilePath);
                _callHistory = JsonSerializer.Deserialize<List<CallHistoryEntry>>(json) ?? new List<CallHistoryEntry>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not load call history: {ex.Message}");
            _callHistory = new List<CallHistoryEntry>();
        }
    }

    private void SaveHistory()
    {
        try
        {
            string json = JsonSerializer.Serialize(_callHistory, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Constants.CallHistoryFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not save call history: {ex.Message}");
        }
    }

    public void AddCallToHistory(string phoneNumber)
    {
        var entry = new CallHistoryEntry(phoneNumber);
        _callHistory.Insert(0, entry);
        SaveHistory();
    }
    public List<CallHistoryEntry> GetAllHistory()
    {
        return _callHistory;
    }
}

