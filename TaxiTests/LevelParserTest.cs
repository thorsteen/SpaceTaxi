using System;
using DIKUArcade.Entities;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.States;
using SpaceTaxi_3.Taxi;
using SpaceTaxi_3.Timer;

namespace TaxiTests
{
    public class LevelParserTest
    {
        private LevelController levelController;
        private FindSymbolCoords symbol;
        private LevelParser lvlParser;

        [Test]
        public void TestCreateLevel()
        {
            var level = new LevelParser().CreateLevel("short-n-sweet.txt");
            Assert.AreSame(level.mapName, "SHORT -N- SWEET");
            /*Level level1 = new LevelParser().CreateLevel("the-beach.txt");
            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreSame("THE BEACH", level1.mapName);*/
        }

        [Test]
        public void TestFilePath()
        {
            lvlParser = new LevelParser();

            Assert.AreEqual(System.IO.Directory.GetCurrentDirectory() + "/TaxiTests/Levels/the-beach.txt",
                lvlParser.GetLevelFilePath("the-beach.txt"));

        }

        [Test]
        public void TestSetLevel0()
        {
            levelController = new LevelController();

            levelController.setLevel(0);

            Assert.AreEqual("short-n-sweet.txt",levelController.returnLevel());
        }

        [Test]
        public void TestSetLevel1()
        {
            levelController = new LevelController();

            levelController.setLevel(1);

            Assert.AreEqual("the-beach.txt",levelController.returnLevel());
        }
        
        [Test]
        public void TestFindChar()
        {
            LevelParser lvlParser = new LevelParser();

            symbol = new FindSymbolCoords();

            string[] tempMap = new string[]{lvlParser.CreateLevel("the-beach.txt").map[15]};

            Assert.AreEqual('A', FindSymbolCoords.Find(tempMap, 'A'));

        }

        [Test]
        public void TestDontFindChar()
        {
            LevelParser lvlParser = new LevelParser();

            symbol = new FindSymbolCoords();

            string[] tempMap = new string[]{lvlParser.CreateLevel("the-beach.txt").map[15]};

            Assert.AreNotEqual('Æ', FindSymbolCoords.Find(tempMap, 'Æ'));

        }
        
        [Test]
        public void TestMapLevel()
        {
            LevelParser lvlParser = new LevelParser();

            var tempMap = lvlParser.CreateLevel("the-beach.txt").map[15];
            
            Assert.IsInstanceOf<String>(tempMap);
        }



    }
}