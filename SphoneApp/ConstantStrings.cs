namespace SphoneApp;

public static class ConstantStrings
{
    // Menu Choice Constants
    public const string CHOICE_ONE = "1";
    public const string CHOICE_TWO = "2";
    public const string CHOICE_THREE = "3";
    public const string CHOICE_FOUR = "4";
    public const string CHOICE_FIVE = "5";
    
    // File Paths
    public const string CALL_HISTORY_FILE_PATH = "call_history_db.json";
    public const string CONTACT_LIST_FILE_PATH = "contact_list.json";
    
    // Main Menu Strings
    public const string APP_TITLE = "========================================";
    public const string APP_WELCOME = "    Welcome to the Sphone App!         ";
    public const string SELECT_OPTION = "Please select an option:";
    public const string DIAL_NUMBER_OPTION = "1. Dial a Number";
    public const string VIEW_HISTORY_OPTION = "2. View History of Dialed Numbers";
    public const string CONTACTS_OPTION = "3. Contacts";
    public const string EXIT_OPTION = "4. Exit";
    public const string ENTER_CHOICE = "Enter your choice: ";
    public const string INVALID_OPTION = "\nInvalid option! Please select 1, 2, 3, or 4.";
    public const string THANK_YOU_MESSAGE = "\nThank you for using Sphone App. Goodbye!";
    public const string PRESS_H_TO_RETURN = "\nPress H to return to the home screen...";
    
    // Dial Number Strings
    public const string DIAL_NUMBER_TITLE = "\n--- Dial a Number ---";
    public const string ENTER_PHONE_NUMBER = "Enter a 10-digit phone number: ";
    public const string CALLING_CONTACT = "\nCalling contact: {0}";
    public const string DIALING_MESSAGE = "\nDialing {0}...";
    public const string CALL_CONNECTED = "Call connected!";
    public const string CALL_ENDED = "Call ended.";
    
    // Call History Strings
    public const string CALL_HISTORY_TITLE = "\n--- Dialed Numbers History ---";
    public const string NO_DIALED_NUMBERS = "No dialed numbers yet.";
    public const string TOTAL_CALLS = "Total calls: {0}\n";
    
    // Contacts Menu Strings
    public const string CONTACTS_MENU_TITLE = "\n--- Contacts Menu ---";
    public const string ADD_CONTACT_OPTION = "1. Add a contact";
    public const string SEARCH_CONTACT_OPTION = "2. Search for contacts";
    public const string EXPORT_CONTACTS_OPTION = "3. Export all contacts to a file";
    public const string CALL_CONTACT_OPTION = "4. Call a contact";
    public const string BACK_TO_HOME_OPTION = "5. Back to Home";
    public const string INVALID_CONTACT_OPTION = "\nInvalid option! Please select 1-5.";
    public const string PRESS_ANY_KEY_TO_RETURN = "\nPress any key to return to contacts menu...";
    
    // Add Contact Strings
    public const string ADD_CONTACT_TITLE = "\n--- Add Contact ---";
    public const string ENTER_CONTACT_NAME = "Enter contact name: ";
    public const string ENTER_PHONE_NUMBER_FOR_CONTACT = "Enter phone number (10 digits): ";
    public const string SELECT_PHONE_NUMBER_TYPE = "Select phone number type:";
    public const string TYPE_HOME = "1. Home";
    public const string TYPE_WORK = "2. Work";
    public const string ENTER_TYPE_CHOICE = "Enter choice (1 or 2): ";
    public const string CONTACT_ADDED_SUCCESS = "\nContact '{0}' added successfully with number {1} ({2})!";
    public const string NAME_CANNOT_BE_EMPTY = "Name cannot be empty!";
    public const string INVALID_PHONE_NUMBER_MESSAGE = "Invalid phone number! Please enter exactly 10 digits.";
    public const string INVALID_TYPE_CHOICE = "Invalid choice! Please enter 1 for Home or 2 for Work.";
    
    // Search Contact Strings
    public const string SEARCH_CONTACT_TITLE = "\n--- Search Contacts ---";
    public const string SEARCH_BY = "Search by:";
    public const string SEARCH_BY_NAME = "1. Name";
    public const string SEARCH_BY_NUMBER = "2. Number";
    public const string ENTER_SEARCH_TERM = "Enter search term: ";
    public const string SEARCH_TERM_CANNOT_BE_EMPTY = "Search term cannot be empty!";
    public const string INVALID_SEARCH_CHOICE = "Invalid choice!";
    public const string NO_CONTACTS_FOUND = "\nNo contacts found matching '{0}'.";
    public const string CONTACTS_FOUND = "\nFound {0} contact(s):\n";
    public const string CONTACT_NUMBER = "Contact {0}:";
    
    // Call Contact Strings
    public const string CALL_CONTACT_TITLE = "\n--- Call a Contact ---";
    public const string NO_CONTACTS_AVAILABLE = "No contacts available!";
    public const string AVAILABLE_CONTACTS = "Available contacts:\n";
    public const string ENTER_CONTACT_NUMBER_TO_CALL = "\nEnter contact number to call: ";
    public const string INVALID_CONTACT_NUMBER = "Invalid contact number!";
    
    // Error Messages
    public const string ERROR_PREFIX = "\nError: {0}";
    public const string UNEXPECTED_ERROR = "\nUnexpected error: {0}";
    public const string WARNING_LOAD_HISTORY = "Warning: Could not load call history: {0}";
    public const string WARNING_SAVE_HISTORY = "Warning: Could not save call history: {0}";
    public const string WARNING_LOAD_CONTACTS = "Warning: Could not load contacts: {0}";
    public const string WARNING_SAVE_CONTACTS = "Warning: Could not save contacts: {0}";
    
    // Phone Number Validation
    public const int VALID_PHONE_NUMBER_LENGTH = 10;
    public const string PHONE_NUMBER_CANNOT_BE_EMPTY = "Phone number cannot be empty!";
    public const string INVALID_PHONE_NUMBER_LENGTH = "Invalid phone number! Please enter exactly {0} digits.";
    public const string PHONE_NUMBER_MUST_BE_DIGITS = "Phone number must contain only digits!";
    
    // Exceptions
    public const string DUPLICATE_CONTACT_MESSAGE = "A contact with name '{0}' and number '{1}' already exists!";
    
    // Export Contacts
    public const string EXPORT_FILE_PATH = "exported_contacts.txt";
    public const string EXPORT_STARTING = "\nExporting contacts to file...";
    public const string EXPORT_SUCCESS = "\nSuccessfully exported {0} contact(s) to '{1}'!";
    public const string EXPORT_FAILED = "\nFailed to export contacts: {0}";
}

