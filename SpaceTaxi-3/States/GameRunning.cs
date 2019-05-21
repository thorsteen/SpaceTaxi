using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using SpaceTaxi_3.Taxi;

namespace SpaceTaxi_3.States {
    public class GameRunning : IGameState{

        public GameEventBus<object> eventBus;
        private EntityContainer EList;
        private Entity backGroundImage;

        private Player player;
        public Customer customer;
        private Level level;
        public LevelParser levelParser;
        private LevelRender levelRender;

        private static GameRunning instance = null;

        public static LevelController levelController;

        private string levelFileName;

        public GameRunning() {
            eventBus = EventBus.GetBus();


            player = new Player();
            player.SetPosition(0.45f, 0.6f);
            player.SetExtent(0.08f, 0.08f);

            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();

            /// customer
            customer = new Customer("Hello",new DynamicShape(new Vec2F(0.45f,0.1f),new Vec2F(0.1f,0.1f)),
                new Image(Path.Combine("Assets","Images","CustomerStandLeft.png")));

            /// Level creation
            levelController = StateMachine.levelController;
            levelParser = new LevelParser();
            levelFileName = levelController.returnLevel();
            level = levelParser.CreateLevel(levelFileName);
            levelRender = new LevelRender();
            EList = levelRender.LevelToEntityList(level);

        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        /// <summary>
        /// Checks if the player hits any wall ( so to say, the taxi is not able to land either ).
        /// </summary>
        public void DetectCollision() {
            foreach (Entity wall in EList) {

                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),wall.Shape).Collision) { //if player hits wall
                    int mapPosX = Convert.ToInt32(wall.Shape.Position.X*40);
                    int mapPosY = Convert.ToInt32(22-wall.Shape.Position.Y*23);

                    if (level.platforms.Contains(level.map[mapPosY][mapPosX]) //if hit object is a platform...
                        && Math.Sqrt(player.Velocity.X*player.Velocity.X+player.Velocity.Y*player.Velocity.Y) < 0.002f) { //...and not moving too fast
                        player.Velocity.Y = 0; //ved stadig ikke hvordan jeg får taxi'en til at lande ordentligt
                      
                        player.Landed = true;
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
                        player.SetPosition(0.45f, 0.6f);
                        player.SetExtent(0.08f, 0.08f);
                        eventBus.Subscribe(GameEventType.PlayerEvent, player);

                    }
                }
            }

        }

        public void GameLoop() {
        }

        public void InitializeGameState() {

        }

        public void UpdateGameLogic() {
            player.UpdateTaxi();
            if (player.Entity.Shape.Position.Y > 0.95) {
                if (levelFileName == "the-beach.txt") {
                    levelFileName = "short-n-sweet.txt";
                } else {
                    levelFileName = "the-beach.txt";
                }
                SetLevel(levelFileName);
            }
            DetectCollision();

        }

        public void RenderState() {
            if (!player.Entity.IsDeleted()) {
                player.RenderPlayer();
            }
            EList.RenderEntities();
        }

        public void SetLevel(string levelFileName) { //sets a level
            level = levelParser.CreateLevel(levelFileName);
            EList = levelRender.LevelToEntityList(level);
            player.SetPosition(0.45f, 0.6f);
            player.Velocity.X = 0.0f;
            player.Velocity.Y = 0.004f;
        }

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
