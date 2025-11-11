# 🏗️ SphoneApp Architecture Documentation

## 📊 Visual Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Program.cs                               │
│                    (Bootstrap & DI Setup)                        │
│  - Configures ServiceCollection                                 │
│  - Registers all services as Singletons                         │
│  - Gets AppRunner from DI container                             │
│  - Wraps everything in global try-catch                         │
└────────────┬────────────────────────────────────────────────────┘
             │ creates & injects dependencies
             ↓
┌─────────────────────────────────────────────────────────────────┐
│                      Core/AppRunner.cs                           │
│                  (Main Menu Orchestrator)                        │
│  - Injected with: IDialingService, IHistoryService,             │
│                   ContactsManager                                │
│  - Displays main menu (1. Dial, 2. History, 3. Contacts)       │
│  - Delegates work to services                                    │
└──┬────────────────────────────────────┬─────────────────────────┘
   │                                    │
   │ Case 1: Dial                       │ Case 3: Contacts
   ↓                                    ↓
┌──────────────────────┐    ┌─────────────────────────────────────┐
│ Services/            │    │   Core/ContactsManager.cs            │
│ DialingService       │    │   (Contacts Sub-Menu Orchestrator)   │
│                      │    │  - Injected with: IContactService,   │
│ - Validates number   │    │                   IDialingService    │
│ - Displays "Dialing" │    │  - Handles contacts sub-menu         │
│ - Calls              │    │  - Add, Search, Export, Call         │
│   HistoryService     │    └──┬────────────────────────┬──────────┘
└──────┬───────────────┘       │                        │
       │                       │ Add/Search             │ Call Contact
       │                       ↓                        ↓
       │              ┌────────────────────┐   ┌────────────────────┐
       │              │ Services/          │   │ Services/          │
       │              │ ContactService     │   │ DialingService     │
       │              │                    │   │                    │
       │              │ - AddContact()     │   │ - DialNumber()     │
       │              │ - SearchContacts() │   └────────────────────┘
       │              │ - GetAllContacts() │
       │              │ - Duplicate check  │
       │              │ - JSON persistence │
       │              └────────────────────┘
       │
       ↓
┌────────────────────────────────────────────────────────────────┐
│              Services/HistoryService                            │
│              (Call History Management)                          │
│  - AddNumberToHistory()  →  Inserts at beginning (latest first)│
│  - GetHistory()          →  Returns all entries                │
│  - ViewHistory()         →  Displays formatted list            │
│  - LoadHistory()         →  Reads from JSON                    │
│  - SaveHistory()         →  Writes to JSON                     │
└────────────────────────────────────────────────────────────────┘
```

## 🔄 Data Flow Example: Dialing a Number

```
User Input: "1234567890"
      ↓
AppRunner.DialNumber()
      ↓
Prompts user for input
      ↓
Calls: dialingService.DialNumber("1234567890")
      ↓
DialingService:
  1. Validates: length == 10 && all digits?
  2. If invalid → throws InvalidPhoneNumberException
  3. If valid:
     - Displays "Dialing 1234567890..."
     - Displays "Call connected!"
     - Calls historyService.AddNumberToHistory("1234567890")
      ↓
HistoryService:
  1. Creates new CallHistoryEntry with DateTime.Now
  2. Inserts at position 0 (beginning of list)
  3. Calls SaveHistory()
  4. Serializes list to JSON
  5. Writes to call_history_db.json
      ↓
User sees: "Call connected!"
User presses: H
      ↓
Returns to main menu
```

## 🔄 Data Flow Example: Adding a Contact

```
User selects: "3. Contacts"
      ↓
AppRunner delegates to: contactsManager.Run()
      ↓
ContactsManager displays sub-menu
      ↓
User selects: "1. Add a contact"
      ↓
ContactsManager.AddContact():
  1. Prompts for Name: "John Doe"
  2. Prompts for Number: "1234567890"
  3. Prompts for Type: "1" (Work)
  4. Creates Contact object:
     {
       Name: "John Doe",
       Number: {
         No: "1234567890",
         Type: ContactType.Work
       }
     }
  5. Calls: contactService.AddContact(newContact)
      ↓
ContactService:
  1. Checks for duplicate (same name + same number)
  2. If duplicate → throws DuplicateContactException
  3. If unique:
     - Adds to _contacts list
     - Calls SaveContacts()
     - Serializes to JSON
     - Writes to contact_list.json
      ↓
User sees: "✓ Contact 'John Doe' added successfully!"
User presses: H
      ↓
Returns to contacts menu
```

## 🧩 Dependency Injection Container Setup

```csharp
ServiceProvider
    ├── IHistoryService  →  HistoryService (Singleton)
    ├── IDialingService  →  DialingService (Singleton)
    │                       └── depends on: IHistoryService
    ├── IContactService  →  ContactService (Singleton)
    ├── ContactsManager  →  ContactsManager (Singleton)
    │                       └── depends on: IContactService, IDialingService
    └── AppRunner        →  AppRunner (Singleton)
                            └── depends on: IDialingService, 
                                           IHistoryService, 
                                           ContactsManager
```

## 📦 Layer Responsibilities

### **1. Program.cs (Bootstrap)**
- **Single Responsibility**: Application startup and DI configuration
- **No Business Logic**
- **Creates**: ServiceProvider
- **Runs**: AppRunner.Run()
- **Handles**: Top-level exceptions

### **2. Core/ (Orchestration Layer)**
- **Single Responsibility**: User interaction flow
- **No Business Logic**
- **AppRunner**: Main menu + delegates to services
- **ContactsManager**: Contacts sub-menu + delegates to services

### **3. Services/ (Business Logic Layer)**
- **Single Responsibility**: Core application logic
- **DialingService**: Validates numbers, displays messages, adds to history
- **HistoryService**: Manages call history, JSON persistence
- **ContactService**: Manages contacts, validation, JSON persistence

### **4. Models/ (Data Layer)**
- **Single Responsibility**: Data structures only
- **No Logic**: Just properties with getters/setters
- **POCOs**: Plain Old CLR Objects

### **5. Exceptions/ (Error Handling)**
- **Single Responsibility**: Custom error types
- **Clear Messages**: Descriptive error information
- **Type Safety**: Catch specific exceptions

## 🎯 SOLID Principles in Action

### **S - Single Responsibility Principle**
- Each class has one reason to change
- `DialingService` only handles dialing
- `HistoryService` only handles history

### **O - Open/Closed Principle**
- Open for extension (interfaces)
- Closed for modification (implementations)
- Can add new `IDialingService` implementation without changing consumers

### **L - Liskov Substitution Principle**
- Any `IDialingService` implementation can replace another
- Consumers depend on interface, not concrete class

### **I - Interface Segregation Principle**
- Small, focused interfaces
- `IDialingService` has only `DialNumber()`
- Clients aren't forced to depend on unused methods

### **D - Dependency Inversion Principle**
- High-level modules (`AppRunner`) depend on abstractions (`IDialingService`)
- Low-level modules (`DialingService`) implement abstractions
- Both depend on interfaces, not concrete classes

## 🧪 Testability

### **Unit Test Example: DialingService**

```csharp
[Fact]
public void DialNumber_ValidNumber_AddsToHistory()
{
    // Arrange
    var mockHistoryService = new Mock<IHistoryService>();
    var dialingService = new DialingService(mockHistoryService.Object);

    // Act
    dialingService.DialNumber("1234567890");

    // Assert
    mockHistoryService.Verify(h => 
        h.AddNumberToHistory("1234567890"), 
        Times.Once);
}

[Fact]
public void DialNumber_InvalidNumber_ThrowsException()
{
    // Arrange
    var mockHistoryService = new Mock<IHistoryService>();
    var dialingService = new DialingService(mockHistoryService.Object);

    // Act & Assert
    Assert.Throws<InvalidPhoneNumberException>(() => 
        dialingService.DialNumber("123"));
}
```

## 📈 Scalability

### **Adding a New Feature: SMS Service**

1. Create `Models/SmsMessage.cs`:
```csharp
public class SmsMessage
{
    public string To { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
}
```

2. Create `Services/ISmsService.cs`:
```csharp
public interface ISmsService
{
    void SendSms(string number, string message);
    IEnumerable<SmsMessage> GetSentMessages();
}
```

3. Create `Services/SmsService.cs`:
```csharp
public class SmsService : ISmsService
{
    private List<SmsMessage> _messages = new();
    
    public void SendSms(string number, string message)
    {
        // Implementation
    }
}
```

4. Register in `Program.cs`:
```csharp
services.AddSingleton<ISmsService, SmsService>();
```

5. Inject into `AppRunner`:
```csharp
public AppRunner(
    IDialingService dialingService,
    IHistoryService historyService,
    ISmsService smsService,      // ← New
    ContactsManager contactsManager)
```

6. Add menu option and use!

## 🎨 Design Patterns Used

1. **Dependency Injection** - Constructor injection throughout
2. **Repository Pattern** - Services act as repositories for data
3. **Facade Pattern** - Services provide simple interfaces to complex operations
4. **Strategy Pattern** - Different service implementations possible
5. **Singleton Pattern** - Single instance of each service (via DI)

## 📝 Code Conventions

- **Interfaces**: Start with `I` (e.g., `IDialingService`)
- **Private fields**: Start with `_` (e.g., `_historyService`)
- **Public methods**: PascalCase (e.g., `DialNumber`)
- **Private methods**: PascalCase (e.g., `SaveHistory`)
- **Async methods**: End with `Async` (when applicable)

---

*This architecture provides a solid foundation for building maintainable, testable, and scalable applications.*

