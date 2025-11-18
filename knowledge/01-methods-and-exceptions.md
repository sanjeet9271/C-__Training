# C# Methods and Exception Handling

## Methods in C#

### What is a Method?
A method is a block of code that performs a specific task. Methods help organize code, promote reusability, and make programs easier to maintain.

### Method Components
```csharp
[access_modifier] [return_type] MethodName([parameters])
{
    // method body
    return value; // if return_type is not void
}
```

### Types of Methods

#### 1. Instance Methods
Methods that belong to an object instance.

**Example from SphoneApp:**
```csharp
// Contact.cs
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
```
These methods operate on a specific `Contact` instance.

#### 2. Static Methods
Methods that belong to the class itself, not to instances.

**Example from SphoneApp:**
```csharp
// PhoneNumberUtils.cs
public static class PhoneNumberUtils
{
    public static string CleanPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        return phoneNumber.Replace(" ", "")
                         .Replace("-", "")
                         .Replace("(", "")
                         .Replace(")", "");
    }

    public static string FormatPhoneNumber(string phoneNumber)
    {
        string cleanNumber = CleanPhoneNumber(phoneNumber);
        
        if (cleanNumber.Length == 10)
        {
            return $"({cleanNumber[0..3]}) {cleanNumber[3..6]}-{cleanNumber[6..]}";
        }
        
        return phoneNumber;
    }
}
```
Static methods are called on the class: `PhoneNumberUtils.CleanPhoneNumber("123-456-7890")`

#### 3. Virtual and Override Methods
Enable polymorphism through method overriding.

**Example from SphoneApp:**
```csharp
// ContactsManager.cs
protected virtual void OnDialRequested(DialRequestedEventArgs e)
{
    DialRequested?.Invoke(this, e);
}
```
Virtual methods can be overridden in derived classes.

#### 4. Extension Methods
Methods that "extend" existing types without modifying them.

```csharp
public static class StringExtensions
{
    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        return !string.IsNullOrWhiteSpace(phoneNumber) && 
               phoneNumber.Length == 10;
    }
}

// Usage: "1234567890".IsValidPhoneNumber()
```

### Method Parameters

#### 1. Value Parameters (default)
```csharp
public void AddContact(Contact contact)
{
    // contact is passed by value (reference types pass reference by value)
}
```

#### 2. `ref` Parameters
Passes by reference - changes affect the original variable.
```csharp
public void UpdateValue(ref int value)
{
    value = 100; // Original variable is modified
}
```

#### 3. `out` Parameters
Must be assigned before method returns.
```csharp
public bool TryParse(string input, out int result)
{
    return int.TryParse(input, out result);
}
```

#### 4. `params` Parameters
Variable number of arguments.
```csharp
public void AddContacts(params Contact[] contacts)
{
    foreach (var contact in contacts)
    {
        _repository.Add(contact);
    }
}
```

#### 5. Optional Parameters
Parameters with default values.
```csharp
public List<Contact> Search(string term, bool caseSensitive = false)
{
    // caseSensitive is optional, defaults to false
}
```

#### 6. Named Arguments
```csharp
// Call method with named arguments
Search(term: "John", caseSensitive: true);
```

### Return Types

#### 1. Void - No return value
```csharp
public void SaveChanges()
{
    // Performs action but doesn't return anything
}
```

#### 2. Value Types
```csharp
public int GetContactCount()
{
    return _repository.GetAll().Count;
}
```

#### 3. Reference Types
```csharp
public Contact? FindContactByNumber(string phoneNumber)
{
    // ? indicates nullable reference type
    return _repository.GetAll().FirstOrDefault(c => c.HasPhoneNumber(phoneNumber));
}
```

#### 4. Collections
```csharp
public List<Contact> GetAllContacts()
{
    return _repository.GetAll();
}
```

---

## Exception Handling

### What are Exceptions?
Exceptions are runtime errors that disrupt the normal flow of a program. C# provides a structured way to handle errors.

### The Try-Catch-Finally Block

#### Basic Structure
```csharp
try
{
    // Code that might throw an exception
}
catch (SpecificException ex)
{
    // Handle specific exception
}
catch (Exception ex)
{
    // Handle any other exception
}
finally
{
    // Always executes (optional)
    // Used for cleanup
}
```

### Exception Handling in SphoneApp

#### Example 1: File I/O Exception Handling
```csharp
// JsonRepository.cs
public void SaveChanges()
{
    try
    {
        string json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not save data: {ex.Message}");
    }
}

public void LoadData()
{
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
        Console.WriteLine($"Warning: Could not load data from {_filePath}: {ex.Message}");
        _data = new List<T>();
    }
}
```

### Custom Exceptions

Creating custom exceptions for specific error scenarios.

**Example from SphoneApp:**
```csharp
// DuplicateContactException.cs
public class DuplicateContactException : Exception
{
    public DuplicateContactException(string message) : base(message) { }
}

// InvalidPhoneNumberException.cs
public class InvalidPhoneNumberException : Exception
{
    public InvalidPhoneNumberException(string message) : base(message) { }
}
```

#### Throwing Custom Exceptions
```csharp
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
```

### Exception Best Practices

#### 1. Be Specific with Catch Blocks
```csharp
try
{
    // Code
}
catch (InvalidPhoneNumberException ex)
{
    // Handle invalid phone number
    Console.WriteLine($"Invalid phone: {ex.Message}");
}
catch (DuplicateContactException ex)
{
    // Handle duplicate contact
    Console.WriteLine($"Duplicate: {ex.Message}");
}
catch (Exception ex)
{
    // Handle unexpected exceptions
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

#### 2. Don't Swallow Exceptions
❌ Bad:
```csharp
try
{
    DoSomething();
}
catch (Exception ex)
{
    // Empty catch - silently ignores error!
}
```

✅ Good:
```csharp
try
{
    DoSomething();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    // Or log it, or rethrow, or handle it
}
```

#### 3. Use Finally for Cleanup
```csharp
FileStream? file = null;
try
{
    file = File.Open("data.txt", FileMode.Open);
    // Use file
}
catch (IOException ex)
{
    Console.WriteLine($"File error: {ex.Message}");
}
finally
{
    // Always closes file, even if exception occurs
    file?.Close();
}
```

#### 4. Throw vs Throw ex
```csharp
try
{
    DoSomething();
}
catch (Exception ex)
{
    // Log the error
    Logger.Log(ex);
    
    throw;    // ✅ Preserves original stack trace
    // throw ex; // ❌ Resets stack trace
}
```

#### 5. Use Try-Catch at Appropriate Levels
Don't catch exceptions at every method. Catch them where you can actually handle them.

```csharp
// Low-level method - let exceptions bubble up
public Contact GetContact(int id)
{
    return _repository.GetById(id); // May throw
}

// High-level method - handle exceptions
public void DisplayContact(int id)
{
    try
    {
        var contact = GetContact(id);
        Console.WriteLine(contact);
    }
    catch (NotFoundException ex)
    {
        Console.WriteLine("Contact not found");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred");
    }
}
```

### Exception Properties

```csharp
try
{
    throw new InvalidPhoneNumberException("Invalid number: 123");
}
catch (Exception ex)
{
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    Console.WriteLine($"Source: {ex.Source}");
    Console.WriteLine($"Inner Exception: {ex.InnerException}");
}
```

### Common Exception Types

- `NullReferenceException` - Accessing null object
- `ArgumentNullException` - Null argument passed
- `ArgumentException` - Invalid argument
- `InvalidOperationException` - Invalid operation for current state
- `IndexOutOfRangeException` - Array/list index out of bounds
- `FileNotFoundException` - File not found
- `IOException` - I/O operation failed
- `FormatException` - String format is invalid
- `DivideByZeroException` - Division by zero

### When to Use Exceptions

✅ Use exceptions for:
- Exceptional conditions (errors)
- Conditions you can't prevent with validation
- Errors that cross method boundaries

❌ Don't use exceptions for:
- Normal control flow
- Expected conditions (use return values or bool)
- Performance-critical code paths

```csharp
// ✅ Good - return bool for expected scenarios
public bool IsValidPhoneNumber(string phoneNumber)
{
    return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 10;
}

// ✅ Good - throw exception for validation
public void ValidatePhoneNumber(string phoneNumber)
{
    if (!IsValidPhoneNumber(phoneNumber))
        throw new InvalidPhoneNumberException("Invalid phone number");
}
```

## Summary

**Methods:**
- Organize code into reusable blocks
- Can be instance, static, virtual, or extension methods
- Support various parameter types: value, ref, out, params, optional
- Return values or void

**Exceptions:**
- Handle runtime errors gracefully
- Use try-catch-finally for exception handling
- Create custom exceptions for specific scenarios
- Follow best practices: be specific, don't swallow, clean up resources
- Use exceptions for exceptional conditions, not normal flow

