using DIKUArcade.State;

namespace SpaceTaxi_2.States {
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