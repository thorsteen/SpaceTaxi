using System;
using NUnit.Framework;
using SpaceTaxi_3;

namespace TestProject1 {
    [TestFixture]
    public class Tests {
        
        private LevelParser lvlParser;
        
        [Test]
        public void TestCreateLevel()
        {
            lvlParser = new LevelParser();

            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreEqual("THE BEACH", tempName);
        }
    }
}