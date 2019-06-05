using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxiApplication.Level;
using SpaceTaxiApplication.Taxi;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxiApplication.States {
    public class GameRunning : IGameState{
        public GameEventBus<object> EventBus;
        private EntityContainer eList;
        private Entity backGroundImage;
        private Player player;
        private Level.Level level;
        public LevelParser LevelParser;
        private LevelRender levelRender;
        private Text[] score;
        private int currentScore;
        private static GameRunning instance;
        public static LevelController LevelController;
        private string levelFileName;
        
        public GameRunning() {
            EventBus = States.EventBus.GetBus();
            player = new Player();
            EventBus.Subscribe(GameEventType.PlayerEvent, player);
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();
            // Level creation
            GameRunning.LevelController = StateMachine.LevelController;
            LevelParser = new LevelParser();
            levelFileName = GameRunning.LevelController.ReturnLevel();
            level = LevelParser.CreateLevel(levelFileName);
            levelRender = new LevelRender();
            eList = levelRender.LevelToEntityList(level);
            //end of level creation
            //score and timer display in-game
            score = new[] {
                new Text("Score: " + currentScore, new Vec2F(0.65f, 0.45f), new Vec2F(0.5f, 0.5f))};
            //initiated with a 0 score
            currentScore = 0;
            //text set to white
            foreach (var txt in score) {
                txt.SetColor(Color.WhiteSmoke);
            }
        }
        
        /// <summary>
        /// Get the instance of GameRunning.
        /// Uses singleton pattern, so there is only one.
        /// </summary>
        /// <returns>GameRunning</returns>
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        /// <summary>
        /// Checks if the player hits any wall, if it does the taxi dies, and the level is changed.
        /// Unless it's a platform, and the taxi is going slow enough.
        /// </summary>
        public void DetectCollisionWall() {
            foreach (Entity wall in eList) {

                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),wall.Shape).Collision) { //if player hits wall
                    int mapPosX = Convert.ToInt32(wall.Shape.Position.X*40);
                    int mapPosY = Convert.ToInt32(22-wall.Shape.Position.Y*23);
                    if (level.platforms.Contains(level.map[mapPosY][mapPosX]) //if hit object is a platform...
                        && Math.Sqrt(player.Velocity.X*player.Velocity.X+player.Velocity.Y*player.Velocity.Y) < 0.002f) { 
                        player.Velocity.Y = 0;
                        player.Landed = true;
                        player.CurrentPlatform = level.map[mapPosY][mapPosX];
                    } else { //player dies
                        player.Entity.DeleteEntity();
                        States.EventBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
                        player = new Player();
                        SetLevel("the-beach.txt");
                        EventBus.Subscribe(GameEventType.PlayerEvent, player);
                    }
                }
            }
        }

        /// <summary>
        /// Checks for player collisions with customer.
        /// If so, the customer is picked up.
        /// </summary>
        public void DetectCollisionCustomer() {
            foreach (Customer.Customer cust in level.customers) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), cust.Entity.Shape).Collision) {
                    cust.PickedUp = true;
                    player.HasCustomer = true;
                }
            }
        }

        public void GameLoop() {
        }
        public void InitializeGameState() {
        }      
        /// <summary>
        /// Updates the game by moving the player and checking for collisions with walls/platforms/customers/levelgates
        /// </summary>
        public void UpdateGameLogic() {
            player.UpdateTaxi();
            //delivery of customer
            if (player.Landed) {
                foreach (Customer.Customer cust in level.customers) {
                    char destination =
                        cust.DestinationPlatform[cust.DestinationPlatform.Length - 1];
                    if (cust.PickedUp && (destination == player.CurrentPlatform || destination == ' ') && !cust.Delivered) {
                        cust.Delivered = true;
                        cust.PickedUp = false;
                        currentScore += cust.ScoreForDelivery;
                        score[0].SetText("Score: " + currentScore);
                    }
                }
            }
            //change of level if gate is hit
            if (player.Entity.Shape.Position.Y > 0.95) {
                if (levelFileName == "the-beach.txt") {
                    levelFileName = "short-n-sweet.txt";
                } else {
                    levelFileName = "the-beach.txt";
                }
                List<Customer.Customer> customers = level.customers;
                SetLevel(levelFileName);              
                //transfer of customers between levels
                foreach (Customer.Customer customer in customers){
                    if (customer.PickedUp){
                        if (customer.DestinationPlatform == "^") {
                            customer.DestinationPlatform = "^ "; // "^ " signifies any platform in THIS level
                        }
                        level.customers.Add(customer);
                    }
                }
            }
            DetectCollisionWall();
            DetectCollisionCustomer();
        }
        
        /// <summary>
        /// Renders all entities.
        /// </summary>
        public void RenderState() {
            if (!player.Entity.IsDeleted()) {
                player.RenderPlayer();
            }
            foreach (var customer in level.customers) {
                if (!customer.PickedUp && !customer.Delivered) {
                    if (levelFileName == "the-beach.txt") {
                        if (StaticTimer.GetElapsedSeconds() >= 10) {
                            customer.Entity.RenderEntity();
                        }
                    }
                    if (levelFileName == "short-n-sweet.txt") {
                        if (StaticTimer.GetElapsedSeconds() >= 5 && StaticTimer.GetElapsedSeconds() < 60) {
                            customer.Entity.RenderEntity();
                        }
                    }
                }
            }
            eList.RenderEntities();
            foreach (var txt in score) {
                txt.RenderText();
            }
        }
        
        /// <summary>
        /// Changes the level, given a level file name.
        /// </summary>
        /// <param name="fileName">filename of a text document, containing level data</param>
        public void SetLevel(string fileName) {  
            foreach (var cust in level.customers) {
                if (cust.PickedUp && player.HasCustomer) {
                    player.Customer = cust;
                }
            }
            level = LevelParser.CreateLevel(fileName);
            eList = levelRender.LevelToEntityList(level);
            player.SetPosition(0.45f, 0.6f);
            player.Velocity.X = 0.0f;
            player.Velocity.Y = 0.00f;
            StaticTimer.RestartTimer();
        }

        /// <summary>
        /// Handles key events
        /// </summary>
        /// <param name="keyValue">The type of key action.</param>
        /// <param name="keyAction">The button used in the key event.</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {
          switch (keyValue) {
            case "KEY_RELEASE":
                switch (keyAction) {

                    case "KEY_LEFT":
                        States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                                        break;
                    case "KEY_RIGHT":
                                        States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                                        break;
                    case "KEY_UP":
                                        States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                                        break;
                }
                break;
            case "KEY_PRESS":
                switch (keyAction) {
                                  case "KEY_UP":
                                      States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                                        break;
                                    case "KEY_LEFT":
                                        States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                                        break;
                                    case "KEY_RIGHT":
                                        States.EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                                        break;

                                  case "KEY_F1":
                                      Console.WriteLine("Changing level to THE BEACH");
                                      SetLevel("the-beach.txt");
                                      break;

                                  case "KEY_F2":
                                      Console.WriteLine("Changing level to THE BEACH");
                                      SetLevel("short-n-sweet.txt");
                                      break;
                                  case "KEY_ESCAPE":
                                      States.EventBus.GetBus().RegisterEvent(
                                          GameEventFactory<object>.CreateGameEventForAllProcessors(
                                              GameEventType.GameStateEvent,
                                              this,
                                              "CHANGE_STATE",
                                              "GAME_PAUSED", ""));
                                      break;
                }
                break;
          }
        }
    }
}
