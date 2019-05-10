using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxi_2.Taxi;

namespace SpaceTaxi_2.States {
    public class GameRunning : IGameState{
        
        public GameEventBus<object> eventBus;
        private List<Entity> EList;
        private Entity backGroundImage;
        
        private Player player;
        public Customer customer;
        private Level level;
        public LevelParser levelParser;
        private LevelRender levelRender;
        
        private static GameRunning instance = null;

        public GameRunning() {
            eventBus = EventBus.GetBus();
            
            levelParser = new LevelParser();
            level = levelParser.CreateLevel("the-beach.txt");
            
            player = new Player();
            player.SetPosition(0.45f, 0.6f);
            player.SetExtent(0.08f, 0.08f);
            
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
            
            levelRender = new LevelRender();
            EList = levelRender.LevelToEntityList(level);
            
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();
            
            /// customer
            customer = new Customer("Hello",new DynamicShape(new Vec2F(0.45f,0.1f),new Vec2F(0.1f,0.1f)),
                new Image(Path.Combine("Assets","Images","CustomerStandLeft.png")));
            
        }

        
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        public void GameLoop() {
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            player.UpdateTaxi();

        }

        public void RenderState() {
            foreach (var texture in EList) {
                texture.RenderEntity();
                player.RenderPlayer();
            }
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
                }

                break;
          }
        }
    }
}