using System;
using SphoneApp.Services;
using SphoneApp.Exceptions;
using SphoneApp.Managers;

namespace SphoneApp;

public class Program
{
    private DialNumberService _dialService;
    private ViewHistoryService _historyService;
    private ContactsMenu _contactsMenu;
    
    private CallHistoryManager _historyManager;
    private ContactsManager _contactsManager;

    public Program()
    {
        // Initialize managers first
        _historyManager = new CallHistoryManager();
        _contactsManager = new ContactsManager();
        
        // Initialize services and menu handlers
        _dialService = new DialNumberService(_historyManager);
        _historyService = new ViewHistoryService(_historyManager);
        _contactsMenu = new ContactsMenu(_contactsManager, _dialService);
        
        WelcomeDisplay();
    }

    static void Main()
    {
        Program instance = new Program();
        
        while (true)
        {
            string? choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1":
                        instance.DialNumber();
                        break;
                    case "2":
                        instance.ViewHistory();
                        break;
                    case "3":
                        instance.Contacts();
                        break;
                    case "4":
                        Console.WriteLine("\nThank you for using Sphone App. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option! Please select 1, 2, 3, or 4.");
                        continue;
                }
            }
            catch (InvalidPhoneNumberException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (NotYetImplementedException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (DuplicateContactException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
            }

            Console.WriteLine("\nPress H to return to the home screen...");
            if (Console.ReadKey(true).Key == ConsoleKey.H)
            {
                Console.Clear();
                instance.WelcomeDisplay();
            }
            else
            {
                Console.WriteLine("\nThank you for using Sphone App. Goodbye!");
                return;
            }
        }
    }

    private void WelcomeDisplay()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("    Welcome to the Sphone App!         ");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Dial a Number");
        Console.WriteLine("2. View History of Dialed Numbers");
        Console.WriteLine("3. Contacts");
        Console.WriteLine("4. Exit");
        Console.WriteLine();
        Console.Write("Enter your choice: ");
    }
    private void DialNumber()
    {
        _dialService.Dial();
    }

    private void ViewHistory()
    {
        _historyService.Display();
    }
    private void Contacts()
    {
        _contactsMenu.Show();
    }
}