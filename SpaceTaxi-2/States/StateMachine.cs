using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace SpaceTaxi_2.States {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActivateState { get; private set; }

        public StateMachine() {
            EventBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            EventBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            EventBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);

            ActivateState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.MainMenu:
                ActivateState = MainMenu.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActivateState = GamePaused.GetInstance();
                break;
            case GameStateType.GameRunning:
///                ActivateState = GameRunning.GetInstance();
                break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            
            if (eventType == GameEventType.GameStateEvent) {
                
                SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                
            }

            if (eventType == GameEventType.InputEvent) {

                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    ActivateState.HandleKeyEvent("KEY_PRESS", gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    ActivateState.HandleKeyEvent("KEY_RELEASE", gameEvent.Message);
                    break;
                }

            }
        }
    }
}