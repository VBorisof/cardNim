using System;
using System.Collections.Generic;
using System.Linq;

namespace CardNim
{   
    public class CardRowConfiguration
    {
        public int Row1 { get; set; }
        public int Row2 { get; set; }
        public int Row3 { get; set; }

        public int[] All => new[]
        {
            Row1, Row2, Row3
        };

        public int GetMaxRowValue(int row)
        {
            switch (row)
            {
                case 0:
                    return Row1;
                case 1:
                    return Row2;
                case 2:
                    return Row3;
            }
            throw new ArgumentException($"Invalid row {row}!");
        }
        
        public CardRowConfiguration(int row1, int row2, int row3)
        {
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
        public CardRowConfiguration(CardRowConfiguration other)
        {
            Row1 = other.Row1;
            Row2 = other.Row2;
            Row3 = other.Row3;
        }

        public bool Equal(int row1, int row2, int row3)
        {
            if (row1 == Row1 && row2 == Row2 && row3 == Row3)
            {
                return true;
            }
            if (row1 == Row1 && row2 == Row3 && row3 == Row2)
            {
                return true;
            }
            
            if (row1 == Row2 && row2 == Row1 && row3 == Row3)
            {
                return true;
            }
            if (row1 == Row2 && row2 == Row3 && row3 == Row1)
            {
                return true;
            }
            
            if (row1 == Row3 && row2 == Row1 && row3 == Row2)
            {
                return true;
            }
            if (row1 == Row3 && row2 == Row2 && row3 == Row1)
            {
                return true;
            }

            return false;
        }

        public void Draw()
        {
            Console.WriteLine("\n=====================================");
            Console.WriteLine($"   {Row1}        {Row2}        {Row3}");
            
            int row = 0;
            while (Row1 >= row || Row2 >= row || Row3 >= row)
            {
                if (row == 0)
                {
                    Console.Write(Row1 > 0 ? " _____   " : "         ");
                    Console.Write(Row2 > 0 ? " _____   " : "         ");
                    Console.Write(Row3 > 0 ? " _____   " : "         ");
                }
                else
                {
                    Console.Write(Row1 >= row ? "|_____|  " : "         ");
                    Console.Write(Row2 >= row ? "|_____|  " : "         ");
                    Console.Write(Row3 >= row ? "|_____|  " : "         ");
                }
                
                Console.WriteLine();
                ++row;
            }
            Console.WriteLine("=====================================");
        }

        public CardGameState GetState()
        {
            if (All.Sum(x => x) == 1) return CardGameState.Lose;
            if (All.All(x => x == 0)) return CardGameState.Win;
            if (All.All(x => x == 1)) return CardGameState.PredictLose;
            
            if (Equal(1, 1, 0))
            {
                return CardGameState.PredictWin;
            }
        
            //
            // A 1-2-3 is known to be a loss.
            // Therefore any move to lead to a 1-2-3 is a win
            if (Equal(1, 2, 3))
            {
                return CardGameState.PredictLose;
            }
            
            
            if (All.Any(x => x == 1)
                && All.Any(x => x == 2)
                && !All.Any(x => x == 3))
            {
                return CardGameState.PredictWin;
            }
            
            if (All.Any(x => x == 2)
                && All.Any(x => x == 3)
                && !All.Any(x => x == 1))
            {
                return CardGameState.PredictWin;
            }
            
            if (All.Any(x => x == 1)
                && All.Any(x => x == 3)
                && !All.Any(x => x == 2))
            {
                return CardGameState.PredictWin;
            }
            ///////////////////////////////////////
            //
            
            
            // If any two columns are equal
            if (Row1 == Row2
                || Row1 == Row3
                || Row2 == Row3)
            {
                // If exactly 1 of the columns is 0, lose
                if (All.Count(x => x == 0) == 1)
                {
                    return CardGameState.PredictLose;
                }

                return CardGameState.PredictWin;
            }
        
            // If any column is 0
            if (All.Any(x => x == 0))
            {
                // If remaining columns are not equal, win and lose otherwise
                if (All.Where(x => x != 0).Distinct().Count() != 1)
                {
                    return CardGameState.PredictWin;
                }
            }
            
            return CardGameState.Neutral;
        }
        
        public List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            foreach (var row in All.Select((amount, index) => (amount, index)))
            {
                for (int i = 1; i <= row.amount; ++i)
                {
                    switch (row.index)
                    {
                        case 0:
                            possibleMoves.Add(new Move(3)
                            {
                                RowSubtracts = new []
                                {
                                    i, 0, 0
                                }
                            });
                            break;
                        case 1:
                            possibleMoves.Add(new Move(3)
                            {
                                RowSubtracts = new []
                                {
                                    0, i, 0
                                }
                            });
                            break;
                        case 2:
                            possibleMoves.Add(new Move(3)
                            {
                                RowSubtracts = new []
                                {
                                    0, 0, i
                                }
                            });
                            break;
                    }
                }
            }

            return possibleMoves;
        }

        public List<CardRowConfiguration> GetChildConfigurations()
        {
            var possibleConfigurations = new List<CardRowConfiguration>();
            foreach (var row in All.Select((amount, index) => (amount, index)))
            {
                for (int i = 1; i <= row.amount; ++i)
                {
                    switch (row.index)
                    {
                        case 0:
                            possibleConfigurations.Add(new CardRowConfiguration(this)
                            {
                                Row1 = Row1 - i
                            });
                            break;
                        case 1:
                            possibleConfigurations.Add(new CardRowConfiguration(this)
                            {
                                Row2 = Row2 - i
                            });
                            break;
                        case 2:
                            possibleConfigurations.Add(new CardRowConfiguration(this)
                            {
                                Row3 = Row3 - i
                            });
                            break;
                    }
                }
            }

            return possibleConfigurations;
        }


        public CardRowConfiguration Move(Move move)
        {
            return new CardRowConfiguration(this)
            {
                Row1 = Row1 - move.RowSubtracts[0],
                Row2 = Row2 - move.RowSubtracts[1],
                Row3 = Row3 - move.RowSubtracts[2],
            };
        }
        
        public override string ToString()
        {
            return $"({Row1}, {Row2}, {Row3})";
        }
    }
}