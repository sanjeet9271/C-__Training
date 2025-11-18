# C# Collections: Arrays, Lists, and Dictionaries

## Overview
Collections are data structures that store and organize multiple elements. C# provides various collection types, each suited for different scenarios.

---

## Arrays

### What is an Array?
An array is a fixed-size collection of elements of the same type stored in contiguous memory.

### Declaration and Initialization

```csharp
// Declaration
int[] numbers;

// Initialization
numbers = new int[5]; // Array of 5 integers (default values: 0)

// Declaration + Initialization
int[] numbers = new int[5];

// Inline initialization
int[] numbers = { 1, 2, 3, 4, 5 };
string[] names = { "John", "Jane", "Bob" };

// Using new with values
int[] numbers = new int[] { 1, 2, 3, 4, 5 };
```

### Accessing Elements
```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

// Access by index (0-based)
int first = numbers[0];    // 10
int third = numbers[2];    // 30

// Modify element
numbers[1] = 25;           // Changes 20 to 25

// Get length
int count = numbers.Length; // 5
```

### Multi-dimensional Arrays

#### 2D Array (Rectangular)
```csharp
// Declaration
int[,] matrix = new int[3, 4]; // 3 rows, 4 columns

// Initialization
int[,] matrix = {
    { 1, 2, 3, 4 },
    { 5, 6, 7, 8 },
    { 9, 10, 11, 12 }
};

// Access
int value = matrix[1, 2]; // 7 (row 1, column 2)

// Get dimensions
int rows = matrix.GetLength(0);    // 3
int cols = matrix.GetLength(1);    // 4
```

#### Jagged Array (Array of Arrays)
```csharp
// Declaration
int[][] jagged = new int[3][];

// Initialize each row separately
jagged[0] = new int[] { 1, 2 };
jagged[1] = new int[] { 3, 4, 5 };
jagged[2] = new int[] { 6 };

// Access
int value = jagged[1][2]; // 5
```

### Array Methods
```csharp
int[] numbers = { 5, 2, 8, 1, 9 };

// Sort
Array.Sort(numbers);        // { 1, 2, 5, 8, 9 }

// Reverse
Array.Reverse(numbers);     // { 9, 8, 5, 2, 1 }

// Find index
int index = Array.IndexOf(numbers, 5); // 2

// Clear (set to default values)
Array.Clear(numbers, 0, numbers.Length);

// Copy
int[] copy = new int[5];
Array.Copy(numbers, copy, numbers.Length);
```

### Limitations of Arrays
- **Fixed size** - cannot grow or shrink
- No built-in methods for adding/removing elements
- Need to know size at creation time

---

## List<T>

### What is a List?
A `List<T>` is a dynamic array that can grow or shrink in size. It's the most commonly used collection in C#.

### Declaration and Initialization

```csharp
// Empty list
List<int> numbers = new List<int>();

// With initial capacity (optimization)
List<int> numbers = new List<int>(100);

// With initial values
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Shorter syntax (C# 9+)
List<int> numbers = new() { 1, 2, 3, 4, 5 };
```

### Common Operations

#### Adding Elements
```csharp
List<string> names = new List<string>();

// Add single element
names.Add("John");
names.Add("Jane");

// Add multiple elements
names.AddRange(new[] { "Bob", "Alice" });

// Insert at specific position
names.Insert(1, "Mike"); // Insert at index 1
```

#### Removing Elements
```csharp
List<string> names = new List<string> { "John", "Jane", "Bob", "Alice" };

// Remove by value
names.Remove("Jane");         // Removes first occurrence

// Remove at index
names.RemoveAt(0);           // Removes "John"

// Remove all matching
names.RemoveAll(n => n.StartsWith("A")); // Removes "Alice"

// Clear all
names.Clear();
```

#### Accessing Elements
```csharp
List<int> numbers = new List<int> { 10, 20, 30, 40, 50 };

// By index
int first = numbers[0];      // 10
int third = numbers[2];      // 30

// Modify
numbers[1] = 25;

// Count
int count = numbers.Count;   // 5

// Check if contains
bool hasThirty = numbers.Contains(30); // true
```

#### Searching
```csharp
List<int> numbers = new List<int> { 10, 20, 30, 40, 50 };

// Find first matching element
int? found = numbers.Find(n => n > 25);  // 30

// Find all matching elements
List<int> greaterThan20 = numbers.FindAll(n => n > 20); // { 30, 40, 50 }

// Find index
int index = numbers.IndexOf(30);         // 2
int indexWhere = numbers.FindIndex(n => n > 35); // 3 (40)

// Check if any match
bool hasLarge = numbers.Any(n => n > 45); // true

// Check if all match
bool allPositive = numbers.All(n => n > 0); // true
```

### Example from SphoneApp

```csharp
// Contact.cs
public class Contact
{
    public string Name { get; set; }
    public List<CustomProperty> PhoneNumbers { get; set; }

    public Contact()
    {
        Name = string.Empty;
        PhoneNumbers = new List<CustomProperty>(); // Initialize empty list
    }

    public void AddPhoneNumber(string phoneNo, ContactType type)
    {
        PhoneNumbers.Add(new CustomProperty(phoneNo, type));
    }

    public bool HasPhoneNumber(string phoneNo)
    {
        // Using LINQ with List
        return PhoneNumbers.Any(p => p.PhoneNo == phoneNo);
    }
}

// ContactsManager.cs
public List<Contact> SearchByName(string searchTerm)
{
    return _repository.GetAll()
        .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        .ToList(); // Convert IEnumerable to List
}

// JsonRepository.cs
public class JsonRepository<T> : IRepository<T> where T : class
{
    private List<T> _data; // List as internal storage

    public JsonRepository(string filePath)
    {
        _data = new List<T>();
        LoadData();
    }

    public List<T> GetAll()
    {
        return _data;
    }

    public void Add(T entity)
    {
        _data.Add(entity);
    }
}
```

### List vs Array

| Feature | Array | List<T> |
|---------|-------|---------|
| Size | Fixed | Dynamic |
| Performance | Faster (no overhead) | Slightly slower |
| Type Safety | Yes | Yes |
| Add/Remove | Manual | Built-in methods |
| LINQ Support | Yes | Yes |
| Use When | Size known, no changes | Size unknown, frequent changes |

---

## Dictionary<TKey, TValue>

### What is a Dictionary?
A dictionary stores key-value pairs, allowing fast lookup by key. Also called a hash table or map.

### Declaration and Initialization

```csharp
// Empty dictionary
Dictionary<string, int> ages = new Dictionary<string, int>();

// With initial values
Dictionary<string, int> ages = new Dictionary<string, int>
{
    { "John", 25 },
    { "Jane", 30 },
    { "Bob", 35 }
};

// Shorter syntax (C# 9+)
Dictionary<string, int> ages = new()
{
    ["John"] = 25,
    ["Jane"] = 30,
    ["Bob"] = 35
};
```

### Common Operations

#### Adding Elements
```csharp
Dictionary<string, string> phoneBook = new Dictionary<string, string>();

// Add with Add method (throws if key exists)
phoneBook.Add("John", "123-456-7890");

// Add with indexer (overwrites if key exists)
phoneBook["Jane"] = "987-654-3210";

// TryAdd (returns false if key exists, doesn't throw)
bool added = phoneBook.TryAdd("Bob", "555-123-4567");
```

#### Accessing Elements
```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    { "John", 25 },
    { "Jane", 30 }
};

// Direct access (throws if key doesn't exist)
int johnAge = ages["John"]; // 25

// Safe access with TryGetValue
if (ages.TryGetValue("Bob", out int bobAge))
{
    Console.WriteLine($"Bob is {bobAge}");
}
else
{
    Console.WriteLine("Bob not found");
}

// Check if key exists
bool hasJohn = ages.ContainsKey("John"); // true

// Check if value exists
bool hasTwentyFive = ages.ContainsValue(25); // true

// Get count
int count = ages.Count; // 2
```

#### Modifying Elements
```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    { "John", 25 }
};

// Update existing
ages["John"] = 26;

// Remove by key
ages.Remove("John");

// Clear all
ages.Clear();
```

#### Iterating
```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    { "John", 25 },
    { "Jane", 30 },
    { "Bob", 35 }
};

// Iterate over key-value pairs
foreach (KeyValuePair<string, int> entry in ages)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

// Shorter with var
foreach (var entry in ages)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

// Iterate over keys only
foreach (string name in ages.Keys)
{
    Console.WriteLine(name);
}

// Iterate over values only
foreach (int age in ages.Values)
{
    Console.WriteLine(age);
}

// Using LINQ
var adults = ages.Where(kvp => kvp.Value >= 30)
                 .Select(kvp => kvp.Key)
                 .ToList();
```

### Dictionary Example for SphoneApp

```csharp
// Example: Contact lookup by phone number
Dictionary<string, Contact> contactsByPhone = new Dictionary<string, Contact>();

// Build lookup dictionary
foreach (var contact in allContacts)
{
    foreach (var phoneNumber in contact.PhoneNumbers)
    {
        contactsByPhone[phoneNumber.PhoneNo] = contact;
    }
}

// Fast lookup
if (contactsByPhone.TryGetValue("1234567890", out Contact? contact))
{
    Console.WriteLine($"Found: {contact.Name}");
}

// Example: Count contacts by type
Dictionary<ContactType, int> countByType = new Dictionary<ContactType, int>();
foreach (var contact in allContacts)
{
    foreach (var phone in contact.PhoneNumbers)
    {
        if (countByType.ContainsKey(phone.Type))
        {
            countByType[phone.Type]++;
        }
        else
        {
            countByType[phone.Type] = 1;
        }
    }
}
```

---

## Other Collection Types

### HashSet<T>
Unordered collection of unique elements (no duplicates).

```csharp
HashSet<int> uniqueNumbers = new HashSet<int> { 1, 2, 3, 4, 5 };

// Add (returns false if already exists)
bool added = uniqueNumbers.Add(6);     // true
added = uniqueNumbers.Add(3);          // false (already exists)

// Set operations
HashSet<int> setA = new HashSet<int> { 1, 2, 3 };
HashSet<int> setB = new HashSet<int> { 3, 4, 5 };

setA.UnionWith(setB);        // { 1, 2, 3, 4, 5 }
setA.IntersectWith(setB);    // { 3 }
setA.ExceptWith(setB);       // { 1, 2 }
```

### Queue<T>
First-In-First-Out (FIFO) collection.

```csharp
Queue<string> queue = new Queue<string>();

queue.Enqueue("First");   // Add to end
queue.Enqueue("Second");
queue.Enqueue("Third");

string first = queue.Dequeue();  // Remove from front: "First"
string next = queue.Peek();      // Look at front without removing: "Second"

int count = queue.Count;         // 2
```

### Stack<T>
Last-In-First-Out (LIFO) collection.

```csharp
Stack<int> stack = new Stack<int>();

stack.Push(1);    // Add to top
stack.Push(2);
stack.Push(3);

int top = stack.Pop();      // Remove from top: 3
int nextTop = stack.Peek(); // Look at top without removing: 2

int count = stack.Count;    // 2
```

---

## LINQ with Collections

LINQ (Language Integrated Query) provides powerful methods for querying collections.

### Common LINQ Methods

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Where - Filter
var evens = numbers.Where(n => n % 2 == 0).ToList(); // { 2, 4, 6, 8, 10 }

// Select - Transform
var doubled = numbers.Select(n => n * 2).ToList(); // { 2, 4, 6, ..., 20 }

// OrderBy / OrderByDescending
var sorted = numbers.OrderByDescending(n => n).ToList(); // { 10, 9, 8, ..., 1 }

// First / FirstOrDefault
int first = numbers.First(n => n > 5);  // 6
int? firstOrNull = numbers.FirstOrDefault(n => n > 100); // null (default)

// Any / All
bool hasLarge = numbers.Any(n => n > 5);    // true
bool allPositive = numbers.All(n => n > 0); // true

// Count
int countGreaterThan5 = numbers.Count(n => n > 5); // 5

// Sum / Average / Min / Max
int sum = numbers.Sum();           // 55
double avg = numbers.Average();    // 5.5
int min = numbers.Min();           // 1
int max = numbers.Max();           // 10

// Take / Skip
var firstThree = numbers.Take(3).ToList();    // { 1, 2, 3 }
var skipTwo = numbers.Skip(2).ToList();       // { 3, 4, 5, ..., 10 }

// Distinct
List<int> dupes = new List<int> { 1, 2, 2, 3, 3, 3 };
var unique = dupes.Distinct().ToList(); // { 1, 2, 3 }

// GroupBy
var grouped = numbers.GroupBy(n => n % 2 == 0 ? "Even" : "Odd");
foreach (var group in grouped)
{
    Console.WriteLine($"{group.Key}: {string.Join(", ", group)}");
}
// Output: Odd: 1, 3, 5, 7, 9
//         Even: 2, 4, 6, 8, 10
```

### LINQ in SphoneApp

```csharp
// ContactsManager.cs

// Search by name (Where + ToList)
public List<Contact> SearchByName(string searchTerm)
{
    return _repository.GetAll()
        .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        .ToList();
}

// Search by number (Where + Any + ToList)
public List<Contact> SearchByNumber(string searchTerm)
{
    return _repository.GetAll()
        .Where(c => c.PhoneNumbers.Any(p => p.PhoneNo.Contains(searchTerm)))
        .ToList();
}

// Find contact (FirstOrDefault)
public Contact? FindContactByNumber(string phoneNumber)
{
    string cleanNumber = PhoneNumberUtils.CleanPhoneNumber(phoneNumber);
    return _repository.GetAll()
        .FirstOrDefault(c => c.HasPhoneNumber(cleanNumber));
}

// Check for duplicate (Any)
public bool IsDuplicateContact(string name, string number)
{
    string cleanedNumber = PhoneNumberUtils.CleanPhoneNumber(number);
    return _repository.GetAll().Any(c =>
        c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
        c.HasPhoneNumber(cleanedNumber));
}

// Contact.cs

// Has phone number (Any)
public bool HasPhoneNumber(string phoneNo)
{
    return PhoneNumbers.Any(p => p.PhoneNo == phoneNo);
}

// Get primary phone (FirstOrDefault)
public string? GetPrimaryPhoneNumber()
{
    return PhoneNumbers.FirstOrDefault().PhoneNo;
}
```

---

## Collection Best Practices

### 1. Choose the Right Collection

- **List<T>**: Default choice for ordered collections
- **Dictionary<TKey, TValue>**: Fast lookup by key
- **HashSet<T>**: Unique elements, no duplicates
- **Array**: Fixed size, performance-critical scenarios
- **Queue<T>**: FIFO operations
- **Stack<T>**: LIFO operations

### 2. Initialize with Capacity (Performance)

```csharp
// If you know approximate size
List<int> numbers = new List<int>(1000);
Dictionary<string, int> map = new Dictionary<string, int>(500);
```

### 3. Use Collection Initializers

```csharp
// Readable and concise
var names = new List<string> { "John", "Jane", "Bob" };
var ages = new Dictionary<string, int> 
{
    ["John"] = 25,
    ["Jane"] = 30
};
```

### 4. Use LINQ for Readability

```csharp
// Instead of loops
var adults = people.Where(p => p.Age >= 18).ToList();

// Instead of manual filtering
var names = contacts.Select(c => c.Name).ToList();
```

### 5. Avoid Modifying During Iteration

```csharp
// ❌ Bad - throws exception
foreach (var item in list)
{
    if (condition)
        list.Remove(item); // Can't modify during iteration
}

// ✅ Good - use ToList() to iterate over copy
foreach (var item in list.ToList())
{
    if (condition)
        list.Remove(item);
}

// ✅ Better - use RemoveAll
list.RemoveAll(item => condition);
```

## Summary

- **Arrays**: Fixed-size, fast, basic collections
- **List<T>**: Dynamic arrays, most commonly used
- **Dictionary<TKey, TValue>**: Key-value pairs, fast lookup
- **HashSet<T>**: Unique elements
- **Queue<T>/Stack<T>**: Specialized ordering (FIFO/LIFO)
- **LINQ**: Powerful querying for all collections

