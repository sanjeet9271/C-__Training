using SphoneApp.Models;
using SphoneApp.Managers;

namespace SphoneApp.Interfaces;

public interface IContactsManager
{
    event EventHandler<DialRequestedEventArgs>? DialRequested;
    Contact? FindContactByNumber(string phoneNumber);
    List<Contact> GetAllContacts();
    void AddContact(Contact contact);
    void AddPhoneNumberToContact(Contact contact, string phoneNumber, ContactType type);
    List<Contact> SearchByName(string searchTerm);
    List<Contact> SearchByNumber(string searchTerm);
    bool IsDuplicateContact(string name, string number);
    void RequestDial(string phoneNumber, string contactName);
}
