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

        PhoneNumberUtils.ValidatePhoneNumber(number);

        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(number!);

        _historyManager.AddCallToHistory(cleanNumber);

        Console.WriteLine($"\nDialing {cleanNumber}...");
        Console.WriteLine("Call connected!");
        Console.WriteLine("Call ended.");
    }

    public void DialNumber(string phoneNumber)
    {
        _historyManager.AddCallToHistory(phoneNumber);

        Console.WriteLine($"\nDialing {phoneNumber}...");
        Console.WriteLine("Call connected!");
        Console.WriteLine("Call ended.");
    }
}