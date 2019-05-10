﻿using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_2.States;
using SpaceTaxi_2.Taxi;

namespace SpaceTaxi_2 {
    public class Game : IGameEventProcessor<object> {
        private Entity backGroundImage;
        public GameEventBus<object> eventBus;
        private GameTimer gameTimer;
        private Player player;
        private Window win;
        private Level level;
        private List<Entity> EList;
        private LevelRender levelRender;
        public LevelParser levelParser;
        public Customer customer;
        public StateMachine stateMachine;

        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
            levelParser = new LevelParser();
            level = levelParser.CreateLevel("the-beach.txt");

            // event bus
            eventBus = EventBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent, // commands issued to the player object, e.g. move,
                                          // destroy, receive health, etc.
                GameEventType.GameStateEvent

            });
       

            win.RegisterEventBus(eventBus);

            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit

            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();

            // game entities
            player = new Player();
            player.SetPosition(0.45f, 0.6f);
            player.SetExtent(0.1f, 0.1f);

            // event delegation
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
            
            /// level rendering
            levelRender = new LevelRender();
            EList = levelRender.LevelToEntityList(level);
            
            /// customer
            customer = new Customer("Hello",new DynamicShape(new Vec2F(0.45f,0.1f),new Vec2F(0.1f,0.1f)),
                new Image(Path.Combine("Assets","Images","CustomerStandLeft.png")));
            
            ///
            stateMachine = new StateMachine();
            
        }
        
        
        public void SetLevel(string levelFileName) { //sets a level
            level = levelParser.CreateLevel(levelFileName);
            EList = levelRender.LevelToEntityList(level);
            
        }
        
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    
                }

                if (gameTimer.ShouldRender()) {
                    
                    win.Clear();
                    stateMachine.ActivateState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                 gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                win.CloseWindow();
                break;
            case "KEY_F11":
                Console.WriteLine("Change Level to: ");
                string newLevel = Console.ReadLine();
                Console.WriteLine("Changing level to " + newLevel);
                SetLevel(newLevel);
                Console.WriteLine(level.mapName);
                break;
                
            case "KEY_F12":
                Console.WriteLine("Saving screenshot");
                win.SaveScreenShot();
                break;
            
            }
        }

        

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            }
        }
    }
}