using SphoneApp.Exceptions;
using SphoneApp.Models;
using SphoneApp.Utils;
using SphoneApp.Interfaces;

namespace SphoneApp.Managers;

/// <summary>
/// Event arguments for when a dial is requested from contacts
/// </summary>
public class DialRequestedEventArgs : EventArgs
{
    public string PhoneNumber { get; }
    public string ContactName { get; }

    public DialRequestedEventArgs(string phoneNumber, string contactName)
    {
        PhoneNumber = phoneNumber;
        ContactName = contactName;
    }
}

/// <summary>
/// ContactsManager: Manages ALL contact operations
/// 
/// SOLID PRINCIPLES:
/// 
/// [S] Single Responsibility Principle:
///     - ONLY responsible for contact management (add, search, display, export, call)
///     - Does NOT handle actual dialing (raises event for PhoneApplication to handle)
///     - Does NOT store call history (handled by CallHistoryManager via events)
///     - Handles contacts sub-menu UI (single responsibility for contact-related UI)
/// 
/// [O] Open/Closed Principle:
///     - Open for extension: Can add new contact operations (delete, edit, groups) without changing existing code
///     - Closed for modification: Core contact logic remains unchanged when extending
///     - Can add new search filters or export formats by extending, not modifying
/// 
/// [L] Liskov Substitution Principle:
///     - Implements IContactsManager interface
///     - Any implementation can replace this (e.g., CloudContactsManager for syncing with cloud)
/// 
/// [I] Interface Segregation Principle:
///     - IContactsManager is focused: only contact-related methods
///     - Clients only depend on what they need
///     - No dependency on DialManager interface - fully decoupled
/// 
/// [D] Dependency Inversion Principle:
///     - Depends ONLY on IRepository<Contact> abstraction (not concrete JsonRepository)
///     - Does NOT depend on IDialManager - uses events instead (Observer Pattern)
///     - Provides contact lookup as a function via public method
///     - Repository can be swapped without changing this manager
///     - FIXED: Removed direct dependency on DialManager!
///     
/// DESIGN PATTERNS:
///     - Repository Pattern: For data access abstraction
///     - Dependency Injection: Dependencies injected via constructor
///     - Observer Pattern: Raises DialRequested event when user wants to dial
/// </summary>
public class ContactsManager : IContactsManager
{
    private readonly IRepository<Contact> _repository;

    // Event for when a dial is requested (Observer Pattern - NO dependency on DialManager)
    public event EventHandler<DialRequestedEventArgs>? DialRequested;

    public ContactsManager(IRepository<Contact> repository)
    {
        _repository = repository;
    }
    
    // Find contact by phone number (searches across all phone numbers)
    public Contact? FindContactByNumber(string phoneNumber)
    {
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        return _repository.GetAll().FirstOrDefault(c => c.HasPhoneNumber(cleanNumber));
    }
    
    // Get all contacts
    public List<Contact> GetAllContacts()
    {
        return _repository.GetAll();
    }

    // Main contacts menu - handles all contact-related UI and operations
    public async Task ShowContactsMenuAsync()
    {
        while (true)
        {
            Console.WriteLine(ConstantStrings.CONTACTS_MENU_TITLE);
            Console.WriteLine(ConstantStrings.ADD_CONTACT_OPTION);
            Console.WriteLine(ConstantStrings.SEARCH_CONTACT_OPTION);
            Console.WriteLine(ConstantStrings.EXPORT_CONTACTS_OPTION);
            Console.WriteLine(ConstantStrings.CALL_CONTACT_OPTION);
            Console.WriteLine(ConstantStrings.BACK_TO_HOME_OPTION);
            Console.Write(ConstantStrings.ENTER_CHOICE);

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case ConstantStrings.CHOICE_ONE:
                        AddContactInteractive();
                        break;
                    case ConstantStrings.CHOICE_TWO:
                        SearchContactInteractive();
                        break;
                    case ConstantStrings.CHOICE_THREE:
                        await ExportContactsAsync();
                        break;
                    case ConstantStrings.CHOICE_FOUR:
                        CallContactInteractive();
                        break;
                    case ConstantStrings.CHOICE_FIVE:
                        return;
                    default:
                        Console.WriteLine(ConstantStrings.INVALID_CONTACT_OPTION);
                        continue;
                }

                Console.WriteLine(ConstantStrings.PRESS_ANY_KEY_TO_RETURN);
                Console.ReadKey(true);
            }
            catch (NotYetImplementedException ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.ERROR_PREFIX, ex.Message));
                Console.WriteLine(ConstantStrings.PRESS_ANY_KEY_TO_RETURN);
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.ERROR_PREFIX, ex.Message));
                Console.WriteLine(ConstantStrings.PRESS_ANY_KEY_TO_RETURN);
                Console.ReadKey(true);
            }
        }
    }

    // Add contact with user interaction
    private void AddContactInteractive()
    {
        Console.WriteLine(ConstantStrings.ADD_CONTACT_TITLE);
        Console.Write(ConstantStrings.ENTER_CONTACT_NAME);
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(ConstantStrings.NAME_CANNOT_BE_EMPTY);
            return;
        }

        // Check if contact with this name already exists
        var existingContact = _repository.GetAll()
            .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (existingContact != null)
        {
            // Add number to existing contact
            Console.WriteLine($"\nContact '{name}' already exists with {existingContact.PhoneNumbers.Count} number(s).");
            Console.Write("Do you want to add another number to this contact? (y/n): ");
            string? addMore = Console.ReadLine();
            
            if (addMore?.ToLower() != "y")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            // Add phone number to existing contact
            AddPhoneNumberToContact(existingContact);
        }
        else
        {
            // Create new contact with one or more phone numbers
            var phoneNumbers = new List<CustomProperty>();
            
            bool addingNumbers = true;
            while (addingNumbers)
            {
                Console.Write($"\nEnter phone number #{phoneNumbers.Count + 1}: ");
                string? number = Console.ReadLine();

                if (!PhoneNumberUtils.IsValidPhoneNumber(number))
                {
                    Console.WriteLine(ConstantStrings.INVALID_PHONE_NUMBER_MESSAGE);
                    continue;
                }

                string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(number!);
                
                // Check if this number already exists in the list
                if (phoneNumbers.Any(p => p.PhoneNo == cleanNumber))
                {
                    Console.WriteLine("This number is already added to this contact!");
                    continue;
                }

                Console.WriteLine(ConstantStrings.SELECT_PHONE_NUMBER_TYPE);
                Console.WriteLine(ConstantStrings.TYPE_HOME);
                Console.WriteLine(ConstantStrings.TYPE_WORK);
                Console.Write(ConstantStrings.ENTER_TYPE_CHOICE);
                string? typeChoice = Console.ReadLine();

                if (typeChoice != ConstantStrings.CHOICE_ONE && typeChoice != ConstantStrings.CHOICE_TWO)
                {
                    Console.WriteLine(ConstantStrings.INVALID_TYPE_CHOICE);
                    continue;
                }
                ContactType type = typeChoice == ConstantStrings.CHOICE_ONE ? ContactType.Home : ContactType.Work;

                phoneNumbers.Add(new CustomProperty(cleanNumber, type));
                Console.WriteLine($"✓ Number added: {cleanNumber} ({type})");

                Console.Write("\nAdd another phone number? (y/n): ");
                string? addAnother = Console.ReadLine();
                addingNumbers = addAnother?.ToLower() == "y";
            }

            if (phoneNumbers.Count == 0)
            {
                Console.WriteLine("No phone numbers added. Contact not created.");
                return;
            }

            Contact contact = new Contact(name, phoneNumbers);
            AddContact(contact);
            Console.WriteLine($"\n✓ Contact '{name}' created successfully with {phoneNumbers.Count} phone number(s)!");
        }
    }

    // Helper method to add phone number to existing contact
    private void AddPhoneNumberToContact(Contact contact)
    {
        Console.Write("Enter phone number: ");
        string? number = Console.ReadLine();

        if (!PhoneNumberUtils.IsValidPhoneNumber(number))
        {
            Console.WriteLine(ConstantStrings.INVALID_PHONE_NUMBER_MESSAGE);
            return;
        }

        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(number!);

        // Check if contact already has this number
        if (contact.HasPhoneNumber(cleanNumber))
        {
            Console.WriteLine($"Contact '{contact.Name}' already has this number!");
            return;
        }

        Console.WriteLine(ConstantStrings.SELECT_PHONE_NUMBER_TYPE);
        Console.WriteLine(ConstantStrings.TYPE_HOME);
        Console.WriteLine(ConstantStrings.TYPE_WORK);
        Console.Write(ConstantStrings.ENTER_TYPE_CHOICE);
        string? typeChoice = Console.ReadLine();

        if (typeChoice != ConstantStrings.CHOICE_ONE && typeChoice != ConstantStrings.CHOICE_TWO)
        {
            Console.WriteLine(ConstantStrings.INVALID_TYPE_CHOICE);
            return;
        }
        ContactType type = typeChoice == ConstantStrings.CHOICE_ONE ? ContactType.Home : ContactType.Work;

        contact.AddPhoneNumber(cleanNumber, type);
        _repository.SaveChanges();
        Console.WriteLine($"✓ Phone number {cleanNumber} ({type}) added to contact '{contact.Name}'");
    }

    // Search contact with user interaction
    private void SearchContactInteractive()
    {
        Console.WriteLine(ConstantStrings.SEARCH_CONTACT_TITLE);
        Console.WriteLine(ConstantStrings.SEARCH_BY);
        Console.WriteLine(ConstantStrings.SEARCH_BY_NAME);
        Console.WriteLine(ConstantStrings.SEARCH_BY_NUMBER);
        Console.Write(ConstantStrings.ENTER_CHOICE);
        string? choice = Console.ReadLine();

        Console.Write(ConstantStrings.ENTER_SEARCH_TERM);
        string? searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine(ConstantStrings.SEARCH_TERM_CANNOT_BE_EMPTY);
            return;
        }

        List<Contact> results;

        if (choice == ConstantStrings.CHOICE_ONE)
        {
            results = SearchByName(searchTerm);
        }
        else if (choice == ConstantStrings.CHOICE_TWO)
        {
            results = SearchByNumber(searchTerm);
        }
        else
        {
            Console.WriteLine(ConstantStrings.INVALID_SEARCH_CHOICE);
            return;
        }

        if (results.Count == 0)
        {
            Console.WriteLine(string.Format(ConstantStrings.NO_CONTACTS_FOUND, searchTerm));
        }
        else
        {
            Console.WriteLine(string.Format(ConstantStrings.CONTACTS_FOUND, results.Count));
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine(string.Format(ConstantStrings.CONTACT_NUMBER, i + 1));
                Console.WriteLine(results[i]);
            }
        }
    }

    // Call contact with user interaction
    private void CallContactInteractive()
    {
        Console.WriteLine(ConstantStrings.CALL_CONTACT_TITLE);
        
        var contacts = _repository.GetAll();

        if (contacts.Count == 0)
        {
            Console.WriteLine(ConstantStrings.NO_CONTACTS_AVAILABLE);
            return;
        }

        Console.WriteLine(ConstantStrings.AVAILABLE_CONTACTS);
        for (int i = 0; i < contacts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {contacts[i].Name} ({contacts[i].PhoneNumbers.Count} number(s))");
        }

        Console.Write(ConstantStrings.ENTER_CONTACT_NUMBER_TO_CALL);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index > 0 && index <= contacts.Count)
        {
            Contact selectedContact = contacts[index - 1];
            
            // If contact has multiple numbers, let user choose
            if (selectedContact.PhoneNumbers.Count > 1)
            {
                Console.WriteLine($"\n{selectedContact.Name} has multiple numbers:");
                for (int i = 0; i < selectedContact.PhoneNumbers.Count; i++)
                {
                    var phoneNumber = selectedContact.PhoneNumbers[i];
                    Console.WriteLine($"{i + 1}. {phoneNumber.PhoneNo} ({phoneNumber.Type})");
                }
                
                Console.Write("Select number to call: ");
                string? numberChoice = Console.ReadLine();
                
                if (int.TryParse(numberChoice, out int phoneIndex) && 
                    phoneIndex > 0 && phoneIndex <= selectedContact.PhoneNumbers.Count)
                {
                    var chosenNumber = selectedContact.PhoneNumbers[phoneIndex - 1];
                    // Raise event to request dial (Observer Pattern - NO direct dependency on DialManager)
                    OnDialRequested(new DialRequestedEventArgs(chosenNumber.PhoneNo, selectedContact.Name));
                }
                else
                {
                    Console.WriteLine("Invalid number selection!");
                }
            }
            else if (selectedContact.PhoneNumbers.Count == 1)
            {
                // Only one number, raise event to dial it
                OnDialRequested(new DialRequestedEventArgs(selectedContact.PhoneNumbers[0].PhoneNo, selectedContact.Name));
            }
            else
            {
                Console.WriteLine($"Contact '{selectedContact.Name}' has no phone numbers!");
            }
        }
        else
        {
            Console.WriteLine(ConstantStrings.INVALID_CONTACT_NUMBER);
        }
    }
    
    // Async export contacts to file
    private async Task ExportContactsAsync()
    {
        try
        {
            var contacts = _repository.GetAll();
            
            if (contacts.Count == 0)
            {
                Console.WriteLine(ConstantStrings.NO_CONTACTS_AVAILABLE);
                return;
            }
            
            Console.WriteLine(ConstantStrings.EXPORT_STARTING);
            
            // Build the export content
            var content = new System.Text.StringBuilder();
            
            for (int i = 0; i < contacts.Count; i++)
            {
                var contact = contacts[i];
                content.AppendLine($"Contact {(char)('A' + i)}) {contact.Name}");
                
                // Export all phone numbers
                if (contact.PhoneNumbers.Count > 0)
                {
                    foreach (var phone in contact.PhoneNumbers)
                    {
                        content.AppendLine($"  Number: {phone.PhoneNo} ({phone.Type})");
                    }
                }
                else
                {
                    content.AppendLine("  No phone numbers");
                }
                
                content.AppendLine(); // Empty line between contacts
            }
            
            // Write to file asynchronously
            await File.WriteAllTextAsync(ConstantStrings.EXPORT_FILE_PATH, content.ToString());
            
            // Report success
            Console.WriteLine(string.Format(ConstantStrings.EXPORT_SUCCESS, contacts.Count, ConstantStrings.EXPORT_FILE_PATH));
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(ConstantStrings.EXPORT_FAILED, ex.Message));
        }
    }

    // Core contact operations
    private void AddContact(Contact contact)
    {
        _repository.Add(contact);
        _repository.SaveChanges();
    }

    private List<Contact> SearchByName(string searchTerm)
    {
        return _repository.GetAll().Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private List<Contact> SearchByNumber(string searchTerm)
    {
        return _repository.GetAll()
            .Where(c => c.PhoneNumbers.Any(p => p.PhoneNo.Contains(searchTerm)))
            .ToList();
    }

    private bool IsDuplicateContact(string name, string number)
    {
        string cleanedNumber = PhoneNumberUtils.CleanPhoneNumber(number);
        return _repository.GetAll().Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                  c.HasPhoneNumber(cleanedNumber));
    }

    // Protected method to raise the DialRequested event
    protected virtual void OnDialRequested(DialRequestedEventArgs e)
    {
        DialRequested?.Invoke(this, e);
    }
}

