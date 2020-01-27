using System;

namespace Shared
{
    /// <summary>
    /// Just a simple console printer.
    /// </summary>
    public class Print
    {
        public static void Message(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Line(ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("*************");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}