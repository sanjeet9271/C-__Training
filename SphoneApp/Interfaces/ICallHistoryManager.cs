using SphoneApp.Models;

namespace SphoneApp.Interfaces;
public interface ICallHistoryManager
{
    void SubscribeToDialManager(IDialManager dialManager);
    void DisplayHistory();
    List<CallHistoryEntry<CallHistoryData>> GetAllHistory();
}

