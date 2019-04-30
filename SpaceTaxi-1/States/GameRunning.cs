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

namespace SpaceTaxi_1 {
    public class GameRunning : IGameState{
        
        private static GameRunning instance = null;
        
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        public void GameLoop() {
        }

        public void InitializeGameState() {
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
        }
    }
}