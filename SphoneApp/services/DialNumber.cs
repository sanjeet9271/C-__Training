using System;
using SphoneApp.Exceptions;
using SphoneApp.Utils;
using SphoneApp.Managers;

namespace SphoneApp.Services;

public class DialNumberService
{
    private CallHistoryManager _historyManager;

    public DialNumberService(CallHistoryManager historyManager)
    {
        _historyManager = historyManager;
    }

    public void Dial()
    {
        Console.WriteLine("\n--- Dial a Number ---");
        Console.Write("Enter a 10-digit phone number: ");
        string? number = Console.ReadLine();

        DialNumber(number!);
    }

    public void DialNumber(string phoneNumber)
    {
        PhoneNumberUtils.ValidatePhoneNumber(phoneNumber);
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        _historyManager.AddCallToHistory(cleanNumber);

        Console.WriteLine($"\nDialing {cleanNumber}...");
        Console.WriteLine("Call connected!");
        Console.WriteLine("Call ended.");
    }
}