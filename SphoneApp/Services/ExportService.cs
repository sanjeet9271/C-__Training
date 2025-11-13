using SphoneApp.Interfaces;
using SphoneApp.Models;

namespace SphoneApp.Services;

public class ExportService : IExportService
{
    public async Task ExportContactsToFileAsync(List<Contact> contacts, string filePath)
    {
        if (contacts == null || contacts.Count == 0)
        {
            throw new InvalidOperationException("No contacts available to export.");
        }

        var content = BuildExportContent(contacts);
        await File.WriteAllTextAsync(filePath, content);
    }

    private string BuildExportContent(List<Contact> contacts)
    {
        var content = new System.Text.StringBuilder();

        for (int i = 0; i < contacts.Count; i++)
        {
            var contact = contacts[i];
            content.AppendLine($"Contact {(char)('A' + i)}) {contact.Name}");

            if (contact.PhoneNumbers.Count > 0)
            {
                foreach (var phone in contact.PhoneNumbers)
                {
                    content.AppendLine($"  Number: {phone.PhoneNo} ({phone.Type})");
                }
            }
            else
            {
                content.AppendLine("  No phone numbers");
            }

            content.AppendLine();
        }

        return content.ToString();
    }
}

