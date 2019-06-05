using NUnit.Framework;
using SpaceTaxiApplication.Level;
using SpaceTaxiApplication.Taxi;

namespace SpaceTaxiApplicationTest
{
    [TestFixture]
    
    public class CustomerTests
    {    
        Player player = new Player();
        Level level = new LevelParser().CreateLevel("the-beach.txt");
        /// <summary>
        /// Checks if the player can pickup the Customer. Thereby also checking for the collision between Customer and player 
        /// </summary>
        [Test]
        public void CheckPickUp()
        {
            var customercoords = level.customers[0].MyCoords;
            player.SetPosition(customercoords.X,customercoords.Y);
            Assert.True(level.customers[0].PickedUp);

        }
        /// <summary>
        /// Checks for the customer end platform, and if the players current platform are the same. 
        /// </summary>
        [Test]
        public void CheckDelivered()
        {
            var customerDelivery = level.customers[0].DestinationPlatform;
            string playerDest = new string (player.CurrentPlatform,1);
            Assert.True(customerDelivery == playerDest);
        }

        [Test]
        public void CheckScore()
        {
            int addScore = 0;
            int score = level.customers[0].ScoreForDelivery;
            level.customers[0].Delivered = true;
            if (level.customers[0].Delivered)
            {
                addScore += score;
            }
            Assert.Greater(addScore,0,"Not greater");
        }
        
        
        
        
    }
}