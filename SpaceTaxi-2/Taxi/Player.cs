using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2.Taxi {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly DynamicShape shape;
        private Orientation taxiOrientation;
        private bool LeftHeld;
        private bool RightHeld;
        private bool UpHeld;
        public Vec2F Velocity;

        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                TaxiImages.TaxiThrustNone();
            taxiBoosterOffImageRight =
                TaxiImages.TaxiThrustNoneRight();
            Velocity = new Vec2F(0.0f,0.004f);

            Entity = new Entity(shape, TaxiImages.TaxiThrustNone());
        }

        public Entity Entity { get; }

        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer() {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();            
        }

        public void UpdateTaxi() {
            if (LeftHeld == true) {
                Velocity.X -= 0.0001f;
            }

            if (RightHeld == true) {
                Velocity.X += 0.0001f;
            }

            if (UpHeld == true) {
                Velocity.Y += 0.0001f;
            }

            Velocity.Y -= 0.00005f; //gravity
            
            SetPosition(shape.Position.X + Velocity.X, shape.Position.Y + Velocity.Y);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                
                    case "BOOSTER_TO_LEFT":
                        LeftHeld = true;
                        taxiOrientation = Orientation.Left;
                        break;
                    case "BOOSTER_TO_RIGHT":
                        RightHeld = true;
                        taxiOrientation = Orientation.Right;
                        break;
                    case "BOOSTER_UPWARDS":
                        UpHeld = true;
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        LeftHeld = false;
                        break;
                    case "STOP_ACCELERATE_RIGHT":
                        RightHeld = false;
                        break;
                    case "STOP_ACCELERATE_UP":
                        UpHeld = false;
                        break;
                }
            }
        }
    }
}