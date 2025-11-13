using SphoneApp.Managers;
using SphoneApp.Models;

namespace SphoneApp.Interfaces;

public interface IDialManager
{
    event EventHandler<CallMadeEventArgs>? CallMade;
    void RegisterContactLookup(Func<string, Contact?> contactLookup);
    void Dial();
    void DialNumber(string phoneNumber, string? knownContactName = null);
}

