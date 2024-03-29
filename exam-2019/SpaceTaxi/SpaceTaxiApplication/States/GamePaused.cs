using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxiApplication.States {
    public class GamePaused : IGameState {
        private static GamePaused instance;

        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        
        public GamePaused() {
            menuButtons = new[] {
                new Text("Continue", new Vec2F(0.3f, 0.1f), new Vec2F(0.5f, 0.5f)),
                new Text("Main Menu", new Vec2F(0.4f, 0.0f), new Vec2F(0.5f, 0.5f)),
                new Text("Press ESC: Quit", new Vec2F(0.2f, 0.2f), new Vec2F(0.5f, 0.5f))
            };
            foreach (var text in menuButtons) {
                text.SetColor(Color.White);
            }

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
        }
        
        /// <summary>
        /// Controls the colour for the menu buttons
        /// </summary>
        public void Chooser() {
            if (activeMenuButton == 0) {
                menuButtons[0].SetColor(Color.Green);
                menuButtons[1].SetColor(Color.White);
            } else if (activeMenuButton == 1) {
                menuButtons[1].SetColor(Color.Green);
                menuButtons[0].SetColor(Color.White);
            }
        }

        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            Chooser();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (var text in menuButtons) {
                text.RenderText();
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

                        EventBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));
                    } else {
                        EventBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "MAIN_MENU", ""));
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