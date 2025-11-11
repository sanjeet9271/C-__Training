using SphoneApp.Exceptions;
using SphoneApp.Models;
using SphoneApp.Utils;
using SphoneApp.Managers;
using SphoneApp.Services;

namespace SphoneApp;

public class ContactsMenu
{
    private ContactsManager _contactsManager;
    private DialNumberService _dialService;

    public ContactsMenu(ContactsManager contactsManager, DialNumberService dialService)
    {
        _contactsManager = contactsManager;
        _dialService = dialService;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n--- Contacts Menu ---");
            Console.WriteLine("1. Add a contact");
            Console.WriteLine("2. Search for contacts");
            Console.WriteLine("3. Export all contacts to a file");
            Console.WriteLine("4. Call a contact");
            Console.WriteLine("5. Back to Home");
            Console.Write("\nEnter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddContact();
                        break;
                    case "2":
                        SearchContact();
                        break;
                    case "3":
                        throw new NotYetImplementedException("Export to file feature is not yet supported!");
                    case "4":
                        CallContact();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("\nInvalid option! Please select 1-5.");
                        continue;
                }

                Console.WriteLine("\nPress any key to return to contacts menu...");
                Console.ReadKey(true);
            }
            catch (NotYetImplementedException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine("\nPress any key to return to contacts menu...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine("\nPress any key to return to contacts menu...");
                Console.ReadKey(true);
            }
        }
    }

    private void AddContact()
    {
        Console.WriteLine("\n--- Add Contact ---");
        Console.Write("Enter contact name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty!");
            return;
        }

        Console.Write("Enter phone number (10 digits): ");
        string? number = Console.ReadLine();

        if (!PhoneNumberUtils.IsValidPhoneNumber(number))
        {
            Console.WriteLine("Invalid phone number! Please enter exactly 10 digits.");
            return;
        }

        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(number!);
       
        if (_contactsManager.IsDuplicateContact(name, cleanNumber))
        {
            throw new DuplicateContactException($"A contact with name '{name}' and number '{cleanNumber}' already exists!");
        }

        Console.WriteLine("Select phone number type:");
        Console.WriteLine("1. Home");
        Console.WriteLine("2. Work");
        Console.Write("Enter choice (1 or 2): ");
        string? typeChoice = Console.ReadLine();

        if (typeChoice != "1" && typeChoice != "2")
        {
            Console.WriteLine("Invalid choice! Please enter 1 for Home or 2 for Work.");
            return;
        }
        ContactType type = typeChoice == "1" ? ContactType.Home : ContactType.Work;

        Contact contact = new Contact(name, cleanNumber, type);
        _contactsManager.AddContact(contact);
        Console.WriteLine($"\nContact '{name}' added successfully with number {cleanNumber} ({type})!");
    }

    private void SearchContact()
    {
        Console.WriteLine("\n--- Search Contacts ---");
        Console.WriteLine("Search by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Number");
        Console.Write("Enter choice: ");
        string? choice = Console.ReadLine();

        Console.Write("Enter search term: ");
        string? searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty!");
            return;
        }

        List<Contact> results;

        if (choice == "1")
        {
            results = _contactsManager.SearchByName(searchTerm);
        }
        else if (choice == "2")
        {
            results = _contactsManager.SearchByNumber(searchTerm);
        }
        else
        {
            Console.WriteLine("Invalid choice!");
            return;
        }

        if (results.Count == 0)
        {
            Console.WriteLine($"\nNo contacts found matching '{searchTerm}'.");
        }
        else
        {
            Console.WriteLine($"\nFound {results.Count} contact(s):\n");
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"Contact {i + 1}:");
                Console.WriteLine(results[i]);
            }
        }
    }

    private void CallContact()
    {
        Console.WriteLine("\n--- Call a Contact ---");

        var contacts = _contactsManager.GetAllContacts();

        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts available!");
            return;
        }

        Console.WriteLine("Available contacts:\n");
        for (int i = 0; i < contacts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {contacts[i].Name}");
        }

        Console.Write("\nEnter contact number to call: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index > 0 && index <= contacts.Count)
        {
            Contact selectedContact = contacts[index - 1];

            if (selectedContact.CustomProperties.Count == 1)
            {
                string number = selectedContact.CustomProperties[0].PhoneNo;
                _dialService.DialNumber(number);
            }
            else
            {
                Console.WriteLine($"\nSelect number for {selectedContact.Name}:");
                for (int i = 0; i < selectedContact.CustomProperties.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedContact.CustomProperties[i]}");
                }

                Console.Write("Enter choice: ");
                string? numChoice = Console.ReadLine();

                if (int.TryParse(numChoice, out int numIndex) && numIndex > 0 && numIndex <= selectedContact.CustomProperties.Count)
                {
                    string number = selectedContact.CustomProperties[numIndex - 1].PhoneNo;
                    _dialService.DialNumber(number);
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid contact number!");
        }
    }
}

