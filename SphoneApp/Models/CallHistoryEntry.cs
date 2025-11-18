namespace SphoneApp.Models;

public class CallHistoryEntry<T>
{
    public T Data { get; set; }
    public DateTime CalledAt { get; set; }
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

public class CallHistoryData
{
    public string PhoneNumber { get; set; }
    public string? ContactName { get; set; }
    public bool HasContact => !string.IsNullOrEmpty(ContactName);

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
