namespace SphoneApp;
public class Program
{
    public static async Task Main(string[] args)
    {
        var app = new PhoneApplication();
        await app.RunAsync();
    }
}