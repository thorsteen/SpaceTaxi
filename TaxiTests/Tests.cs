using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.States;
using SpaceTaxi_3.Taxi;
using SpaceTaxi_3.Timer;

namespace TaxiTests
{
    [TestFixture]
    public class Tests
    {
        private StateMachine stateMachine;
        private LevelController levelController;
        private DynamicShape shape;
        private TimerIndex time;
        private FindSymbolCoords symbol;
        private Player player;


        [Test]
        public void TestCreateLevel()
        {
            LevelParser lvlParser = new LevelParser();
            
            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreSame("THE BEACH", tempName);
        }
        

        [Test]
        public void TestFilePath()
        {
            LevelParser lvlParser = new LevelParser();

            Assert.AreEqual(System.IO.Directory.GetCurrentDirectory() + "/TaxiTests/Levels/the-beach.txt",
                lvlParser.GetLevelFilePath("the-beach.txt"));

        }

        [SetUp]
            public void InitiateStateMachine() {
                DIKUArcade.Window.CreateOpenGLContext();
                stateMachine = new StateMachine();
            
                EventBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                    GameEventType.GameStateEvent,
                    GameEventType.InputEvent
                });
                EventBus.GetBus().Subscribe(GameEventType.GameStateEvent,stateMachine);
                EventBus.GetBus().Subscribe(GameEventType.InputEvent,stateMachine);
                EventBus.GetBus().ProcessEvents();
                
            }

        [Test]
            public void TestInitailState() {
                Assert.That(stateMachine.ActivateState, Is.InstanceOf<MainMenu>());
            }

        [Test]
            public void TestEventGamePaused() {
                EventBus.GetBus().RegisterEvent(GameEventFactory<object>
                    .CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "CHANGE_STATE",
                        "GAME_PAUSED", ""));
            
                EventBus.GetBus().ProcessEventsSequentially();
                Assert.That(stateMachine.ActivateState, Is.InstanceOf<GamePaused>());
            }
        [Test]
            public void TestEventGameRunning() {
                EventBus.GetBus().RegisterEvent(GameEventFactory<object>
                    .CreateGameEventForAllProcessors(GameEventType.GameStateEvent, this, "CHANGE_STATE",
                        "GAME_RUNNING", ""));
            
                EventBus.GetBus().ProcessEventsSequentially();
                Assert.That(stateMachine.ActivateState, Is.InstanceOf<GameRunning>());
            }
            

        [Test]
        public void TransformStringToStateRunning() {
            
            Assert.AreEqual(GameStateType.GameRunning,StateTransformer.TransformStringToState("GAME_RUNNING"));
        }

        [Test]
        public void TransformStringToStatePaused() {
            Assert.AreEqual(GameStateType.GamePaused,StateTransformer.TransformStringToState("GAME_PAUSED"));
        }

        [Test]
        public void TransformStringToStateMainMenu() {
            Assert.AreEqual(GameStateType.MainMenu,StateTransformer.TransformStringToState("MAIN_MENU"));
        }
        
        [Test]
        public void TransformStringToStateChooseLevel() {
            Assert.AreEqual(GameStateType.MainMenu,StateTransformer.TransformStringToState("Choose_Level"));
        }

        [Test]
        public void TransformStateToStringRunning() {
            Assert.AreEqual("GAME_RUNNING",StateTransformer.TransformStateToString(GameStateType.GameRunning));
        }

        [Test]
        public void TransformStateToStringPaused() {
            Assert.AreEqual("GAME_PAUSED",StateTransformer.TransformStateToString(GameStateType
                .GamePaused));
        }

        [Test]
        public void TransformStateToStringMainMenu() {
            Assert.AreEqual("MAIN_MENU",StateTransformer.TransformStateToString(GameStateType.MainMenu));
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
        public void TestSetPoistionExtent0()
        {
            shape = new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(0.00f, 0.00f));
            
            player = new Player();
            
            player.SetPosition(0.00f, 0.00f);
            player.SetExtent(0.00f, 0.00f);
            
            Assert.AreEqual(shape, player);
            Assert.AreEqual(shape,player);

        }
        
        [Test]
        public void TestSetPoistionExtent99()
        {
            shape = new DynamicShape(new Vec2F(0.99f, 0.99f), new Vec2F(0.99f, 0.99f));
            
            player = new Player();
            
            player.SetPosition(0.99f, 0.99f);
            player.SetExtent(0.99f, 0.99f);

            
            Assert.AreEqual(shape, player);
            Assert.AreEqual(shape,player);

        }
        
        [Test]
        public void TestSetPoistionExtentminus99()
        {
            shape = new DynamicShape(new Vec2F(-0.99f, -0.99f), new Vec2F(-0.99f, -0.99f));
            
            player = new Player();
            
            player.SetPosition(-0.99f, -0.99f);
            player.SetExtent(-0.99f, -0.99f);

            
            Assert.AreEqual(shape, player);
            Assert.AreEqual(shape,player);

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
        public void TestTimer()
        {
            time = new TimerIndex();
            
            time.PrintSeconds(1,1);
            
            Assert.AreEqual(1, time.Timer);
            Assert.AreEqual(1, time.OneMin);

            
        }
        
        [Test]
        public void TestTimer0()
        {
            time = new TimerIndex();

            Assert.AreEqual(0, time.Timer);
            Assert.AreEqual(0, time.OneMin);

            
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