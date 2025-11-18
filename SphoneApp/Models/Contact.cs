namespace SphoneApp.Models;

public enum ContactType
{
    Home,
    Work
}

public struct CustomProperty
{
    public string PhoneNo { get; set; }
    public ContactType Type { get; set; }

    public CustomProperty(string phoneNo, ContactType type)
    {
        PhoneNo = phoneNo;
        Type = type;
    }

    public override string ToString()
    {
        return $"{PhoneNo} ({Type})";
    }
}

public class Contact
{
    public string Name { get; set; }
    public List<CustomProperty> PhoneNumbers { get; set; }

    public Contact()
    {
        Name = string.Empty;
        PhoneNumbers = new List<CustomProperty>();
    }

    public Contact(string name, List<CustomProperty> phoneNumbers)
    {
        Name = name;
        PhoneNumbers = phoneNumbers ?? new List<CustomProperty>();
    }

    public Contact(string name, string phoneNo, ContactType type)
    {
        Name = name;
        PhoneNumbers = new List<CustomProperty> { new CustomProperty(phoneNo, type) };
    }

    public void AddPhoneNumber(string phoneNo, ContactType type)
    {
        PhoneNumbers.Add(new CustomProperty(phoneNo, type));
    }
    public string? GetPrimaryPhoneNumber()
    {
        return PhoneNumbers.FirstOrDefault().PhoneNo;
    }

    public bool HasPhoneNumber(string phoneNo)
    {
        return PhoneNumbers.Any(p => p.PhoneNo == phoneNo);
    }

    public override string ToString()
    {
        if (PhoneNumbers.Count == 0)
        {
            return $"Name: {Name}\n  No phone numbers";
        }
        
        var phoneLines = string.Join("\n  ", PhoneNumbers.Select(p => p.ToString()));
        return $"Name: {Name}\n  {phoneLines}";
    }
}
