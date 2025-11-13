using SphoneApp.Utils;
using SphoneApp.Models;
using SphoneApp.Interfaces;

namespace SphoneApp.Managers;

/// <summary>
/// Event arguments for call made event
/// Contains all necessary data about a completed call
/// </summary>
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

/// <summary>
/// DialManager: Handles ALL dialing operations
/// 
/// SOLID PRINCIPLES:
/// 
/// [S] Single Responsibility Principle:
///     - ONLY responsible for dialing phone numbers
///     - Does NOT store call history (delegates to CallHistoryManager via events)
///     - Does NOT manage contacts (receives contact lookup as a dependency)
///     - Does NOT validate beyond basic phone number format
/// 
/// [O] Open/Closed Principle:
///     - Open for extension: Can add new dial types (video call, conference call) by extending
///     - Uses events to notify subscribers without knowing who they are
///     - ContactLookup function can be replaced with different implementations
/// 
/// [L] Liskov Substitution Principle:
///     - Implements IDialManager interface
///     - Any class implementing IDialManager can replace this without breaking code
/// 
/// [I] Interface Segregation Principle:
///     - IDialManager is focused: only dial-related methods
///     - Doesn't force clients to implement unrelated methods
/// 
/// [D] Dependency Inversion Principle:
///     - Depends on abstraction: Func<string, Contact?> for contact lookup (not ContactsManager directly)
///     - Uses events (Observer Pattern) to notify history manager (loose coupling)
///     - High-level code doesn't depend on DialManager, depends on IDialManager interface
/// </summary>
public class DialManager : IDialManager
{
    // Event delegate for when a call is made
    public event EventHandler<CallMadeEventArgs>? CallMade;
    
    // Function to check if a number belongs to a contact
    private Func<string, Contact?>? _contactLookup;

    // Register contact lookup function
    public void RegisterContactLookup(Func<string, Contact?> contactLookup)
    {
        _contactLookup = contactLookup;
    }

    // Dial from user input
    public void Dial()
    {
        Console.WriteLine(ConstantStrings.DIAL_NUMBER_TITLE);
        Console.Write(ConstantStrings.ENTER_PHONE_NUMBER);
        string? number = Console.ReadLine();

        DialNumber(number!);
    }

    // Dial a specific number (used by contacts manager)
    public void DialNumber(string phoneNumber, string? knownContactName = null)
    {
        // Validate the phone number
        PhoneNumberUtils.ValidatePhoneNumber(phoneNumber);
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        
        // Check if this number belongs to a contact (if not already known)
        string? contactName = knownContactName;
        if (contactName == null && _contactLookup != null)
        {
            var contact = _contactLookup(cleanNumber);
            contactName = contact?.Name;
            
            // If contact found, show the contact name
            if (contact != null)
            {
                Console.WriteLine(string.Format(ConstantStrings.CALLING_CONTACT, contact.Name));
            }
        }
        
        // Simulate dialing
        Console.WriteLine(string.Format(ConstantStrings.DIALING_MESSAGE, cleanNumber));
        Console.WriteLine(ConstantStrings.CALL_CONNECTED);
        Console.WriteLine(ConstantStrings.CALL_ENDED);

        // Raise event to notify subscribers (e.g., CallHistoryManager)
        OnCallMade(new CallMadeEventArgs(cleanNumber, contactName, DateTime.Now));
    }

    // Protected method to raise the event
    protected virtual void OnCallMade(CallMadeEventArgs e)
    {
        CallMade?.Invoke(this, e);
    }
}

