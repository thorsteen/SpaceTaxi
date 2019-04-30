using System;
using NUnit.Framework;
using SpaceTaxi_1;

namespace TaxiTests {
    [TestFixture]
    public class Tests {
        [Test]
        public void Test1() {
            Assert.True(true);
        }
       /* [SetUp]
        public void CreateGame() {
            Game game = new Game();
            game.levelParser.CreateLevel("shot-n-sweet.txt");
        }*/

        [Test]
        public void TestCreateLevel() {
            LevelParser lvlParser = new LevelParser();
            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreSame("the beach",tempName);
        }
    }
}