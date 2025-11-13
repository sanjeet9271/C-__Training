using SphoneApp.Exceptions;

namespace SphoneApp.Utils;

public static class PhoneNumberUtils
{
    public static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new InvalidPhoneNumberException(ConstantStrings.PHONE_NUMBER_CANNOT_BE_EMPTY);
        }

        // Remove spaces and dashes for validation
        string cleanNumber = CleanPhoneNumber(phoneNumber);

        if (cleanNumber.Length != ConstantStrings.VALID_PHONE_NUMBER_LENGTH)
        {
            throw new InvalidPhoneNumberException(string.Format(ConstantStrings.INVALID_PHONE_NUMBER_LENGTH, ConstantStrings.VALID_PHONE_NUMBER_LENGTH));
        }

        if (!IsAllDigits(cleanNumber))
        {
            throw new InvalidPhoneNumberException(ConstantStrings.PHONE_NUMBER_MUST_BE_DIGITS);
        }
    }

    public static bool IsValidPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        string cleanNumber = CleanPhoneNumber(phoneNumber);
        return cleanNumber.Length == ConstantStrings.VALID_PHONE_NUMBER_LENGTH && IsAllDigits(cleanNumber);
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
        
        if (cleanNumber.Length == ConstantStrings.VALID_PHONE_NUMBER_LENGTH)
        {
            return $"({cleanNumber[0..3]}) {cleanNumber[3..6]}-{cleanNumber[6..]}";
        }
        
        return phoneNumber;
    }

    private static bool IsAllDigits(string value)
    {
        return value.All(char.IsDigit);
    }
}

