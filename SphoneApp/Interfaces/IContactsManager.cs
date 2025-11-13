using SphoneApp.Models;
using SphoneApp.Managers;

namespace SphoneApp.Interfaces;

/// <summary>
/// Interface for contacts management
/// Follows Interface Segregation Principle - only contact-related operations
/// </summary>
public interface IContactsManager
{
    /// <summary>
    /// Event raised when a dial is requested from contacts
    /// Uses Observer Pattern to decouple from DialManager
    /// </summary>
    event EventHandler<DialRequestedEventArgs>? DialRequested;

    /// <summary>
    /// Show the contacts menu asynchronously
    /// </summary>
    Task ShowContactsMenuAsync();
    
    /// <summary>
    /// Find a contact by phone number
    /// </summary>
    Contact? FindContactByNumber(string phoneNumber);
    
    /// <summary>
    /// Get all contacts
    /// </summary>
    List<Contact> GetAllContacts();
}

