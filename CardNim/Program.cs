using System;
using CardNim;
using CardNim.Extensions;

namespace CardNim
{
    static class Program
    {
        public static void Main()
        {
            var time = DateTime.Now;
            ConsoleEx.WriteLine($@"
[~] == CARD NIM ========================== {time:HH:mm:ss} == [~]
", ConsoleColor.Green);
 
            time = DateTime.Now;
            new CardNim().Run();
            var timePassed = DateTime.Now - time;

            ConsoleEx.WriteLine($@"
[~] == DONE in {timePassed.TotalSeconds:0.000000}s ============================= [~]
", ConsoleColor.Green);

            Console.Read();
            Console.WriteLine("Bye!");
        }
    }
}