using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SpaceTaxi_3;

namespace TaxiTests
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void TestCreateLevel()
        {
            LevelParser lvlParser = new LevelParser();

            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreEqual("THE BEACH", tempName);
        }

        [Test]
        public void TestMapLevel()
        {
            LevelParser lvlParser = new LevelParser();

            var tempMap = lvlParser.CreateLevel("the-beach.txt").map[15];

            Assert.AreEqual("A                           a  po      B", tempMap);
        }

        [Test]
        public void TestFilePath()
        {
            LevelParser lvlParser = new LevelParser();

            Assert.AreEqual("/Users/Thor/Documents/GitHub/su19-SpaceTaxi-BertTheoThor/TaxiTests/Levels/the-beach.txt",
                lvlParser.GetLevelFilePath("the-beach.txt"));

        }
    }
}