using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;


namespace SpaceTaxi_2.States {
    public class ChooseLevel : IGameState {
        private static ChooseLevel instance = null;

        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButton;

        public static ChooseLevel GetInstance() {
            return ChooseLevel.instance ?? (ChooseLevel.instance = new ChooseLevel());
        }

        public ChooseLevel() {
            menuButtons = new Text[] {
                new Text("Beach level", new Vec2F(0.3f, 0.1f), new Vec2F(0.5f, 0.5f)),
                new Text("Short 'n Sweet", new Vec2F(0.40f, 0.0f), new Vec2F(0.5f, 0.5f))
            };
            foreach (var Text in menuButtons) {
                Text.SetColor(Color.White);
            }

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

        }

        public void Chooser() {
            if (activeMenuButton == 0) {
                menuButtons[0].SetColor(Color.Green);
                menuButtons[1].SetColor(Color.White);
            } else if (activeMenuButton == 1) {
                menuButtons[1].SetColor(Color.Green);
                menuButtons[0].SetColor(Color.White);
            }
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            Chooser();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (var Text in menuButtons) {
                Text.RenderText();
            }

        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_PRESS":
                switch (keyAction) {
                case "KEY_UP":
                    activeMenuButton = 1;
                    break;
                case "KEY_DOWN":
                    activeMenuButton = 0;
                    break;
                case "KEY_ENTER":
                    if (activeMenuButton == 1) {
                        /// mangler ordentlig implementation 
                        EventBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));

                    } else {
                        /// mangler ordentlig implementation 
                        EventBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));
                
                    }

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

