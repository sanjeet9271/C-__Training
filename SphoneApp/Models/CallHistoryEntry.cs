namespace SphoneApp.Models;
public class CallHistoryEntry
{
    public string PhoneNumber { get; set; }
    public DateTime CalledAt { get; set; }

    public CallHistoryEntry(string phoneNumber = "", DateTime? calledAt = null)
    {
        PhoneNumber = phoneNumber;
        CalledAt = calledAt ?? DateTime.Now;
    }
}
