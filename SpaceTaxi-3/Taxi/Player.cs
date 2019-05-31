using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_3.Taxi {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly DynamicShape shape;
        private Orientation taxiOrientation;
        private bool LeftHeld;
        private bool RightHeld;
        private bool UpHeld;
        public Vec2F Velocity;
        public AnimationContainer Thrusters;
        public List<Image> ThrusterStrides;
        public bool Landed;
        public char CurrentPlatform;
        public List<Image> taxiThrustbottom;
        public List<Image> taxiThrustrightBack;
        public List<Image> taxiThrustright;
        public List<Image> taxiThrustleft;
        public List<Image> taxiThrustBottomBack;
        public List<Image> taxiThrustBottomBackleft;
        public bool haveCustomer;
        public Customer Customer;



        public Player() {
            shape = new DynamicShape(new Vec2F(0.45f, 0.6f), new Vec2F(0.05f, 0.05f));
            taxiBoosterOffImageLeft =
                TaxiImages.TaxiThrustNone();
            taxiBoosterOffImageRight =
                TaxiImages.TaxiThrustNoneRight();
            Velocity = new Vec2F(0.0f,0.004f);
            Landed = false;

            Entity = new Entity(shape, TaxiImages.TaxiThrustNone());
            Thrusters = new AnimationContainer(500);
            taxiThrustbottom =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
            taxiThrustrightBack =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
            taxiThrustright =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
            taxiThrustleft =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
            taxiThrustBottomBack = 
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
            taxiThrustBottomBackleft = 
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
            haveCustomer = false;
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
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;

            if (UpHeld && taxiOrientation == Orientation.Left) {
                Entity.Image = new ImageStride(80, taxiThrustbottom);
            }
            if (UpHeld && taxiOrientation == Orientation.Right) {
                Entity.Image = new ImageStride(80,taxiThrustright);
            }
            if (LeftHeld) { ;
                Entity.Image = new ImageStride(80,taxiThrustleft);
            }
            if (RightHeld) {
                Entity.Image = new ImageStride(80,taxiThrustrightBack);
            }

            if (UpHeld && RightHeld) {
                Entity.Image = new ImageStride(80,taxiThrustBottomBack);
            }

            if (UpHeld && LeftHeld)  {
                Entity.Image = new ImageStride(80, taxiThrustBottomBackleft);
            }
            
            
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

            if (Velocity.Y > 0f) {
                Landed = false; //Taxi is not landed if it is moving upward
            }

            if (Landed) {
                Velocity.X = 0f; //Taxi cannot glide on platform
            } else {
                Velocity.Y -= 0.00005f; //gravity
            }

            Entity.Shape.AsDynamicShape().Direction = Velocity;

            Entity.Shape.AsDynamicShape().Move();
            
            
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