namespace SphoneApp.Models;
public class CallHistoryEntry
{
    public string PhoneNumber { get; set; }
    public DateTime CalledAt { get; set; }

    public CallHistoryEntry()
    {
        PhoneNumber = string.Empty;
        CalledAt = DateTime.Now;
    }

    public CallHistoryEntry(string phoneNumber, DateTime calledAt)
    {
        PhoneNumber = phoneNumber;
        CalledAt = calledAt;
    }

    public CallHistoryEntry(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        CalledAt = DateTime.Now;
    }
}
