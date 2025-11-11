# C# Tutorials - SphoneApp

A professional phone application simulator built with clean architecture and dependency injection.

## 📐 Architecture Pattern

This project follows a **layered architecture** with clear separation of concerns:

```
SphoneApp/
│
├── Program.cs                      # 1. Bootstrap (DI Setup & Entry Point)
├── appsettings.json                # Configuration file
├── SphoneApp.csproj                # Project file with dependencies
│
├── Core/                           # 2. Application Flow & Orchestration
│   ├── AppRunner.cs                #    - Main menu handler
│   └── ContactsManager.cs          #    - Contacts sub-menu handler
│
├── Services/                       # 3. Business Logic (Encapsulated)
│   ├── IDialingService.cs          #    - Interface
│   ├── DialingService.cs           #    - Handles dialing logic
│   ├── IHistoryService.cs          #    - Interface
│   ├── HistoryService.cs           #    - Manages call history
│   ├── IContactService.cs          #    - Interface
│   └── ContactService.cs           #    - Manages contacts
│
├── Models/                         # 4. Data Structures (POCOs)
│   ├── ContactType.cs              #    - Enum (Work, Home)
│   ├── ContactNumber.cs            #    - Phone number with type
│   ├── Contact.cs                  #    - Contact model
│   └── CallHistoryEntry.cs         #    - Call history model
│
└── Exceptions/                     # 5. Custom Exceptions
    ├── InvalidPhoneNumberException.cs
    ├── DuplicateContactException.cs
    └── NotYetImplementedException.cs
```

## 🎯 Design Principles

### 1. **Dependency Injection (DI)**
- All services are registered in `Program.cs`
- Dependencies are injected through constructors
- Promotes loose coupling and testability

### 2. **Interface-Based Design**
- All services have interfaces (`IDialingService`, `IHistoryService`, etc.)
- Easy to mock for testing
- Allows for multiple implementations

### 3. **Separation of Concerns**
- **Models**: What the data is
- **Services**: How to manipulate data
- **Core**: When to perform operations (based on user input)
- **Program.cs**: How to wire everything together
- **Exceptions**: How to handle errors

### 4. **Single Responsibility Principle**
- Each class has one job
- `AppRunner` handles main menu only
- `ContactsManager` handles contacts sub-menu only
- Services handle specific business logic

## 🚀 Features

### ✅ Main Menu
1. **Dial a Number** - Call a 10-digit phone number
2. **View History** - See all dialed numbers (newest first)
3. **Contacts** - Manage contacts
4. **Exit** - Close the application

### ✅ Contacts Menu
1. **Add a Contact** - Add name, number (10 digits), and type (Work/Home)
2. **Search Contacts** - Search by name or number (substring matching)
3. **Export Contacts** - (Not yet implemented)
4. **Call a Contact** - Select and call a saved contact
5. **Back to Home** - Return to main menu

### ✅ Data Persistence
- **Call History**: Saved to `call_history_db.json`
- **Contacts**: Saved to `contact_list.json`
- Data persists between application runs

### ✅ Validation & Error Handling
- Phone numbers must be exactly 10 digits
- Duplicate contacts prevented (same name + same number)
- Custom exceptions for clear error messages
- Press 'H' to navigate back to menus

## 🛠️ Technology Stack

- **.NET 9.0** - Latest .NET framework
- **C# 12** - Modern C# features
- **Microsoft.Extensions.DependencyInjection** - DI container
- **System.Text.Json** - JSON serialization
- **LINQ** - Data querying

## 📦 Installation & Running

### Prerequisites
- .NET 9.0 SDK or higher

### Clone & Run
```bash
git clone https://github.com/sanjeet9271/C-__Training.git
cd C#_Tutorials/SphoneApp
dotnet restore
dotnet run
```

### Build Only
```bash
dotnet build
```

## 📚 Code Flow Explanation

### 1. **Application Startup** (`Program.cs`)

```csharp
// 1. Build DI container
var serviceProvider = BuildServiceProvider();

// 2. Get AppRunner from DI
var appRunner = serviceProvider.GetRequiredService<AppRunner>();

// 3. Run the application
appRunner.Run();
```

### 2. **Service Registration**

```csharp
services.AddSingleton<IHistoryService, HistoryService>();
services.AddSingleton<IDialingService, DialingService>();
services.AddSingleton<IContactService, ContactService>();
services.AddSingleton<ContactsManager>();
services.AddSingleton<AppRunner>();
```

### 3. **Dependency Injection in Action**

```csharp
// AppRunner receives all dependencies via constructor
public AppRunner(
    IDialingService dialingService,
    IHistoryService historyService,
    ContactsManager contactsManager)
{
    _dialingService = dialingService;
    _historyService = historyService;
    _contactsManager = contactsManager;
}
```

### 4. **Service Interaction**

```csharp
// DialingService depends on HistoryService
public class DialingService : IDialingService
{
    private readonly IHistoryService _historyService;
    
    public void DialNumber(string number)
    {
        // Validate number
        // Display "Dialing..."
        // Add to history via injected service
        _historyService.AddNumberToHistory(number);
    }
}
```

## 🎨 Benefits of This Architecture

### ✅ **Testability**
- Easy to unit test with mocked dependencies
- Each service can be tested in isolation

### ✅ **Maintainability**
- Clear structure makes code easy to navigate
- Changes to one layer don't affect others

### ✅ **Scalability**
- Easy to add new features
- Can swap implementations without changing consumers

### ✅ **Readability**
- Code is organized by responsibility
- Interfaces document contracts

### ✅ **Professional**
- Industry-standard patterns
- Production-ready structure

## 📊 Example Usage Flow

```
User runs application
    ↓
Program.cs sets up DI
    ↓
AppRunner.Run() shows main menu
    ↓
User selects "1. Dial a Number"
    ↓
AppRunner.DialNumber() prompts for input
    ↓
Calls dialingService.DialNumber(number)
    ↓
DialingService validates number
    ↓
If valid: displays "Dialing..." and calls historyService.AddNumberToHistory()
    ↓
HistoryService adds to _callHistory list
    ↓
Saves to call_history_db.json
    ↓
User presses H to return to menu
```

## 🔒 Data Models

### **Contact**
```json
{
  "Name": "John Doe",
  "Number": {
    "No": "1234567890",
    "Type": 0
  }
}
```

### **CallHistoryEntry**
```json
{
  "PhoneNumber": "1234567890",
  "CalledAt": "2025-11-11T10:30:00"
}
```

## 🚧 Future Enhancements

- [ ] Export contacts to CSV/Excel
- [ ] Import contacts from file
- [ ] Call duration tracking
- [ ] Contact groups/favorites
- [ ] Search with advanced filters
- [ ] Unit tests for all services
- [ ] Configuration from appsettings.json

## 👨‍💻 Development

### Adding a New Service

1. Create interface in `Services/IYourService.cs`
2. Implement in `Services/YourService.cs`
3. Register in `Program.cs` DI setup
4. Inject into `AppRunner` or `ContactsManager`
5. Use in your menu logic

### Adding a New Model

1. Create class in `Models/YourModel.cs`
2. Define properties with `{ get; set; }`
3. Use in your services

## 📝 License

Educational project for learning C# and software architecture.

## 👤 Author

**Ayansh** - Learning professional C# development

---

*Last Updated: November 11, 2025*
*Architecture: Clean Layered Architecture with Dependency Injection*
