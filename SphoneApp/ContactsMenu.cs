using SphoneApp.Interfaces;
using SphoneApp.Managers;
using SphoneApp.Models;
using SphoneApp.Utils;
using SphoneApp.Exceptions;

namespace SphoneApp;

public class ContactsMenu
{
    private readonly IContactsManager _contactsManager;
    private readonly IExportService _exportService;

    public ContactsMenu(IContactsManager contactsManager, IExportService exportService)
    {
        _contactsManager = contactsManager;
        _exportService = exportService;
    }

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
                        await ExportContactsInteractive();
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

        var existingContact = _contactsManager.GetAllContacts()
            .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (existingContact != null)
        {
            Console.WriteLine($"\nContact '{name}' already exists with {existingContact.PhoneNumbers.Count} number(s).");
            Console.Write("Do you want to add another number to this contact? (y/n): ");
            string? addMore = Console.ReadLine();

            if (addMore?.ToLower() != "y")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            AddPhoneNumberToContact(existingContact);
        }
        else
        {
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
            _contactsManager.AddContact(contact);
            Console.WriteLine($"\n✓ Contact '{name}' created successfully with {phoneNumbers.Count} phone number(s)!");
        }
    }

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

        _contactsManager.AddPhoneNumberToContact(contact, cleanNumber, type);
        Console.WriteLine($"✓ Phone number {cleanNumber} ({type}) added to contact '{contact.Name}'");
    }

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
            results = _contactsManager.SearchByName(searchTerm);
        }
        else if (choice == ConstantStrings.CHOICE_TWO)
        {
            results = _contactsManager.SearchByNumber(searchTerm);
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

    private void CallContactInteractive()
    {
        Console.WriteLine(ConstantStrings.CALL_CONTACT_TITLE);

        var contacts = _contactsManager.GetAllContacts();

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
                    _contactsManager.RequestDial(chosenNumber.PhoneNo, selectedContact.Name);
                }
                else
                {
                    Console.WriteLine("Invalid number selection!");
                }
            }
            else if (selectedContact.PhoneNumbers.Count == 1)
            {
                _contactsManager.RequestDial(selectedContact.PhoneNumbers[0].PhoneNo, selectedContact.Name);
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

    private async Task ExportContactsInteractive()
    {
        try
        {
            var contacts = _contactsManager.GetAllContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine(ConstantStrings.NO_CONTACTS_AVAILABLE);
                return;
            }

            Console.WriteLine(ConstantStrings.EXPORT_STARTING);

            await _exportService.ExportContactsToFileAsync(contacts, ConstantStrings.EXPORT_FILE_PATH);

            Console.WriteLine(string.Format(ConstantStrings.EXPORT_SUCCESS, contacts.Count, ConstantStrings.EXPORT_FILE_PATH));
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(ConstantStrings.EXPORT_FAILED, ex.Message));
        }
    }
}

