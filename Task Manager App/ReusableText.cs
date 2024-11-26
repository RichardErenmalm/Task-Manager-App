using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager_App
{
    public class ReusableText
    {
        public static void ReturnToMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Press anywhere to return to menu");
            Console.ReadLine();
        }

        public static void TryAgain()
        {
            Console.WriteLine();
            Console.WriteLine("Something went wrong, please try again");
            Console.WriteLine();

        }

        public static void Header(string headerName)
        {
            Console.Clear();
            AnsiConsole.Markup($"[bold blue]{headerName}[/]");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
