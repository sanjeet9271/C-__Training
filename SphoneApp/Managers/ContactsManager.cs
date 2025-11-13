using SphoneApp.Models;
using SphoneApp.Utils;
using SphoneApp.Interfaces;

namespace SphoneApp.Managers;

public class DialRequestedEventArgs : EventArgs
{
    public string PhoneNumber { get; }
    public string ContactName { get; }

    public DialRequestedEventArgs(string phoneNumber, string contactName)
    {
        PhoneNumber = phoneNumber;
        ContactName = contactName;
    }
}

public class ContactsManager : IContactsManager
{
    private readonly IRepository<Contact> _repository;
    public event EventHandler<DialRequestedEventArgs>? DialRequested;

    public ContactsManager(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public Contact? FindContactByNumber(string phoneNumber)
    {
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        return _repository.GetAll().FirstOrDefault(c => c.HasPhoneNumber(cleanNumber));
    }

    public List<Contact> GetAllContacts()
    {
        return _repository.GetAll();
    }

    public void AddContact(Contact contact)
    {
        _repository.Add(contact);
        _repository.SaveChanges();
    }

    public void AddPhoneNumberToContact(Contact contact, string phoneNumber, ContactType type)
    {
        contact.AddPhoneNumber(phoneNumber, type);
        _repository.SaveChanges();
    }

    public List<Contact> SearchByName(string searchTerm)
    {
        return _repository.GetAll()
            .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Contact> SearchByNumber(string searchTerm)
    {
        return _repository.GetAll()
            .Where(c => c.PhoneNumbers.Any(p => p.PhoneNo.Contains(searchTerm)))
            .ToList();
    }

    public bool IsDuplicateContact(string name, string number)
    {
        string cleanedNumber = PhoneNumberUtils.CleanPhoneNumber(number);
        return _repository.GetAll().Any(c =>
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
            c.HasPhoneNumber(cleanedNumber));
    }

    public void RequestDial(string phoneNumber, string contactName)
    {
        OnDialRequested(new DialRequestedEventArgs(phoneNumber, contactName));
    }

    protected virtual void OnDialRequested(DialRequestedEventArgs e)
    {
        DialRequested?.Invoke(this, e);
    }
}
