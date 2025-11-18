# Events and Delegates in C#

## Delegates

### What is a Delegate?
A delegate is a type-safe function pointer. It's a reference type that holds references to methods with a specific signature.

Think of it as a variable that stores a method, so you can pass methods as parameters or call them later.

### Basic Delegate Declaration

```csharp
// Declare a delegate type
public delegate void MessageHandler(string message);

// Method that matches the delegate signature
public void PrintMessage(string message)
{
    Console.WriteLine(message);
}

public void LogMessage(string message)
{
    Console.WriteLine($"[LOG] {message}");
}

// Usage:
MessageHandler handler = PrintMessage;
handler("Hello!"); // Calls PrintMessage

handler = LogMessage;
handler("Hello!"); // Calls LogMessage
```

### Delegate Syntax

```csharp
// Delegate declaration
[access_modifier] delegate [return_type] DelegateName([parameters]);

// Examples:
public delegate void SimpleDelegate();
public delegate int MathOperation(int a, int b);
public delegate bool Validator<T>(T item);
public delegate void EventHandler(object sender, EventArgs e);
```

### Multicast Delegates

Delegates can reference multiple methods (multicast).

```csharp
public delegate void NotifyHandler(string message);

public void SendEmail(string message)
{
    Console.WriteLine($"Email: {message}");
}

public void SendSMS(string message)
{
    Console.WriteLine($"SMS: {message}");
}

// Combine delegates with +=
NotifyHandler notify = SendEmail;
notify += SendSMS; // Now points to both methods

notify("Important message!");
// Output:
// Email: Important message!
// SMS: Important message!

// Remove with -=
notify -= SendEmail;
notify("Another message"); // Only sends SMS
```

### Built-in Delegates

C# provides common delegate types so you don't need to declare custom ones.

#### Action Delegate
For methods that return void.

```csharp
// Action with no parameters
Action greet = () => Console.WriteLine("Hello!");
greet();

// Action with 1 parameter
Action<string> print = message => Console.WriteLine(message);
print("Hello World!");

// Action with 2 parameters
Action<string, int> printWithCount = (msg, count) => 
{
    for (int i = 0; i < count; i++)
        Console.WriteLine(msg);
};
printWithCount("Hello", 3);

// Action with up to 16 parameters
Action<int, int, int> sum = (a, b, c) => Console.WriteLine(a + b + c);
```

#### Func Delegate
For methods that return a value.

```csharp
// Func with no parameters, returns int
Func<int> getNumber = () => 42;
int result = getNumber(); // 42

// Func with 1 parameter, returns bool
Func<int, bool> isEven = num => num % 2 == 0;
bool result = isEven(4); // true

// Func with 2 parameters, returns int
Func<int, int, int> add = (a, b) => a + b;
int sum = add(5, 3); // 8

// Last type parameter is always the return type
Func<string, int, bool> checkLength = (str, len) => str.Length > len;
```

#### Predicate Delegate
For methods that return bool (used for testing conditions).

```csharp
// Predicate<T> - takes T, returns bool
Predicate<int> isPositive = num => num > 0;
bool result = isPositive(5); // true

// Often used with collection methods
List<int> numbers = new List<int> { -5, 3, -2, 8, -1 };
List<int> positive = numbers.FindAll(isPositive); // { 3, 8 }
```

### Lambda Expressions with Delegates

Lambda expressions are anonymous functions used with delegates.

```csharp
// Traditional method
public bool IsEven(int number)
{
    return number % 2 == 0;
}
Func<int, bool> check1 = IsEven;

// Lambda expression (one line)
Func<int, bool> check2 = number => number % 2 == 0;

// Lambda with multiple statements
Func<int, bool> check3 = number =>
{
    Console.WriteLine($"Checking {number}");
    return number % 2 == 0;
};

// Lambda with no parameters
Action greet = () => Console.WriteLine("Hello!");

// Lambda with multiple parameters
Func<int, int, int> add = (a, b) => a + b;
```

### Delegate Use Cases

#### 1. Callbacks
```csharp
public class DataLoader
{
    public void LoadData(Action<string> onComplete)
    {
        // Simulate loading
        System.Threading.Thread.Sleep(1000);
        string data = "Loaded data";
        
        // Call the callback
        onComplete(data);
    }
}

// Usage:
var loader = new DataLoader();
loader.LoadData(data => Console.WriteLine($"Received: {data}"));
```

#### 2. Filtering/Transforming Collections
```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Filter with Predicate
List<int> evens = numbers.FindAll(n => n % 2 == 0);

// Transform with Func
List<int> doubled = numbers.Select(n => n * 2).ToList();

// Custom filter
List<int> FilterNumbers(List<int> list, Func<int, bool> filter)
{
    List<int> result = new List<int>();
    foreach (int num in list)
    {
        if (filter(num))
            result.Add(num);
    }
    return result;
}

var bigNumbers = FilterNumbers(numbers, n => n > 5);
```

#### 3. Strategy Pattern
```csharp
public class Calculator
{
    public int Calculate(int a, int b, Func<int, int, int> operation)
    {
        return operation(a, b);
    }
}

var calc = new Calculator();
int sum = calc.Calculate(5, 3, (a, b) => a + b);      // 8
int product = calc.Calculate(5, 3, (a, b) => a * b);  // 15
int max = calc.Calculate(5, 3, Math.Max);             // 5
```

---

## Events

### What is an Event?
An event is a notification mechanism that allows a class to notify other classes when something happens. Events are built on top of delegates.

Think of it as a "subscription" system - objects can subscribe to events and get notified when they occur.

### Event Declaration and Usage

```csharp
// Publisher class
public class Button
{
    // Declare event with EventHandler delegate
    public event EventHandler? Clicked;
    
    public void Click()
    {
        Console.WriteLine("Button clicked!");
        
        // Raise the event
        OnClicked(EventArgs.Empty);
    }
    
    // Protected method to raise event
    protected virtual void OnClicked(EventArgs e)
    {
        // Invoke only if there are subscribers
        Clicked?.Invoke(this, e);
    }
}

// Subscriber
public class Application
{
    public void Start()
    {
        Button button = new Button();
        
        // Subscribe to event
        button.Clicked += Button_Clicked;
        
        // Trigger button click
        button.Click();
    }
    
    // Event handler method
    private void Button_Clicked(object? sender, EventArgs e)
    {
        Console.WriteLine("Application received click event!");
    }
}

// Output:
// Button clicked!
// Application received click event!
```

### Event Pattern

Standard event pattern in C#:

```csharp
// 1. Define custom EventArgs (if needed)
public class CustomEventArgs : EventArgs
{
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    
    public CustomEventArgs(string message)
    {
        Message = message;
        Timestamp = DateTime.Now;
    }
}

// 2. Declare event using EventHandler<T>
public class Publisher
{
    public event EventHandler<CustomEventArgs>? DataReceived;
    
    public void ProcessData(string data)
    {
        // Do processing...
        
        // Raise event
        OnDataReceived(new CustomEventArgs(data));
    }
    
    protected virtual void OnDataReceived(CustomEventArgs e)
    {
        DataReceived?.Invoke(this, e);
    }
}

// 3. Subscribe to event
public class Subscriber
{
    public void Initialize()
    {
        var publisher = new Publisher();
        publisher.DataReceived += OnDataReceived;
    }
    
    private void OnDataReceived(object? sender, CustomEventArgs e)
    {
        Console.WriteLine($"Received: {e.Message} at {e.Timestamp}");
    }
}
```

### Events in SphoneApp

**Example from ContactsManager:**

```csharp
// DialRequestedEventArgs.cs
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

// IContactsManager.cs
public interface IContactsManager
{
    // Event declaration in interface
    event EventHandler<DialRequestedEventArgs>? DialRequested;
    
    // Other methods...
    void RequestDial(string phoneNumber, string contactName);
}

// ContactsManager.cs
public class ContactsManager : IContactsManager
{
    // Event implementation
    public event EventHandler<DialRequestedEventArgs>? DialRequested;

    public void RequestDial(string phoneNumber, string contactName)
    {
        // Raise the event
        OnDialRequested(new DialRequestedEventArgs(phoneNumber, contactName));
    }

    // Protected method to raise event
    protected virtual void OnDialRequested(DialRequestedEventArgs e)
    {
        // ?. ensures null check (only invoke if there are subscribers)
        DialRequested?.Invoke(this, e);
    }
}

// Usage in ContactsMenu or other class:
public class ContactsMenu
{
    private IContactsManager _contactsManager;
    private IDialManager _dialManager;

    public void Initialize()
    {
        // Subscribe to event
        _contactsManager.DialRequested += OnDialRequested;
    }

    // Event handler
    private void OnDialRequested(object? sender, DialRequestedEventArgs e)
    {
        // Handle the dial request
        _dialManager.DialNumber(e.PhoneNumber, e.ContactName);
    }
}
```

### Multiple Subscribers

Events support multiple subscribers:

```csharp
public class Stock
{
    private decimal _price;
    public event EventHandler<decimal>? PriceChanged;
    
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPriceChanged(value);
            }
        }
    }
    
    protected virtual void OnPriceChanged(decimal newPrice)
    {
        PriceChanged?.Invoke(this, newPrice);
    }
}

// Multiple subscribers
var stock = new Stock();

// Subscriber 1: Display
stock.PriceChanged += (sender, price) => 
    Console.WriteLine($"Display: Price is now {price:C}");

// Subscriber 2: Alert
stock.PriceChanged += (sender, price) => 
{
    if (price > 100)
        Console.WriteLine("ALERT: Price exceeded $100!");
};

// Subscriber 3: Logger
stock.PriceChanged += (sender, price) => 
    Console.WriteLine($"LOG: {DateTime.Now}: Price changed to {price}");

stock.Price = 105;
// Output:
// Display: Price is now $105.00
// ALERT: Price exceeded $100!
// LOG: 11/18/2025 10:30:00: Price changed to 105
```

### Event vs Delegate

| Feature | Delegate | Event |
|---------|----------|-------|
| Purpose | Reference to method(s) | Notification mechanism |
| Assignment | Can be reassigned | Can only add/remove |
| Invocation | Anyone can invoke | Only publisher can invoke |
| Encapsulation | Less encapsulated | More encapsulated |
| Usage | Callbacks, strategies | Publish-subscribe pattern |

```csharp
// Delegate example
public class Example
{
    public Action? MyDelegate; // Can be reassigned
}

var example = new Example();
example.MyDelegate = () => Console.WriteLine("First");
example.MyDelegate = () => Console.WriteLine("Second"); // Replaces first!
example.MyDelegate?.Invoke(); // Only "Second" executes

// Event example
public class Example2
{
    public event Action? MyEvent; // Cannot be reassigned
}

var example2 = new Example2();
example2.MyEvent += () => Console.WriteLine("First");
example2.MyEvent += () => Console.WriteLine("Second"); // Adds to first!
// example2.MyEvent?.Invoke(); // ERROR: Cannot invoke from outside
// example2.MyEvent = null; // ERROR: Cannot reassign
```

### Unsubscribing from Events

Important to prevent memory leaks:

```csharp
public class Subscriber
{
    private Publisher _publisher;
    
    public void Subscribe(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.DataReceived += OnDataReceived;
    }
    
    public void Unsubscribe()
    {
        if (_publisher != null)
        {
            _publisher.DataReceived -= OnDataReceived;
        }
    }
    
    private void OnDataReceived(object? sender, EventArgs e)
    {
        Console.WriteLine("Data received");
    }
}

// Usage:
var publisher = new Publisher();
var subscriber = new Subscriber();

subscriber.Subscribe(publisher);
// Use...
subscriber.Unsubscribe(); // Clean up
```

### Anonymous Event Handlers

```csharp
var button = new Button();

// Lambda expression as event handler
button.Clicked += (sender, e) => 
{
    Console.WriteLine("Button was clicked!");
};

// Multiple statements
button.Clicked += (sender, e) => 
{
    Console.WriteLine("Handling click...");
    // More logic
};

// Unsubscribing anonymous handlers is difficult!
// Better to use named methods if you need to unsubscribe
```

### Event Best Practices

#### 1. Use the Standard Event Pattern
```csharp
// ✅ Good - follows pattern
public event EventHandler<CustomEventArgs>? MyEvent;

protected virtual void OnMyEvent(CustomEventArgs e)
{
    MyEvent?.Invoke(this, e);
}
```

#### 2. Always Check for Null
```csharp
// ✅ Good - null-conditional operator
MyEvent?.Invoke(this, e);

// ❌ Bad - can throw NullReferenceException
if (MyEvent != null)
{
    MyEvent(this, e); // Not thread-safe!
}
```

#### 3. Name Events with Verb or Verb Phrase
```csharp
// ✅ Good
public event EventHandler? Clicked;
public event EventHandler? DataReceived;
public event EventHandler? ConnectionOpened;

// ❌ Bad
public event EventHandler? Click;
public event EventHandler? Data;
```

#### 4. Use EventArgs or Derived Class
```csharp
// ✅ Good - custom EventArgs
public class DataReceivedEventArgs : EventArgs
{
    public string Data { get; }
    public DataReceivedEventArgs(string data) => Data = data;
}

public event EventHandler<DataReceivedEventArgs>? DataReceived;

// ❌ Bad - custom delegate (harder to maintain)
public delegate void DataReceivedHandler(string data);
public event DataReceivedHandler? DataReceived;
```

#### 5. Make Event Raising Method Protected Virtual
```csharp
// ✅ Good - allows overriding in derived classes
protected virtual void OnDataReceived(EventArgs e)
{
    DataReceived?.Invoke(this, e);
}
```

#### 6. Unsubscribe When Done
```csharp
// ✅ Good - prevents memory leaks
public void Cleanup()
{
    publisher.DataReceived -= OnDataReceived;
}
```

---

## Practical Examples

### Example 1: Progress Reporting

```csharp
public class ProgressEventArgs : EventArgs
{
    public int PercentComplete { get; }
    public string Status { get; }
    
    public ProgressEventArgs(int percent, string status)
    {
        PercentComplete = percent;
        Status = status;
    }
}

public class FileProcessor
{
    public event EventHandler<ProgressEventArgs>? ProgressChanged;
    
    public void ProcessFiles(string[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {
            ProcessFile(files[i]);
            
            int percent = (i + 1) * 100 / files.Length;
            OnProgressChanged(new ProgressEventArgs(percent, $"Processed {files[i]}"));
        }
    }
    
    protected virtual void OnProgressChanged(ProgressEventArgs e)
    {
        ProgressChanged?.Invoke(this, e);
    }
    
    private void ProcessFile(string file)
    {
        // Process file...
        System.Threading.Thread.Sleep(100);
    }
}

// Usage:
var processor = new FileProcessor();
processor.ProgressChanged += (sender, e) =>
{
    Console.WriteLine($"{e.PercentComplete}%: {e.Status}");
};

processor.ProcessFiles(new[] { "file1.txt", "file2.txt", "file3.txt" });
```

### Example 2: Custom Event in Repository

```csharp
public class ContactAddedEventArgs : EventArgs
{
    public Contact Contact { get; }
    public ContactAddedEventArgs(Contact contact) => Contact = contact;
}

public class ContactRepository
{
    private List<Contact> _contacts = new();
    
    // Event for when contact is added
    public event EventHandler<ContactAddedEventArgs>? ContactAdded;
    
    public void AddContact(Contact contact)
    {
        _contacts.Add(contact);
        OnContactAdded(new ContactAddedEventArgs(contact));
    }
    
    protected virtual void OnContactAdded(ContactAddedEventArgs e)
    {
        ContactAdded?.Invoke(this, e);
    }
}

// Subscribers
var repo = new ContactRepository();

// Logger subscriber
repo.ContactAdded += (sender, e) =>
    Console.WriteLine($"LOG: Added contact {e.Contact.Name}");

// Notification subscriber
repo.ContactAdded += (sender, e) =>
    Console.WriteLine($"NOTIFY: New contact added!");

// Validation subscriber
repo.ContactAdded += (sender, e) =>
{
    if (e.Contact.PhoneNumbers.Count == 0)
        Console.WriteLine("WARNING: Contact has no phone numbers");
};
```

---

## Summary

### Delegates
- Type-safe function pointers
- Can reference single or multiple methods (multicast)
- Built-in types: `Action<T>`, `Func<T>`, `Predicate<T>`
- Used for callbacks, strategies, LINQ

### Events
- Built on delegates
- Publish-subscribe pattern
- Only publisher can raise events
- Subscribers add/remove handlers with += and -=
- Follow standard pattern with EventArgs

### Key Differences
- **Delegates**: Pass methods as parameters, flexibility
- **Events**: Notification system, encapsulation, safety

### When to Use
- **Delegates**: Callbacks, LINQ operations, strategy pattern
- **Events**: User interactions, state changes, notifications

### Best Practices
- Use standard event pattern
- Check for null before invoking
- Unsubscribe to prevent memory leaks
- Make event raising methods protected virtual
- Use descriptive names (verbs for events)

