using CardProblem.CardProblem;
using Xunit;

namespace CardProblem.Tests
{
    public class ConfigExpectation
    {
        public CardGameState Expect { get; set; }
        public CardRowConfiguration Config { get; set; }

        public ConfigExpectation(int row1, int row2, int row3, CardGameState expect)
        {
            Config = new CardRowConfiguration(row1, row2, row3);
            Expect = expect;
        }
    }

    public class CardRowConfigurationTests
    {
        [Theory]
        [InlineData( 1, 1,  0,  CardGameState.PredictWin)]
        [InlineData( 1, 1,  1,  CardGameState.PredictLose)]
        [InlineData( 1, 0,  1,  CardGameState.PredictWin)]
        [InlineData( 0, 1,  1,  CardGameState.PredictWin)]
        [InlineData( 2, 2,  0,  CardGameState.PredictLose)]
        [InlineData(54, 54, 0,  CardGameState.PredictLose)]
        [InlineData( 2, 2,  1,  CardGameState.PredictWin)]
        [InlineData(54, 54, 13, CardGameState.PredictWin)]
        [InlineData(54, 23, 0,  CardGameState.PredictWin)]
        [InlineData( 1, 2,  3,  CardGameState.PredictLose)]
        [InlineData( 1, 2,  34, CardGameState.PredictWin)]
        [InlineData( 2, 3,  13, CardGameState.PredictWin)]
        [InlineData( 3, 1,  15, CardGameState.PredictWin)]
        public void AllKnownCasesPassWinCheck(int row1, int row2, int row3, CardGameState expect)
        {
            var expectation = new ConfigExpectation(row1, row2, row3, expect);

            var state = expectation.Config.GetState();
            Assert.True(
                state == expect, 
                $"[!] Fail {expectation.Config}. Should be {expect}. Was {state}"
            );
        }
    }
}