using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.States;

namespace TaxiTests
{
    public class StateTest
    {
        private StateMachine stateMachine;
        private GameEventBus<object> eventBus;


        
        [SetUp]
        public void SetUp() {
            Window.CreateOpenGLContext();
            eventBus = EventBus.GetBus();
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }
        
        [TestCase("MainMenu", "MAIN_MENU")]
        [TestCase("ChooseLevel", "Choose_Level")]
        [TestCase("GameRunning", "GAME_RUNNING")]
        [TestCase("GamePaused", "GAME_PAUSED")]
        public void SwitchStateTest(string gameStateType, string newState) {
            
            IGameState testStateType;

            stateMachine = new StateMachine();
            
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.ControlEvent,
                GameEventType.GameStateEvent,
                GameEventType.InputEvent,
                GameEventType.PlayerEvent,
                GameEventType.WindowEvent
            });
            
            eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
            
            switch (gameStateType) {
                case "GameRunning":
                    testStateType = GameRunning.GetInstance();
                    break;
                case "GamePaused":
                    testStateType = GamePaused.GetInstance();
                    break;
                case "ChooseLevel":
                    testStateType = ChooseLevel.GetInstance();
                    break;
                case "MainMenu":
                    testStateType = MainMenu.GetInstance();
                    break;
                default:
                    testStateType = MainMenu.GetInstance();
                    break;
            }
            
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, 
                    stateMachine.ActivateState, 
                    "CHANGE_STATE",
                    newState, ""));
           
            
            eventBus.ProcessEvents();
            
            Assert.AreEqual(testStateType, stateMachine.ActivateState);
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
        
    }
}