using SeaFightToolkit.ClassicSeaFight.AI;
using SeaFightToolkit.Common.Models;
using SeaFightToolkit.Tests.TestHelpers;

namespace SeaFightToolkit.Tests
{
    public class StandartStrategyTests
    {
        [Test]
        public void OpponentMove_OneCellNotVisited()
        {
            (int correctX, int correctY) = (4, 4);
            var strategy = new StandartStrategy();
            var sea = SeaCreator.SeaWithOneNotVisitedCell();

            (int x, int y) = strategy.OpponentMove(sea, Enumerable.Empty<Ship>().ToList());

            Assert.That(correctX, Is.EqualTo(x), $"Некорректная координата X - {x}");
            Assert.That(correctY, Is.EqualTo(y), $"Некорректная координата Y - {y}");
        }

        [Test]
        public void OpponentMove_OneCellVisited()
        {
            (int incorrectX, int incorrectY) = (4, 4);
            var isVisitedTwice = false;
            var strategy = new StandartStrategy();
            var sea = SeaCreator.SeaWithOneVisitedCell();

            for (var i = 0; i < 99; i++) 
            {
                (int x, int y) = strategy.OpponentMove(sea, Enumerable.Empty<Ship>().ToList());
                if (incorrectX == x && incorrectY == y) 
                {
                    isVisitedTwice = true;
                }
            }

            Assert.That(isVisitedTwice, Is.False, $"Произведен ход по уже посещенной клетке с координатами X - {incorrectX}, Y - {incorrectY}");
        }
    }
}