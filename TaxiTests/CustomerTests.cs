using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.Taxi;

namespace TaxiTests
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
            var customercoords = level.customers[0].myCoords;
            player.SetPosition(customercoords.X,customercoords.Y);
            Assert.True(level.customers[0].pickedUp);

        }
        /// <summary>
        /// Checks for the customer end platform, and if the players current platform are the same. 
        /// </summary>
        [Test]
        public void CheckDelivered()
        {
            var customerDelivery = level.customers[0].destinationPlatform;
            string playerDest = new string (player.CurrentPlatform,1);
            Assert.True(customerDelivery == playerDest);
        }

        [Test]
        public void CheckScore()
        {
            int addScore = 0;
            int score = level.customers[0].scoreForDelivery;
            level.customers[0].delivered = true;
            if (level.customers[0].delivered)
            {
                addScore += score;
            }
            Assert.Greater(addScore,0,"Not greater");
        }
        
        
        
        
    }
}