namespace SphoneApp.Models;

// Generic call history entry that can store either contact info or just a number
public class CallHistoryEntry<T>
{
    public T Data { get; set; }
    public DateTime CalledAt { get; set; }

    // Parameterless constructor for JSON deserialization
    public CallHistoryEntry()
    {
        Data = default(T)!;
        CalledAt = DateTime.Now;
    }

    public CallHistoryEntry(T data, DateTime? calledAt = null)
    {
        Data = data;
        CalledAt = calledAt ?? DateTime.Now;
    }
}

// Wrapper for call history data - can be either contact or just phone number
public class CallHistoryData
{
    public string PhoneNumber { get; set; }
    public string? ContactName { get; set; }
    public bool HasContact => !string.IsNullOrEmpty(ContactName);

    // Parameterless constructor for JSON deserialization
    public CallHistoryData()
    {
        PhoneNumber = string.Empty;
        ContactName = null;
    }

    public CallHistoryData(string phoneNumber, string? contactName = null)
    {
        PhoneNumber = phoneNumber;
        ContactName = contactName;
    }

    public override string ToString()
    {
        if (HasContact)
        {
            return $"{ContactName} - {PhoneNumber}";
        }
        return PhoneNumber;
    }
}
