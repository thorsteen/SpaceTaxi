using DIKUArcade.EventBus;

namespace SpaceTaxiApplication.States {
    public static class EventBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return eventBus ?? (eventBus =
                       new GameEventBus<object>());
        }
    }
}