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
    public List<CustomProperty> CustomProperties { get; set; }

    public Contact()
    {
        Name = string.Empty;
        CustomProperties = new List<CustomProperty>();
    }

    public Contact(string name)
    {
        Name = name;
        CustomProperties = new List<CustomProperty>();
    }

    public Contact(string name, string phoneNo, ContactType type)
    {
        Name = name;
        CustomProperties = new List<CustomProperty> { new CustomProperty(phoneNo, type) };
    }

    public void AddPhoneNumber(string phoneNo, ContactType type)
    {
        CustomProperties.Add(new CustomProperty(phoneNo, type));
    }

    public bool HasPhoneNumber(string phoneNo)
    {
        return CustomProperties.Any(cp => cp.PhoneNo == phoneNo);
    }

    public override string ToString()
    {
        string result = $"Name: {Name}\n";
        foreach (var prop in CustomProperties)
        {
            result += $"  - {prop}\n";
        }
        return result;
    }
}
