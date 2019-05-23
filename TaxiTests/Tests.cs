using System;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.States;

//using SpaceTaxi_3;

namespace TaxiTests
{
    [TestFixture]
    public class Tests
    {
        private StateMachine stateMachine;

        [Test]
        public void TestCreateLevel()
        {
            LevelParser lvlParser = new LevelParser();
            
            var tempName = lvlParser.CreateLevel("the-beach.txt").mapName;
            Assert.AreSame("THE BEACH", tempName);
        }

        [Test]
        public void TestMapLevel()
        {
            LevelParser lvlParser = new LevelParser();

            var tempMap = lvlParser.CreateLevel("the-beach.txt").map[15];

            Assert.AreEqual("A  ", tempMap);
        }

        [Test]
        public void TestFilePath()
        {
            LevelParser lvlParser = new LevelParser();

            Assert.AreEqual("/Users/Thor/Documents/GitHub/su19-SpaceTaxi-BertTheoThor/TaxiTests/Levels/the-beach.txt",
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
        public void TestSetLevel()
        {
            
        }

        [Test]
        public void TestSetPoistionExtent()
        {
            
        }

        [Test]
        public void TestCollisionDetection()
        {
            
        }

        [Test]
        public void TestLevelController()
        {
            
        }
        
    }
}