# Classes, Encapsulation, Enums, and Structs

## Classes

### What is a Class?
A class is a blueprint for creating objects. It defines properties (data) and methods (behavior) that objects of that class will have.

### Class Declaration

```csharp
public class Contact
{
    // Fields (private data)
    private string _internalId;
    
    // Properties (public data with encapsulation)
    public string Name { get; set; }
    public List<CustomProperty> PhoneNumbers { get; set; }
    
    // Constructor
    public Contact()
    {
        Name = string.Empty;
        PhoneNumbers = new List<CustomProperty>();
    }
    
    // Methods (behavior)
    public void AddPhoneNumber(string phoneNo, ContactType type)
    {
        PhoneNumbers.Add(new CustomProperty(phoneNo, type));
    }
}
```

### Constructors

Constructors initialize objects when they are created.

**Example from SphoneApp:**
```csharp
public class Contact
{
    // Default constructor (no parameters)
    public Contact()
    {
        Name = string.Empty;
        PhoneNumbers = new List<CustomProperty>();
    }

    // Parameterized constructor
    public Contact(string name, List<CustomProperty> phoneNumbers)
    {
        Name = name;
        PhoneNumbers = phoneNumbers ?? new List<CustomProperty>();
    }

    // Another parameterized constructor
    public Contact(string name, string phoneNo, ContactType type)
    {
        Name = name;
        PhoneNumbers = new List<CustomProperty> 
        { 
            new CustomProperty(phoneNo, type) 
        };
    }
}

// Usage:
var contact1 = new Contact();
var contact2 = new Contact("John", phoneList);
var contact3 = new Contact("Jane", "1234567890", ContactType.Home);
```

### Properties

Properties provide controlled access to class fields.

#### Auto-Properties (Simple)
```csharp
public class Contact
{
    // Auto-property - compiler creates backing field automatically
    public string Name { get; set; }
    
    // Read-only auto-property
    public int Id { get; }
    
    // Property with private setter
    public DateTime CreatedDate { get; private set; }
}
```

#### Properties with Backing Fields (Advanced)
```csharp
public class Contact
{
    private string _name;
    
    public string Name
    {
        get { return _name; }
        set 
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }
}
```

#### Property Initializers
```csharp
public class Contact
{
    // Initialize with default value
    public string Name { get; set; } = string.Empty;
    public List<CustomProperty> PhoneNumbers { get; set; } = new();
    public DateTime CreatedDate { get; } = DateTime.Now;
}
```

#### Computed Properties
```csharp
public class Contact
{
    public List<CustomProperty> PhoneNumbers { get; set; }
    
    // Computed property - no setter, calculates value
    public int PhoneNumberCount => PhoneNumbers.Count;
    
    // Or with full syntax
    public int PhoneNumberCount
    {
        get { return PhoneNumbers.Count; }
    }
}
```

### Access Modifiers

Control visibility and accessibility of class members.

```csharp
public class Example
{
    // public - accessible from anywhere
    public string PublicField;
    
    // private - only within this class
    private string _privateField;
    
    // protected - this class and derived classes
    protected string ProtectedField;
    
    // internal - within the same assembly
    internal string InternalField;
    
    // protected internal - same assembly OR derived classes
    protected internal string ProtectedInternalField;
    
    // private protected - same assembly AND derived classes only
    private protected string PrivateProtectedField;
}
```

**Best Practice:** Keep fields private, expose through properties.

```csharp
public class Contact
{
    // ❌ Don't do this - public field
    public string name;
    
    // ✅ Do this - private field with public property
    private string _name;
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    // ✅ Or use auto-property
    public string Name { get; set; }
}
```

---

## Encapsulation

### What is Encapsulation?
Encapsulation is hiding internal implementation details and exposing only what's necessary through a public interface.

### Benefits of Encapsulation
1. **Data Protection** - Prevent invalid states
2. **Flexibility** - Change implementation without affecting users
3. **Maintenance** - Easier to update and debug
4. **Control** - Validate data before setting

### Encapsulation Examples from SphoneApp

#### Example 1: Contact Class
```csharp
public class Contact
{
    // Properties provide controlled access
    public string Name { get; set; }
    public List<CustomProperty> PhoneNumbers { get; set; }

    public Contact()
    {
        Name = string.Empty;
        PhoneNumbers = new List<CustomProperty>(); // Ensure never null
    }

    // Public method with controlled behavior
    public void AddPhoneNumber(string phoneNo, ContactType type)
    {
        // Could add validation here
        PhoneNumbers.Add(new CustomProperty(phoneNo, type));
    }

    // Encapsulate phone number checking logic
    public bool HasPhoneNumber(string phoneNo)
    {
        return PhoneNumbers.Any(p => p.PhoneNo == phoneNo);
    }

    // Encapsulate getting primary phone
    public string? GetPrimaryPhoneNumber()
    {
        return PhoneNumbers.FirstOrDefault().PhoneNo;
    }
}
```

#### Example 2: JsonRepository - Encapsulating Data Access
```csharp
public class JsonRepository<T> : IRepository<T> where T : class
{
    // Private fields - hidden from outside
    private readonly string _filePath;
    private List<T> _data;

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
        _data = new List<T>();
        LoadData(); // Internal initialization
    }

    // Public interface
    public List<T> GetAll()
    {
        return _data; // Could return copy for better encapsulation
    }

    public void Add(T entity)
    {
        _data.Add(entity);
    }

    public void SaveChanges()
    {
        // Encapsulate file I/O complexity
        try
        {
            string json = JsonSerializer.Serialize(_data, 
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: {ex.Message}");
        }
    }

    // Private method - implementation detail
    public void LoadData()
    {
        // Complex loading logic hidden from users
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _data = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: {ex.Message}");
            _data = new List<T>();
        }
    }
}
```

#### Example 3: ContactsManager - Business Logic Encapsulation
```csharp
public class ContactsManager : IContactsManager
{
    // Private dependency - implementation detail
    private readonly IRepository<Contact> _repository;
    
    public event EventHandler<DialRequestedEventArgs>? DialRequested;

    public ContactsManager(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    // Public interface - simple for users
    public Contact? FindContactByNumber(string phoneNumber)
    {
        // Encapsulate cleaning logic
        string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
        return _repository.GetAll()
            .FirstOrDefault(c => c.HasPhoneNumber(cleanNumber));
    }

    // Encapsulate duplicate checking logic
    public bool IsDuplicateContact(string name, string number)
    {
        string cleanedNumber = PhoneNumberUtils.CleanPhoneNumber(number);
        return _repository.GetAll().Any(c =>
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
            c.HasPhoneNumber(cleanedNumber));
    }

    // Protected method - for inheritance
    protected virtual void OnDialRequested(DialRequestedEventArgs e)
    {
        DialRequested?.Invoke(this, e);
    }
}
```

#### Example 4: PhoneNumberUtils - Encapsulating Utility Logic
```csharp
// Static class - pure utility, no state
public static class PhoneNumberUtils
{
    // Encapsulate validation logic
    public static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new InvalidPhoneNumberException("Phone number cannot be empty");
        }

        string cleanNumber = CleanPhoneNumber(phoneNumber);

        if (cleanNumber.Length != 10)
        {
            throw new InvalidPhoneNumberException("Phone number must be 10 digits");
        }

        if (!IsAllDigits(cleanNumber))
        {
            throw new InvalidPhoneNumberException("Phone number must contain only digits");
        }
    }

    // Public utility method
    public static string CleanPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        return phoneNumber.Replace(" ", "")
                         .Replace("-", "")
                         .Replace("(", "")
                         .Replace(")", "");
    }

    // Private helper - implementation detail
    private static bool IsAllDigits(string value)
    {
        return value.All(char.IsDigit);
    }
}
```

### Encapsulation Best Practices

1. **Make fields private, expose through properties**
   ```csharp
   private string _name;
   public string Name { get => _name; set => _name = value; }
   ```

2. **Use properties for validation**
   ```csharp
   private string _phoneNumber;
   public string PhoneNumber
   {
       get => _phoneNumber;
       set
       {
           PhoneNumberUtils.ValidatePhoneNumber(value);
           _phoneNumber = value;
       }
   }
   ```

3. **Hide complex logic behind simple interfaces**
   ```csharp
   // User doesn't need to know about cleaning, validation, etc.
   public Contact? FindContactByNumber(string phoneNumber)
   {
       // All complexity hidden inside
   }
   ```

4. **Use readonly for immutable fields**
   ```csharp
   private readonly IRepository<Contact> _repository;
   ```

5. **Provide minimal public interface**
   ```csharp
   // Only expose what's necessary
   public interface IContactsManager
   {
       void AddContact(Contact contact);
       Contact? FindContactByNumber(string phoneNumber);
       // Don't expose internal details
   }
   ```

---

## Enums

### What is an Enum?
An enum (enumeration) is a value type that defines a set of named constants. Makes code more readable and type-safe.

### Basic Enum

**Example from SphoneApp:**
```csharp
public enum ContactType
{
    Home,
    Work
}

// Usage:
var contact = new Contact("John", "1234567890", ContactType.Home);

// In conditionals:
if (contact.PhoneNumbers[0].Type == ContactType.Work)
{
    Console.WriteLine("Work phone");
}
```

### Enum with Explicit Values
```csharp
public enum ContactType
{
    Home = 1,
    Work = 2,
    Mobile = 3,
    Other = 4
}
```

### Enum with Different Base Types
```csharp
// Byte enum (saves memory if many instances)
public enum Priority : byte
{
    Low = 1,
    Medium = 2,
    High = 3
}
```

### Flags Enum (Bit Fields)
Used when multiple values can be combined.

```csharp
[Flags]
public enum ContactCapabilities
{
    None = 0,
    Voice = 1,      // 0001
    SMS = 2,        // 0010
    Video = 4,      // 0100
    Email = 8       // 1000
}

// Usage:
ContactCapabilities caps = ContactCapabilities.Voice | ContactCapabilities.SMS;

// Check if has capability
bool hasVoice = caps.HasFlag(ContactCapabilities.Voice); // true
bool hasVideo = caps.HasFlag(ContactCapabilities.Video); // false

// Add capability
caps |= ContactCapabilities.Video;

// Remove capability
caps &= ~ContactCapabilities.SMS;
```

### Enum Methods

```csharp
public enum ContactType
{
    Home,
    Work,
    Mobile
}

// Convert to string
string name = ContactType.Home.ToString(); // "Home"

// Parse from string
ContactType type = Enum.Parse<ContactType>("Work");

// Try parse (safe)
if (Enum.TryParse<ContactType>("Mobile", out ContactType result))
{
    Console.WriteLine($"Parsed: {result}");
}

// Get all values
ContactType[] allTypes = Enum.GetValues<ContactType>();
foreach (ContactType type in allTypes)
{
    Console.WriteLine(type);
}

// Get names
string[] names = Enum.GetNames<ContactType>(); // ["Home", "Work", "Mobile"]

// Check if defined
bool isDefined = Enum.IsDefined(typeof(ContactType), "Home"); // true
```

### Enum in SphoneApp

```csharp
// Contact.cs
public enum ContactType
{
    Home,
    Work
}

public struct CustomProperty
{
    public string PhoneNo { get; set; }
    public ContactType Type { get; set; } // Using enum

    public override string ToString()
    {
        return $"{PhoneNo} ({Type})"; // Enum automatically converts to string
    }
}

// Usage in AddContact:
contact.AddPhoneNumber("1234567890", ContactType.Home);
contact.AddPhoneNumber("9876543210", ContactType.Work);
```

### Enum Best Practices

1. **Use enums for fixed sets of values**
   ```csharp
   public enum Status { Active, Inactive, Pending }
   ```

2. **Name enum types singular**
   ```csharp
   public enum ContactType { } // Good
   public enum ContactTypes { } // Bad
   ```

3. **Use PascalCase for enum values**
   ```csharp
   public enum Status { Active, Pending } // Good
   public enum Status { active, pending } // Bad
   ```

4. **Consider [Flags] for combinations**
   ```csharp
   [Flags]
   public enum Permissions { Read = 1, Write = 2, Execute = 4 }
   ```

---

## Structs

### What is a Struct?
A struct is a value type (like int, bool) that can contain data and methods. Structs are stored on the stack, making them faster for small, simple data.

### Struct vs Class

| Feature | Struct | Class |
|---------|--------|-------|
| Type | Value type | Reference type |
| Storage | Stack | Heap |
| Default constructor | Cannot override | Can override |
| Inheritance | Cannot inherit | Can inherit |
| Null | Cannot be null (unless Nullable<T>) | Can be null |
| Performance | Faster for small data | Slower (garbage collection) |
| Use case | Small, immutable data | Complex objects, inheritance |

### Struct Declaration

**Example from SphoneApp:**
```csharp
public struct CustomProperty
{
    // Properties
    public string PhoneNo { get; set; }
    public ContactType Type { get; set; }

    // Constructor
    public CustomProperty(string phoneNo, ContactType type)
    {
        PhoneNo = phoneNo;
        Type = type;
    }

    // Method
    public override string ToString()
    {
        return $"{PhoneNo} ({Type})";
    }
}

// Usage:
var prop1 = new CustomProperty("1234567890", ContactType.Home);
var prop2 = new CustomProperty { PhoneNo = "9876543210", Type = ContactType.Work };

// Value type behavior:
var prop3 = prop1; // Creates a COPY
prop3.PhoneNo = "0000000000"; // Doesn't affect prop1
Console.WriteLine(prop1.PhoneNo); // Still "1234567890"
```

### When to Use Struct

✅ **Use struct when:**
- Small data structure (< 16 bytes recommended)
- Immutable data
- Frequently created and destroyed
- Don't need inheritance
- Examples: Point, Rectangle, Color, CustomProperty

❌ **Use class when:**
- Large data structure
- Need inheritance
- Need reference semantics
- Examples: Contact, ContactsManager, Repository

### Readonly Struct (Immutable)

```csharp
public readonly struct Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// Cannot modify after creation
var point = new Point(10, 20);
// point.X = 30; // Error - readonly
```

### Struct with Methods

```csharp
public struct PhoneNumber
{
    public string Number { get; set; }
    public ContactType Type { get; set; }

    public PhoneNumber(string number, ContactType type)
    {
        Number = number;
        Type = type;
    }

    // Methods work fine in structs
    public bool IsWork()
    {
        return Type == ContactType.Work;
    }

    public string Format()
    {
        return PhoneNumberUtils.FormatPhoneNumber(Number);
    }

    public override string ToString()
    {
        return $"{Format()} ({Type})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not PhoneNumber other)
            return false;
        
        return Number == other.Number && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number, Type);
    }
}
```

### Record Struct (C# 10+)

Simpler syntax for immutable structs.

```csharp
// Record struct - automatically immutable
public record struct CustomProperty(string PhoneNo, ContactType Type);

// Usage:
var prop = new CustomProperty("1234567890", ContactType.Home);

// Has ToString, Equals, GetHashCode automatically
Console.WriteLine(prop); // CustomProperty { PhoneNo = 1234567890, Type = Home }

// With expression for creating modified copies
var updated = prop with { Type = ContactType.Work };
```

### Struct Best Practices

1. **Keep structs small** (< 16 bytes)
   ```csharp
   public struct Point { public int X; public int Y; } // Good - 8 bytes
   ```

2. **Make structs immutable** (readonly)
   ```csharp
   public readonly struct CustomProperty
   {
       public string PhoneNo { get; }
       public ContactType Type { get; }
   }
   ```

3. **Override Equals and GetHashCode**
   ```csharp
   public override bool Equals(object? obj) { }
   public override int GetHashCode() { }
   ```

4. **Implement IEquatable<T> for performance**
   ```csharp
   public struct CustomProperty : IEquatable<CustomProperty>
   {
       public bool Equals(CustomProperty other)
       {
           return PhoneNo == other.PhoneNo && Type == other.Type;
       }
   }
   ```

---

## Summary

### Classes
- Reference types stored on heap
- Support inheritance and polymorphism
- Use for complex objects with behavior
- Example: `Contact`, `ContactsManager`

### Encapsulation
- Hide internal details, expose public interface
- Use private fields, public properties
- Validate data, protect invariants
- Makes code maintainable and flexible

### Enums
- Named constants for fixed sets of values
- Type-safe alternative to magic numbers
- Use [Flags] for bit fields
- Example: `ContactType`

### Structs
- Value types stored on stack
- Use for small, immutable data
- Better performance for simple types
- Example: `CustomProperty`

**Key Distinction:**
- **Classes** = Complex objects, inheritance, reference semantics
- **Structs** = Simple data, value semantics, performance
- **Enums** = Named constants
- **Encapsulation** = Information hiding principle

