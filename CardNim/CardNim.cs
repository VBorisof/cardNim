using System;
using System.Threading;
using CardNim.Extensions;

namespace CardNim
{
    public class CardNim
    {
        private CardProblemSolver _solver = new CardProblemSolver();
        
        public void DisplayConfig(CardRowConfiguration config, bool isPlayerTurn)
        {
            Console.Clear();
            config.Draw();
            if (!isPlayerTurn)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }

        public Move GetUserMove(CardRowConfiguration configuration)
        {
            int row = -1;
            int maxRowValue = 0;
            int value = 0;
            
            var rowKey = new ConsoleKeyInfo();
            while (maxRowValue == 0)
            {
                Console.Write("Enter row [0 - 2] -- must have more than 0 items: ");
                rowKey = Console.ReadKey();
                Console.WriteLine();

                if (char.IsDigit(rowKey.KeyChar))
                {
                    row = int.Parse(rowKey.KeyChar.ToString());
                    if (row >= 0 && row <= 2)
                    {
                        maxRowValue = configuration.GetMaxRowValue(row);
                    }
                }
            }
            
            
            var valueKey = new ConsoleKeyInfo();
            while (!char.IsDigit(valueKey.KeyChar) 
                   || value < 1 || value > maxRowValue)
            {
                Console.Write($"How much to take? [1 - {maxRowValue}]: ");            
                valueKey = Console.ReadKey();
                Console.WriteLine();
                if (char.IsDigit(valueKey.KeyChar))
                {
                    value = int.Parse(valueKey.KeyChar.ToString());
                }
            }

            var move = new Move(3)
            {
                RowSubtracts = {[row] = value}
            };

            return move;
        }
        
        public bool Play(CardRowConfiguration startConfig, bool isPlayerTurn)
        {
            var config = startConfig;

            DisplayConfig(config, isPlayerTurn);
            while (config.GetState() != CardGameState.Win && config.GetState() != CardGameState.Lose)
            {
                var move = 
                    isPlayerTurn ? 
                    GetUserMove(config) : 
                    _solver.GetBestMove(config);
                config = config.Move(move);
                isPlayerTurn = !isPlayerTurn;
                DisplayConfig(config, isPlayerTurn);
            }

            var state = config.GetState();
            switch (state)
            {
                case CardGameState.Win:
                    return isPlayerTurn;
                case CardGameState.Lose:
                    return !isPlayerTurn;
            }
            
            throw new Exception("Invalid game state.");
        }
        
        public void Run()
        {
            ConsoleEx.WriteLine(@"
                                 Welcome to the card nim! 
Whenever you need to move, you have to take any nonzero number of cards from one of three rows.
                   You lose whenever you are the one to draw the last card.
                      You will play against a perfect computer opponent.
                                       Good luck...
", ConsoleColor.Red);

            var running = true;
            while (running)
            {   
                Console.WriteLine("Choose, who starts. Press 'i' so that you start or anything else to let computer start.");
                var isPlayerTurn = Console.ReadKey(intercept: true).Key == ConsoleKey.I;
                
                var won = Play(new CardRowConfiguration(3, 5, 7), isPlayerTurn);
                Console.WriteLine(won ? "Congradulations! You Won!" : "Sorry! You Lost!");

                Console.WriteLine("Enter 'x' to quit or anything else to play again.");
                running = Console.ReadKey(intercept: true).Key != ConsoleKey.X;
            }
        }
    }
}