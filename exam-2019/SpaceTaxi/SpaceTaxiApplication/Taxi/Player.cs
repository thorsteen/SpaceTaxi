using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxiApplication.Taxi {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly DynamicShape shape;
        private Orientation taxiOrientation;
        private bool leftHeld;
        private bool rightHeld;
        private bool upHeld;
        public Vec2F Velocity;
        public AnimationContainer Thrusters;
        public List<Image> ThrusterStrides;
        public bool Landed;
        public char CurrentPlatform;
        public List<Image> TaxiThrustbottom;
        public List<Image> TaxiThrustrightBack;
        public List<Image> TaxiThrustright;
        public List<Image> TaxiThrustleft;
        public List<Image> TaxiThrustBottomBack;
        public List<Image> TaxiThrustBottomBackleft;
        public bool HasCustomer;
        public Customer.Customer Customer;



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
            TaxiThrustbottom =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
            TaxiThrustrightBack =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
            TaxiThrustright =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
            TaxiThrustleft =
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
            TaxiThrustBottomBack = 
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
            TaxiThrustBottomBackleft = 
                new List<Image>(ImageStride.CreateStrides(2,
                    Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
            HasCustomer = false;
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
        /// <summary>
        /// Renders the players thruster images. 
        /// </summary>
        public void RenderPlayer() {
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;

            if (upHeld && taxiOrientation == Orientation.Left) {
                Entity.Image = new ImageStride(80, TaxiThrustbottom);
            }
            if (upHeld && taxiOrientation == Orientation.Right) {
                Entity.Image = new ImageStride(80,TaxiThrustright);
            }
            if (leftHeld) { ;
                Entity.Image = new ImageStride(80,TaxiThrustleft);
            }
            if (rightHeld) {
                Entity.Image = new ImageStride(80,TaxiThrustrightBack);
            }

            if (upHeld && rightHeld) {
                Entity.Image = new ImageStride(80,TaxiThrustBottomBack);
            }

            if (upHeld && leftHeld)  {
                Entity.Image = new ImageStride(80, TaxiThrustBottomBackleft);
            }           
            Entity.RenderEntity();
        }
        /// <summary>
        /// Keeps a track of the players movement and velocity, as well as gravity
        /// </summary>
        public void UpdateTaxi() {
            if (leftHeld) {
                Velocity.X -= 0.0001f;
            }
            if (rightHeld) {
                Velocity.X += 0.0001f;
            }
            if (upHeld) {
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
                        leftHeld = true;
                        taxiOrientation = Orientation.Left;
                        break;
                    case "BOOSTER_TO_RIGHT":
                        rightHeld = true;
                        taxiOrientation = Orientation.Right;
                        break;
                    case "BOOSTER_UPWARDS":
                        upHeld = true;
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        leftHeld = false;
                        break;
                    case "STOP_ACCELERATE_RIGHT":
                        rightHeld = false;
                        break;
                    case "STOP_ACCELERATE_UP":
                        upHeld = false;
                        break;
                }
            }
        }
    }
}