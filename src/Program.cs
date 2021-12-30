namespace ducker;

internal class Program
{
    private static void Main(string[] args)
    {
        var bot = new Bot();
        bot.RunAsync().GetAwaiter().GetResult();
    }
}