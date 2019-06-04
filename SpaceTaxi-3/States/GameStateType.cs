using System;

namespace SpaceTaxi_3.States {
    public enum GameStateType {
        GameRunning,
        GamePaused,
        MainMenu,
        ChooseLevel,
    }
    
    /// <summary>
    /// Transformation of string into a relevant state type.
    /// </summary>
    /// <param name>String</param>
    /// <returns>GameStateType</returns>
    public class StateTransformer {
            
        public static GameStateType TransformStringToState(String state) {
            switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            case "Choose_Level":
                return GameStateType.ChooseLevel;
            default:
                throw new ArgumentException("String was not a valid Input");
            }
        }
       /// <summary>
       /// Transformation of state type into relevant string.
       /// </summary>
       /// <param name="state">GameStateType</param>
       /// <returns>String</returns>
       /// <exception cref="ArgumentException"></exception>
        public static string TransformStateToString(GameStateType state) {
            switch (state) {
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";
            case GameStateType.ChooseLevel:
                return "Choose_Level";
            default:
                throw new ArgumentException("GameState was not a valid Input");
            }
        }
    }
}