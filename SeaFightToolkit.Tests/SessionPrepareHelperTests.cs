using SeaFightToolkit.ClassicSeaFight.Game;
using SeaFightToolkit.ClassicSeaFight.AI;
using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.Tests
{
    public class SessionPrepareHelperTests
    {
        [Test]
        public void MouseIn_IsNotWithinField()
        {
            var session = new ComputerSession(new Computer(new StandartStrategy()));
            var sessionPrepareHelper = new SessionPrepareHelper(session);
            var isIndifferent = false;
            sessionPrepareHelper.ChangeShipType(2);
            sessionPrepareHelper.IndifferentPositionEvent += tempShip => isIndifferent = true;

            sessionPrepareHelper.MouseIn(9, 0);

            Assert.That(isIndifferent, Is.True);
        }

        [Test]
        public void MouseIn_IsNotColliding()
        {
            var session = new ComputerSession(new Computer(new StandartStrategy()));
            var sessionPrepareHelper = new SessionPrepareHelper(session);
            var isGoodPosition = false;
            var newShip = new TwoDeck();
            newShip.CreateShip([(4, 2), (4, 3), (4, 4)], session.GetPlayerSea());
            sessionPrepareHelper.ChangeShipType(2);
            sessionPrepareHelper.ChangeShipOrientation();
            sessionPrepareHelper.GoodPositionEvent += tempShip => isGoodPosition = true;

            sessionPrepareHelper.MouseIn(2, 2);

            Assert.That(isGoodPosition, Is.True);
        }

        [Test]
        public void MouseIn_IsColliding()
        {
            var session = new ComputerSession(new Computer(new StandartStrategy()));
            var sessionPrepareHelper = new SessionPrepareHelper(session);
            var isGoodPosition = false;
            var newShip = new TwoDeck();
            newShip.CreateShip([(4, 2), (4, 3), (4, 4)], session.GetPlayerSea());
            sessionPrepareHelper.ChangeShipType(2);
            sessionPrepareHelper.ChangeShipOrientation();
            sessionPrepareHelper.GoodPositionEvent += tempShip => isGoodPosition = true;

            sessionPrepareHelper.MouseIn(4, 2);

            Assert.That(isGoodPosition, Is.False);
        }
    }
}
