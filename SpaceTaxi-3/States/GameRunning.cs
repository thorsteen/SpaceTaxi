using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxi_3.Taxi;
using SpaceTaxi_3.Timer;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_3.States {
    public class GameRunning : IGameState{

        public GameEventBus<object> eventBus;
        
        private EntityContainer EList;
        
        private Entity backGroundImage;
        
        private Player player;
        
        private Level level;
        
        public LevelParser levelParser;
        
        private LevelRender levelRender;
        
        private Text[] score;
        
        private int currentScore;
        
        private System.Timers.Timer timer;
        
        private TimerIndex TIMER;
        
        private StaticTimer tid;

        private static GameRunning instance;

        public static LevelController levelController;

        private string levelFileName;

        public GameRunning() {
            eventBus = EventBus.GetBus();
            
            tid = new StaticTimer();

            player = new Player();

            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            
            backGroundImage.RenderEntity();
            
            // Level creation
            levelController = StateMachine.levelController;
            
            levelParser = new LevelParser();
            
            levelFileName = levelController.returnLevel();
            
            level = levelParser.CreateLevel(levelFileName);
            
            levelRender = new LevelRender();
            
            EList = levelRender.LevelToEntityList(level);
            //end of level creation

            TIMER = new TimerIndex();

            score = new Text[] {
                new Text("Score: " + currentScore, new Vec2F(0.65f, 0.45f), new Vec2F(0.5f, 0.5f)),
                new Text("Timer: " + TIMER.Timer, new Vec2F(0.65f,0.35f),new Vec2F(0.5f,0.5f))};

            currentScore = 0;

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
            foreach (Entity wall in EList) {

                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),wall.Shape).Collision) { //if player hits wall
                    int mapPosX = Convert.ToInt32(wall.Shape.Position.X*40);
                    int mapPosY = Convert.ToInt32(22-wall.Shape.Position.Y*23);

                    if (level.platforms.Contains(level.map[mapPosY][mapPosX]) //if hit object is a platform...
                        && Math.Sqrt(player.Velocity.X*player.Velocity.X+player.Velocity.Y*player.Velocity.Y) < 0.002f) { //...and not moving too fast
                        player.Velocity.Y = 0;

                        player.Landed = true;
                        player.CurrentPlatform = level.map[mapPosY][mapPosX];
                    } else { //player dead
                        player.Entity.DeleteEntity();
                        Console.WriteLine("Your are dead!");
                        EventBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
                        player = new Player();
                        SetLevel(levelFileName);

                        eventBus.Subscribe(GameEventType.PlayerEvent, player);
                    }
                }
            }
        }
        
        /// <summary>
        /// Delivers the customer if taxi on correct platform.
        /// </summary>
        public void CheckCustomer() {
            string custDest = player.Customer.destinationPlatform;
            string playerDest = new string (player.CurrentPlatform,1);
            if (custDest == playerDest) {
                player.Customer.delivered = true;
                player.Customer.pickedUp = false;
                currentScore += player.Customer.scoreForDelivery;
            }           
        }

        /// <summary>
        /// Checks for player collisions with customer.
        /// If so, the customer is picked up.
        /// </summary>
        public void DetectCollisionCustomer() {
            foreach (Customer cust in level.customers) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), cust.Entity.Shape).Collision) {
                    cust.pickedUp = true;
                    player.hasCustomer = true;
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
            if (player.Landed) {
                foreach (Customer cust in level.customers) {
                    char destination =
                        cust.destinationPlatform[cust.destinationPlatform.Length - 1];
                    if (cust.pickedUp && destination == player.CurrentPlatform && !cust.delivered) {
                        cust.delivered = true;
                        cust.pickedUp = false;
                        currentScore += cust.scoreForDelivery;
                        score[0].SetText("Score: " + currentScore);
                    }
                }
            }
            if (player.Entity.Shape.Position.Y > 0.95) {
                if (levelFileName == "the-beach.txt") {
                    levelFileName = "short-n-sweet.txt";
                } else {
                    levelFileName = "the-beach.txt";
                }
                SetLevel(levelFileName);
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
                if (!customer.pickedUp && !customer.delivered) {
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
            EList.RenderEntities();
            foreach (var txt in score) {
                txt.RenderText();
            }
        }
        
        /// <summary>
        /// Changes the level, given a level file name.
        /// </summary>
        /// <param name="levelFileName">filename of a text document, containing level data</param>
        public void SetLevel(string levelFileName) {  
            foreach (var cust in level.customers) {
                if (cust.pickedUp && player.hasCustomer) {
                    player.Customer = cust;
                }
            }
            level = levelParser.CreateLevel(levelFileName);
            EList = levelRender.LevelToEntityList(level);
            player.SetPosition(0.45f, 0.6f);
            player.Velocity.X = 0.0f;
            player.Velocity.Y = 0.00f;
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
                        EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                                        break;
                    case "KEY_RIGHT":
                                        EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                                        break;
                    case "KEY_UP":
                                        EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                                        break;
                }
                break;
            case "KEY_PRESS":
                switch (keyAction) {
                                  case "KEY_UP":
                                      EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                                        break;
                                    case "KEY_LEFT":
                                        EventBus.GetBus().RegisterEvent(
                                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                                        break;
                                    case "KEY_RIGHT":
                                        EventBus.GetBus().RegisterEvent(
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
                                      EventBus.GetBus().RegisterEvent(GameEventFactory<object>
                                          .CreateGameEventForAllProcessors(GameEventType.WindowEvent, this,
                                              "CLOSE_WINDOW", "", ""));
                                      break;
                }
                break;
          }
        }
    }
}
