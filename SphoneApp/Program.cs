using System;
using System.Linq;

while (true)
{
    try
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("    Welcome to the Sphone App!         ");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Dial a Number");
        Console.WriteLine("2. View History of Dialled Numbers");
        Console.WriteLine("3. Contacts");
        Console.WriteLine("4. Exit");
        Console.WriteLine();
        Console.Write("Enter your choice: ");
        
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            throw new FormatException("Invalid input! Please enter a number.");
        }
        int choice = int.Parse(input);
        
        switch (choice)
        {
            case 1:
                Console.WriteLine();
                Console.Write("Enter the phone number to call (10 digits): ");
                string? phoneNumber = Console.ReadLine();
                

                if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
                {
                    throw new InvalidPhoneNumberException("Invalid phone number! Please enter exactly 10 digits.");
                }
                
                Console.WriteLine();
                Console.WriteLine($"Calling {phoneNumber}...");
                Console.WriteLine("Call connected!");
                Console.WriteLine();
                break;
                
            case 2:
                throw new FeatureNotSupportedException("View History feature is not supported yet. This functionality will be available in future updates.");
                
            case 3:
                throw new FeatureNotSupportedException("Contacts feature is not supported yet. This functionality will be available in future updates.");
                
            case 4:
                Console.WriteLine();
                Console.WriteLine("Thank you for using Sphone App. Goodbye!");
                return;
                
            default:
                Console.WriteLine();
                Console.WriteLine("Invalid option! Please select a valid option (1-4).");
                break;
        }
    }
    catch (InvalidPhoneNumberException ex)
    {
        Console.WriteLine();
        Console.WriteLine($"ERROR: {ex.Message}");
    }
    catch (FeatureNotSupportedException ex)
    {
        Console.WriteLine();
        Console.WriteLine($"FEATURE NOT AVAILABLE: {ex.Message}");
    }
    catch (FormatException)
    {
        Console.WriteLine();
        Console.WriteLine("ERROR: Invalid input! Please enter a number.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"ERROR: An unexpected error occurred - {ex.Message}");
    }
    
    // Prompt user to return to home screen
    Console.WriteLine();
    Console.WriteLine("Press H to return to the home screen...");
    ConsoleKeyInfo key;
    do
    {
        key = Console.ReadKey(true); 
    } while (key.Key != ConsoleKey.H);
}

// Custom exceptions for unsupported features
class FeatureNotSupportedException : Exception
{
    public FeatureNotSupportedException(string message) : base(message) { }
}

class InvalidPhoneNumberException : Exception
{
    public InvalidPhoneNumberException(string message) : base(message) { }
}