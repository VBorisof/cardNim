using System.Linq;
using MoreLinq;

namespace CardNim
{
    public class Move
    {
        public int[] RowSubtracts { get; set; }
        
        public Move(int rows)
        {
            RowSubtracts = new int [rows];
        }

        public override string ToString()
        {
            return $"({string.Join(", ", RowSubtracts)}))";
        }
    }
    
    public class CardProblemSolver
    {
        // Get next best move
        public Move GetBestMove(CardRowConfiguration config)
        {
            return config.GetPossibleMoves()
                .MinBy(move => GetNodeScore(config.Move(move)))
                .First();
        }


        public int GetNodeScore(CardRowConfiguration config, int depth = 1)
        {
            var isPlayerTurn = depth % 2 != 0;

            var state = config.GetState();
            
            if (isPlayerTurn)
            {
                switch (state)
                {
                    case CardGameState.PredictLose:
                        return -depth;
                    case CardGameState.PredictWin:
                        return 100 - depth;
                    case CardGameState.Lose:
                        return -100;
                    case CardGameState.Win:
                        return 100;
                }
            }
            else
            {
                switch (state)
                {
                    case CardGameState.PredictLose:
                        return 100 - depth;
                    case CardGameState.PredictWin:
                        return -depth;
                    case CardGameState.Lose:
                        return 100;
                    case CardGameState.Win:
                        return -100;
                }
            }
            
            var children = config.GetChildConfigurations();
            
            return children.Select(c => GetNodeScore(c, depth + 1))
                .OrderBy(x => x < 0)
                .ThenByDescending(x => x)
                .First();
        }
    }
}