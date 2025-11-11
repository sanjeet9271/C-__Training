using SphoneApp.Exceptions;

namespace SphoneApp.Utils;

public static class PhoneNumberUtils
{
    private const int ValidPhoneNumberLength = 10;
    public static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new InvalidPhoneNumberException("Phone number cannot be empty!");
        }

        // Remove spaces and dashes for validation
        string cleanNumber = CleanPhoneNumber(phoneNumber);

        if (cleanNumber.Length != ValidPhoneNumberLength)
        {
            throw new InvalidPhoneNumberException($"Invalid phone number! Please enter exactly {ValidPhoneNumberLength} digits.");
        }

        if (!IsAllDigits(cleanNumber))
        {
            throw new InvalidPhoneNumberException("Phone number must contain only digits!");
        }
    }

    public static bool IsValidPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        string cleanNumber = CleanPhoneNumber(phoneNumber);
        return cleanNumber.Length == ValidPhoneNumberLength && IsAllDigits(cleanNumber);
    }

    public static string CleanPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        return phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
    }

    public static string FormatPhoneNumber(string phoneNumber)
    {
        string cleanNumber = CleanPhoneNumber(phoneNumber);
        
        if (cleanNumber.Length == ValidPhoneNumberLength)
        {
            return $"({cleanNumber.Substring(0, 3)}) {cleanNumber.Substring(3, 3)}-{cleanNumber.Substring(6)}";
        }
        
        return phoneNumber;
    }

    private static bool IsAllDigits(string value)
    {
        return value.All(char.IsDigit);
    }
}

