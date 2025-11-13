using SphoneApp.Models;
using SphoneApp.Managers;

namespace SphoneApp.Interfaces;
public interface ICallHistoryManager
{

    void OnCallMade(object? sender, CallMadeEventArgs e);
    void DisplayHistory();
    List<CallHistoryEntry<CallHistoryData>> GetAllHistory();
}

