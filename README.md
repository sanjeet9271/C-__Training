# C# Tutorials

A collection of C# learning projects and applications.

## Projects

### 📱 SphoneApp

A simple phone application simulator built with C# console application.

#### Features
- **Dial a Number**: Call a 10-digit phone number with validation
- **View History**: View history of dialled numbers *(Coming Soon)*
- **Contacts**: Manage your contacts *(Coming Soon)*
- **Custom Exception Handling**: Robust error handling for user inputs
- **User-Friendly Navigation**: Press 'H' to return to home screen

#### How to Run

1. **Prerequisites**
   - .NET 9.0 SDK or higher
   - Visual Studio 2022 or Visual Studio Code

2. **Clone the Repository**
   ```bash
   git clone https://github.com/sanjeet9271/C-__Training.git
   cd C#_Tutorials
   ```

3. **Run the Application**
   ```bash
   cd SphoneApp
   dotnet run
   ```

#### Usage

When you run the application, you'll see a menu with options:

```
========================================
    Welcome to the Sphone App!
========================================

Please select an option:
1. Dial a Number
2. View History of Dialled Numbers
3. Contacts
4. Exit
```

- **Option 1**: Enter a 10-digit phone number to simulate a call
- **Option 2-3**: Features in development
- **Option 4**: Exit the application
- **Press H**: Return to home screen after any operation

#### Technical Details

- **Language**: C# 12.0
- **Framework**: .NET 9.0
- **Architecture**: Console Application
- **Error Handling**: Custom exceptions for validation
  - `InvalidPhoneNumberException`: Thrown when phone number is invalid
  - `FeatureNotSupportedException`: Thrown when accessing features under development

#### Code Highlights

- **Nullable Reference Types**: Uses `string?` for safe null handling
- **LINQ**: Uses `char.IsDigit` for phone number validation
- **Custom Exceptions**: Implements domain-specific exception classes
- **Input Validation**: Comprehensive validation for user inputs

## Project Structure

```
C#_Tutorials/
├── .gitignore
├── README.md
├── C#_Tutorials.sln
└── SphoneApp/
    ├── Program.cs
    └── SphoneApp.csproj
```

## Development

### Branch Strategy
- `main` - Production-ready code
- `dev` - Development branch for new features

### Contributing

1. Create a feature branch from `dev`
   ```bash
   git checkout dev
   git checkout -b feature/your-feature-name
   ```

2. Make your changes and commit
   ```bash
   git add .
   git commit -m "Add your feature description"
   ```

3. Push to the repository
   ```bash
   git push origin feature/your-feature-name
   ```

4. Create a Pull Request to `dev` branch

## Future Enhancements

- [ ] Implement call history tracking
- [ ] Add contacts management system
- [ ] Store data persistently (file/database)
- [ ] Add call duration timer
- [ ] Support for contact search and filtering

## License

This project is for educational purposes.

## Author

**Ayansh** - Learning C# Development

---

*Last Updated: November 10, 2025*

