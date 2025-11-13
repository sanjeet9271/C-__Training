using SphoneApp.Models;

namespace SphoneApp.Interfaces;
public interface IExportService
{
    Task ExportContactsToFileAsync(List<Contact> contacts, string filePath);
}

