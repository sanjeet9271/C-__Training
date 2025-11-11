using System.Text.Json;
using SphoneApp.Exceptions;
using SphoneApp.Models;
using SphoneApp.Utils;

namespace SphoneApp.Managers;
public class ContactsManager
{
    private List<Contact> _contacts;

    public ContactsManager()
    {
        _contacts = new List<Contact>();
        LoadContacts();
    }

    private void LoadContacts()
    {
        try
        {
            if (File.Exists(Constants.ContactListFilePath))
            {
                string json = File.ReadAllText(Constants.ContactListFilePath);
                _contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not load contacts: {ex.Message}");
            _contacts = new List<Contact>();
        }
    }

    private void SaveContacts()
    {
        try
        {
            string json = JsonSerializer.Serialize(_contacts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Constants.ContactListFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not save contacts: {ex.Message}");
        }
    }
    public void AddContact(Contact contact)
    {
        _contacts.Add(contact);
        SaveContacts();
    }
    public List<Contact> GetAllContacts()
    {
        return _contacts;
    }
    public List<Contact> SearchByName(string searchTerm)
    {
        return _contacts.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public List<Contact> SearchByNumber(string searchTerm)
    {
        return _contacts.Where(c => c.CustomProperties.Any(cp => cp.PhoneNo.Contains(searchTerm))).ToList();
    }
    public bool IsDuplicateContact(string name, string number)
    {
        return _contacts.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                  c.HasPhoneNumber(number));
    }
}

