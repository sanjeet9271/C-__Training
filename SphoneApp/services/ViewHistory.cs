using System;
using SphoneApp.Utils;
using SphoneApp.Managers;

namespace SphoneApp.Services;

public class ViewHistoryService
{
    private CallHistoryManager _historyManager;

    public ViewHistoryService(CallHistoryManager historyManager)
    {
        _historyManager = historyManager;
    }

    public void Display()
    {
        Console.WriteLine("\n--- Dialed Numbers History ---");
        
        var history = _historyManager.GetAllHistory();
        
        if (history.Count == 0)
        {
            Console.WriteLine("No dialed numbers yet.");
            return;
        }

        Console.WriteLine($"Total calls: {history.Count}\n");
        
        for (int i = 0; i < history.Count; i++)
        {
            var entry = history[i];
            string formattedNumber = PhoneNumberUtils.FormatPhoneNumber(entry.PhoneNumber);
            string formattedDate = FormatDateTime(entry.CalledAt);
            
            Console.WriteLine($"{i + 1}. {formattedNumber,-15} - {formattedDate}");
        }
    }

    private string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yyyy hh:mm tt");
    }
}