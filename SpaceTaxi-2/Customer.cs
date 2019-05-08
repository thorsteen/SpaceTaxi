using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2 {
    public class Customer {
        private DynamicShape shape;
        private Image customerLeft;
        private Image customerMoveLeft;
        private Image customerRight;
        private Image customerMoveRight;
        public List<Customer> CustomerList;
        public List<Image> CustomerLeftStride;
        public List<Image> CustomerRightStride;
        private AnimationContainer walking;
        private int walkingLenght = 200;
        public Entity Entity { get; private set; }

        private string name;
        
        public Customer(string customer, DynamicShape shape, IBaseImage image) {
            name = customer;
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            customerLeft = new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            customerMoveLeft = new Image(Path.Combine("Assets","Images","CustomerWalkLeft.png"));
            customerRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            customerMoveRight = new Image(Path.Combine("Assets", "Images", "CustomerWalkRight.png"));
            CustomerList = new List<Customer>();
            CustomerLeftStride = ImageStride.CreateStrides(2, Path.Combine("Assets","Images",
            "CustomerWalkLeft.png"));
            CustomerLeftStride = ImageStride.CreateStrides(2, Path.Combine("Assets","Images",
                "CustomerWalkRight.png"));
            walking = new AnimationContainer(200);
            Entity = new Entity(shape,image);

        }
       
        private void AddWalkingLeft(float posX, float posY, float extentX, float extentY) {
            walking.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                walkingLenght, new ImageStride(walkingLenght / 2, CustomerLeftStride));
        }
        private void AddWalkingRight(float posX, float posY, float extentX, float extentY) {
            walking.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                walkingLenght, new ImageStride(walkingLenght / 2, CustomerRightStride));
        }
        /// <summary>
        /// Sets a direction for the Customer. 
        /// </summary>
        /// <param name="direction"></param>
        private void Direction(Vec2F direction) {
            DynamicShape dynamicShape = Entity.Shape.AsDynamicShape();
            dynamicShape.ChangeDirection(direction);
        }
        
        /// <summary>
        /// Moves the customer ( for now until its not going out of bounds ). 
        /// </summary>
        public void Move() {
            DynamicShape dynamicShape = Entity.Shape.AsDynamicShape();
            if (Entity.Shape.Position.X + dynamicShape.Direction.X < 0.9f &&
                Entity.Shape.Position.X + dynamicShape.Direction.X > 0.0f) {
                Entity.Shape.Move();
            }
        }
        
    
        
    }
}