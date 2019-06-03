using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_3;
using SpaceTaxi_3.Taxi;

namespace TaxiTests {
    [TestFixture]
    public class PlayerTest {
        
        // Declaration of variables used in multiple tests
        private Player player;
        private GameEventBus<object> eventBus;
        private DynamicShape shape;

        [SetUp]
        public void SetUp() 
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
            Window.CreateOpenGLContext();
            player = new Player();
            eventBus = EventBus.GetBus();
            
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.ControlEvent,
                GameEventType.GameStateEvent,
                GameEventType.InputEvent,
                GameEventType.PlayerEvent,
                GameEventType.WindowEvent,
                GameEventType.TimedEvent
            });
        }

        [TestCase("Up")]
        [TestCase("LEFT")]
        [TestCase("RIGHT")]
        public void AffectedByGravityWhileUsingBoosterTest(string boosterType) 
        {
            Vec2F gravityResult = new Vec2F(0.0f, -0.00001f);
            
            player.SetPosition(0.0f,0.0f);

            switch (boosterType) {
                case "Up":
                    EventBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                    
                    break;
                case "LEFT":
                    EventBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                    break;
                case "RIGHT":
                    EventBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                    break;
                default:
                    break;
            }
            
            eventBus.ProcessEvents();

            Assert.IsTrue(player.Velocity == gravityResult);
        }

        [TestCase(0.0f, 0.0f)]
        [TestCase(0.1f, 0.1f)]
        [TestCase(-0.1f, -0.1f)]
        [TestCase(0.1f, -0.1f)]
        [TestCase(-0.1f, 0.1f)]
        public void ChangeVelocityTest(float x, float y) {
            
            // Arrange
            Vec2F force = new Vec2F(x,y);
            Vec2F startDir = ((DynamicShape) player.Entity.Shape).Direction.Copy();
            
            // Act
            player.Velocity = force;
            
            // Assert
            Assert.AreEqual((startDir + force).X, ((DynamicShape) player.Entity.Shape).Direction.X);
            Assert.AreEqual((startDir + force).Y, ((DynamicShape) player.Entity.Shape).Direction.Y);
        }
        
/*
        [Test]
        public void PickUpCustomerTest() {
            
            // Arrange
            LevelContainer levelContainer = LevelContainer.GetLevelContainer();
            levelContainer.SetActiveLevel("SHORT -N- SWEET");
            
            // Act
           
            eventBus.RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.TimedEvent, this, "ADD_CUSTOMER", "Alice", ""));
             
            eventBus.ProcessEvents();

            eventBus.RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.ControlEvent, this, "SWITCH_MODE", "PARK", "1"));
            
            eventBus.ProcessEvents();
            
            levelContainer.ActiveLevel.CheckPlatformCollisions(player);
            eventBus.ProcessEvents();
    
            // Assert
            Assert.IsNotNull(player.Passenger);

        }
        */

        [TestCase(0.0f, 0.0f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(0.99f, 0.99f)]
        [TestCase(-0.99f, -0.99f)]
        public void SetPositionTest(float x,  float y) 
        {
            Vec2F pos = new Vec2F(x,y);
            
            player.SetPosition(x, y);
            
            Assert.AreEqual(player.Entity.Shape.Position, pos);
        }
        
        [TestCase(0.0f, 0.0f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(0.99f, 0.99f)]
        [TestCase(-0.99f, -0.99f)]
        public void TestSetExtent(float x, float y)
        {
            shape = new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(x, y));

            player = new Player();

            player.SetExtent(x, y);

            Assert.AreEqual(shape.Extent, player.Entity.Shape.Extent);

        }


    }
}