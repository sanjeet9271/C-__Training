using SphoneApp.Utils;
using SphoneApp.Models;
using SphoneApp.Interfaces;

namespace SphoneApp.Managers;
public class CallMadeEventArgs : EventArgs
{
    public string PhoneNumber { get; }
    public string? ContactName { get; }
    public DateTime CalledAt { get; }

    public CallMadeEventArgs(string phoneNumber, string? contactName, DateTime calledAt)
    {
        PhoneNumber = phoneNumber;
        ContactName = contactName;
        CalledAt = calledAt;
    }
}

public class DialManager : IDialManager
{
    public event EventHandler<CallMadeEventArgs>? CallMade;
    
    private Func<string, Contact?>? _contactLookup;

    public void RegisterContactLookup(Func<string, Contact?> contactLookup)
    {
        _contactLookup = contactLookup;
    }

    public void Dial()
    {
        Console.WriteLine(ConstantStrings.DIAL_NUMBER_TITLE);
        Console.Write(ConstantStrings.ENTER_PHONE_NUMBER);
        string? number = Console.ReadLine();

        DialNumber(number!);
    }

    public void DialNumber(string phoneNumber, string? knownContactName = null)
    {
        PhoneNumberUtils.ValidatePhoneNumber(phoneNumber);
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        
        string? contactName = knownContactName;
        if (contactName == null && _contactLookup != null)
        {
            var contact = _contactLookup(cleanNumber);
            contactName = contact?.Name;
            
            if (contact != null)
            {
                Console.WriteLine(string.Format(ConstantStrings.CALLING_CONTACT, contact.Name));
            }
        }
        
        Console.WriteLine(string.Format(ConstantStrings.DIALING_MESSAGE, cleanNumber));
        Console.WriteLine(ConstantStrings.CALL_CONNECTED);
        Console.WriteLine(ConstantStrings.CALL_ENDED);

        OnCallMade(new CallMadeEventArgs(cleanNumber, contactName, DateTime.Now));
    }

    protected virtual void OnCallMade(CallMadeEventArgs e)
    {
        CallMade?.Invoke(this, e);
    }
}

