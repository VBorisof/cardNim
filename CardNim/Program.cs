using System;
using CSPlaypen.Extensions;

namespace CardProblem
{
    static class Program
    {
        public static void Main()
        {
            var time = DateTime.Now;
            ConsoleEx.WriteLine($@"
[~] == CARD GAME v.0.1. PPR Republic. ==== {time:HH:mm:ss} == [~]
", ConsoleColor.Green);
 
            time = DateTime.Now;
            new Playpen().Run();
            var timePassed = DateTime.Now - time;

            ConsoleEx.WriteLine($@"
[~] == DONE in {timePassed.TotalSeconds:0.000000}s ============================= [~]
", ConsoleColor.Green);

            Console.Read();
            Console.WriteLine("Bye!");
        }
    }
}