using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace jus1dBot
{
    class Program
    {   
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}