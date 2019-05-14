﻿using System;
using System.Collections.Generic;
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
        public AnimationContainer Thrusters;
        public List<Image> ThrusterStrides;

        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                TaxiImages.TaxiThrustNone();
            taxiBoosterOffImageRight =
                TaxiImages.TaxiThrustNoneRight();
            Velocity = new Vec2F(0.0f,0.004f);

            Entity = new Entity(shape, TaxiImages.TaxiThrustNone());
            Thrusters = new AnimationContainer(500);
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

           /* if (UpHeld) {
                ThrusterStrides = ImageStride.CreateStrides(2,
                    Path.Combine("Assests", "Images", "Taxi_Thrust_Bottom.png"));
                Thrusters.AddAnimation(new DynamicShape(new Vec2F(this.Entity.Shape.Position.X,
                    this.Entity.Shape.Position.Y), new Vec2F(this.Entity.Shape.Extent.X, this.Entity
                    .Shape.Extent.Y)), 500, new ImageStride(500 / 2, ThrusterStrides));
                TaxiImages.TaxiThrustBottom().GetTexture()
                    .Render(new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)));*/
            
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