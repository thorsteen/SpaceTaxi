using System;
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
        
        public GameEventBus<object> eventBus;
        private GameTimer gameTimer;
        
        private Window win;
        
        
        
        
        public StateMachine stateMachine;

        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);

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
            

            // game entities
            

            // event delegation
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            
            
            /// level rendering
            
            
            
            
            ///
            stateMachine = new StateMachine();
            
        }
        
        
        
        
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    stateMachine.ActivateState.UpdateGameLogic();
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