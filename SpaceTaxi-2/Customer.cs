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
        private AnimationContainer Walking;
        private int walkingLenght = 200;
        private string Name;
        
        public Customer(string customer) {
            Name = customer;
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

        }
       
        private void AddWalkingLeft(float posX, float posY, float extentX, float extentY) {
            Walking.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                walkingLenght, new ImageStride(walkingLenght / 8, CustomerLeftStride));
        }
        private void AddWalkingRight(float posX, float posY, float extentX, float extentY) {
            Walking.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                walkingLenght, new ImageStride(walkingLenght / 8, CustomerRightStride));
        }

        
    }
}