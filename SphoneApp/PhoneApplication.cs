using SphoneApp.Exceptions;
using SphoneApp.Interfaces;
using SphoneApp.Managers;
using SphoneApp.Models;
using SphoneApp.Repositories;
using SphoneApp.Services;
namespace SphoneApp;

public class PhoneApplication
{
    private readonly IDialManager _dialManager;
    private readonly ICallHistoryManager _callHistoryManager;
    private readonly IContactsManager _contactsManager;
    private readonly IExportService _exportService;
    private readonly ContactsMenu _contactsMenu;

    public PhoneApplication()
    {
        var callHistoryRepository = new JsonRepository<CallHistoryEntry<CallHistoryData>>(ConstantStrings.CALL_HISTORY_FILE_PATH);
        var contactsRepository = new JsonRepository<Contact>(ConstantStrings.CONTACT_LIST_FILE_PATH);
        
        _dialManager = new DialManager();
        _callHistoryManager = new CallHistoryManager(callHistoryRepository);
        _contactsManager = new ContactsManager(contactsRepository);
        _exportService = new ExportService();
        _contactsMenu = new ContactsMenu(_contactsManager, _exportService);
        
        _dialManager.CallMade += _callHistoryManager.OnCallMade;
        _contactsManager.DialRequested += OnDialRequested;
        _dialManager.RegisterContactLookup(_contactsManager.FindContactByNumber);
    }

    private void OnDialRequested(object? sender, DialRequestedEventArgs e)
    {
        _dialManager.DialNumber(e.PhoneNumber, e.ContactName);
    }

    public async Task RunAsync()
    {
        ShowWelcomeScreen();
        
        while (true)
        {
            string? choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case ConstantStrings.CHOICE_ONE:
                        _dialManager.Dial();
                        break;
                    case ConstantStrings.CHOICE_TWO:
                        _callHistoryManager.DisplayHistory();
                        break;
                    case ConstantStrings.CHOICE_THREE:
                        await _contactsMenu.ShowContactsMenuAsync();
                        Console.Clear();
                        ShowWelcomeScreen();
                        continue;
                    case ConstantStrings.CHOICE_FOUR:
                        Console.WriteLine(ConstantStrings.THANK_YOU_MESSAGE);
                        return;
                    default:
                        Console.WriteLine(ConstantStrings.INVALID_OPTION);
                        Console.WriteLine(ConstantStrings.PRESS_H_TO_RETURN);
                        if (Console.ReadKey(true).Key == ConsoleKey.H)
                        {
                            Console.Clear();
                            ShowWelcomeScreen();
                        }
                        else
                        {
                            Console.WriteLine(ConstantStrings.THANK_YOU_MESSAGE);
                            return;
                        }
                        continue;
                }
            }
            catch (InvalidPhoneNumberException ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.ERROR_PREFIX, ex.Message));
            }
            catch (NotYetImplementedException ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.ERROR_PREFIX, ex.Message));
            }
            catch (DuplicateContactException ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.ERROR_PREFIX, ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ConstantStrings.UNEXPECTED_ERROR, ex.Message));
            }

            Console.WriteLine(ConstantStrings.PRESS_H_TO_RETURN);
            if (Console.ReadKey(true).Key == ConsoleKey.H)
            {
                Console.Clear();
                ShowWelcomeScreen();
            }
            else
            {
                Console.WriteLine(ConstantStrings.THANK_YOU_MESSAGE);
                return;
            }
        }
    }

    private void ShowWelcomeScreen()
    {
        Console.WriteLine(ConstantStrings.APP_TITLE);
        Console.WriteLine(ConstantStrings.APP_WELCOME);
        Console.WriteLine(ConstantStrings.APP_TITLE);
        Console.WriteLine();
        Console.WriteLine(ConstantStrings.SELECT_OPTION);
        Console.WriteLine(ConstantStrings.DIAL_NUMBER_OPTION);
        Console.WriteLine(ConstantStrings.VIEW_HISTORY_OPTION);
        Console.WriteLine(ConstantStrings.CONTACTS_OPTION);
        Console.WriteLine(ConstantStrings.EXIT_OPTION);
        Console.WriteLine();
        Console.Write(ConstantStrings.ENTER_CHOICE);
    }
}

