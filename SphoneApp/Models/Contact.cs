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
    public string PhoneNo { get; set; }
    public ContactType Type { get; set; }

    public Contact()
    {
        Name = string.Empty;
        PhoneNo = string.Empty;
        Type = ContactType.Home;
    }

    public Contact(string name, string phoneNo, ContactType type)
    {
        Name = name;
        PhoneNo = phoneNo;
        Type = type;
    }

    public override string ToString()
    {
        return $"Name: {Name}\n  Phone: {PhoneNo} ({Type})";
    }
}
