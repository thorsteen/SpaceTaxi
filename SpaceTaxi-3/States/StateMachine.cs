using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace SpaceTaxi_3.States {
    public class StateMachine : IGameEventProcessor<object>
    {
        public static LevelController LevelController;

        public IGameState ActivateState { get; private set; }

        public StateMachine() {     
            StateMachine.LevelController = new LevelController();

            EventBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            EventBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            EventBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);

            ActivateState = MainMenu.GetInstance();         
        }
        
        /// <summary>
        /// Switches between states.
        /// </summary>
        /// <param name="stateType"></param>
        private void SwitchState(GameStateType stateType) {       
            switch (stateType) {
                case GameStateType.MainMenu:
                    ActivateState = MainMenu.GetInstance();
                    break;
                case GameStateType.GamePaused:
                    ActivateState = GamePaused.GetInstance();
                    break;
                case GameStateType.GameRunning:
                    ActivateState = GameRunning.GetInstance();
                    break;
                case GameStateType.ChooseLevel:
                    ActivateState = ChooseLevel.GetInstance();
                    break;
                default:
                    ActivateState = MainMenu.GetInstance();
                    break;
            }   
            // remembers last set state for eventual later restarts of level
            if (stateType != GameStateType.GameRunning) { }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
                case GameEventType.GameStateEvent:
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                    break;
                case GameEventType.InputEvent:
                    switch (gameEvent.Parameter1) {
                        case "KEY_PRESS":
                            ActivateState.HandleKeyEvent("KEY_PRESS", gameEvent.Message);
                            break;
                        case "KEY_RELEASE":
                            ActivateState.HandleKeyEvent("KEY_RELEASE", gameEvent.Message);
                            break;
                    }
                    break;
            }
        }
    }
}