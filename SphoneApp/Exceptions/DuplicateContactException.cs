namespace SphoneApp.Exceptions;

public class DuplicateContactException : Exception
{
    public DuplicateContactException(string message) : base(message) { }
}

