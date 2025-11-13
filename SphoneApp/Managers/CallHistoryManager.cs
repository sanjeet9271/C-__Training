using SphoneApp.Models;
using SphoneApp.Utils;
using SphoneApp.Interfaces;

namespace SphoneApp.Managers;

public class CallHistoryManager : ICallHistoryManager
{
    private readonly IRepository<CallHistoryEntry<CallHistoryData>> _repository;

    public CallHistoryManager(IRepository<CallHistoryEntry<CallHistoryData>> repository)
    {
        _repository = repository;
    }

    // Subscribe to DialManager's CallMade event
    public void SubscribeToDialManager(IDialManager dialManager)
    {
        dialManager.CallMade += OnCallMade;
    }

    // Event handler for when a call is made
    private void OnCallMade(object? sender, CallMadeEventArgs e)
    {
        AddCallToHistory(e.PhoneNumber, e.ContactName, e.CalledAt);
    }

    // Display call history
    public void DisplayHistory()
    {
        Console.WriteLine(ConstantStrings.CALL_HISTORY_TITLE);
        
        var history = _repository.GetAll();
        
        if (history.Count == 0)
        {
            Console.WriteLine(ConstantStrings.NO_DIALED_NUMBERS);
            return;
        }

        Console.WriteLine(string.Format(ConstantStrings.TOTAL_CALLS, history.Count));
        
        for (int i = 0; i < history.Count; i++)
        {
            var entry = history[i];
            var data = entry.Data;
            
            string displayText;
            if (data.HasContact)
            {
                // Show contact name with number
                string formattedNumber = PhoneNumberUtils.FormatPhoneNumber(data.PhoneNumber);
                displayText = $"{data.ContactName,-20} {formattedNumber,-15}";
            }
            else
            {
                // Show just the number
                string formattedNumber = PhoneNumberUtils.FormatPhoneNumber(data.PhoneNumber);
                displayText = $"{formattedNumber,-15}";
            }
            
            string formattedDate = FormatDateTime(entry.CalledAt);
            Console.WriteLine($"{i + 1}. {displayText} - {formattedDate}");
        }
    }

    public List<CallHistoryEntry<CallHistoryData>> GetAllHistory()
    {
        return _repository.GetAll();
    }

    private void AddCallToHistory(string phoneNumber, string? contactName, DateTime calledAt)
    {
        var data = new CallHistoryData(phoneNumber, contactName);
        var entry = new CallHistoryEntry<CallHistoryData>(data, calledAt);
        
        // Insert at beginning for latest-first ordering
        var allHistory = _repository.GetAll();
        allHistory.Insert(0, entry);
        _repository.SaveChanges();
    }

    private string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yyyy hh:mm tt");
    }
}

