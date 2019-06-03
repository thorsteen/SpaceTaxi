using System.Collections.Generic;
using DIKUArcade.EventBus;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.States;

namespace TaxiTests
{
    public class StateTest
    {
        private StateMachine stateMachine;
        private LevelController levelController;
        private FindSymbolCoords symbol;
        
        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();
            stateMachine = new StateMachine();
            var eventBus = EventBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent
            });
            eventBus.Subscribe(GameEventType.GameStateEvent,stateMachine);
            eventBus.Subscribe(GameEventType.InputEvent,stateMachine);
        }
        /// <summary>
        /// Skal fikses med process event eller nye states machines hele vejen igennem
        /// </summary>


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
    }
}